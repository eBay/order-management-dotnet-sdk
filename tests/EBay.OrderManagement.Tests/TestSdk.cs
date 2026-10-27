using System.Net;
using System.Text;
using System.Text.Json;
using EBay.OrderManagement.Configuration;
using EBay.OrderManagement.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace EBay.OrderManagement.Tests;

/// <summary>SDK wired through DI with a fake primary handler on every named HttpClient.</summary>
internal sealed class TestSdk
{
    public FakeHandler Handler { get; } = new();
    public OrderManagementClient Client { get; }

    public TestSdk(string? accessToken = "config-token")
    {
        var services = new ServiceCollection();
        services.AddOrderManagementSdk(new OrderManagementConfiguration { AccessToken = accessToken });
        services.ConfigureAll<HttpClientFactoryOptions>(options =>
            options.HttpMessageHandlerBuilderActions.Add(builder => builder.PrimaryHandler = Handler));
        Client = services.BuildServiceProvider().CreateScope().ServiceProvider.GetRequiredService<OrderManagementClient>();
    }
}

internal sealed class FakeHandler : HttpMessageHandler
{
    public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
    public string ResponseBody { get; set; } = "{}";
    public JsonDocument? Request { get; private set; }
    public string? Authorization { get; private set; }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var body = request.Content is null ? "{}" : await request.Content.ReadAsStringAsync().ConfigureAwait(false);
        Request = JsonDocument.Parse(body);
        Authorization = request.Headers.Authorization?.ToString();
        return new HttpResponseMessage(Status)
        {
            Content = new StringContent(ResponseBody, Encoding.UTF8, "application/json"),
            RequestMessage = request
        };
    }
}
