# Order Management .NET SDK

A production-ready .NET SDK for eBay Order Management APIs, combining REST and GraphQL clients with a unified facade.

## Features

- **REST API Support**: Fulfillment and Logistics APIs generated with Microsoft Kiota
- **GraphQL Support**: Typed operations for orders, cancellations, returns, and more
- **OAuth Authentication**: Token refresh handler interface for seamless authentication
- **Dependency Injection**: First-class support for ASP.NET Core DI
- **Strong Typing**: Internal generated types with public abstraction layer
- **Target frameworks**: .NET Framework 4.7.2, .NET 8, .NET 9 and .NET 10

## Architecture

```
OrderManagementSdk/
├── Generated/              # Kiota-generated REST clients (internal)
│   ├── Fulfillment/
│   └── Logistics/
├── Clients/
│   ├── Rest/              # REST client wrappers (public API)
│   │   ├── FulfillmentClient.cs
│   │   └── LogisticsClient.cs
│   └── GraphQL/           # GraphQL clients
│       ├── OrderGraphQLClient.cs      # Typed operations
│       ├── DynamicGraphQLClient.cs    # Escape hatch
│       └── Generated/                 # Generated GraphQL types and operations
├── Configuration/         # SDK configuration and auth
├── Models/               # Public models
├── Exceptions/           # SDK exceptions
└── OrderManagementClient.cs  # Main SDK facade
```

## Installation

```bash
# From source
dotnet pack OrderManagementSdk/OrderManagementSdk.csproj
dotnet add reference path/to/OrderManagementSdk.1.0.0.nupkg

# Or add project reference
dotnet add reference OrderManagementSdk/OrderManagementSdk.csproj
```

## Quick Start

### 1. Implement Token Refresh Handler

```csharp
using OrderManagement.Configuration;

public class MyTokenRefreshHandler : ITokenRefreshHandler
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private string? _accessToken;
    private DateTime _tokenExpiry;

    public MyTokenRefreshHandler(string clientId, string clientSecret)
    {
        _clientId = clientId;
        _clientSecret = clientSecret;
    }

    public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        if (_accessToken != null && DateTime.UtcNow < _tokenExpiry.AddMinutes(-5))
        {
            return _accessToken;
        }

        // Refresh token using eBay OAuth API
        using var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.ebay.com/identity/v1/oauth2/token");
        request.Headers.Authorization = new AuthenticationHeaderValue(
            "Basic",
            Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}")));
        
        request.Content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("scope", "https://api.ebay.com/oauth/api_scope https://api.ebay.com/oauth/api_scope/sell.fulfillment")
        });

        var response = await client.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(json);

        _accessToken = tokenResponse.AccessToken;
        _tokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn);

        return _accessToken;
    }
}
```

### 2. Configure and Use the SDK

```csharp
using OrderManagement;
using OrderManagement.Configuration;

// Create configuration
var config = new OrderManagementConfiguration
{
    TokenRefreshHandler = new MyTokenRefreshHandler(clientId, secret),
    FulfillmentBaseUrl = "https://api.ebay.com/sell/fulfillment/v1",
    LogisticsBaseUrl = "https://api.ebay.com/sell/logistics/v1",
    GraphQLBaseUrl = "https://api.ebay.com/graphql"
};

// Create SDK client
using var client = new OrderManagementClient(config);

// Use REST APIs
var order = await client.Fulfillment.GetOrderAsync("12345");
var orders = await client.Fulfillment.GetOrdersAsync(
    filter: "orderfulfillmentstatus:{NOT_STARTED|IN_PROGRESS}",
    limit: 50);

var shipment = await client.Logistics.GetShipmentAsync("shipment-123");

// Use GraphQL APIs
var ordersResult = await client.GraphQL.GetOrdersAsync(
    new OrdersInput
    {
        Filter = new OrderFilter { OrderStatus = "ACTIVE" },
        Pagination = new PaginationInput { Limit = 20 }
    });

// Use GraphQL escape hatch
var rawResult = await client.GraphQL.ExecuteRawAsync(@"
    query {
      orders(input: {}) {
        orders {
          orderId
          orderStatus
        }
      }
    }");
```

### 3. Dependency Injection (ASP.NET Core)

`AddOrderManagementSdk` registers the configuration and one shared `OrderManagementClient` (a singleton that owns its HTTP connections and is disposed with the container). Inject the client and use its properties for the individual APIs:

```csharp
using EBay.OrderManagement;
using EBay.OrderManagement.Configuration;
using EBay.OrderManagement.Extensions;

builder.Services.AddOrderManagementSdk(new OrderManagementConfiguration());

public class OrdersController : ControllerBase
{
    private readonly OrderManagementClient _sdk;

    public OrdersController(OrderManagementClient sdk) => _sdk = sdk;

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrder(string orderId, [FromHeader] string authorization)
    {
        var order = await _sdk.Fulfillment.GetOrderAsync(orderId, accessToken: authorization["Bearer ".Length..]);
        return Ok(order);
    }
}
```

To route requests through your own handler (proxy, client certificates, a test double), pass it to the constructor: `new OrderManagementClient(config, handler)`. The handler is shared by all APIs and is not disposed by the client.

## REST API Examples

### Fulfillment API

```csharp
// Get a specific order
var order = await client.Fulfillment.GetOrderAsync("12345");

// Get orders with filter
var orders = await client.Fulfillment.GetOrdersAsync(
    filter: "orderfulfillmentstatus:{NOT_STARTED}",
    limit: 100,
    offset: 0);

// Get payment dispute
var dispute = await client.Fulfillment.GetPaymentDisputeAsync("dispute-123");

// Get payment dispute summaries
var summaries = await client.Fulfillment.GetPaymentDisputeSummariesAsync();

// Get shipping fulfillments for an order
var fulfillments = await client.Fulfillment.GetShippingFulfillmentsAsync("order-123");
```

