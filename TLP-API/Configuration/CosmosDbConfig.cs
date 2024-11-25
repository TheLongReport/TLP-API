using System;

namespace TLP_API.Configuration
{
    public class CosmosDbConfig
    {
        public string AccountEndpoint { get; private set; }
        public string AccountKey { get; private set; }
        public string DatabaseName { get; private set; }
        public string ContainerName { get; private set; }

        public CosmosDbConfig()
        {
            // Fetch and validate settings from environment variables
            AccountEndpoint = GetRequiredEnvVariable("COSMOS_DB_ENDPOINT");
            AccountKey = GetRequiredEnvVariable("COSMOS_DB_KEY");
            DatabaseName = GetRequiredEnvVariable("COSMOS_DB_NAME");
            ContainerName = GetRequiredEnvVariable("COSMOS_DB_CONTAINER");
        }

        private static string GetRequiredEnvVariable(string variableName)
        {
            var value = Environment.GetEnvironmentVariable(variableName);
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidOperationException($"Environment variable '{variableName}' is not set.");
            }
            return value;
        }
    }
}
