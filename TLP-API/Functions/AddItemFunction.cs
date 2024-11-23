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

        [Function("AddItem12131")]
        public async Task<HttpResponseData> AddItem(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "listing")] HttpRequestData req,
            FunctionContext context)
        {
            var logger = context.GetLogger("AddItem12131");

            // Deserialize the request body into MyItem
            var myItem = await req.ReadFromJsonAsync<MyItem>();

            if (myItem == null)
            {
                logger.LogWarning("Invalid data received.");
                var badResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                await badResponse.WriteStringAsync("Invalid data.");
                return badResponse;
            }

            // Add item to Cosmos DB
            await _cosmosDbService.AddItemAsync(myItem);
            logger.LogInformation("Item added successfully.");

            // Return successful response
            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteStringAsync("Item added successfully.");
            return response;
        }
    }
}
