using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EBay.OrderManagement;

namespace OrderManagementSample
{
    /// <summary>
    /// Offline end-to-end check of the sample on the real runtime: the SDK, Kiota, System.Text.Json and the
    /// application's Newtonsoft.Json all load under the binding redirects in App.config, and the REST and
    /// GraphQL results are joined as the UI shows them. Only the network is replaced (canned eBay responses).
    /// </summary>
    internal static class SmokeTest
    {
        private const string Token = "smoke-token";

        public static int Run(TextWriter output)
        {
            var failures = new List<string>();
            try
            {
                RunAsync(output, failures).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                failures.Add("unexpected " + ex);
            }

            VerifyAssemblyClosure(output, failures);
            PrintAssemblies(output);
            foreach (var failure in failures)
            {
                output.WriteLine("FAIL " + failure);
            }
            output.WriteLine(failures.Count == 0 ? "SMOKE OK" : "SMOKE FAILED (" + failures.Count + ")");
            return failures.Count == 0 ? 0 : 1;
        }

        private static async Task RunAsync(TextWriter output, List<string> failures)
        {
            foreach (var environment in new[] { EbayEnvironment.Production, EbayEnvironment.Sandbox })
            {
                var handler = new CannedEbay();
                var config = SdkSetup.Create(environment, Token, null);
                using (var client = new OrderManagementClient(config, handler))
                {
                    var result = await OrderOverview.LoadAsync(client, 30);
                    output.WriteLine(environment + ": " + result.Rows.Count + " rows");
                    Check(failures, result.RestError == null, environment + " REST error: " + result.RestError);
                    Check(failures, result.GraphQLError == null, environment + " GraphQL error: " + result.GraphQLError);
                    CheckRows(failures, environment, result.Rows);

                    var details = await OrderOverview.DetailsJsonAsync(client, "01-10001-00001");
                    Check(failures, details.Contains("\"OrderFulfillmentStatus\": \"FULFILLED\""), "Newtonsoft details: " + details);

                    var expected = new[] { config.FulfillmentBaseUrl, config.GraphQLBaseUrl };
                    Check(failures, handler.Requests.Count == 3, environment + " expected 3 requests, saw " + handler.Requests.Count);
                    foreach (var request in handler.Requests)
                    {
                        output.WriteLine("  " + request.Method + " " + request.Uri);
                        Check(failures, expected.Any(url => request.Uri.AbsoluteUri.StartsWith(url, StringComparison.Ordinal)),
                            environment + " request outside the configured base URLs: " + request.Uri);
                        Check(failures, environment != EbayEnvironment.Sandbox || request.Uri.Host.EndsWith(".sandbox.ebay.com", StringComparison.Ordinal),
                            "sandbox request left the sandbox: " + request.Uri);
                        Check(failures, request.Authorization == "Bearer " + Token, "missing token on " + request.Uri);
                    }
                }
            }

            // A GraphQL endpoint outside eBay's hosts (e.g. a corporate proxy) is only used once it is trusted.
            var proxied = SdkSetup.Create(EbayEnvironment.Production, Token, "https://ebay-proxy.example.com/graphql");
            Check(failures, proxied.AllowedHosts.Contains("ebay-proxy.example.com"), "proxy host not added to AllowedHosts");
            proxied.AllowedHosts.Clear();
            try
            {
                new OrderManagementClient(proxied, new CannedEbay()).Dispose();
                failures.Add("untrusted GraphQL host was accepted");
            }
            catch (InvalidOperationException ex)
            {
                output.WriteLine("Untrusted host rejected: " + ex.Message);
            }
        }

        private static void CheckRows(List<string> failures, EbayEnvironment environment, IReadOnlyList<OrderRow> rows)
        {
            var byId = rows.ToDictionary(r => r.OrderId);
            OrderRow both, restOnly, graphQLOnly;
            Check(failures, rows.Count == 3, environment + " expected 3 joined rows, saw " + rows.Count);
            if (byId.TryGetValue("01-10001-00001", out both))
            {
                Check(failures, both.Fulfillment == "FULFILLED", "REST fulfillment status: " + both.Fulfillment);
                Check(failures, both.Total == "24.99 USD", "REST total: " + both.Total);
                Check(failures, both.State == "Confirmed", "GraphQL state: " + both.State);
                Check(failures, both.Payment == "Paid", "GraphQL funding state: " + both.Payment);
            }
            else
            {
                failures.Add("order present in both APIs is missing");
            }
            Check(failures, byId.TryGetValue("01-10002-00002", out restOnly) && restOnly.State == null, "REST-only order");
            Check(failures, byId.TryGetValue("01-10003-00003", out graphQLOnly) && graphQLOnly.State == "Cancelled"
                && graphQLOnly.Created == "2024-05-01T09:30:00.0000000+00:00", "GraphQL-only order");
        }

        /// <summary>
        /// Loads every assembly the app references, transitively, the way the runtime would: through the binding
        /// redirects in App.config. A missing or wrong redirect fails here even if no code path has needed it yet.
        /// </summary>
        private static void VerifyAssemblyClosure(TextWriter output, List<string> failures)
        {
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var pending = new Queue<System.Reflection.Assembly>();
            pending.Enqueue(typeof(SmokeTest).Assembly);
            while (pending.Count > 0)
            {
                var assembly = pending.Dequeue();
                foreach (var reference in assembly.GetReferencedAssemblies())
                {
                    if (!seen.Add(reference.FullName))
                    {
                        continue;
                    }
                    try
                    {
                        pending.Enqueue(System.Reflection.Assembly.Load(reference));
                    }
                    catch (Exception ex)
                    {
                        failures.Add("cannot load " + reference.FullName + " (referenced by " + assembly.GetName().Name + "): "
                            + ex.GetType().Name + ": " + ex.Message);
                    }
                }
            }
            output.WriteLine("Loaded " + seen.Count + " referenced assembly versions through App.config");
        }

