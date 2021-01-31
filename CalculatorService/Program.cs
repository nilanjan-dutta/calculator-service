using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CalculatorService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            //.UseDefaultServiceProvider(options =>
            //{
            //    options.ValidateScopes = true;
            //})
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureLogging(logging =>
            {
                // clear default logging providers
                logging.ClearProviders();

                // add built-in providers manually, as needed 
                logging.AddConsole();
            });
    }
}
