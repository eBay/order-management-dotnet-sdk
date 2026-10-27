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
using EBay.OrderManagement.Generated.Logistics.Models;
using EBay.OrderManagement.Generated.Logistics;
using Microsoft.Kiota.Abstractions;

namespace EBay.OrderManagement.Clients.Rest;

public sealed class LogisticsClient
{
    private readonly LogisticsApiClient _kiota;
    private readonly OrderManagementConfiguration _config;
    private readonly ITokenContext _tokenContext;
    private readonly IRequestAdapter _adapter;

    internal LogisticsClient(LogisticsApiClient kiota, OrderManagementConfiguration config, ITokenContext tokenContext, IRequestAdapter adapter)
    {
        _kiota = kiota;
        _config = config;
        _tokenContext = tokenContext;
        _adapter = adapter;
    }

    /// <summary>
    /// This method cancels the shipment associated with the specified shipment ID and the associated shipping label is deleted. When you cancel a shipment, the <b>totalShippingCost</b> of the canceled shipment is refunded to the account established by the user's billing agreement.
    /// Note that you cannot cancel a shipment if you have used the associated shipping label.
    /// </summary>
    public Task<Shipment?> CancelShipmentAsync(
        string shipmentId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Shipment[shipmentId].Cancel.PostAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method creates a shipment based on the <b>shippingQuoteId</b> and <b>rateId</b> values supplied in the request. The rate identified by the <b>rateId</b> value specifies the carrier and service for the package shipment, and the rate ID must be contained in the shipping quote identified by the <b>shippingQuoteId</b> value. Call <b>createShippingQuote</b> to retrieve a set of live shipping rates.
    /// <b>Note:</b> The Logistics API only supports USPS shipping rates and labels.
    /// When you create a shipment, eBay generates a shipping label that you can download and use to ship your package.
    /// In a <b>createFromShippingQuote</b> request, sellers can include a list of shipping options they want to add to the base service quoted in the selected rate. The list of available shipping options is specific to each quoted rate and if available, the options are listed in the rate container of the shipping quote.
    /// In addition to a configurable return-to location and other details about the shipment, the response to this method includes:
    /// - The shipping carrier and service to be used for the package shipment
    /// - A list of selected shipping options, if any
    /// - The shipment tracking number
    /// - The total shipping cost (the sum cost of the base shipping service and any added options)
    /// When you create a shipment, your billing agreement account is charged the sum of the <b>baseShippingCost</b> and the total cost of any additional shipping options you might have selected. Use the URL returned in <b>labelDownloadUrl</b> field, or call <b>downloadLabelFile</b> with the <b>shipmentId</b> value from the response, to download a shipping label for your package.
    /// Important! "&gt;<b>Important!</b> Sellers must set up their payment method before they can use this method to create a shipment and the associated shipping label.
    /// Set up a billing agreement
    /// Prior to using this method to create a shipment, sellers must first set up their billing agreement. Failure to do so will return <c>Error 90030 Payment could not be completed.</c>
    /// The preferred method for sellers to set up their billing agreement is to go to <see href="https://www.ebay.com/ship/bulk/payments">Set up billing agreement</see> and follow the on-screen directions.
    /// Alternatively, sellers can do the following:
    /// - Go to https://www.ebay.com/ship/single/{order_id}, where {order_id} is that of the order for which the label is being printed.
    /// - When prompted, select <b>PayPal</b>.
    /// - Verify that <b>Save PayPal for future purchases</b> is selected.
    /// - Click <b>Set up Payments</b> which will open PayPal in a pop-up window.
    /// - Log in using <i>PayPal credentials</i>, and then follow the on-screen prompts to set up the billing agreement.
    /// - Once the agreement has been set up, sellers can leave this page as there is no need to actually print a label.
    /// </summary>
    public Task<Shipment?> CreateFromShippingQuoteAsync(
        string xEBAYCMARKETPLACEID,
        CreateShipmentFromQuoteRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Shipment.Create_from_shipping_quote.PostAsync(body, cfg =>
        {
            cfg.Headers.TryAdd("X-EBAY-C-MARKETPLACE-ID", xEBAYCMARKETPLACEID);
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method returns the shipping label file that was generated for the <b>shipmentId</b> value specified in the request. Call <b>createFromShippingQuote</b> to generate a shipment ID.
    /// <b>Note:</b> The Logistics API only supports USPS shipping rates and labels.
    /// Use the <c>Accept</c> HTTP header to specify the format of the returned file. The default file format is a PDF file.
    /// </summary>
    public Task DownloadLabelFileAsync(
        string shipmentId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Shipment[shipmentId].Download_label_file.GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method retrieves the shipment details for the specified shipment ID. Call <b>createFromShippingQuote</b> to generate a shipment ID.
    /// </summary>
    public Task<Shipment?> GetShipmentAsync(
        string shipmentId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Shipment[shipmentId].GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// The <b>createShippingQuote</b> method returns a <i>shipping quote </i> that contains a list of live "rates."
    /// Each rate represents an offer made by a shipping carrier for a specific service and each offer has a live quote for the base service cost. Rates have a time window in which they are "live," and rates expire when their purchase window ends. If offered by the carrier, rates can include shipping options (and their associated prices), and users can add any offered shipping option to the base service should they desire. Also, depending on the services required, rates can also include pickup and delivery windows.
    /// <b>Note:</b> The Logistics API only supports USPS shipping rates and labels.
    /// Each rate is for a single package and is based on the following information:
    /// - The shipping origin
    /// - The shipping destination
    /// - The package size (weight and dimensions)
    /// Rates are identified by a unique eBay-assigned <b>rateId</b> and rates are based on price points, pickup and delivery time frames, and other user requirements. Because each rate offered must be compliant with the eBay shipping program, all rates reflect eBay-negotiated prices.
    /// The various rates returned in a shipping quote offer the user a choice from which they can choose a shipping service that best fits their needs. Select the rate for your shipment and using the associated <b>rateId</b>, call <b>createFromShippingQuote</b> to create a shipment and generate a shipping label that you can use to ship the package.
    /// </summary>
    public Task<ShippingQuote?> CreateShippingQuoteAsync(
        string xEBAYCMARKETPLACEID,
        ShippingQuoteRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Shipping_quote.PostAsync(body, cfg =>
        {
            cfg.Headers.TryAdd("X-EBAY-C-MARKETPLACE-ID", xEBAYCMARKETPLACEID);
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method retrieves the complete details of the shipping quote associated with the specified <b>shippingQuoteId</b> value.
    /// A "shipping quote" pertains to a single specific package and contains a set of shipping "rates" that quote the cost to ship the package by different shipping carriers and services. The quotes are based on the package's origin, destination, and size.
    /// Call <b>createShippingQuote</b> to create a <b>shippingQuoteId</b>.
    /// </summary>
    public Task<ShippingQuote?> GetShippingQuoteAsync(
        string shippingQuoteId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Shipping_quote[shippingQuoteId].GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));
}
