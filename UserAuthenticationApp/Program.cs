using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UserAuthenticationApp.Data;
using UserAuthenticationApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.UI.Services;

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

            // Add Identity services using KieranProjectUser
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

            // Register the IEmailSender implementation
            var sendGridApiKey = builder.Configuration["SendGrid:ApiKey"];
            builder.Services.AddSingleton<IEmailSender, EmailSender>(serviceProvider => {
                var logger = serviceProvider.GetRequiredService<ILogger<EmailSender>>();
                return new EmailSender(sendGridApiKey, logger);
            });

            Console.WriteLine($"FacebookAppId: {Environment.GetEnvironmentVariable("FacebookAppId")}");
            Console.WriteLine($"FacebookAppSecret: {Environment.GetEnvironmentVariable("FacebookAppSecret")}");

            builder.Services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Environment.GetEnvironmentVariable("FacebookAppId");
                facebookOptions.AppSecret = Environment.GetEnvironmentVariable("FacebookAppSecret");
            });

            


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
