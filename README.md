# eBay Order Management SDK for .NET

Process and fulfill orders, buy shipping labels, and handle post-order cases. This SDK wraps the eBay Fulfillment API, Logistics API and eBay GraphQL behind one client, `OrderManagementClient`.

- **Target frameworks:** .NET Framework 4.7.2, .NET 8, .NET 9 and .NET 10
- **C#:** usable from C# 7.3 and later

## Installation

The package `EBay.OrderManagement` is not on nuget.org yet. Reference the project, or build the package and add it to a local feed:

```bash
dotnet pack EBay.OrderManagement -c Release -o artifacts/packages
```

## Quick start

```csharp
using EBay.OrderManagement;
using EBay.OrderManagement.Configuration;

var config = new OrderManagementConfiguration
{
    // A user or application access token from your own OAuth flow (see "Authentication").
    AccessToken = Environment.GetEnvironmentVariable("EBAY_ACCESS_TOKEN")
};

using var client = new OrderManagementClient(config);

var orders = await client.Fulfillment.GetOrdersAsync(limit: "10");
Console.WriteLine($"{orders?.Total} orders");
```

The GraphQL API is on the same client:

```csharp
using EBay.OrderManagement.Generated.GraphQL;

var to = DateTimeOffset.UtcNow;
var result = await client.GraphQL.OrdersAsync(new OrdersInput
{
    OrdersFirstPage = new OrdersFirstPageInput
    {
        MaxPageSize = 10,
        Filter = new OrdersFirstFilterInput
        {
            DateRange = new OrdersDateRangeInput
            {
                DateType = OrdersDateType.Confirmed,
                DateTimeRange = new DateTimeRangeInput { StartDateTime = to.AddDays(-30), EndDateTime = to }
            }
        }
    }
});

foreach (var order in result?.SellerOrders.Orders ?? [])
{
    // GraphQL unions surface as interfaces: test for the concrete type.
    var state = order.OrderState is IOrders_Orders_SellerOrders_Orders_OrderState_OrderConfirmed confirmed
        ? confirmed.Label
        : order.OrderState.__typename;
    Console.WriteLine($"{order.OrderId}: {state}");
}
```

## APIs

| `OrderManagementClient` property | API | Default base URL | Access token |
|---|---|---|---|
| `Fulfillment` | Fulfillment API | `https://apiz.ebay.com/sell/fulfillment/v1` | User (some operations: application) |
| `Logistics` | Logistics API | `https://api.ebay.com/sell/logistics/v1_beta` | User |
| `GraphQL` | eBay GraphQL | `https://api.ebay.com/graphql` | See eBay's GraphQL docs |

"Access token" is what eBay's OpenAPI spec declares: an application token (client credentials) or a user token (authorization code). Scopes are listed per API below.

<details>
<summary><b>Fulfillment</b>: Fulfillment API, 16 operations</summary>

Scopes: `eBayUser`, `https://api.ebay.com/oauth/api_scope/sell.finances`, `https://api.ebay.com/oauth/api_scope/sell.fulfillment`, `https://api.ebay.com/oauth/api_scope/sell.fulfillment.readonly`, `https://api.ebay.com/oauth/api_scope/sell.payment.dispute`

| Method | Description |
|---|---|
| `GetOrderAsync` | Get a seller order by ID |
| `GetOrdersAsync` | Search for seller orders |
| `IssueRefundAsync` | Issues a full or partial refund to a buyer on behalf of the seller. |
| `GetShippingFulfillmentsAsync` | Get all shipping fulfillments for an order |
| `CreateShippingFulfillmentAsync` | Create a shipping fulfillment for an order |
| `GetShippingFulfillmentAsync` | Get a specific shipping fulfillment |
| `GetPaymentDisputeAsync` | Get Payment Dispute Details |
| `FetchEvidenceContentAsync` | Get Payment Dispute Evidence File |
| `GetActivitiesAsync` | Get Payment Dispute Activity |
| `GetPaymentDisputeSummariesAsync` | Search Payment Dispute by Filters |
| `ContestPaymentDisputeAsync` | Contest Payment Dispute |
| `AcceptPaymentDisputeAsync` | Accept Payment Dispute |
| `UploadEvidenceFileAsync` | Upload an Evidence File |
| `AddEvidenceAsync` | Add an Evidence File |
| `UpdateEvidenceAsync` | Update evidence |
| `CreateAdditionalShippingFulfillmentAsync` | Create an additional shipping fulfillment for an order |