        private static void PrintAssemblies(TextWriter output)
        {
            var names = new[]
            {
                "EBay.OrderManagement", "Newtonsoft.Json", "System.Text.Json", "System.Text.Encodings.Web",
                "System.Memory", "System.Runtime.CompilerServices.Unsafe", "Microsoft.Bcl.AsyncInterfaces",
                "System.ValueTuple", "System.IO.Pipelines",
                "Microsoft.Kiota.Abstractions", "Microsoft.Kiota.Http.HttpClientLibrary", "Microsoft.Kiota.Serialization.Json"
            };
            var loaded = AppDomain.CurrentDomain.GetAssemblies().ToDictionary(a => a.GetName().Name, a => a.GetName());
            output.WriteLine("Runtime: " + Environment.Version + " (" + System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription + ")");
            foreach (var name in names)
            {
                AssemblyNameOrMissing(output, loaded, name);
            }
        }

        private static void AssemblyNameOrMissing(TextWriter output, Dictionary<string, System.Reflection.AssemblyName> loaded, string name)
        {
            System.Reflection.AssemblyName assembly;
            output.WriteLine("  " + name.PadRight(42) + (loaded.TryGetValue(name, out assembly) ? assembly.Version.ToString() : "(not loaded)"));
        }

        private static void Check(List<string> failures, bool condition, string message)
        {
            if (!condition)
            {
                failures.Add(message);
            }
        }

        private sealed class SeenRequest
        {
            public HttpMethod Method;
            public Uri Uri;
            public string Authorization;
        }

        /// <summary>Answers the three calls the sample makes with fixed eBay-shaped payloads.</summary>
        private sealed class CannedEbay : HttpMessageHandler
        {
            public readonly List<SeenRequest> Requests = new List<SeenRequest>();

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                lock (Requests)
                {
                    Requests.Add(new SeenRequest
                    {
                        Method = request.Method,
                        Uri = request.RequestUri,
                        Authorization = request.Headers.Authorization != null ? request.Headers.Authorization.ToString() : null
                    });
                }
                var path = request.RequestUri.AbsolutePath;
                string body;
                if (path.EndsWith("/graphql", StringComparison.Ordinal))
                {
                    body = GraphQLOrders;
                }
                else if (path.EndsWith("/order", StringComparison.Ordinal))
                {
                    body = "{\"total\":2,\"orders\":[" + RestOrder1 + "," + RestOrder2 + "]}";
                }
                else if (path.EndsWith("/order/01-10001-00001", StringComparison.Ordinal))
                {
                    body = RestOrder1;
                }
                else
                {
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound) { RequestMessage = request });
                }
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    RequestMessage = request,
                    Content = new StringContent(body, Encoding.UTF8, "application/json")
                });
            }
        }

        private const string RestOrder1 =
            "{\"orderId\":\"01-10001-00001\",\"creationDate\":\"2024-05-03T12:00:00.000Z\",\"orderFulfillmentStatus\":\"FULFILLED\","
            + "\"pricingSummary\":{\"total\":{\"value\":\"24.99\",\"currency\":\"USD\"}}}";

        private const string RestOrder2 =
            "{\"orderId\":\"01-10002-00002\",\"creationDate\":\"2024-05-02T12:00:00.000Z\",\"orderFulfillmentStatus\":\"NOT_STARTED\","
            + "\"pricingSummary\":{\"total\":{\"value\":\"5.00\",\"currency\":\"USD\"}}}";

        private const string GraphQLOrders =
            "{\"data\":{\"orders\":{\"__typename\":\"OrdersOutput\",\"sellerOrders\":{\"__typename\":\"PaginatedOrders\",\"orders\":["
            + "{\"__typename\":\"Order\",\"orderId\":\"01-10001-00001\",\"placedAt\":\"2024-05-03T12:00:00Z\",\"updatedAt\":\"2024-05-03T12:05:00Z\","
            + "\"logistics\":{\"__typename\":\"PurchaseLogistics\",\"fulfillmentPlan\":null},"
            + "\"orderState\":{\"__typename\":\"OrderConfirmed\",\"label\":\"Confirmed\",\"confirmedAt\":\"2024-05-03T12:01:00Z\","
            + "\"fundingState\":{\"__typename\":\"PaymentsCompleted\",\"label\":\"Paid\"}}},"
            + "{\"__typename\":\"Order\",\"orderId\":\"01-10003-00003\",\"placedAt\":\"2024-05-01T09:30:00Z\",\"updatedAt\":\"2024-05-01T10:00:00Z\","
            + "\"logistics\":{\"__typename\":\"PurchaseLogistics\",\"fulfillmentPlan\":null},"
            + "\"orderState\":{\"__typename\":\"OrderCancelled\",\"label\":\"Cancelled\",\"cancelledAt\":\"2024-05-01T10:00:00Z\",\"reason\":null}}"
            + "],\"pagination\":{\"__typename\":\"Pagination\",\"nextCursor\":null,\"previousCursor\":null}}}}}";
    }
}
