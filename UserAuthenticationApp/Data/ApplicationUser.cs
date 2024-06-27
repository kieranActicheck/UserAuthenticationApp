using Microsoft.AspNetCore.Identity;

namespace UserAuthenticationApp.Data
{
    /// <summary>
    /// ApplicationUser class extending IdentityUser to include additional properties if needed.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public bool TwoFactorEnabled { get; set; }
    }
}
