using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserAuthenticationApp.Data;

namespace UserAuthenticationApp.Data
{
    /// <summary>
    /// ApplicationDbContext class extending IdentityDbContext for ASP.NET Core Identity.
    /// This serves as the database context for the application, defining the entities that are managed
    /// by Entity Framework Core. This class typically includes 'DbSet' properties for each entity that
    /// represents a table in the database.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<KieranProjectUser> KieranProjectUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Customise the ASP.NET Identity model and override the defaults if needed.
        }
    }
}
