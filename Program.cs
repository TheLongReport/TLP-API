using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers(); // Enables controller support
builder.Services.AddEndpointsApiExplorer(); // Enables API endpoint exploration
builder.Services.AddSwaggerGen(); // Enables Swagger for API documentation

// Optional: Configure authentication (if needed for future steps)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "<Your-Issuer-URL>"; // Replace with your JWT provider
        options.Audience = "<Your-Audience>";    // Replace with your API audience
    });

// Example: Add Azure Cosmos DB Service (commented if not yet implemented)
// builder.Services.AddSingleton(new CosmosDbService(
//     "<Your-CosmosDB-Connection-String>",
//     "<Your-Database-Name>",
//     "<Your-Container-Name>"
// ));

var app = builder.Build();

// Enable Swagger in development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware
app.UseHttpsRedirection(); // Redirect HTTP to HTTPS
app.UseAuthorization();    // Enables authorization middleware

// Define routes
app.MapGet("/", () => "Welcome to TLP-API!"); // Root URL response
app.MapGet("/hello", () => "Hello, World!");  // Example route
app.MapGet("/api/status", () => new { Status = "Running", Version = "1.0.0" }); // API status route

// Map controllers for additional API functionality
app.MapControllers();

// Run the application
app.Run();
