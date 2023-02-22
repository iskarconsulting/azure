using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace AzureFuncCosmosDBDemo.DomainModels
{
    // TODO Domain models can be moved to their own library / NuGet package so enable them to be used across the domain estate.
    public class Account
    {
        [JsonProperty("id")] // Newtonsoft library attribute (serialising to CosmosDB).
        [JsonPropertyName("id")] // Microsoft JSON serialisation library attribute (deserialising from request body).
        public int Id {get; set;}

        [JsonProperty("type")]
        [JsonPropertyName("type")] //
        public string Type {get; set;}

        [JsonProperty("desc")]
        [JsonPropertyName("description")]
        public string Description {get; set;}

        // TODO Add validation / business logic.
    }
}