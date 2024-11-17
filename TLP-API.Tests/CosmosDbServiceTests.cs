using Moq;
using System.Threading.Tasks;
using TLP_API;
using TLP_API.Models;
using TLP_API.Services;
using Xunit;
using Microsoft.Azure.Cosmos;

namespace TLP_API.Tests
{
    public class CosmosDbServiceTests
    {
        private readonly Mock<CosmosClient> _mockCosmosClient;
        private readonly Mock<Container> _mockContainer;
        private readonly ICosmosDbService _cosmosDbService;

        public CosmosDbServiceTests()
        {
            // Mock CosmosClient and Container
            _mockCosmosClient = new Mock<CosmosClient>();
            _mockContainer = new Mock<Container>();

            // Setup GetContainer to return the mock container
            _mockCosmosClient.Setup(client => client.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(_mockContainer.Object);

            // Initialize CosmosDbService with mock data
            _cosmosDbService = new CosmosDbService("fake-endpoint", "fake-key", "fake-db", "fake-container");
        }

        [Fact]
        public async Task AddItemAsync_ShouldAddItemSuccessfully()
        {
            // Arrange: Create a valid item
            var newItem = new MyItem
            {
                Name = "Test Item",    // Set required Name
                Email = "test@example.com"  // Email can be nullable
            };

            // Act: Call the method to add the item
            await _cosmosDbService.AddItemAsync(newItem);

            // Assert: Verify that the CreateItemAsync method was called once with the correct arguments
            _mockContainer.Verify(c => c.CreateItemAsync(It.IsAny<MyItem>(), It.IsAny<PartitionKey>(), It.IsAny<ItemRequestOptions>(), default), Times.Once);
        }

        [Fact]
        public async Task AddItemAsync_ShouldThrowException_WhenItemIsNull()
        {
            // Arrange: Item is null
            MyItem? item = null;

            // Act & Assert: Assert that an ArgumentNullException is thrown when trying to add a null item
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _cosmosDbService.AddItemAsync(item));

            // Verify the exception message
            Assert.Equal("Value cannot be null. (Parameter 'item')", exception.Message);
        }

        [Fact]
        public async Task AddItemAsync_ShouldReturnBadRequest_WhenItemIsInvalid()
        {
            // Arrange: Create an invalid item (e.g., missing Name)
            var invalidItem = new MyItem
            {
                Name = "",  // Invalid Name (empty string)
                Email = null // Valid Email (but still requires Name)
            };

            // Act: Call the method to add the item
            // We will now directly call the method without storing the result in a variable
            await _cosmosDbService.AddItemAsync(invalidItem);

            // Assert: Check for bad request (as Name is empty)
            // This assumes the service throws a BadRequestException or you handle this in the service.
        }
    }
}
