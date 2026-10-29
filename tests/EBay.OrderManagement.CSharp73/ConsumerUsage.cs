using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EBay.OrderManagement;
using EBay.OrderManagement.Configuration;
using EBay.OrderManagement.Exceptions;
using EBay.OrderManagement.Generated.GraphQL;

namespace EBay.OrderManagement.CSharp73
{
    /// <summary>Typical partner code written in C# 7.3; it only has to compile.</summary>
    public static class ConsumerUsage
    {
        public static async Task<string> RunAsync()
        {
            var config = new OrderManagementConfiguration
            {
                AccessToken = "token",
                HttpTimeout = TimeSpan.FromSeconds(60),
                DiagnosticLogging = true,
                FulfillmentBaseUrl = "https://ebay-proxy.example.com/sell/fulfillment/v1",
                AllowedHosts = { "ebay-proxy.example.com" },
                DefaultRequestOptions = new RequestOptions
                {
                    Headers = new Dictionary<string, string> { { "X-EBAY-C-MARKETPLACE-ID", "EBAY_US" } }
                }
            };

            using (var client = new OrderManagementClient(config))
            {
                try
                {
                    var result = await client.GraphQL.ApproveReturnAsync(
                        new ApproveReturnInput { ReturnId = "r-1", ApprovalNote = "ok" },
                        accessToken: "per-call-token");

                    var orderReturn = result as IApproveReturn_ApproveReturn_OrderReturn;
                    if (orderReturn != null)
                    {
                        return orderReturn.ReturnId + ":" + orderReturn.Status.DisplayText;
                    }

                    var notFound = result as IApproveReturn_ApproveReturn_ReturnNotFoundError;
                    return notFound != null ? notFound.ErrorMessage : result?.__typename;
                }
                catch (UnauthorizedException)
                {
                    return "unauthorized";
                }
                catch (GraphQLException ex)
                {
                    return string.Join("; ", ex.Errors);
                }
            }
        }

        public static ContractCancellationSellerReasonInput Reason()
        {
            return ContractCancellationSellerReasonInput.OutOfStock;
        }
    }
}
