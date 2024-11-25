using Microsoft.Azure.Cosmos;
using System;
using System.Reflection;
using System.Threading;
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

        public async Task AddItemAsync<T>(T item, CancellationToken cancellationToken = default)
        {

            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Item cannot be null.");
            }

            try
            {
                var partitionKeyValue = GetPartitionKeyValue(item);

                var partitionKey = new PartitionKey(partitionKeyValue);
                Console.WriteLine($"Adding item with partition key: {partitionKey}");
                await _container.CreateItemAsync(item, partitionKey, cancellationToken: cancellationToken);
                Console.WriteLine("Item successfully added to Cosmos DB.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding item to Cosmos DB: {ex.Message}");
                throw;
            }
        }

        public async Task<T> GetItemAsync<T>(string id, string partitionKey, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id), "ID cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(partitionKey))
            {
                throw new ArgumentNullException(nameof(partitionKey), "Partition key cannot be null or empty.");
            }

            try
            {
                var response = await _container.ReadItemAsync<T>(id, new PartitionKey(partitionKey), cancellationToken: cancellationToken);
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default;
            }
        }

        public async Task DeleteItemAsync(string id, string partitionKey, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id), "ID cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(partitionKey))
            {
                throw new ArgumentNullException(nameof(partitionKey), "Partition key cannot be null or empty.");
            }

            await _container.DeleteItemAsync<object>(id, new PartitionKey(partitionKey), cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllItemsAsync<T>(CancellationToken cancellationToken = default)
        {
            var query = _container.GetItemQueryIterator<T>();
            var results = new List<T>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync(cancellationToken);
                results.AddRange(response.ToList());
            }

            return results;
        }

        private static string GetPartitionKeyValue<T>(T item)
        {
            var property = typeof(T).GetProperty("id", System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            return property?.GetValue(item)?.ToString();
        }
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                var database = _cosmosClient.GetDatabase("TheLongPlanCore"); // Replace with your actual database name
                var container = database.GetContainer("Users"); // Replace with your actual container name
                var response = await database.ReadAsync();
                return response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }
    }
}