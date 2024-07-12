using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Quiz.Services
{
    /// <summary>
    /// Implements IEmailSender to provide email sending functionality.
    /// </summary>
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the EmailSender class.
        /// </summary>
        /// <param name="configuration">The configuration to access email settings.</param>
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Sends an email asynchronously.
        /// </summary>
        /// <param name="email">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="htmlMessage">The HTML content of the email.</param>
        /// <returns>A task that represents the asynchronous email operation.</returns>
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Retrieve email settings from configuration
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderPassword = _configuration["EmailSettings:SenderPassword"];

            // Create and configure the SMTP client
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                // Create the email message
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(email);

                // Send the email asynchronously
                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
