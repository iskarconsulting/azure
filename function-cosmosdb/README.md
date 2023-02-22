# Azure Function to CosmosDB Demo Project

## Introduction

This example project demonstrates a way to create a CosmosDB document from an HTTP POST request (with a route parameter and a JSON array in the body) to an Azure Function implementing C# domain model objects.

One of the requirements I had when considering this demo was to be able to write the JSON property keys to the CosmosDB document in camelCase. The Azure Functions CosmosDB output binding exposes the `IAsyncCollector<>` interface, however, this doesn’t expose functionality to control JSON formatting. In this demo I use a `DocumentClient` object instead.

I’ve used a very simple domain model to demonstrate deserialising the JSON array in the body of the HTTP POST request into domain objects. The deserialisation is achieved using the System.Text.Json.JsonSerializer that is included with .Net Core.

In a production application I’d recommend applying domain logic / validation to the domain objects. Otherwise they’ll just be an [anemic domain model](https://martinfowler.com/bliki/AnemicDomainModel.html).

Finally, the domain objects are serialised and written to a CosmosDB document. The CosmosDB DocumentClient uses the Newtonsoft serialiser.

## Prerequisites

### Azure Functions Extension for VS Code / Azure Functions Core Tools

https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-local

### Azure Storage for Functions

Azure Functions use Azure Storage.  If you're developing locally and aren't using a cloud storage account then you can use the AzureWebJobsStorage Emulator.

https://docs.microsoft.com/en-gb/azure/storage/common/storage-use-emulator

### CosmosDB

Configure your CosmosDB account, database and container.

Get the following values from your CosmosDB account / database and update the `local.settings.json` file.
- Account Endpoint
- Primary Access Key
- Database Id
- Container Id

## Testing

Start the project and copy the URL.

Open Postman (or other HTTP client of your choosing) and enter the URL (update the userName route parameter using URL encoding. For example:

`http://localhost:7071/api/users/Bob%20Smith/accounts`

Configure the body of the request to be RAW JSON and use something similar to the following (keep the key names as shown but feel free to change the values):

``` JSON
[
    {
        "id": 1,
        "type": "Cloud",
        "description": "Bob's Azure account."
    },
    {
        "id": 2,
        "type": "Local",
        "description": "Bob's PC account."
    }
]
```

Send the request.

Check your CosmosDB collection. A new document should have been inserted.

## References

https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs-code?tabs=csharp

https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-core-3-1
