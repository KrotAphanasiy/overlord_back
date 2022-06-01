using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Flash.Central.Api
{
    /// <summary>
    /// Class. The main app's class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The application's entry point
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var built = CreateHostBuilder(args)
                 .Build();
            built.Run();
        }

        /// <summary>
        /// Configures host builder
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.local.json", optional: true);
                    config.AddEnvironmentVariables("FLASH_");
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
