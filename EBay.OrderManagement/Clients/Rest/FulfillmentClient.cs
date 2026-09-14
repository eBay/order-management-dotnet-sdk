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

using EBay.OrderManagement.Configuration;
using EBay.OrderManagement.Generated.Fulfillment.Models;
using EBay.OrderManagement.Generated.Fulfillment;
using Microsoft.Kiota.Abstractions;
using System.IO;

namespace EBay.OrderManagement.Clients.Rest;

public sealed class FulfillmentClient
{
    private readonly FulfillmentApiClient _kiota;
    private readonly OrderManagementConfiguration _config;
    private readonly ITokenContext _tokenContext;
    private readonly IRequestAdapter _adapter;

    internal FulfillmentClient(FulfillmentApiClient kiota, OrderManagementConfiguration config, ITokenContext tokenContext, IRequestAdapter adapter)
    {
        _kiota = kiota;
        _config = config;
        _tokenContext = tokenContext;
        _adapter = adapter;
    }

    /// <summary>
    /// Use this call to retrieve the contents of an order based on its unique identifier, <i>orderId</i>. This value was returned in the <b> getOrders</b> call's <b>orders.orderId</b> field when you searched for orders by creation date, modification date, or fulfillment status. Include the optional <b>fieldGroups</b> query parameter set to <c>TAX_BREAKDOWN</c> to return a breakdown of the taxes and fees.
    /// The returned <b>Order</b> object contains information you can use to create and process fulfillments, including:
    /// - Information about the buyer and seller
    /// - Information about the order's line items
    /// - The plans for packaging, addressing and shipping the order
    /// - The status of payment, packaging, addressing, and shipping the order
    /// - A summary of monetary amounts specific to the order such as pricing, payments, and shipping costs
    /// - A summary of applied taxes and fees, and optionally a breakdown of each
    /// </summary>
    public Task<Order?> GetOrderAsync(
        string orderId,
        string? fieldGroups = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Order[orderId].GetAsync(cfg =>
        {
            if (fieldGroups is not null)
            {
                cfg.QueryParameters.FieldGroups = fieldGroups;
            }
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Use this method to search for and retrieve one or more orders based on their creation date, last modification date, or fulfillment status using the <b>filter</b> parameter. You can alternatively specify a list of orders using the <b>orderIds</b> parameter. Include the optional <b>fieldGroups</b> query parameter set to <c>TAX_BREAKDOWN</c> to return a breakdown of the taxes and fees. By default, when no filters are used this call returns all orders created within the last 90 days.
    /// The returned <b>Order</b> objects contain information you can use to create and process fulfillments, including:
    /// - Information about the buyer and seller
    /// - Information about the order's line items
    /// - The plans for packaging, addressing and shipping the order
    /// - The status of payment, packaging, addressing, and shipping the order
    /// - A summary of monetary amounts specific to the order such as pricing, payments, and shipping costs
    /// - A summary of applied taxes and fees, and optionally a breakdown of each
    /// <b>Important:</b> In this call, the <b>cancelStatus.cancelRequests</b> array is returned but is always empty. Use the <b>getOrder</b> call instead, which returns this array fully populated with information about any cancellation requests.
    /// </summary>
    public Task<OrderSearchPagedCollection?> GetOrdersAsync(
        string? fieldGroups = null,
        string? filter = null,
        string? limit = null,
        string? offset = null,
        string? orderIds = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Order.GetAsync(cfg =>
        {
            if (fieldGroups is not null)
            {
                cfg.QueryParameters.FieldGroups = fieldGroups;
            }
            if (filter is not null)
            {
                cfg.QueryParameters.Filter = filter;
            }
            if (limit is not null)
            {
                cfg.QueryParameters.Limit = limit;
            }
            if (offset is not null)
            {
                cfg.QueryParameters.Offset = offset;
            }
            if (orderIds is not null)
            {
                cfg.QueryParameters.OrderIds = orderIds;
            }
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Issue Refund
    /// </summary>
    public Task<Refund?> IssueRefundAsync(
        string orderId,
        IssueRefundRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Order[orderId].Issue_refund.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Use this call to retrieve the contents of all fulfillments currently defined for a specified order based on the order's unique identifier, <b>orderId</b>. This value is returned in the <b>getOrders</b> call's <b>members.orderId</b> field when you search for orders by creation date or shipment status.
    /// </summary>
    public Task<ShippingFulfillmentPagedCollection?> GetShippingFulfillmentsAsync(
        string orderId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Order[orderId].Shipping_fulfillment.GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// When you group an order's line items into one or more packages, each package requires a corresponding plan for handling, addressing, and shipping; this is a <i>shipping fulfillment</i>. For each package, execute this call once to generate a shipping fulfillment associated with that package.
    /// <b>Note:</b> A single line item in an order can consist of multiple units of a purchased item, and one unit can consist of multiple parts or components. Although these components might be provided by the manufacturer in separate packaging, the seller must include all components of a given line item in the same package.
    /// Before using this call for a given package, you must determine which line items are in the package. If the package has been shipped, you should provide the date of shipment in the request. If not provided, it will default to the current date and time.
    /// </summary>
    public Task CreateShippingFulfillmentAsync(
        string orderId,
        ShippingFulfillmentDetails body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Order[orderId].Shipping_fulfillment.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Use this call to retrieve the contents of a fulfillment based on its unique identifier, <b>fulfillmentId</b> (combined with the associated order's <b>orderId</b>). The <b>fulfillmentId</b> value was originally generated by the <b>createShippingFulfillment</b> call, and is returned by the <b>getShippingFulfillments</b> call in the <b>members.fulfillmentId</b> field.
    /// </summary>
    public Task<ShippingFulfillment?> GetShippingFulfillmentAsync(
        string fulfillmentId,
        string orderId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Order[orderId].Shipping_fulfillment[fulfillmentId].GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Get Payment Dispute Details
    /// </summary>
    public Task<PaymentDispute?> GetPaymentDisputeAsync(
        string paymentDisputeId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Payment_dispute[paymentDisputeId].GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Get Payment Dispute Evidence File
    /// </summary>
    public Task FetchEvidenceContentAsync(
        string paymentDisputeId,
        string evidenceId,
        string fileId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Payment_dispute[paymentDisputeId].Fetch_evidence_content.GetAsync(cfg =>
        {
            cfg.QueryParameters.EvidenceId = evidenceId;
            cfg.QueryParameters.FileId = fileId;
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Get Payment Dispute Activity
    /// </summary>
    public Task<PaymentDisputeActivityHistory?> GetActivitiesAsync(
        string paymentDisputeId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Payment_dispute[paymentDisputeId].Activity.GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Search Payment Dispute by Filters
    /// </summary>
    public Task<DisputeSummaryResponse?> GetPaymentDisputeSummariesAsync(
        string? orderId = null,
        string? buyerUsername = null,
        string? openDateFrom = null,
        string? openDateTo = null,
        string? paymentDisputeStatus = null,
        string? limit = null,
        string? offset = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Payment_dispute_summary.GetAsync(cfg =>
        {
            if (orderId is not null)
            {
                cfg.QueryParameters.OrderId = orderId;
            }
            if (buyerUsername is not null)
            {
                cfg.QueryParameters.BuyerUsername = buyerUsername;
            }
            if (openDateFrom is not null)
            {
                cfg.QueryParameters.OpenDateFrom = openDateFrom;
            }
            if (openDateTo is not null)
            {
                cfg.QueryParameters.OpenDateTo = openDateTo;
            }
            if (paymentDisputeStatus is not null)
            {
                cfg.QueryParameters.PaymentDisputeStatus = paymentDisputeStatus;
            }
            if (limit is not null)
            {
                cfg.QueryParameters.Limit = limit;
            }
            if (offset is not null)
            {
                cfg.QueryParameters.Offset = offset;
            }
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Contest Payment Dispute
    /// </summary>
    public Task ContestPaymentDisputeAsync(
        string paymentDisputeId,
        ContestPaymentDisputeRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Payment_dispute[paymentDisputeId].Contest.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Accept Payment Dispute
    /// </summary>
    public Task AcceptPaymentDisputeAsync(
        string paymentDisputeId,
        AcceptPaymentDisputeRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Payment_dispute[paymentDisputeId].Accept.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Upload an Evidence File
    /// </summary>
    public Task<FileEvidence?> UploadEvidenceFileAsync(
        string paymentDisputeId,
        Stream fileContent,
        string fileName,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () =>
            {
                var requestInfo = _kiota.Payment_dispute[paymentDisputeId].Upload_evidence_file.ToPostRequestInformation();
                RestInvocation.ApplyHeaders(requestInfo.Headers, _config, requestOptions);
                return RestInvocation.SendMultipartFileAsync(
                    _adapter,
                    requestInfo,
                    "file",
                    fileContent,
                    fileName,
                    FileEvidence.CreateFromDiscriminatorValue,
                    ct);
            });

    /// <summary>
    /// Add an Evidence File
    /// </summary>
    public Task<AddEvidencePaymentDisputeResponse?> AddEvidenceAsync(
        string paymentDisputeId,
        AddEvidencePaymentDisputeRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Payment_dispute[paymentDisputeId].Add_evidence.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Update evidence
    /// </summary>
    public Task UpdateEvidenceAsync(
        string paymentDisputeId,
        UpdateEvidencePaymentDisputeRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Payment_dispute[paymentDisputeId].Update_evidence.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));
}
