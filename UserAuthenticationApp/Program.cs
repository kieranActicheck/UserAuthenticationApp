using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UserAuthenticationApp.Data;
using UserAuthenticationApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.UI.Services;
using Serilog;

namespace UserAuthenticationApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .WriteTo.Console()
                .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Add Serilog
            builder.Host.UseSerilog();

            // Add services to the container.
            builder.Services.AddRazorPages();

            // Configure ApplicationDbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configure LogContext
            builder.Services.AddDbContext<LogContext>(options =>
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
            var sendGridApiKey = builder.Configuration["SendGrid:ApiKey"] ?? throw new InvalidOperationException("SendGrid API key is not configured.");
            builder.Services.AddSingleton<IEmailSender, EmailSender>(serviceProvider => {
                var logger = serviceProvider.GetRequiredService<ILogger<EmailSender>>();
                return new EmailSender(sendGridApiKey, logger);
            });

            builder.Services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                var appId = builder.Configuration["Authentication:Facebook:AppId"] ?? throw new InvalidOperationException("Facebook AppId is not configured.");
                var appSecret = builder.Configuration["Authentication:Facebook:AppSecret"] ?? throw new InvalidOperationException("Facebook AppSecret is not configured.");
                facebookOptions.AppId = appId;
                facebookOptions.AppSecret = appSecret;
            });

            // Register LogFileProcessor as a hosted service
            builder.Services.AddHostedService<LogFileProcessor>();

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

            // Test method to trigger breakpoints
            //TestMsgCoordinator();
        }

        ///// <summary>
        ///// TEMPORARY method to test the MsgCoordinator class.
        ///// </summary>
        //private static void TestMsgCoordinator()
        //{
        //    var coordinator = new MsgCoordinator();
        //    string message = "|BDSTAT:B4994C3317DF,316B6A00005508E718804F6B555D8233||";
        //    coordinator.ProcessRequest(message);
        //}
    }
}
