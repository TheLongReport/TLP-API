namespace TLP_API.Configuration
{
    public class CosmosDbConfig
    {
        public string AccountEndpoint { get; set; }
        public string AccountKey { get; set; }
        public string DatabaseName { get; set; }
        public string ContainerName { get; set; }

        public CosmosDbConfig()
        {
            // Fetch settings from environment variables
            AccountEndpoint = System.Environment.GetEnvironmentVariable("COSMOS_DB_ENDPOINT")!;
            AccountKey = System.Environment.GetEnvironmentVariable("COSMOS_DB_KEY")!;
            DatabaseName = System.Environment.GetEnvironmentVariable("COSMOS_DB_NAME")!;
            ContainerName = System.Environment.GetEnvironmentVariable("COSMOS_DB_CONTAINER")!;

            // Optionally validate the settings
            if (string.IsNullOrEmpty(AccountEndpoint) || string.IsNullOrEmpty(AccountKey) ||
                string.IsNullOrEmpty(DatabaseName) || string.IsNullOrEmpty(ContainerName))
            {
                throw new InvalidOperationException("Cosmos DB configuration is incomplete.");
            }
        }
    }
}
