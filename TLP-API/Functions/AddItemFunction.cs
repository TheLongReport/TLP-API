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
            FunctionContext context,
            CancellationToken cancellationToken)
        {
            var logger = context.GetLogger("AddItem");

            logger.LogInformation("Received a request to add an item.");

            try
            {
                // Step 1: Deserialize the request body into MyItem
                logger.LogInformation("Deserializing request body.");
                var myItem = await req.ReadFromJsonAsync<MyItem>(cancellationToken);

                if (myItem == null)
                {
                    logger.LogWarning("Request body is null or invalid.");
                    var badResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    await badResponse.WriteStringAsync("Invalid data.", cancellationToken);
                    return badResponse;
                }

                // Step 2: Add item to Cosmos DB
                logger.LogInformation("Adding item to Cosmos DB.");
                await _cosmosDbService.AddItemAsync(myItem, cancellationToken);
                logger.LogInformation("Item added successfully.");

                // Step 3: Return a successful response
                var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
                await response.WriteStringAsync("Item added successfully.", cancellationToken);
                logger.LogInformation("Response sent successfully.");
                return response;
            }
            catch (TaskCanceledException)
            {
                logger.LogError("The request was canceled, likely due to a timeout.");
                var timeoutResponse = req.CreateResponse(System.Net.HttpStatusCode.RequestTimeout);
                await timeoutResponse.WriteStringAsync("Request timed out.", cancellationToken);
                return timeoutResponse;
            }
            catch (Exception ex)
            {
                logger.LogError($"An unexpected error occurred: {ex.Message}");
                var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync("An internal server error occurred.", cancellationToken);
                return errorResponse;
            }
        }
    }
}
