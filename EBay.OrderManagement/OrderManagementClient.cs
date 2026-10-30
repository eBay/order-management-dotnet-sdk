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
using EBay.OrderManagement.Generated.Fulfillment;
using EBay.OrderManagement.Generated.Logistics;
using EBay.OrderManagement.Generated.PostOrderApi;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace EBay.OrderManagement;

/// <summary>
/// Entry point for the Order Management APIs. The client owns its HTTP connections:
/// create one per application (or register it with <c>AddOrderManagementSdk</c>, which adds it
/// as a singleton) and dispose it on shutdown. Reach the individual APIs through its properties.
/// </summary>
public class OrderManagementClient : IDisposable
{
    private readonly HttpClient[] _ownedClients;

    public FulfillmentClient Fulfillment { get; private set; } = null!;
    public LogisticsClient Logistics { get; private set; } = null!;
    public PostOrderApiClient PostOrderApi { get; private set; } = null!;

    public OrderGraphQLClient GraphQL { get; }

    /// <summary>Creates a client with its own HTTP connections.</summary>
    public OrderManagementClient(OrderManagementConfiguration config)
        : this(config, CreateDefaultPrimaryHandler, disposeHandlers: true)
    {
    }

    /// <summary>
    /// Creates a client that sends every request through <paramref name="primaryHandler"/>, for example to
    /// configure a proxy or client certificates, or to substitute a test double. The handler is shared by
    /// all APIs and is not disposed by this client.
    /// </summary>
    public OrderManagementClient(OrderManagementConfiguration config, HttpMessageHandler primaryHandler)
        : this(config, RequireHandler(primaryHandler), disposeHandlers: false)
    {
    }

    private OrderManagementClient(
        OrderManagementConfiguration config,
        Func<HttpMessageHandler> primaryHandler,
        bool disposeHandlers)
    {
        config.ValidateTrustedHosts();
        var tokenContext = new AsyncLocalTokenContext();
        var auth = new BaseBearerTokenAuthenticationProvider(new KiotaAuthProvider(config, tokenContext));
        var owned = new List<HttpClient>();
        HttpClient Build(DelegatingHandler logging, string baseUrl)
        {
            logging.InnerHandler = primaryHandler();
            var client = new HttpClient(logging, disposeHandlers)
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = config.HttpTimeout
            };
            owned.Add(client);
            return client;
        }
        InitializeRestClients(auth, config, tokenContext, endpoint => Build(new DiagnosticLoggingHandler(config), endpoint.BaseUrl));
        var graphQLHttp = Build(new GraphQLHttpHandler(config), config.GraphQLBaseUrl);
        GraphQL = new OrderGraphQLClient(
            new GraphQLExecutor(graphQLHttp, config, tokenContext),
            new DynamicGraphQLClient(graphQLHttp, config, tokenContext),
            tokenContext);
        _ownedClients = [.. owned];
    }

    private static HttpMessageHandler CreateDefaultPrimaryHandler() =>
#if NETFRAMEWORK
        // .NET Framework defaults to 2 connections per host outside ASP.NET.
        new HttpClientHandler { MaxConnectionsPerServer = 64 };
#else
        new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan.FromMinutes(2) };
#endif

    private static Func<HttpMessageHandler> RequireHandler(HttpMessageHandler primaryHandler)
    {
#if NETFRAMEWORK
        if (primaryHandler is null)
        {
            throw new ArgumentNullException(nameof(primaryHandler));
        }
#else
        ArgumentNullException.ThrowIfNull(primaryHandler);
#endif
        return () => primaryHandler;
    }

    internal static IReadOnlyList<RestEndpoint> RestEndpoints(OrderManagementConfiguration config) =>
    [
        new RestEndpoint("fulfillment", config.RestBaseUrl("fulfillment")),
        new RestEndpoint("logistics", config.RestBaseUrl("logistics")),
        new RestEndpoint("post-order-api", config.RestBaseUrl("post-order-api"))
    ];

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
        var postOrderApiAdapter = new HttpClientRequestAdapter(auth, httpClient: httpClientFor(endpoints.First(endpoint => endpoint.ApiId == "post-order-api")));
        PostOrderApi = new PostOrderApiClient(new PostOrderApiApiClient(postOrderApiAdapter), config, tokenContext, postOrderApiAdapter);
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

internal sealed record RestEndpoint(string ApiId, string BaseUrl);

internal delegate HttpClient RestHttpClientProvider(RestEndpoint endpoint);
