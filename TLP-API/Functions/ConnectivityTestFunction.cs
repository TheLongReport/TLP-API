using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TLP_API.Models;
using TLP_API.Services;

namespace TLP_API.Functions
{
    public class ConnectivityTestFunction
    {
        private readonly ICosmosDbService _cosmosDbService;

        public ConnectivityTestFunction(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [Function("ConnectivityTest")]
        public async Task<HttpResponseData> Test(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "test-connectivity")] HttpRequestData req,
            FunctionContext context)
        {
            var logger = context.GetLogger("ConnectivityTest");
            logger.LogInformation("Starting connectivity test...");
            logger.LogInformation("Received a request to test connectivity.");

            try
            {
                var isConnected = await _cosmosDbService.TestConnectionAsync();
                logger.LogInformation($"Cosmos DB connectivity test result: {isConnected}");

                var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
                await response.WriteStringAsync($"Cosmos DB Connectivity: {isConnected}");
                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during connectivity test.");
                var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync($"Error: {ex.Message}");
                return errorResponse;
            }
        }
    }
}
