using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserAuthenticationApp.Data;
using UserAuthenticationApp.ViewModels;
using System.Threading.Tasks;

namespace UserAuthenticationApp.Controllers
{
    /// <summary>
    /// Controller responsible for handling account-related actions such as login and logout.
    /// </summary>
    public class AccountController : Controller
    {
        private readonly UserManager<KieranProjectUser> _userManager;
        private readonly SignInManager<KieranProjectUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        /// <summary>
        /// Initialises a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userManager">The user manager for handling user-related operations.</param>
        /// <param name="signInManager">The sign-in manager for handling user sign-in operations.</param>
        /// <param name="logger">The logger for logging information.</param>
        public AccountController(UserManager<KieranProjectUser> userManager, SignInManager<KieranProjectUser> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        /// Signs out the current user and redirects to the home page.
        /// </summary>
        /// <returns>A redirect action to the home page.</returns>
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Presents the login page to the user or redirects if already authenticated.
        /// </summary>
        /// <param name="returnUrl">The URL to return to after successful login, if any.</param>
        /// <returns>A redirect to the login page or to the specified returnUrl.</returns>
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                return LocalRedirect(returnUrl ?? Url.Content("~/"));
            }

            returnUrl = returnUrl ?? Url.Content("~/");
            return RedirectToPage("/Account/Login", new { area = "Identity", ReturnUrl = returnUrl });
        }

        /// <summary>
        /// Processes the user login attempt.
        /// </summary>
        /// <param name="model">The login view model containing the user's login information.</param>
        /// <param name="returnUrl">The URL to return to after successful login, if any.</param>
        /// <returns>A redirect to the specified returnUrl on success, or the login view with errors on failure.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UsernameOrEmail);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.UsernameOrEmail, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");
                        if (await _userManager.GetTwoFactorEnabledAsync(user))
                        {
                            return RedirectToAction("LoginWith2fa", "Account", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                        }
                        return LocalRedirect(returnUrl);
                    }
                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToAction("LoginWith2fa", "Account", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("/Account/Lockout", new { area = "Identity" });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View(model);
                    }
                }
            }

            return View(model);
        }
    }
}
