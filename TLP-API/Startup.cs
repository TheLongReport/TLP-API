using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using TLP_API.Configuration;
using TLP_API.Services;

[assembly: FunctionsStartup(typeof(TLP_API.Startup))]

namespace TLP_API
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Register CosmosDbConfig as a Singleton to use across functions
            builder.Services.AddSingleton<CosmosDbConfig>(new CosmosDbConfig());

            // Register CosmosDbService using CosmosDbConfig
            builder.Services.AddSingleton<ICosmosDbService, CosmosDbService>();
        }
    }
}
