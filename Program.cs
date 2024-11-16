using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TLP_API.Models;
using TLP_API.Services;

var builder = WebApplication.CreateBuilder(args);

// Load Cosmos DB settings from configuration
var config = builder.Configuration.GetSection("CosmosDb");
var accountEndpoint = config["AccountEndpoint"];
var accountKey = config["AccountKey"];
var databaseName = config["DatabaseName"];
var containerName = config["ContainerName"];

// Validate that all required configuration values are present
if (string.IsNullOrWhiteSpace(accountEndpoint) ||
    string.IsNullOrWhiteSpace(accountKey) ||
    string.IsNullOrWhiteSpace(databaseName) ||
    string.IsNullOrWhiteSpace(containerName))
{
    throw new InvalidOperationException("Cosmos DB configuration is incomplete. Please check 'appsettings.json' or environment variables.");
}

// Add CosmosDbService to DI container
builder.Services.AddSingleton<ICosmosDbService>(_ =>
    new CosmosDbService(accountEndpoint, accountKey, databaseName, containerName));

var app = builder.Build();

// Basic test endpoint
app.MapGet("/", () => "Welcome to TLP-API!");

// API endpoint to add an item
app.MapPost("/addItem", async (MyItem myItem, ICosmosDbService cosmosDbService) =>
{
    if (myItem == null)
    {
        return Results.BadRequest("Invalid data.");
    }

    await cosmosDbService.AddItemAsync(myItem);
    return Results.Ok("Item added successfully.");
});

app.Run();
