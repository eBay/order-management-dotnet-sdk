using System.Net;
using System.Text.Json;
using EBay.OrderManagement.Configuration;
using EBay.OrderManagement.Exceptions;
using EBay.OrderManagement.Generated.GraphQL;
using Xunit;

namespace EBay.OrderManagement.Tests;

public sealed class GraphQLClientTests
{
    private const string OrderReturnResponse = """
        {"data":{"approveReturn":{"__typename":"OrderReturn","returnId":"r-1",
          "status":{"__typename":"ReturnOpen","code":"OPEN","displayText":"Open"}}}}
        """;

    [Fact]
    public async Task Typed_operation_sends_document_and_parses_union_result()
    {
        var sdk = new TestSdk { Handler = { ResponseBody = OrderReturnResponse } };

        var result = await sdk.Client.GraphQL.ApproveReturnAsync(new ApproveReturnInput { ReturnId = "r-1" });

        var orderReturn = Assert.IsAssignableFrom<IApproveReturn_ApproveReturn_OrderReturn>(result);
        Assert.Equal("r-1", orderReturn.ReturnId);
        Assert.IsAssignableFrom<IApproveReturn_ApproveReturn_Status_ReturnOpen>(orderReturn.Status);
        Assert.Equal("Open", orderReturn.Status.DisplayText);

        var request = sdk.Handler.Request!.RootElement;
        Assert.Equal("ApproveReturn", request.GetProperty("operationName").GetString());
        Assert.StartsWith("mutation ApproveReturn(", request.GetProperty("query").GetString());
        Assert.Equal("r-1", request.GetProperty("variables").GetProperty("input").GetProperty("returnId").GetString());
        Assert.Equal("Bearer config-token", sdk.Handler.Authorization);
    }

    [Fact]
    public async Task Per_call_access_token_overrides_configuration()
    {
        var sdk = new TestSdk { Handler = { ResponseBody = OrderReturnResponse } };

        await sdk.Client.GraphQL.ApproveReturnAsync(new ApproveReturnInput { ReturnId = "r-1" }, accessToken: "call-token");

        Assert.Equal("Bearer call-token", sdk.Handler.Authorization);
    }

    [Fact]
    public async Task Unset_input_fields_are_omitted_and_explicit_null_is_sent()
    {
        var sdk = new TestSdk { Handler = { ResponseBody = OrderReturnResponse } };

        await sdk.Client.GraphQL.ApproveReturnAsync(new ApproveReturnInput { ReturnId = "r-1" });
        Assert.False(Input(sdk).TryGetProperty("approvalNote", out _));

        await sdk.Client.GraphQL.ApproveReturnAsync(new ApproveReturnInput { ReturnId = "r-1", ApprovalNote = null });
        Assert.Equal(JsonValueKind.Null, Input(sdk).GetProperty("approvalNote").ValueKind);
    }

    [Fact]
    public async Task DateTime_variables_are_sent_in_utc_z_format()
    {
        var sdk = new TestSdk
        {
            Handler = { ResponseBody = """{"data":{"sellerContractCancellations":null}}""" }
        };
        var instant = new DateTimeOffset(2024, 1, 2, 3, 4, 5, 6, TimeSpan.Zero);

        await Assert.ThrowsAsync<GraphQLException>(() => sdk.Client.GraphQL.SellerContractCancellationsAsync(
            new SellerContractCancellationsInput
            {
                InitialPage = new ContractCancellationInitialPageInput
                {
                    Filter = new ContractCancellationFilterInput
                    {
                        CreatedBetween = new DateTimeRangeInput { StartDateTime = instant, EndDateTime = instant }
                    }
                }
            }));

        var range = Input(sdk).GetProperty("initialPage").GetProperty("filter").GetProperty("createdBetween");
        Assert.Equal("2024-01-02T03:04:05.006Z", range.GetProperty("startDateTime").GetString());
    }

    [Fact]
    public async Task GraphQL_errors_throw_GraphQLException_with_every_message()
    {
        var sdk = new TestSdk
        {
            Handler = { ResponseBody = """{"errors":[{"message":"boom"},{"message":"bang"}],"data":null}""" }
        };

        var error = await Assert.ThrowsAsync<GraphQLException>(() =>
            sdk.Client.GraphQL.ApproveReturnAsync(new ApproveReturnInput { ReturnId = "r-1" }));

        Assert.Equal(["boom", "bang"], error.Errors);
    }

    [Fact]
    public async Task Http_401_throws_UnauthorizedException()
    {
        var sdk = new TestSdk { Handler = { Status = HttpStatusCode.Unauthorized } };

        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            sdk.Client.GraphQL.ApproveReturnAsync(new ApproveReturnInput { ReturnId = "r-1" }));
    }

    [Fact]
    public async Task Http_500_throws_SdkException_with_status()
    {
        var sdk = new TestSdk { Handler = { Status = HttpStatusCode.InternalServerError } };

        var error = await Assert.ThrowsAsync<SdkException>(() =>
            sdk.Client.GraphQL.ApproveReturnAsync(new ApproveReturnInput { ReturnId = "r-1" }));

        Assert.Contains("HTTP 500", error.Message);
    }

    [Fact]
    public async Task Raw_queries_map_401_to_UnauthorizedException()
    {
        // Previously the IHttpClientFactory pipeline threw a raw HttpRequestException here.
        var sdk = new TestSdk { Handler = { Status = HttpStatusCode.Unauthorized } };

        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            sdk.Client.GraphQL.ExecuteRawUnsafeAsync("query { __typename }"));
    }

    [Fact]
    public void Standalone_client_exposes_typed_graphql_operations()
    {
        // Before the in-house codegen, typed operations required DI and threw NotSupportedException.
        using var client = new OrderManagementClient(new OrderManagementConfiguration { AccessToken = "t" });

        Assert.NotNull(client.GraphQL);
    }

    private static JsonElement Input(TestSdk sdk) =>
        sdk.Handler.Request!.RootElement.GetProperty("variables").GetProperty("input");
}
