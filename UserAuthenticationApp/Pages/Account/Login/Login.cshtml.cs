using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserAuthenticationApp.Data;
using UserAuthenticationApp.Pages.Account.Register;

namespace UserAuthenticationApp.Pages.Account.Login
{
    /// <summary>
    /// Page model for handling user login functionality.
    /// </summary>
    public class LoginModel : PageModel
    {
        private readonly SignInManager<KieranProjectUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        /// <summary>
        /// Gets or sets the input model for user login credentials.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="LoginModel"/> class.
        /// </summary>
        public LoginModel(SignInManager<KieranProjectUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        /// Handles GET requests to the login page.
        /// </summary>
        public void OnGet()
        {
        }

        /// <summary>
        /// Handles POST requests when the login form is submitted.
        /// </summary>
        /// <param name="returnUrl">Optional return URL after successful login.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            // Check if the ModelState is valid based on data annotations in InputModel.
            if (ModelState.IsValid)
            {
                // Attempt to sign in the user using the provided username and password.
                var result = await _signInManager.PasswordSignInAsync(Input.Username, Input.Password, false, lockoutOnFailure: true);

                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                // If sign-in is successful, redirect to the returnUrl.
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    // If sign-in fails, add an error to ModelState to display in the view.
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            // If we reach here, something went wrong with ModelState validation or sign-in attempt.
            // Ensure ModelState errors are displayed in the view.
            return Page();
        }
    }
}
