using System.ComponentModel.DataAnnotations;

namespace UserAuthenticationApp.Pages.Account.Register
{
    /// <summary>
    /// Represents the data model for user registration input.
    /// </summary>
    public class InputModel
    {
        /// <summary>
        /// Gets or sets the username entered by the user.
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the email entered by the user.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password entered by the user.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the confirmation of the password entered by the user.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user's login session should be persistent.
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Two-Factor Authentication (TFA) is enabled for the user.
        /// </summary>
        [Display(Name = "Enable Two-Factor Authentication")]
        public bool EnableTfa { get; set; }
    }
}
