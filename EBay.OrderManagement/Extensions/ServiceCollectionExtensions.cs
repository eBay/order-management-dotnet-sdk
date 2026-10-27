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

using EBay.OrderManagement;
using EBay.OrderManagement.Clients.GraphQL;
using EBay.OrderManagement.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EBay.OrderManagement.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrderManagementSdk(
        this IServiceCollection services,
        OrderManagementConfiguration config)
    {
        services.AddSingleton(config);
        services.AddSingleton<ITokenContext, AsyncLocalTokenContext>();

        services.AddTransient<BearerTokenHandler>();
        services
            .AddHttpClient(OrderManagementClient.GraphQLClientName, client =>
                client.BaseAddress = new Uri(config.GraphQLBaseUrl))
            .ConfigureHttpClient(c => c.Timeout = config.HttpTimeout)
            .AddHttpMessageHandler<BearerTokenHandler>()
            .AddHttpMessageHandler(_ => new GraphQLHttpHandler(config))
#if NETFRAMEWORK
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { MaxConnectionsPerServer = 64 })
#endif
            ;
        services.AddScoped<DynamicGraphQLClient>(sp =>
            new DynamicGraphQLClient(
                sp.GetRequiredService<IHttpClientFactory>().CreateClient(OrderManagementClient.GraphQLClientName),
                sp.GetRequiredService<OrderManagementConfiguration>(),
                sp.GetRequiredService<ITokenContext>()));

        foreach (var endpoint in OrderManagementClient.RestEndpointNames)
        {
            var id = endpoint.ApiId;
            services.AddHttpClient(endpoint.HttpClientName, (sp, c) =>
                    c.BaseAddress = new Uri(sp.GetRequiredService<OrderManagementConfiguration>().RestBaseUrl(id)))
                .ConfigureHttpClient((sp, c) =>
                    c.Timeout = sp.GetRequiredService<OrderManagementConfiguration>().HttpTimeout)
                .AddHttpMessageHandler(sp =>
                    new DiagnosticLoggingHandler(sp.GetRequiredService<OrderManagementConfiguration>()))
#if NETFRAMEWORK
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { MaxConnectionsPerServer = 64 })
#endif
                ;
        }

        services.AddScoped<OrderManagementClient>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            return new OrderManagementClient(
                sp.GetRequiredService<OrderManagementConfiguration>(),
                factory,
                sp.GetRequiredService<ITokenContext>());
        });

        services.AddScoped(sp => sp.GetRequiredService<OrderManagementClient>().Fulfillment);
        services.AddScoped(sp => sp.GetRequiredService<OrderManagementClient>().Logistics);
        services.AddScoped(sp => sp.GetRequiredService<OrderManagementClient>().PostOrderApi);
        services.AddScoped(sp => sp.GetRequiredService<OrderManagementClient>().GraphQL);
        return services;
    }
}