</details>

<details>
<summary><b>Logistics</b>: Logistics API, 6 operations</summary>

Scopes: `https://api.ebay.com/oauth/api_scope/sell.logistics`

> **Note:** The Logistics API only supports USPS shipping rates and labels.

| Method | Description |
|---|---|
| `CancelShipmentAsync` | This method cancels the shipment associated with the specified shipment ID and the associated shipping label is deleted. |
| `CreateFromShippingQuoteAsync` | This method creates a shipment based on the shippingQuoteId and rateId values supplied in the request. |
| `DownloadLabelFileAsync` | This method returns the shipping label file that was generated for the shipmentId value specified in the request. |
| `GetShipmentAsync` | This method retrieves the shipment details for the specified shipment ID. |
| `CreateShippingQuoteAsync` | The createShippingQuote method returns a shipping quote that contains a list of live "rates." |
| `GetShippingQuoteAsync` | This method retrieves the complete details of the shipping quote associated with the specified shippingQuoteId value. |

</details>

<details>
<summary><b>GraphQL</b>: eBay GraphQL, 52 operations</summary>

Scopes (any operation may need any of these): `https://api.ebay.com/oauth/api_scope/buy.commitment`, `https://api.ebay.com/oauth/api_scope/buy.purchase.quote`, `https://api.ebay.com/oauth/api_scope/sell.cancellation`, `https://api.ebay.com/oauth/api_scope/sell.commitment`, `https://api.ebay.com/oauth/api_scope/sell.inquiry`, `https://api.ebay.com/oauth/api_scope/sell.order`, `https://api.ebay.com/oauth/api_scope/sell.purchase.quote`, `https://api.ebay.com/oauth/api_scope/sell.return`, `https://api.ebay.com/oauth/api_scope/sell.return.preference`

