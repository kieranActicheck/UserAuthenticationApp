using Microsoft.AspNetCore.Identity;
using System;

namespace UserAuthenticationApp.Data
{
    /// <summary>
    /// Represents an entity that extends IdentityUser and maps to the 'dbo.KieranProjectUsers' table in the database.
    /// </summary>
    public class KieranProjectUser : IdentityUser
    {
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
