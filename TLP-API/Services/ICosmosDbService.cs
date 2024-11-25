using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TLP_API.Services
{
    public interface ICosmosDbService
    {
        /// <summary>
        /// Adds an item to the Cosmos DB container.
        /// </summary>
        /// <typeparam name="T">The type of the item to add.</typeparam>
        /// <param name="item">The item to add.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddItemAsync<T>(T item, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves an item from the Cosmos DB container by its ID and partition key.
        /// </summary>
        /// <typeparam name="T">The type of the item to retrieve.</typeparam>
        /// <param name="id">The unique identifier of the item.</param>
        /// <param name="partitionKey">The partition key of the item.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>The retrieved item or null if not found.</returns>
        Task<T> GetItemAsync<T>(string id, string partitionKey, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an item from the Cosmos DB container by its ID and partition key.
        /// </summary>
        /// <param name="id">The unique identifier of the item.</param>
        /// <param name="partitionKey">The partition key of the item.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteItemAsync(string id, string partitionKey, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all items of the specified type from the Cosmos DB container.
        /// </summary>
        /// <typeparam name="T">The type of the items to retrieve.</typeparam>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>A list of all items of the specified type.</returns>
        Task<IEnumerable<T>> GetAllItemsAsync<T>(CancellationToken cancellationToken = default);


        Task<bool> TestConnectionAsync();

    }
}
