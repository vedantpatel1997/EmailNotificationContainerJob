using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmailJobService.Services
{
    public class EmailMessageService
    {
        private readonly EmailSettings _emailSettings;

        public EmailMessageService(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }

        /// <summary>
        /// Generates the contact information section in HTML.
        /// </summary>
        private string CreateContactInfoHtml()
        {
            return @"
                <div style='max-width: 400px; margin: 0 auto;'>
                    <p style='font-size: 16px; color: #333; font-weight: bold; text-align: center;'>Contact Information:</p>
                    <table style='font-size: 14px; color: #666; border-collapse: collapse; width: 100%; border: 1px solid #333; border-radius: 8px;'>
                        <tr><td style='padding: 8px;'>Name:</td><td style='padding: 8px;'>Vedant Patel</td></tr>
                        <tr><td style='padding: 8px;'>Email:</td><td style='padding: 8px;'>vedantp9@gmail.com</td></tr>
                        <tr><td style='padding: 8px;'>Phone:</td><td style='padding: 8px;'>+1 (647) 627 4235</td></tr>
                        <tr><td style='padding: 8px;'>LinkedIn:</td>
                            <td style='padding: 8px;'>
                                <a href='https://www.linkedin.com/in/vedant-patel-38b743110/' style='color: #0070f3; text-decoration: none;'>Vedant Patel on LinkedIn</a>
                            </td>
                        </tr>
                    </table>
                </div>";
        }

        /// <summary>
        /// Sends an email with the provided subject and body.
        /// </summary>
        public async Task<int> SendEmailAsync(string subject, string bodyHtml)
        {
            string contactInfoHtml = CreateContactInfoHtml();

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromAddress),
                Subject = subject,
                Body = $"{bodyHtml}{contactInfoHtml}",
                IsBodyHtml = true
            };
            mailMessage.To.Add(_emailSettings.ToAddress);

            using (var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(_emailSettings.FromAddress, _emailSettings.FromPassword);
                smtpClient.EnableSsl = true;

                try
                {
                    await smtpClient.SendMailAsync(mailMessage);
                    Console.WriteLine("Email sent successfully!");
                    return 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email: {ex.Message}");
                    return 1;
                }
            }
        }
    }

    public class EmailSettings
    {
        public string FromAddress { get; set; }
        public string FromPassword { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string ToAddress { get; set; }
    }
}
