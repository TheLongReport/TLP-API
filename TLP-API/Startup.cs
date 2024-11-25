using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TLP_API.Services;                         // services like ICosmosDbService
using TLP_API.Configuration;                    // config classes like CosmosDbConfig
using Microsoft.Extensions.Logging;


public class Program
{
    public static void Main(string[] args)
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWebApplication()
            .ConfigureServices(services =>
            {
                // Register your services here
                services.AddSingleton<CosmosDbConfig>(new CosmosDbConfig());
                services.AddSingleton<ICosmosDbService, CosmosDbService>();

                // Optionally add more services
            })
            .ConfigureLogging(logging =>
            {
                logging.AddConsole(); // Enables console logging
                logging.SetMinimumLevel(LogLevel.Debug); // Adjust log level as needed
            })
            .Build();

        host.Run();
    }
}