| Method | Description |
|---|---|
| `AppendSellerReturnResponseRuleAsync` | The **appendSellerReturnResponseRule** mutation adds a new return-respond automation rule. |
| `AppendSellerReturnToAddressRuleAsync` | The **appendSellerReturnToAddressRule** mutation adds a new return-to-address automation rule. |
| `ApproveContractCancellationAsync` | The **approveContractCancellation** mutation approves a contract cancellation using an existing cancellation entity. |
| `ApproveReturnAsync` | The **approveReturn** mutation allows a seller to approve a return request. |
| `AttachReturnShipmentLabelAsync` | The **attachReturnShipmentLabel** mutation attaches a return shipment label to the specified return. |
| `BulkUpdateItemCommitmentTagsAsync` | This mutation allows a seller to update the acknowledgment status of up to 100 item commitments per operation. |
| `BulkUpdateOrderTagsAsync` | The **bulkUpdateOrderTags** mutation allows a seller to update the acknowledgment status for up to 100 orders in a single operation. |
| `BulkUpdatePurchaseQuotesAsync` | This mutation allows a seller to update up to 100 existing purchase quotes in a single request. |
| `CommitToPurchaseAsync` | This mutation is used to create an item commitment on a listing, where a buyer commits to purchase an item and pay for it at a later date. |
| `ContractCancellationAsync` | The **contractCancellation** query retrieves details about a contract cancellation by its ID. |
| `CreateItemCommitmentCancellationAsync` | The **createItemCommitmentCancellation** mutation creates a cancellation on an item commitment. |
| `CreateOrderCancellationAsync` | The **createOrderCancellation** mutation creates a cancellation on an order. |
| `CreatePurchaseQuoteAsync` | This mutation allows a seller to create a new purchase quote containing multiple item commitments with price details and optional shipping estimates. |
| `CreatePurchaseQuoteCancellationAsync` | The **createPurchaseQuoteCancellation** mutation creates a cancellation on a purchase quote for item commitments which are part of a **PurchaseQuote**. |
| `DeclineContractCancellationAsync` | The **declineContractCancellation** mutation declines a contract cancellation using an existing cancellation entity. |
| `DeclineReturnAsync` | The **declineReturn** mutation declines a return request and can include a message. |
| `DeleteSellerReturnRuleAsync` | The **deleteSellerReturnRule** mutation allows the seller to delete the specified existing return automation rule. |
| `ItemCommitmentsAsync` | This query can be used to retrieve item commitments associated with a user. |
| `ItemCommitmentsByIdsAsync` | This query is used to retrieve detailed information on up to 100 of an authenticated user's item commitments. |
| `MarkItemCommitmentsPaidAsync` | This mutation allows a seller to mark up to 100 item commitments as paid. |
| `MarkOrdersPaidAsync` | This mutation can be used to mark up to 100 specified orders as payment received and is available only for the authorized user (the seller of the orders). |
| `OrderInquiryAsync` | This query can be used to retrieve full inquiry details for an inquiry, specified by its inquiry ID. |
| `OrderLineItemsByIdsAsync` | This query can be used to retrieve one or more line items from orders using their unique IDs. |
| `OrderReturnAsync` | The **orderReturn** query retrieves information on a specific return by return ID. |
| `OrdersAsync` | This query can be used to retrieve detailed information about one or more of a user's orders. |
| `OrdersByIdsAsync` | This query can be used to retrieve one or more orders by their unique IDs. |
| `ProcessOrderInquiryRefundAsync` | This mutation can be used to issue a full refund for an inquiry. |
| `ProcessReturnRefundAsync` | The **processReturnRefund** mutation processes a refund for a return in accordance with the applicable refund policy. |
| `ProposeKeepItemWithPartialRefundAsync` | The **proposeKeepItemWithPartialRefund** mutation proposes a keep-item-with-partial-refund offer on one or more items in an existing return. |
| `ProvideRmaNumberAsync` | The **provideRmaNumber** mutation supplies the Return Merchandise Authorization (RMA) number for a return. |
| `ProvideShipmentInfoForItemNotReceivedInquiryAsync` | This mutation can be used to provide shipment-related information in response to an Item Not Received inquiry. |
| `PurchaseQuoteLineItemsByIdsAsync` | This query is used to retrieve detailed information on up to 100 of an authenticated user's purchase quote line items. |
| `PurchaseQuotesAsync` | This query is used to search for and retrieve detailed information on multiple purchase quotes. |
| `PurchaseQuotesByIdsAsync` | This query is used to retrieve detailed information on up to 100 of an authenticated user's purchase quotes using provided purchase quote identifiers. |
| `RecordReturnLabelSentOfflineAsync` | The **recordReturnLabelSentOffline** mutation records that a return shipment label has been sent to the buyer for the specified return |
| `RecordReturnReceivedAsync` | The **recordReturnReceived** mutation is an optional step to notify eBay that the returned item has been received. |
| `RequestPurchaseQuoteAsync` | This mutation allows a buyer to submit a new purchase quote request to the seller. |
| `SellerContractCancellationsAsync` | The **sellerContractCancellations** query retrieves a list of contract cancellations with optional filtering. |
| `SellerOrderInquiriesAsync` | This query can be used to retrieve a paginated list of order inquiries visible to the seller. |
| `SellerOrderReturnsAsync` | The **sellerOrderReturns** query retrieves a list of order returns for the seller, with filtering and sorting options. |
| `SellerReturnPreferenceAsync` | The **sellerReturnPreference** query retrieves the return preferences set by a seller on a specified marketplace. |
| `SendOrderInquiryMessageAsync` | This mutation can be used to send a message within the context of a specific order inquiry. |
| `SendReturnMessageAsync` | The **sendReturnMessage** mutation sends a message to a return conversation. |
| `SubmitOrderInquiryReferralAsync` | This mutation can be used to submit an order inquiry to eBay as a referral request. |
| `SubmitReturnReferralAsync` | The **submitReturnReferral** mutation submits an order return to eBay as a referral request. |
| `UpdateSellerReturnResponseRuleConditionAsync` | The **updateSellerReturnResponseRuleCondition** mutation updates a seller's return automation rule. |
| `UpdateSellerReturnResponseRulesPrioritiesAsync` | The **updateSellerReturnResponseRulesPriorities** mutation updates the priority order of existing return-respond automation rules. |
| `UpdateSellerReturnRuleStatusAsync` | The **updateSellerReturnRuleStatus** mutation is used to enable or disable an automation return rule which determines how returns are processed based on their defined conditions. |
| `UpdateSellerReturnToAddressRuleConditionAsync` | The **updateSellerReturnToAddressRuleCondition** mutation updates a seller's return automation rule. |
| `UpdateSellerReturnToAddressRulesPrioritiesAsync` | The **updateSellerReturnToAddressRulesPriorities** mutation updates the priority order of existing return-to-address automation rules. |
| `UpdateSellerRmaPreferenceAsync` | The **updateSellerRmaPreference** mutation updates the seller's Return Merchandise Authorization (RMA) preference. |
| `ExecuteRawUnsafeAsync` | Runs any GraphQL query or mutation and returns the raw JSON response. |

