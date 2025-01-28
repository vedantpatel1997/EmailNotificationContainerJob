using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmailJobService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Application Version 1.4");
                // Build the configuration (e.g., from appsettings.json)
                IConfiguration configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                await Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                })
                .Build()
                .RunAsync();
            }
            catch (Exception ex)
            {
                // Catch any unhandled exceptions during startup or execution
                Console.WriteLine($"An error occurred: {ex.Message}");
                return; // Return a non-zero value to indicate failure
            }
            finally
            {
                // Ensure cleanup is done (if any), this can be extended as needed
                Console.WriteLine("Application terminating...");
            }
        }
    }
}