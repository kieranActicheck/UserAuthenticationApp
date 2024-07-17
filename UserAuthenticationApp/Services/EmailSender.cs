using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using SendGrid.Helpers.Mail;
using SendGrid;
using Microsoft.Extensions.Logging; // Add this for logging

namespace UserAuthenticationApp.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string sendGridKey;
        private readonly ILogger<EmailSender> _logger; // Logger instance

        // Modify the constructor to accept ILogger<EmailSender>
        public EmailSender(string apiKey, ILogger<EmailSender> logger)
        {
            sendGridKey = apiKey;
            _logger = logger; // Initialize the logger
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                _logger.LogInformation($"Sending email to {email} with subject '{subject}'.");

                var client = new SendGridClient(sendGridKey);
                var from = new EmailAddress("kierane@acticheck.com", "Acticheck");
                var to = new EmailAddress(email);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
                var response = await client.SendEmailAsync(msg);

                // Log the response from SendGrid
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Email to {email} sent successfully.");
                }
                else
                {
                    _logger.LogWarning($"Failed to send email to {email}. Response status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during the email sending process
                _logger.LogError(ex, $"An error occurred while sending email to {email}.");
                throw; // Consider rethrowing the exception if you want the calling code to handle it
            }
        }
    }
}
