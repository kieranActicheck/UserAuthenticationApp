using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging; 
using UserAuthenticationApp.Data;

namespace UserAuthenticationApp.ViewModels
{
    /// <summary>
    /// Represents the view model for user login, including local and external authentication methods.
    /// </summary>
    public class LoginViewModel
    {
        private readonly SignInManager<KieranProjectUser> _signInManager;
        private readonly ILogger<LoginViewModel> _logger;

        /// <summary>
        /// Gets or sets the username or email for login.
        /// </summary>
        [Required(ErrorMessage = "Please enter your username or email.")]
        [Display(Name = "Username or Email")]
        public string UsernameOrEmail { get; set; }

        /// <summary>
        /// Gets or sets the password for login.
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the login session should be remembered across browser sessions.
        /// </summary>
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        /// <summary>
        /// Gets or sets the URL to redirect to after a successful login.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Gets or sets the list of external login providers available for authentication.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        /// Constructor that ensures that ExternalLogins is never null.
        /// </summary>
        public LoginViewModel(SignInManager<KieranProjectUser> signInManager, ILogger<LoginViewModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
            ExternalLogins = new List<AuthenticationScheme>();
        }

    }
}
