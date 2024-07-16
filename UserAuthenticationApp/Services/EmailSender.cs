using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace UserAuthenticationApp.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Send email
            Console.WriteLine($"Sending email to {email} with subject {subject}.");
            return Task.CompletedTask;
        }
    }
}
