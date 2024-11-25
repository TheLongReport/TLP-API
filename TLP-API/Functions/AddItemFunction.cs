using Microsoft.Azure.Functions.Worker;                     // Required for Azure Functions Worker SDK
using Microsoft.Azure.Functions.Worker.Http;                // For HttpTrigger and HttpRequestData
using Microsoft.Extensions.Logging;                         // For logging within the function
using TLP_API.Models;                                       // For MyItem model
using TLP_API.Services;                                     // For ICosmosDbService
using System.Threading.Tasks;                               // For async/await
using Microsoft.AspNetCore.Mvc;                             // For action results like OkObjectResult, BadRequestObjectResult
using Newtonsoft.Json;                                      // For JSON serialization

namespace TLP_API.Functions
{
    public class AddItemFunction
    {
        private readonly ICosmosDbService _cosmosDbService;

        public AddItemFunction(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [Function("AddItem")]
        public async Task<HttpResponseData> AddItem(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "listing")] HttpRequestData req,
            FunctionContext context)
        {
            var logger = context.GetLogger("AddItem");
            logger.LogInformation("Start processing AddItem function.");

            // Deserialize the request body into MyItem
            logger.LogInformation("Attempting to deserialize request body.");
            var myItem = await req.ReadFromJsonAsync<MyItem>();

            if (myItem == null)
            {
                logger.LogWarning("Invalid data received.");
                var badResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                await badResponse.WriteStringAsync("Invalid data.");
                return badResponse;
            }

            try
            {
                logger.LogInformation("Attempting to add item to Cosmos DB.");
                // Add item to Cosmos DB
                await _cosmosDbService.AddItemAsync(myItem);
                logger.LogInformation("Item added successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error adding item to Cosmos DB: {ex.Message}");
                var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync("Failed to add item.");
                return errorResponse;
            }

            // Return successful response
            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteStringAsync("Item added successfully.");
            return response;
        }
    }
}
