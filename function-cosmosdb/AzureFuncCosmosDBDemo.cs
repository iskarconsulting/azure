using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AzureFuncCosmosDBDemo
{
    public static class AzureFuncCosmosDBDemo
    {
        // Recommend moving DocumentClient code to its own class so that it can be referenced by other functions in the FunctionApp.
        // Reference: https://docs.microsoft.com/en-us/azure/azure-functions/manage-connections#documentclient-code-example-c
        private static Lazy<DocumentClient> lazyClient = new Lazy<DocumentClient>(InitializeDocumentClient);
        internal static DocumentClient documentClient => lazyClient.Value;

        private static DocumentClient InitializeDocumentClient()
        {
            var uri = new Uri(Environment.GetEnvironmentVariable("CosmosDBAccountEndpoint"));
            var authKey = Environment.GetEnvironmentVariable("CosmosDBAccountKey");

            // The Newtonsoft serialiser used the CosmosDB DocumentClient can be customised with a JsonSerializerSettings object.
            var settings = new JsonSerializerSettings
            {
                // This is redundant as there are [JsonProperty("")] attributes applied to the domain object properties.
                // Included to illustrate how the Json serialiser can be configured.
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return new DocumentClient(uri, authKey, settings);
        }

        [FunctionName("AzureFuncCosmosDBDemo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "users/{userName}/accounts")] HttpRequest req,
            string userName,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Create the domain objects.
            // Use the deserialiser included in the .Net System.Text.Json library as this has a async methods which accept a stream (req.Body).
            // This removes the need to read the stream into a string and then deserialise the string.
            var user = new DomainModels.User
            {
                UserName = userName, // From the userName value in the route.
                Accounts = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<DomainModels.Account>>(req.Body)
            };

            // TODO Remove
            // Debug code to write out the object(s) to the log using the Newtonsoft deserialiser.
            log.LogInformation(JsonConvert.SerializeObject(user));

            // TODO Perform any domain validation here (once the request body has been deserialised into domain object(s)).

            Uri collectionUri = UriFactory.CreateDocumentCollectionUri(Environment.GetEnvironmentVariable("CosmosDBDatabaseId"), Environment.GetEnvironmentVariable("CosmosDBCollectionId"));
            await documentClient.CreateDocumentAsync(collectionUri, user);

            return new NoContentResult();
        }
    }
}