### Logistics API

```csharp
// Get shipping quote
var quote = await client.Logistics.GetShippingQuoteAsync("quote-123");

// Get shipment
var shipment = await client.Logistics.GetShipmentAsync("shipment-123");
```

## GraphQL API Examples

### Typed Operations

```csharp
using OrderManagement.Models.GraphQL;

// Get orders with filtering and pagination
var result = await client.GraphQL.GetOrdersAsync(
    new OrdersInput
    {
        Filter = new OrderFilter
        {
            OrderStatus = "ACTIVE",
            CreationDateRange = "2024-01-01T00:00:00Z..2024-12-31T23:59:59Z"
        },
        Pagination = new PaginationInput
        {
            Limit = 50,
            Offset = 0
        }
    });

if (result != null)
{
    Console.WriteLine($"Total orders: {result.PaginationOutput?.Total}");
    
    foreach (var order in result.Orders)
    {
        Console.WriteLine($"Order {order.OrderId}: {order.OrderStatus}");
        Console.WriteLine($"Buyer: {order.Buyer?.Username}");
        Console.WriteLine($"Total: {order.PricingSummary?.Total?.Value} " +
                          $"{order.PricingSummary?.Total?.Currency}");
        
        foreach (var item in order.LineItems)
        {
            Console.WriteLine($"  - {item.Title} x{item.Quantity}");
        }
    }
}
```

### Dynamic Queries (Escape Hatch)

```csharp
// Execute arbitrary GraphQL query
var result = await client.GraphQL.ExecuteRawAsync(@"
    query GetOrderDetails($orderId: ID!) {
      ordersByIds(input: { orderIds: [$orderId] }) {
        orderId
        orderStatus
        lineItems {
          lineItemId
          title
          quantity
          lineItemCost {
            value
            currency
          }
        }
      }
    }",
    variables: new { orderId = "12345" });

// Parse the JSON response
var data = result.RootElement.GetProperty("data");
// ... process the response
```

## Supported Operations

### Fulfillment API (REST)
- Order Management: `getOrder`, `getOrders`
- Payment Disputes: `getPaymentDispute`, `getPaymentDisputeSummaries`
- Shipping Fulfillments: `getShippingFulfillments`

### Logistics API (REST)
- Shipping Quotes: `createShippingQuote`, `getShippingQuote`
- Shipments: `getShipment`, `cancelShipment`

### GraphQL Operations
- **Orders**: `orders`, `ordersByIds`, `markOrdersPaid`
- **Cancellations**: `sellerContractCancellations`, `createOrderCancellation`, `approveContractCancellation`
- **Returns**: `orderReturn`, `approveReturn`, `processReturnRefund`
- **Item Commitments**: `itemCommitments`, `itemCommitmentsByIds`

## Regenerating the SDK

When OpenAPI specs or GraphQL schemas change:

```bash
# Run the regeneration script
./generate.sh

# Or manually:
dotnet tool restore
dotnet kiota generate -l csharp -d specs/openapi/fulfillment.yaml ...
dotnet build OrderManagementSdk/OrderManagementSdk.csproj
```

## Error Handling

```csharp
using OrderManagement.Exceptions;

try
{
    var result = await client.GraphQL.GetOrdersAsync(input);
}
catch (GraphQLException ex)
{
    foreach (var error in ex.Errors)
    {
        Console.WriteLine($"GraphQL Error: {error.Message}");
        if (error.Path != null)
        {
            Console.WriteLine($"Path: {string.Join(" > ", error.Path)}");
        }
    }
}
catch (SdkException ex)
{
    Console.WriteLine($"SDK Error: {ex.Message}");
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"HTTP Error: {ex.Message}");
}
```

## Configuration Options

| Property | Default | Description |
|----------|---------|-------------|
| `FulfillmentBaseUrl` | `https://api.ebay.com/sell/fulfillment/v1` | Fulfillment API base URL |
| `LogisticsBaseUrl` | `https://api.ebay.com/sell/logistics/v1` | Logistics API base URL |
| `GraphQLBaseUrl` | `https://api.ebay.com/graphql` | GraphQL API endpoint |
| `TokenRefreshHandler` | (required) | OAuth token refresh handler |

## Project Structure

- **Generated/**: Kiota-generated REST clients (internal, committed to git)
- **Clients/Rest/**: Hand-crafted wrappers exposing public API
- **Clients/GraphQL/**: GraphQL typed client + dynamic escape hatch
- **Configuration/**: SDK config, auth providers
- **Models/**: Public models and GraphQL types
- **Exceptions/**: SDK exceptions
- **Extensions/**: DI extension methods

## Development

### Prerequisites

- .NET 10 SDK, plus the .NET 8 and .NET 9 SDKs so every target framework can build
- `net472` builds on any OS (reference assemblies come from NuGet); its tests run on Windows only
- Kiota CLI tool (auto-installed via dotnet tools)

### Building

```bash
dotnet build OrderManagementSdk/OrderManagementSdk.csproj
```

### Packaging

```bash
dotnet pack OrderManagementSdk/OrderManagementSdk.csproj
```

## Dependencies

- **Microsoft.Kiota.Bundle**: REST client generation and runtime
- **Microsoft.Extensions.DependencyInjection.Abstractions**: `AddOrderManagementSdk` registration
- **System.Text.Json** (.NET Framework only): GraphQL request and response serialization

## License

[Apache 2.0](LICENSE) © 2026 eBay Inc.

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Run `./generate.sh` if specs changed
5. Ensure `dotnet build` succeeds
6. Submit a pull request
