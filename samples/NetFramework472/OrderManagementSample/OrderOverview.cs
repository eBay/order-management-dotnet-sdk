using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using EBay.OrderManagement;
using EBay.OrderManagement.Configuration;
using EBay.OrderManagement.Generated.Fulfillment.Models;
using EBay.OrderManagement.Generated.GraphQL;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OrderManagementSample
{
    public enum EbayEnvironment
    {
        Production,
        Sandbox
    }

    /// <summary>Builds the SDK configuration for production or the eBay sandbox.</summary>
    public static class SdkSetup
    {
        // The sandbox GraphQL API is not on api.sandbox.ebay.com (that returns 404).
        public const string SandboxGraphQLUrl = "https://graphqlapi.sandbox.ebay.com/graphql";

        public static OrderManagementConfiguration Create(EbayEnvironment environment, string accessToken, string graphQLUrl)
        {
            var config = new OrderManagementConfiguration { AccessToken = accessToken };
            if (environment == EbayEnvironment.Sandbox)
            {
                // Sandbox hosts (api, apiz and graphqlapi.sandbox.ebay.com) are trusted by default.
                config.FulfillmentBaseUrl = "https://api.sandbox.ebay.com/sell/fulfillment/v1";
                config.LogisticsBaseUrl = "https://api.sandbox.ebay.com/sell/logistics/v1_beta";
                config.PostOrderApiBaseUrl = "https://api.sandbox.ebay.com/post-order/v2";
                config.GraphQLBaseUrl = SandboxGraphQLUrl;
            }
            if (!string.IsNullOrWhiteSpace(graphQLUrl))
            {
                var host = new Uri(graphQLUrl).Host;
                if (!OrderManagementConfiguration.DefaultAllowedHosts.Contains(host))
                {
                    // Any other host must be trusted explicitly before it receives the access token.
                    config.AllowedHosts.Add(host);
                }
                config.GraphQLBaseUrl = graphQLUrl;
            }
            return config;
        }
    }

    /// <summary>One order, combining the Fulfillment REST API and the GraphQL orders query.</summary>
    public sealed class OrderRow
    {
        public string OrderId { get; set; }
        public string Created { get; set; }
        public string Fulfillment { get; set; }
        public string Total { get; set; }
        public string State { get; set; }
        public string Payment { get; set; }
    }

    public sealed class OverviewResult
    {
        public OverviewResult(IReadOnlyList<OrderRow> rows, string restError, string graphQLError)
        {
            Rows = rows;
            RestError = restError;
            GraphQLError = graphQLError;
        }

        public IReadOnlyList<OrderRow> Rows { get; }
        public string RestError { get; }
        public string GraphQLError { get; }
    }

    /// <summary>Loads recent orders through REST and GraphQL concurrently and joins them by order id.</summary>
    public static class OrderOverview
    {
        private static readonly JsonSerializerSettings DetailSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            Converters = { new StringEnumConverter() }
        };

        public static async Task<OverviewResult> LoadAsync(OrderManagementClient client, int days)
        {
            var to = DateTimeOffset.UtcNow;
            var from = to.AddDays(-days);
            var restTask = LoadRestAsync(client, from);
            var graphQLTask = LoadGraphQLAsync(client, from, to);

            string restError = null;
            string graphQLError = null;
            IReadOnlyList<Order> restOrders = new Order[0];
            IReadOnlyList<IOrders_Orders_SellerOrders_Orders> graphQLOrders = new IOrders_Orders_SellerOrders_Orders[0];
            try
            {
                restOrders = await restTask.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                restError = ex.GetType().Name + ": " + ex.Message;
            }
            try
            {
                graphQLOrders = await graphQLTask.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                graphQLError = ex.GetType().Name + ": " + ex.Message;
            }

            var rows = new Dictionary<string, OrderRow>(StringComparer.Ordinal);
            foreach (var order in restOrders)
            {
                var row = RowFor(rows, order.OrderId);
                row.Created = order.CreationDate;
                row.Fulfillment = order.OrderFulfillmentStatus.HasValue ? order.OrderFulfillmentStatus.Value.ToString() : null;
                var total = order.PricingSummary != null ? order.PricingSummary.Total : null;
                row.Total = total != null ? total.Value + " " + total.Currency : null;
            }
            foreach (var order in graphQLOrders)
            {
                var row = RowFor(rows, order.OrderId);
                if (row.Created == null)
                {
                    row.Created = order.PlacedAt.ToString("o", CultureInfo.InvariantCulture);
                }
                row.State = DescribeState(order.OrderState);
                row.Payment = DescribePayment(order.OrderState);
            }
            return new OverviewResult(rows.Values.OrderByDescending(r => r.Created).ToList(), restError, graphQLError);
        }

        /// <summary>Full REST order details, rendered with the application's own Newtonsoft.Json.</summary>
        public static async Task<string> DetailsJsonAsync(OrderManagementClient client, string orderId)
        {
            var order = await client.Fulfillment.GetOrderAsync(orderId).ConfigureAwait(false);
            return JsonConvert.SerializeObject(order, DetailSettings);
        }

        private static async Task<IReadOnlyList<Order>> LoadRestAsync(OrderManagementClient client, DateTimeOffset from)
        {
            var filter = "creationdate:[" + from.ToString("yyyy-MM-ddTHH:mm:ss.fff'Z'", CultureInfo.InvariantCulture) + "..]";
            var page = await client.Fulfillment.GetOrdersAsync(filter: filter, limit: "50").ConfigureAwait(false);
            return page != null && page.Orders != null ? (IReadOnlyList<Order>)page.Orders : new Order[0];
        }

        private static async Task<IReadOnlyList<IOrders_Orders_SellerOrders_Orders>> LoadGraphQLAsync(
            OrderManagementClient client, DateTimeOffset from, DateTimeOffset to)
        {
            var input = new OrdersInput
            {
                OrdersFirstPage = new OrdersFirstPageInput
                {
                    MaxPageSize = 50,
                    Filter = new OrdersFirstFilterInput
                    {
                        DateRange = new OrdersDateRangeInput
                        {
                            DateType = OrdersDateType.Confirmed,
                            DateTimeRange = new DateTimeRangeInput { StartDateTime = from, EndDateTime = to }
                        },
                        Sort = new OrdersSortInput { SortField = OrdersSortField.ConfirmedAt, SortOrder = SortOrder.Desc }
                    }
                }
            };
            var result = await client.GraphQL.OrdersAsync(input).ConfigureAwait(false);
            return result != null ? result.SellerOrders.Orders : new IOrders_Orders_SellerOrders_Orders[0];
        }

        private static OrderRow RowFor(Dictionary<string, OrderRow> rows, string orderId)
        {
            var key = orderId ?? "(no id)";
            OrderRow row;
            if (!rows.TryGetValue(key, out row))
            {
                row = new OrderRow { OrderId = key };
                rows.Add(key, row);
            }
            return row;
        }

        // GraphQL unions surface as interfaces; C# 7 type patterns pick the concrete state.
        private static string DescribeState(IOrders_Orders_SellerOrders_Orders_OrderState state)
        {
            if (state is IOrders_Orders_SellerOrders_Orders_OrderState_OrderConfirmed confirmed) return confirmed.Label;
            if (state is IOrders_Orders_SellerOrders_Orders_OrderState_OrderPlaced placed) return placed.Label;
            if (state is IOrders_Orders_SellerOrders_Orders_OrderState_OrderCancelled cancelled) return cancelled.Label;
            if (state is IOrders_Orders_SellerOrders_Orders_OrderState_OrderAbandoned abandoned) return abandoned.Label;
            return state != null ? state.__typename : null;
        }

        private static string DescribePayment(IOrders_Orders_SellerOrders_Orders_OrderState state)
        {
            var confirmed = state as IOrders_Orders_SellerOrders_Orders_OrderState_OrderConfirmed;
            var funding = confirmed != null ? confirmed.FundingState : null;
            switch (funding)
            {
                case IOrders_Orders_SellerOrders_Orders_OrderState_FundingState_PaymentsCompleted completed: return completed.Label;
                case IOrders_Orders_SellerOrders_Orders_OrderState_FundingState_PaymentsInProgress inProgress: return inProgress.Label;
                case IOrders_Orders_SellerOrders_Orders_OrderState_FundingState_PaymentsNotStarted notStarted: return notStarted.Label;
                case IOrders_Orders_SellerOrders_Orders_OrderState_FundingState_PaymentsFailure failure: return failure.Label;
                case null: return null;
                default: return funding.__typename;
            }
        }
    }
}
