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


using Microsoft.Kiota.Abstractions;
using EBay.OrderManagement.Configuration;
using EBay.OrderManagement.Exceptions;
using EBay.OrderManagement.Generated.Fulfillment;
using EBay.OrderManagement.Generated.Fulfillment.Models;
using System.Globalization;
using System.Net;

namespace EBay.OrderManagement.Clients.Rest;

public sealed class FulfillmentClient
{
    private readonly FulfillmentApiClient _kiota;
    private readonly OrderManagementConfiguration _config;
    private readonly ITokenContext _tokenContext;

    internal FulfillmentClient(FulfillmentApiClient kiota, OrderManagementConfiguration config, ITokenContext tokenContext)
        => (_kiota, _config, _tokenContext) = (kiota, config, tokenContext);

    private void ApplyHeaders(RequestHeaders headers, RequestOptions? callOptions)
    {
        foreach (var (k, v) in _config.DefaultRequestOptions.Headers)
            headers.TryAdd(k, v);
        if (callOptions is null) return;
        foreach (var (k, v) in callOptions.Headers)
            headers[k] = new[] { v };
    }

    // ── Orders ────────────────────────────────────────────────────────────────

    public async Task<Order?> GetOrderAsync(
        string orderId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken,
                () => _kiota.Order[orderId].GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (ApiException ex) when (ex.ResponseStatusCode == (int)HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<OrderSearchPagedCollection?> GetOrdersAsync(
        string? filter = null,
        int? limit = null,
        int? offset = null,
        string? fieldGroups = null,
        string? orderIds = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () => _kiota.Order.GetAsync(config =>
            {
                if (!string.IsNullOrEmpty(filter))
                    config.QueryParameters.Filter = filter;
                if (limit.HasValue)
                    config.QueryParameters.Limit = limit.Value.ToString(CultureInfo.InvariantCulture);
                if (offset.HasValue)
                    config.QueryParameters.Offset = offset.Value.ToString(CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(fieldGroups))
                    config.QueryParameters.FieldGroups = fieldGroups;
                if (!string.IsNullOrEmpty(orderIds))
                    config.QueryParameters.OrderIds = orderIds;
                ApplyHeaders(config.Headers, requestOptions);
            }, ct));
        }
        catch (ApiException ex) when (ex.ResponseStatusCode == (int)HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<Refund?> IssueRefundAsync(
        string orderId,
        IssueRefundRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken,
                () => _kiota.Order[orderId].Issue_refund.PostAsync(body,
                    cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (ApiException ex) when (ex.ResponseStatusCode == (int)HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedException(ex);
        }
    }

    // ── Shipping Fulfillments ─────────────────────────────────────────────────

    public async Task<ShippingFulfillmentPagedCollection?> GetShippingFulfillmentsAsync(
        string orderId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken,
                () => _kiota.Order[orderId].Shipping_fulfillment.GetAsync(
                    cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (ApiException ex) when (ex.ResponseStatusCode == (int)HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<ShippingFulfillment?> GetShippingFulfillmentAsync(
        string orderId,
        string fulfillmentId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken,
                () => _kiota.Order[orderId].Shipping_fulfillment[fulfillmentId].GetAsync(
                    cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (ApiException ex) when (ex.ResponseStatusCode == (int)HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task CreateShippingFulfillmentAsync(
        string orderId,
        ShippingFulfillmentDetails body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
    {
        try
        {
            await _tokenContext.RunAsync(accessToken, async () =>
                { await _kiota.Order[orderId].Shipping_fulfillment.PostAsync(body,
                    cfg => ApplyHeaders(cfg.Headers, requestOptions), ct); });
        }
        catch (ApiException ex) when (ex.ResponseStatusCode == (int)HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedException(ex);
        }
    }

    // ── Payment Disputes ──────────────────────────────────────────────────────

    public async Task<PaymentDispute?> GetPaymentDisputeAsync(
        string paymentDisputeId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken,
                () => _kiota.Payment_dispute[paymentDisputeId].GetAsync(
                    cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (ApiException ex) when (ex.ResponseStatusCode == (int)HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task AcceptPaymentDisputeAsync(
        string paymentDisputeId,
        AcceptPaymentDisputeRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
    {
        try
        {
            await _tokenContext.RunAsync(accessToken,
                () => _kiota.Payment_dispute[paymentDisputeId].Accept.PostAsync(body,
                    cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (ApiException ex) when (ex.ResponseStatusCode == (int)HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task ContestPaymentDisputeAsync(
        string paymentDisputeId,
        ContestPaymentDisputeRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
    {
        try
        {
            await _tokenContext.RunAsync(accessToken,
                () => _kiota.Payment_dispute[paymentDisputeId].Contest.PostAsync(body,
                    cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (ApiException ex) when (ex.ResponseStatusCode == (int)HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<AddEvidencePaymentDisputeResponse?> AddEvidenceAsync(
        string paymentDisputeId,
        AddEvidencePaymentDisputeRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken,
                () => _kiota.Payment_dispute[paymentDisputeId].Add_evidence.PostAsync(body,
                    cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (ApiException ex) when (ex.ResponseStatusCode == (int)HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task UpdateEvidenceAsync(
        string paymentDisputeId,
        UpdateEvidencePaymentDisputeRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
    {
        try
        {
            await _tokenContext.RunAsync(accessToken,
                () => _kiota.Payment_dispute[paymentDisputeId].Update_evidence.PostAsync(body,
                    cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (ApiException ex) when (ex.ResponseStatusCode == (int)HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<FileEvidence?> UploadEvidenceFileAsync(
        string paymentDisputeId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken,
                () => _kiota.Payment_dispute[paymentDisputeId].Upload_evidence_file.PostAsync(
                    cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (ApiException ex) when (ex.ResponseStatusCode == (int)HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<Stream?> FetchEvidenceContentAsync(
        string paymentDisputeId,
        string evidenceId,
        string fileId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken,
                () => _kiota.Payment_dispute[paymentDisputeId].Fetch_evidence_content.GetAsync(config =>
                {
                    config.QueryParameters.EvidenceId = evidenceId;
                    config.QueryParameters.FileId = fileId;
                    ApplyHeaders(config.Headers, requestOptions);
                }, ct));
        }
        catch (ApiException ex) when (ex.ResponseStatusCode == (int)HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<PaymentDisputeActivityHistory?> GetPaymentDisputeActivityAsync(
        string paymentDisputeId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken,
                () => _kiota.Payment_dispute[paymentDisputeId].Activity.GetAsync(
                    cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (ApiException ex) when (ex.ResponseStatusCode == (int)HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<DisputeSummaryResponse?> GetPaymentDisputeSummariesAsync(
        string? buyerUsername = null,
        int? limit = null,
        int? offset = null,
        string? openDateFrom = null,
        string? openDateTo = null,
        string? orderId = null,
        string? paymentDisputeStatus = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () => _kiota.Payment_dispute_summary.GetAsync(config =>
            {
                if (!string.IsNullOrEmpty(buyerUsername))
                    config.QueryParameters.BuyerUsername = buyerUsername;
                if (limit.HasValue)
                    config.QueryParameters.Limit = limit.Value.ToString(CultureInfo.InvariantCulture);
                if (offset.HasValue)
                    config.QueryParameters.Offset = offset.Value.ToString(CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(openDateFrom))
                    config.QueryParameters.OpenDateFrom = openDateFrom;
                if (!string.IsNullOrEmpty(openDateTo))
                    config.QueryParameters.OpenDateTo = openDateTo;
                if (!string.IsNullOrEmpty(orderId))
                    config.QueryParameters.OrderId = orderId;
                if (!string.IsNullOrEmpty(paymentDisputeStatus))
                    config.QueryParameters.PaymentDisputeStatus = paymentDisputeStatus;
                ApplyHeaders(config.Headers, requestOptions);
            }, ct));
        }
        catch (ApiException ex) when (ex.ResponseStatusCode == (int)HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedException(ex);
        }
    }
}
