using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

using TLP_API.Models;

namespace TLP_API.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private CosmosClient _cosmosClient;
        private Container _container;

        public CosmosDbService(string accountEndpoint, string accountKey, string databaseName, string containerName)
        {
            _cosmosClient = new CosmosClient(accountEndpoint, accountKey);
            _container = _cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task AddItemAsync<T>(T item)
        {
            // Get the partition key from the 'id' property of the item
            var partitionKey = new PartitionKey(item.GetType().GetProperty("id")?.GetValue(item)?.ToString());

            // Create item with correct partition key
            await _container.CreateItemAsync(item, partitionKey);
        }
    }
}