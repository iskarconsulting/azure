using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace AzureFuncCosmosDBDemo.DomainModels
{
    // TODO Domain models can be moved to their own library / NuGet package so enable them to be used across the domain estate.
    public class User
    {
        [JsonProperty("userName")] // Newtonsoft library attribute (serialising to CosmosDB).
        [JsonPropertyName("userName")] // Microsoft JSON serialisation library attribute (deserialising from request body).
        public string UserName { get; set; }

        [JsonProperty("accounts")]
        [JsonPropertyName("accounts")]
        public IEnumerable<Account> Accounts { get; set; }

        // TODO Add validation / business logic.
    }
}