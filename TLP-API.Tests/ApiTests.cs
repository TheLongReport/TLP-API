using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TLP_API;  // Correct namespace
using Newtonsoft.Json;
using Xunit;
using TLP_API.Models;  // Ensure this is included to reference MyItem

namespace TLP_API.Tests
{
    public class ApiTests : IClassFixture<WebApplicationFactory<TLP_API.Program>>
    {
        private readonly WebApplicationFactory<TLP_API.Program> _factory;
        private readonly HttpClient _client;

        public ApiTests(WebApplicationFactory<TLP_API.Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task PostAddItem_ShouldReturnOk_WhenItemIsValid()
        {
            // Arrange
            var newItem = new MyItem
            {
                Name = "Test Item", // Required Name
                Email = "test@example.com" // Valid Email
            };

            var content = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/addItem", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task PostAddItem_ShouldReturnBadRequest_WhenItemIsInvalid()
        {
            // Arrange
            var invalidItem = new MyItem
            {
                Name = "", // Invalid (empty Name)
                Email = null // Valid, but still need Name
            };
            var content = new StringContent(JsonConvert.SerializeObject(invalidItem), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/addItem", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