</details>

## GraphQL

`client.GraphQL` has a typed method per GraphQL operation the SDK ships (see the table above). Inputs are classes with settable properties; only the properties you set are sent, so leaving a property unset is different from setting it to `null`. Results are interfaces named after their path in the query, and GraphQL unions surface as one interface per possible type: use `is`/`as` to pick the concrete type, as in the quick start.

For anything else, `ExecuteRawUnsafeAsync` sends any query and returns the response as a `JsonDocument`:

```csharp
using var document = await client.GraphQL.ExecuteRawUnsafeAsync("query { __typename }");
Console.WriteLine(document.RootElement.GetRawText());
```

## Authentication

The SDK does not obtain or refresh tokens: get them with your own OAuth flow ([eBay OAuth guide](https://developer.ebay.com/develop/guides/sell/authorization)) and give them to the SDK in one of two ways:

- `OrderManagementConfiguration.AccessToken`: used for every call that doesn't pass its own token.
- The `accessToken` parameter on every method: used for that call only, for example a user token where the configuration holds an application token.

```csharp
string userToken = Environment.GetEnvironmentVariable("EBAY_USER_TOKEN")!;
var orders = await client.Fulfillment.GetOrdersAsync(limit: "10", accessToken: userToken);
```

A token is only ever sent to trusted hosts, see below.

## Configuration

| Property | Type | Default | Description |
|---|---|---|---|
| `FulfillmentBaseUrl` | `string` | `https://apiz.ebay.com/sell/fulfillment/v1` | Base URL of the Fulfillment API. |
| `LogisticsBaseUrl` | `string` | `https://api.ebay.com/sell/logistics/v1_beta` | Base URL of the Logistics API. |
| `GraphQLBaseUrl` | `string` | `https://api.ebay.com/graphql` | Base URL of the GraphQL endpoint. |
| `AccessToken` | `string?` |  | Fallback bearer token used when no per-call accessToken is supplied. |
| `DiagnosticLogging` | `bool` | `false` | When true, logs the HTTP method, URL, status code, and response reason phrase for all non-success responses to stderr. |
| `HttpTimeout` | `TimeSpan` | 30 seconds | Timeout applied to all HTTP requests. |
| `DefaultRequestOptions` | `RequestOptions` | no headers | Default headers applied to every REST request. |
| `AllowedHosts` | `IList<string>` | empty | Additional hosts trusted with the access token, for example a proxy (see below). |

`OrderManagementConfiguration.DefaultAllowedHosts` lists the hosts trusted by default.

## Sandbox and trusted hosts

The access token is only sent to trusted hosts: the hosts in `OrderManagementConfiguration.DefaultAllowedHosts`, which are the eBay production hosts and their `*.sandbox.ebay.com` counterparts, plus any in `AllowedHosts`. Switching to the sandbox needs only the base URLs:

```csharp
var sandboxConfig = new OrderManagementConfiguration
{
    AccessToken = Environment.GetEnvironmentVariable("EBAY_SANDBOX_ACCESS_TOKEN"),
    FulfillmentBaseUrl = "https://apiz.sandbox.ebay.com/sell/fulfillment/v1",
    LogisticsBaseUrl = "https://api.sandbox.ebay.com/sell/logistics/v1_beta",
    GraphQLBaseUrl = "https://graphqlapi.sandbox.ebay.com/graphql",
};
```

Any other host, for example a proxy that forwards to eBay, has to be added to `AllowedHosts`; the client constructor throws `InvalidOperationException` otherwise:

```csharp
var proxied = new OrderManagementConfiguration
{
    FulfillmentBaseUrl = "https://ebay-proxy.example.com/sell/fulfillment/v1",
    AllowedHosts = { "ebay-proxy.example.com" },
};
```

## Dependency injection

`AddOrderManagementSdk` registers the configuration and one shared `OrderManagementClient` as a singleton. It owns its HTTP connections and is disposed with the container. Inject the client and use its properties for the individual APIs:

```csharp
using EBay.OrderManagement;
using EBay.OrderManagement.Configuration;
using EBay.OrderManagement.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOrderManagementSdk(new OrderManagementConfiguration
{
    AccessToken = builder.Configuration["EBay:AccessToken"]
});

var app = builder.Build();
app.MapGet("/example", async (OrderManagementClient client) =>
{
    var orders = await client.Fulfillment.GetOrdersAsync(limit: "10");
    return Results.Ok();
});
app.Run();
```

## Custom HTTP handler

To send requests through your own handler, for example for a proxy, client certificates or a test double, pass it to the constructor. The handler is shared by all APIs and is not disposed by the client:

```csharp
var handler = new HttpClientHandler { Proxy = new WebProxy("http://corporate-proxy:8080") };
using var proxiedClient = new OrderManagementClient(config, handler);
```

## Error handling

```csharp
try
{
    var orders = await client.Fulfillment.GetOrdersAsync(limit: "10");
}
catch (UnauthorizedException)
{
    // HTTP 401: the token is missing, expired, for the other environment, or lacks a scope.
}
catch (ApiException ex)
{
    // Any other REST error.
    Console.WriteLine($"HTTP {ex.ResponseStatusCode}");
}
catch (GraphQLException ex)
{
    // GraphQL "errors" in the response.
    Console.WriteLine(string.Join("; ", ex.Errors));
}
```

- `UnauthorizedException` (HTTP 401) and `GraphQLException` derive from `SdkException`.
- Other REST errors throw Kiota's `ApiException`, or a generated subclass named in eBay's spec for that operation, with `ResponseStatusCode`. eBay's error text is only available where the spec describes the error body correctly; set `DiagnosticLogging = true` to log failed requests to stderr.

## .NET Framework

The `net472` build is a classic package that `packages.config` projects can use. Such projects must list every dependency themselves. At the minimum versions the SDK supports, these are:

| Package | Version |
|---|---|
| Microsoft.Kiota.Abstractions, .Bundle, .Http.HttpClientLibrary, .Serialization.Form, .Serialization.Json, .Serialization.Multipart, .Serialization.Text | 1.22.0 |
| Std.UriTemplate | 2.0.8 |
| Microsoft.IO.RecyclableMemoryStream | 3.0.1 |
| Microsoft.Extensions.DependencyInjection.Abstractions | 6.0.0 |
| System.Diagnostics.DiagnosticSource, System.Net.Http.WinHttpHandler | 6.0.0 |
| System.Text.Json, System.Text.Encodings.Web, System.IO.Pipelines, Microsoft.Bcl.AsyncInterfaces | 10.0.0 |
| System.Runtime.CompilerServices.Unsafe | 6.1.2 |
| System.Memory, System.Threading.Tasks.Extensions | 4.6.3 |
| System.Buffers, System.Numerics.Vectors, System.ValueTuple | 4.6.1 |

With these versions the app needs binding redirects for Microsoft.Bcl.AsyncInterfaces (10.0.0.0), System.Buffers (4.0.5.0), System.Memory (4.0.5.0), System.Runtime.CompilerServices.Unsafe (6.0.3.0), System.Text.Encodings.Web (10.0.0.0), System.Text.Json (10.0.0.0) and System.Threading.Tasks.Extensions (4.2.4.0). There must be no redirect for System.ValueTuple: .NET Framework 4.7.1+ provides it. Newer package versions work too and change the redirect versions.

## Development

- .NET 10 SDK, plus the .NET 8 and .NET 9 SDKs so every target framework builds. `net472` builds on any OS; its tests run on Windows only.
- Build: `dotnet build EBay.OrderManagement`
- Test: `dotnet test tests/EBay.OrderManagement.Tests`
- Most of this repository is generated from eBay's API specs by the SDK generator (`api-sdk-definitions`): the API clients, `Configuration/`, `Extensions/`, `OrderManagementClient.cs`, `specs/` and `generate.sh`. Change the generator, not the generated files. `./generate.sh` reruns Kiota for the specs in `specs/openapi`.

See [CONTRIBUTING.adoc](CONTRIBUTING.adoc).

## License

Apache 2.0, see [LICENSE](LICENSE).
