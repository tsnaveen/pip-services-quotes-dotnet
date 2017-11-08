# <img src="https://github.com/pip-services/pip-services/raw/master/design/Logo.png" alt="Pip.Services Logo" style="max-width:30%"> <br/> Quotes microservice

This is quotes microservice from Pip.Services library. 
It shows to users inspirational quotes on various topics.

The microservice currently supports the following deployment options:
* Deployment platforms: Standalone Process, Seneca
* External APIs: HTTP/REST, Seneca
* Persistence: Flat Files, MongoDB

This microservice has no dependencies on other microservices.

<a name="links"></a> Quick Links:

* [Download Links](doc/Downloads.md)
* [Development Guide](doc/Development.md)
* [Configuration Guide](doc/Configuration.md)
* [Deployment Guide](doc/Deployment.md)
* Client SDKs
  - [Node.js SDK](https://github.com/pip-services/pip-clients-quotes-node)
  - [Java SDK](https://github.com/pip-services/pip-clients-quotes-java)
  - [.NET SDK](https://github.com/pip-services/pip-clients-quotes-dotnet)
  - [Go SDK](https://github.com/pip-services/pip-clients-quotes-go)
* Communication Protocols
  - [HTTP Version 1](doc/HttpProtocolV1.md)
  - [Seneca Version 1](doc/SenecaProtocolV1.md)
  - [Lambda Version 1](doc/LambdaProtocolV1.md)

## Contract

Logical contract of the microservice is presented below. For physical implementation (HTTP/REST, Thrift, Seneca, Lambda, etc.),
please, refer to documentation of the specific protocol.

```cs
public class QuoteV1 : IStringIdentifiable
{
    public string Id { get; set; }
    public MultiString Text { get; set; }
    public MultiString Author { get; set; }
    public string Status { get; set; }
    public string[] Tags { get; set; }
    public string[] All_Tags { get; set; }
}

public class QuoteStatusV1
{
    public const string New = "new";
    public const string Writing = "writing";
    public const string Translating = "translating";
    public const string Verifying = "verifying";
    public const string Completed = "completed";
}

public interface IQuotes
{
    Task<DataPage<QuoteV1>> GetQuotesAsync(string correlationId, FilterParams filter, PagingParams paging);
    Task<QuoteV1> GetRandomQuoteAsync(string correlationId, FilterParams filter);
    Task<QuoteV1> GetQuoteByIdAsync(string correlationId, string quoteId);
    Task<QuoteV1> CreateQuoteAsync(string correlationId, QuoteV1 quote);
    Task<QuoteV1> UpdateQuoteAsync(string correlationId, QuoteV1 quote);
    Task<QuoteV1> DeleteQuoteByIdAsync(string correlationId, string quoteId);
}

```

## Download

Right now the only way to get the microservice is to check it out directly from github repository
```bash
git clone git@github.com:pip-services-content/pip-services-quotes-dotnet.git
```

Pip.Service team is working to implement packaging and make stable releases available for your 
as zip downloadable archieves.

## Run

Add **config.yml** file to the root of the microservice folder and set configuration parameters.
As the starting point you can use example configuration from **config.example.yml** file. 

Example of microservice configuration
```yaml
- descriptor: "pip-services-container:container-info:default:default:1.0"
  name: "pip-services-quotes"
  description: "Quotes microservice"

- descriptor: "pip-services-commons:logger:console:default:1.0"
  level: "trace"

- descriptor: "pip-services-quotes:persistence:file:default:1.0"
  path: "../data/quotes.json"

- descriptor: "pip-services-quotes:controller:default:default:1.0"

- descriptor: "pip-services-quotes:service:http:default:1.0"
  connection:
    protocol: "http"
    host: "0.0.0.0"
    port: 8080
```
 
For more information on the microservice configuration see [Configuration Guide](Configuration.md).

Start the microservice using the command:
```bash
cd run
dotnet run
```

## Use

The easiest way to work with the microservice is to use client SDK. 
The complete list of available client SDKs for different languages is listed in the [Quick Links](#links)

If you use .NET then get the reference to the required libraries:
- Pip.Services.Commons : https://github.com/pip-services/pip-services-commons-dotnet
- Pip.Services.Net : https://github.com/pip-services/pip-services-net-dotnet
- This .NET Client SDK for Quotes microservices: https://github.com/pip-services-content/pip-clients-quotes-dotnet 

Add **PipServices.Commons** and **PipServices.Quotes.Client.Version1** namespaces
```cs
using PipServices.Commons.Config;
using PipServices.Commons.Data;
using PipServices.Quotes.Client.Version1;
```

Define client configuration parameters that match configuration of the microservice external API
```cs
// Client configuration
var config = ConfigParams.FromTuples(
	"connection.type", "http",
	"connection.host", "localhost",
	"connection.port", 8080
);
```

Instantiate the client and open connection to the microservice
```cs
// Create the client instance
var client = new QuotesHttpClientV1();

// Connect to the microservice
client.OpenAsync(null);
    
// Work with the microservice
...
```

Now the client is ready to perform operations
```cs
// Create a new quote
var quote = new QuoteV1
{
	Text = new MultiString("Get in hurry slowly"),
	Author = new MultiString("Russian proverb"),
	Status = "completed"
};

client.CreateQuoteAsync(null, quote);
```

```cs
// Get the list of quotes on 'time management' topic
var quotePage = client.GetQuotesAsync(null,
    FilterParams.FromTuples(
        "search", "hurry",
        "status", "completed"
    ),
    new PagingParams(
        Skip =  0,
        Take = 10
    )
).Result;
```

## Acknowledgements

This microservice was created and currently maintained by *Sergey Seroukhov*.
