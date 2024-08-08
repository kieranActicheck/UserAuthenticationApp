using Microsoft.AspNetCore.Identity;

namespace UserAuthenticationApp.Data
{
    /// <summary>
    /// ApplicationUser class extending IdentityUser to include additional properties if needed.
    /// This represents the user entity for your application and extends the identity-related functionality.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public override bool TwoFactorEnabled { get; set; }
    }
}
