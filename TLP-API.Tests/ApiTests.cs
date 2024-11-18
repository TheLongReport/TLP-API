using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TLP_API;
using TLP_API.Models;
using Newtonsoft.Json;
using Xunit;
using System.Net;

namespace TLP_API.Tests
{
    public class ApiTests
    {
        private readonly HttpClient _client;

        // HttpClient is setup in the constructor.
        public ApiTests()
        {
            // Create an HttpClient with the correct address for local testing
            _client = new HttpClient { BaseAddress = new Uri("http://localhost:7071") }; // Local Azure Functions runtime
        }

        [Fact]
        public async Task PostAddItem_ShouldReturnOk_WhenItemIsValid()
        {
            // Arrange
            var newItem = new MyItem
            {
                Name = "Test Item",
                Email = "test@example.com"
            };

            var content = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/listing", content); // Ensure route is correct

            // Assert
            response.EnsureSuccessStatusCode(); // Verifies that the status code is 2xx
        }

        [Fact]
        public async Task PostAddItem_ShouldReturnBadRequest_WhenItemIsInvalid()
        {
            // Arrange
            var invalidItem = new MyItem
            {
                Name = "", // Invalid name
                Email = null
            };

            var content = new StringContent(JsonConvert.SerializeObject(invalidItem), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/listing", content); // Ensure route is correct

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // Verifies that the status code is 400
        }
    }
}