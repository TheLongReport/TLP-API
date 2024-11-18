using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;
using TLP_API.Models;
using TLP_API.Configuration;

namespace TLP_API.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;

        public CosmosDbService(CosmosDbConfig config)
        {
            _cosmosClient = new CosmosClient(config.AccountEndpoint, config.AccountKey);
            _container = _cosmosClient.GetContainer(config.DatabaseName, config.ContainerName);
        }

        public async Task AddItemAsync<T>(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Item cannot be null.");
            }

            var partitionKey = new PartitionKey(item.GetType().GetProperty("id")?.GetValue(item)?.ToString());
            await _container.CreateItemAsync(item, partitionKey);
        }
    }
}
