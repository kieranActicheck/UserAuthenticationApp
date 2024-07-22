using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;

namespace UserAuthenticationApp.ViewModels
{
    /// <summary>
    /// Represents the view model for user login, including local and external authentication methods.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Gets or sets the username or email for login.
        /// </summary>
        [Required]
        [Display(Name = "Username or Email")]
        public string UsernameOrEmail { get; set; }

        /// <summary>
        /// Gets or sets the password for login.
        /// </summary>
        [Required]
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
    }
}
