using Microsoft.Azure.Cosmos;

public class CosmosDbService
{
    private readonly CosmosClient _client;
    private readonly Container _container;

    public CosmosDbService(string connectionString, string databaseName, string containerName)
    {
        _client = new CosmosClient(connectionString);
        _container = _client.GetContainer(databaseName, containerName);
    }

    public async Task AddItemAsync<T>(T item, string id)
    {
        await _container.CreateItemAsync(item, new PartitionKey(id));
    }

    public async Task<T> GetItemAsync<T>(string id)
    {
        var response = await _container.ReadItemAsync<T>(id, new PartitionKey(id));
        return response.Resource;
    }
}
