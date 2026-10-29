using EBay.OrderManagement.Configuration;
using Xunit;

namespace EBay.OrderManagement.Tests;

public sealed class TrustedHostsTests
{
    private const string SandboxFulfillment = "https://api.sandbox.ebay.com/sell/fulfillment/v1";
    private const string ProxyFulfillment = "https://ebay-proxy.example.com/sell/fulfillment/v1";

    [Fact]
    public void Untrusted_base_url_is_rejected_by_both_constructors()
    {
        var config = new OrderManagementConfiguration { AccessToken = "t", FulfillmentBaseUrl = ProxyFulfillment };

        var standalone = Assert.Throws<InvalidOperationException>(() => new OrderManagementClient(config));
        Assert.Contains("AllowedHosts", standalone.Message);
        Assert.Throws<InvalidOperationException>(() =>
            new TestSdk(configure: c => c.FulfillmentBaseUrl = ProxyFulfillment));
    }

    [Fact]
    public async Task Sandbox_hosts_are_trusted_by_default()
    {
        var sdk = new TestSdk(configure: c => c.FulfillmentBaseUrl = SandboxFulfillment);
        sdk.Handler.ResponseBody = """{"orders":[],"total":0}""";

        await sdk.Client.Fulfillment.GetOrdersAsync(limit: "1");

        Assert.Equal("api.sandbox.ebay.com", sdk.Handler.RequestUri!.Host);
        Assert.Equal("Bearer config-token", sdk.Handler.Authorization);
    }

    [Fact]
    public async Task Additional_allowed_host_receives_rest_requests_with_token()
    {
        var sdk = new TestSdk(configure: c =>
        {
            c.FulfillmentBaseUrl = ProxyFulfillment;
            c.AllowedHosts.Add("ebay-proxy.example.com");
        });
        sdk.Handler.ResponseBody = """{"orders":[],"total":0}""";

        await sdk.Client.Fulfillment.GetOrdersAsync(limit: "1");

        Assert.Equal("ebay-proxy.example.com", sdk.Handler.RequestUri!.Host);
        Assert.Equal("Bearer config-token", sdk.Handler.Authorization);
    }

    [Fact]
    public async Task Token_is_never_sent_to_a_host_configured_after_construction()
    {
        // Regression: the token used to be attached for any HTTPS host.
        var sdk = new TestSdk();
        sdk.Config.GraphQLBaseUrl = "https://attacker.example.com/graphql";

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            sdk.Client.GraphQL.ExecuteRawUnsafeAsync("query { __typename }"));

        Assert.Equal(0, sdk.Handler.Calls);
    }

    [Fact]
    public void Default_hosts_are_read_only()
    {
        Assert.Contains("api.ebay.com", OrderManagementConfiguration.DefaultAllowedHosts);
        Assert.Throws<NotSupportedException>(() =>
            ((IList<string>)OrderManagementConfiguration.DefaultAllowedHosts).Add("evil.example.com"));
    }
}
