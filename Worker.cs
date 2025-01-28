using EmailJobService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace EmailJobService
{
    public class Worker : BackgroundService
    {
        private readonly EmailMessageService _emailService;

        public Worker(IConfiguration configuration)
        {
            var emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();
            _emailService = new EmailMessageService(emailSettings);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Starting email job...");

            try
            {
                var isEmailSend = await _emailService.SendEmailAsync(
                     "Scheduled Email",
                     "<p>This is a test email from Azure Container App Job.</p>"
                 );

                if (isEmailSend == 0) Console.WriteLine("Email job completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during the email job: {ex.Message}");
            }
            finally
            {
                // Ensure the application exits after the job completes
                Console.WriteLine("Signal to Shut down..");
                Environment.Exit(0); // Exit the application with code 0 (success)
            }
        }
    }
}


