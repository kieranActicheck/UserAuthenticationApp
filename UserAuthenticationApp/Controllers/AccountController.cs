using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserAuthenticationApp.Data; // Assuming KieranProjectUser is in this namespace
using System.Threading.Tasks;

namespace UserAuthenticationApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<KieranProjectUser> _userManager;
        private readonly SignInManager<KieranProjectUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<KieranProjectUser> userManager, SignInManager<KieranProjectUser> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // ACTIONS

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            // If the user is already authenticated, redirect to home or a return URL
            if (User.Identity.IsAuthenticated)
            {
                return LocalRedirect(returnUrl ?? Url.Content("~/"));
            }

            // Redirect to the Razor Pages login page
            returnUrl = returnUrl ?? Url.Content("~/");
            return RedirectToPage("/Account/Login", new { area = "Identity", ReturnUrl = returnUrl });
        }

        [HttpPost]
        public IActionResult Login()
        {
            // POST requests to this action are not supported, redirect to the GET action.
            return RedirectToAction(nameof(Login));
        }
    }
}
