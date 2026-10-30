using EBay.OrderManagement.Clients.GraphQL;
using EBay.OrderManagement.Clients.Rest;
using EBay.OrderManagement.Configuration;
using EBay.OrderManagement.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EBay.OrderManagement.Tests;

public sealed class DependencyInjectionTests
{
    [Fact]
    public void Registers_one_shared_entry_client_and_no_individual_api_clients()
    {
        var services = new ServiceCollection();
        services.AddOrderManagementSdk(new OrderManagementConfiguration { AccessToken = "t" });

        using var provider = services.BuildServiceProvider();
        var client = provider.GetRequiredService<OrderManagementClient>();

        Assert.Same(client, provider.CreateScope().ServiceProvider.GetRequiredService<OrderManagementClient>());
        Assert.NotNull(client.Fulfillment);
        Assert.NotNull(client.GraphQL);
        Assert.Null(provider.GetService<FulfillmentClient>());
        Assert.Null(provider.GetService<OrderGraphQLClient>());
    }

    [Fact]
    public void Rejects_a_null_primary_handler()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new OrderManagementClient(new OrderManagementConfiguration(), (HttpMessageHandler)null!));
    }
}
