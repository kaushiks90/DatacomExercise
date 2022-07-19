using DatacomConsole;
using DatacomConsole.Models.Appsettings;
using DatacomConsole.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Threading.Tasks;


namespace DatacomExercise
{
    class Program
    {
        private static ILogger<Program> logger;
        static async Task Main(string[] args)
        {
            try
            {
                var host = new HostBuilder()
                  .ConfigureAppConfiguration((hostingContext, config) =>
                  {
                      config.SetBasePath(AppContext.BaseDirectory);
                      config.AddJsonFile("appsettings.json", true, true);
                  }).ConfigureServices((hostingContext, services) =>
                  {
                      services.AddSingleton<IRunService, RunService>();
                      services.AddSingleton<IRestUtility, RestUtility>();

                      services.AddHttpClient("MSPServices", client =>
                      {
                          client.BaseAddress = new Uri(hostingContext.Configuration.GetSection("BaseEndpoint").Value);
                          client.DefaultRequestHeaders.Add("Accept", "application/json");
                      });

                      if (Convert.ToBoolean(hostingContext.Configuration.GetSection("LoggingConfig:EnableLog").Value))
                      {
                          Log.Logger = new LoggerConfiguration()
                             .WriteTo.RollingFile(hostingContext.Configuration.GetSection("LoggingConfig:LogPath").Value, fileSizeLimitBytes: 10485760)
                             .CreateLogger();

                          services.AddLogging(configure => configure.AddSerilog());
                      }

                      services.AddConfiguration<ApiEndPoint>(hostingContext.Configuration, "ApiEndPoint");
                      services.AddConfiguration<AccessToken>(hostingContext.Configuration, "AccessToken");
                  })
                 .UseConsoleLifetime()
                    .Build();
                using (var serviceScope = host.Services.CreateScope())
                {
                    var services = serviceScope.ServiceProvider;
                    var runService = services.GetRequiredService<IRunService>();
                    logger = services.GetRequiredService<ILogger<Program>>();
                    await runService.RunAsync();

                }

                
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception occured in Main Method. Message: {ex.Message} Stacktrace: {ex.StackTrace}");
            }
        }
    }
}
