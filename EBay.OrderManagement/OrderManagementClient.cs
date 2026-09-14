// Copyright 2026 eBay Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using EBay.OrderManagement.Clients.GraphQL;
using EBay.OrderManagement.Clients.Rest;
using EBay.OrderManagement.Configuration;
using EBay.OrderManagement.Extensions;
using EBay.OrderManagement.Generated.Fulfillment;
using EBay.OrderManagement.Generated.GraphQL;
using EBay.OrderManagement.Generated.Logistics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace EBay.OrderManagement;

public class OrderManagementClient : IDisposable
{
    internal const string FulfillmentClientName = "OrderManagementClient.Fulfillment";
    internal const string LogisticsClientName = "OrderManagementClient.Logistics";
    internal const string GraphQLClientName = "OrderManagementGraphQLClient";

    private readonly HttpClient[] _ownedClients;

    public FulfillmentClient Fulfillment { get; private set; } = null!;
    public LogisticsClient Logistics { get; private set; } = null!;

    public OrderGraphQLClient GraphQL { get; }

    /// <summary>
    /// DI-friendly constructor. Named HttpClients come from <see cref="IHttpClientFactory"/>.
    /// </summary>
    internal OrderManagementClient(
        OrderManagementConfiguration config,
        IHttpClientFactory factory,
        IOrderManagementGraphQLClient graphQLClient,
        ITokenContext tokenContext)
    {
        _ownedClients = [];
        var auth = new BaseBearerTokenAuthenticationProvider(new KiotaAuthProvider(config, tokenContext));
        InitializeRestClients(auth, config, tokenContext, endpoint => factory.CreateClient(endpoint.HttpClientName));
        GraphQL = new OrderGraphQLClient(
            graphQLClient,
            new DynamicGraphQLClient(factory.CreateClient(GraphQLClientName), config, tokenContext),
            tokenContext);
    }

    /// <summary>
    /// Standalone constructor for use outside DI (tests, scripts).
    /// Creates its own HttpClients with DiagnosticLoggingHandler.
    /// Typed GraphQL operations are unavailable; use GraphQL.ExecuteRawUnsafeAsync instead.
    /// </summary>
    public OrderManagementClient(OrderManagementConfiguration config)
    {
        var tokenContext = new AsyncLocalTokenContext();
        var auth = new BaseBearerTokenAuthenticationProvider(new KiotaAuthProvider(config, tokenContext));
        var owned = new List<HttpClient>();
        InitializeRestClients(auth, config, tokenContext, endpoint =>
        {
            var client = BuildStandaloneHttpClient(config, endpoint.BaseUrl);
            owned.Add(client);
            return client;
        });
        var graphQLHttp = BuildStandaloneHttpClient(config, config.GraphQLBaseUrl);
        owned.Add(graphQLHttp);
        _ownedClients = [.. owned];
        GraphQL = new OrderGraphQLClient(
            BuildStandaloneGqlClient(config, tokenContext),
            new DynamicGraphQLClient(graphQLHttp, config, tokenContext),
            tokenContext);
    }

    private static IOrderManagementGraphQLClient BuildStandaloneGqlClient(
        OrderManagementConfiguration config, ITokenContext tokenContext)
    {
        var services = new ServiceCollection();
        services.AddOrderManagementSdk(config);
        services.AddSingleton<ITokenContext>(tokenContext);
        return services.BuildServiceProvider().GetRequiredService<IOrderManagementGraphQLClient>();
    }

    private static HttpClient BuildStandaloneHttpClient(OrderManagementConfiguration config, string baseUrl)
    {
        var handler = new DiagnosticLoggingHandler(config)
        {
            InnerHandler = new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan.FromMinutes(2) }
        };
        return new HttpClient(handler) { BaseAddress = new Uri(baseUrl), Timeout = config.HttpTimeout };
    }

    internal static IReadOnlyList<(string ApiId, string HttpClientName)> RestEndpointNames { get; } =
    [
        ("fulfillment", FulfillmentClientName),
        ("logistics", LogisticsClientName)
    ];

    internal static IReadOnlyList<RestEndpoint> RestEndpoints(OrderManagementConfiguration config) =>
        [.. RestEndpointNames.Select(name => new RestEndpoint(name.ApiId, name.HttpClientName, config.RestBaseUrl(name.ApiId)))];

    private void InitializeRestClients(
        IAuthenticationProvider auth,
        OrderManagementConfiguration config,
        ITokenContext tokenContext,
        RestHttpClientProvider httpClientFor)
    {
        var endpoints = RestEndpoints(config);
        var fulfillmentAdapter = new HttpClientRequestAdapter(auth, httpClient: httpClientFor(endpoints.First(endpoint => endpoint.ApiId == "fulfillment")));
        Fulfillment = new FulfillmentClient(new FulfillmentApiClient(fulfillmentAdapter), config, tokenContext, fulfillmentAdapter);
        var logisticsAdapter = new HttpClientRequestAdapter(auth, httpClient: httpClientFor(endpoints.First(endpoint => endpoint.ApiId == "logistics")));
        Logistics = new LogisticsClient(new LogisticsApiClient(logisticsAdapter), config, tokenContext, logisticsAdapter);
    }

    public void Dispose()
    {
        foreach (var client in _ownedClients)
        {
            client.Dispose();
        }
        GC.SuppressFinalize(this);
    }
}

internal sealed record RestEndpoint(string ApiId, string HttpClientName, string BaseUrl);

internal delegate HttpClient RestHttpClientProvider(RestEndpoint endpoint);
