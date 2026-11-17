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
    /// Get a seller order by ID
    /// </summary>
    public Task<Order?> GetOrderAsync(
        string orderId,
        Generated.Fulfillment.Order.Item.GetFieldGroupsQueryParameterType? fieldGroups = null,
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
    /// Search for seller orders
    /// </summary>
    public Task<OrderSearchPagedCollection?> GetOrdersAsync(
        Generated.Fulfillment.Order.GetFieldGroupsQueryParameterType? fieldGroups = null,
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
    /// Issues a full or partial refund to a buyer on behalf of the seller.
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
    /// Get all shipping fulfillments for an order
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
    /// Create a shipping fulfillment for an order
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
    /// Get a specific shipping fulfillment
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
    public Task<Stream?> FetchEvidenceContentAsync(
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
    public Task<UploadEvidenceFilePaymentDisputeResponse?> UploadEvidenceFileAsync(
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
                    UploadEvidenceFilePaymentDisputeResponse.CreateFromDiscriminatorValue,
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

    /// <summary>
    /// Create an additional shipping fulfillment for an order
    /// </summary>
    public Task CreateAdditionalShippingFulfillmentAsync(
        string orderId,
        ShippingTrackingDetails body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Order[orderId].Shipping_fulfillment.Create_additional.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));
}
