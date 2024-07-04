using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserAuthenticationApp.Data;

namespace UserAuthenticationApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            // Configure DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add Identity services
            builder.Services.AddIdentity<KieranProjectUser, IdentityRole>(options =>
            {
                // Configure password requirements
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;

                // Configure lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // Configure TFA (Two-Factor Authentication)
                options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
                options.SignIn.RequireConfirmedAccount = true; // Ensure users have confirmed their account to enable TFA
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Configure logging
            builder.Logging.ClearProviders(); // Clear existing logging providers
            builder.Logging.AddConsole(); // Add console logger

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllers();

            app.Run();
        }
    }
}
