using System.ComponentModel.DataAnnotations;

namespace UserAuthenticationApp.Pages.Account.Register
{
    /// <summary>
    /// Represents the data model for user login input.
    /// </summary>
    public class InputModel
    {
        /// <summary>
        /// Gets or sets the username entered by the user.
        /// </summary>
        [Required]
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the password entered by the user.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the user's login session should be persistent.
        /// </summary>
        /// <value>
        /// True if the login session should be persistent; otherwise, false.
        /// </value>
        public bool RememberMe { get; set; }
    }
}
