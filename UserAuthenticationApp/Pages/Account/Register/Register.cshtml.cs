using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using UserAuthenticationApp.Data;

namespace UserAuthenticationApp.Pages.Account.Register
{
    /// <summary>
    /// Page model for handling user registration functionality.
    /// </summary>
    public class RegisterModel : PageModel
    {
        private readonly UserManager<KieranProjectUser> _userManager;
        private readonly SignInManager<KieranProjectUser> _signInManager;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(UserManager<KieranProjectUser> userManager, SignInManager<KieranProjectUser> signInManager, ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        /// Gets or sets the input model for user registration details.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        /// Handles the GET request to the registration page.
        /// </summary>
        public void OnGet()
        {
        }

        /// <summary>
        /// Handles POST requests when the registration form is submitted.
        /// </summary>
        /// <param name="returnUrl">Optional return URL after successful registration.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = new KieranProjectUser
                {
                    UserName = Input.UsernameOrEmail,
                    Email = Input.Email,
                    TwoFactorEnabled = Input.EnableTfa,
                    Forename = Input.Forename,
                    Surname = Input.Surname
                };
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}