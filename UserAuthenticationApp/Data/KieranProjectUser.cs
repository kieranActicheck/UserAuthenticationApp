using Microsoft.AspNetCore.Identity;

namespace UserAuthenticationApp.Data
{
    /// <summary>
    /// Represents an entity that maps to the 'dbo.KieranProjectUsers' table in acticheckdev_db databasse.
    /// This class includes porperties that correspond to the columns in the database table.
    /// </summary>
    public class KieranProjectUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets the unique Identifier of the user.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the email address for this user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user name for this user.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password hash for this user.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the user account was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the user's last login.
        /// </summary>
        public DateTime LastLoginDate { get; set; }
    }
}
