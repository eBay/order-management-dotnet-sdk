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
using EBay.OrderManagement.Generated.GraphQL;
using System.Text.Json;

namespace EBay.OrderManagement.Clients.GraphQL;

public sealed class OrderGraphQLClient
{
    private readonly IOrderManagementGraphQLClient? _client;
    private readonly DynamicGraphQLClient _dynamic;
    private readonly ITokenContext _tokenContext;

    internal OrderGraphQLClient(
        IOrderManagementGraphQLClient? client,
        DynamicGraphQLClient dynamic,
        ITokenContext tokenContext)
        => (_client, _dynamic, _tokenContext) = (client, dynamic, tokenContext);

    private void EnsureTypedClientAvailable()
    {
        if (_client is null)
            throw new NotSupportedException(
                "Typed GraphQL operations require DI. Use ExecuteRawUnsafeAsync for standalone mode.");
    }

    public Task<IAddListingsToReturnRuleListingGroup_AddListingsToReturnRuleListingGroup?> AddListingsToReturnRuleListingGroupAsync(
        AddListingsToReturnRuleListingGroupInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.AddListingsToReturnRuleListingGroup.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.AddListingsToReturnRuleListingGroup;
        });

    public Task<IAppendSellerReturnResponseRule_AppendSellerReturnResponseRule?> AppendSellerReturnResponseRuleAsync(
        AppendSellerReturnResponseRuleInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.AppendSellerReturnResponseRule.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.AppendSellerReturnResponseRule;
        });

    public Task<IAppendSellerReturnToAddressRule_AppendSellerReturnToAddressRule?> AppendSellerReturnToAddressRuleAsync(
        AppendSellerReturnToAddressRuleInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.AppendSellerReturnToAddressRule.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.AppendSellerReturnToAddressRule;
        });

    public Task<IApproveContractCancellation_ApproveContractCancellation?> ApproveContractCancellationAsync(
        ApproveContractCancellationInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.ApproveContractCancellation.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.ApproveContractCancellation;
        });

    public Task<IApproveReturn_ApproveReturn?> ApproveReturnAsync(
        ApproveReturnInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.ApproveReturn.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.ApproveReturn;
        });

    public Task<IAttachReturnShipmentLabel_AttachReturnShipmentLabel?> AttachReturnShipmentLabelAsync(
        AttachReturnShipmentLabelInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.AttachReturnShipmentLabel.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.AttachReturnShipmentLabel;
        });

    public Task<IBulkUpdateItemCommitmentTags_BulkUpdateItemCommitmentTags?> BulkUpdateItemCommitmentTagsAsync(
        BulkUpdateItemCommitmentTagsInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.BulkUpdateItemCommitmentTags.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.BulkUpdateItemCommitmentTags;
        });

    public Task<IBulkUpdateOrderTags_BulkUpdateOrderTags?> BulkUpdateOrderTagsAsync(
        BulkUpdateOrderTagsInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.BulkUpdateOrderTags.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.BulkUpdateOrderTags;
        });

    public Task<IBulkUpdatePurchaseQuotes_BulkUpdatePurchaseQuotes?> BulkUpdatePurchaseQuotesAsync(
        BulkUpdatePurchaseQuotesInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.BulkUpdatePurchaseQuotes.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.BulkUpdatePurchaseQuotes;
        });

    public Task<ICommitToPurchase_CommitToPurchase?> CommitToPurchaseAsync(
        CommitToPurchaseInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.CommitToPurchase.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.CommitToPurchase;
        });

    public Task<IContractCancellation_ContractCancellation?> ContractCancellationAsync(
        ContractCancellationInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.ContractCancellation.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.ContractCancellation;
        });

    public Task<ICreateInStoreReturn_CreateInStoreReturn?> CreateInStoreReturnAsync(
        CreateInStoreReturnInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.CreateInStoreReturn.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.CreateInStoreReturn;
        });

    public Task<ICreateItemCommitmentCancellation_CreateItemCommitmentCancellation?> CreateItemCommitmentCancellationAsync(
        CreateItemCommitmentCancellationInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.CreateItemCommitmentCancellation.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.CreateItemCommitmentCancellation;
        });

    public Task<ICreateOrderCancellation_CreateOrderCancellation?> CreateOrderCancellationAsync(
        CreateOrderCancellationInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.CreateOrderCancellation.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.CreateOrderCancellation;
        });

    public Task<ICreatePurchaseQuote_CreatePurchaseQuote?> CreatePurchaseQuoteAsync(
        CreatePurchaseQuoteInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.CreatePurchaseQuote.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.CreatePurchaseQuote;
        });

    public Task<ICreatePurchaseQuoteCancellation_CreatePurchaseQuoteCancellation?> CreatePurchaseQuoteCancellationAsync(
        CreatePurchaseQuoteCancellationInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.CreatePurchaseQuoteCancellation.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.CreatePurchaseQuoteCancellation;
        });

    public Task<ICreateReturnRuleListingGroup_CreateReturnRuleListingGroup?> CreateReturnRuleListingGroupAsync(
        CreateReturnRuleListingGroupInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.CreateReturnRuleListingGroup.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.CreateReturnRuleListingGroup;
        });

    public Task<IDeclineContractCancellation_DeclineContractCancellation?> DeclineContractCancellationAsync(
        DeclineContractCancellationInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.DeclineContractCancellation.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.DeclineContractCancellation;
        });

    public Task<IDeclineReturn_DeclineReturn?> DeclineReturnAsync(
        DeclineReturnInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.DeclineReturn.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.DeclineReturn;
        });

    public Task<IDeleteReturnRuleListingGroup_DeleteReturnRuleListingGroup?> DeleteReturnRuleListingGroupAsync(
        DeleteReturnRuleListingGroupInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.DeleteReturnRuleListingGroup.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.DeleteReturnRuleListingGroup;
        });

    public Task<IDeleteSellerReturnRule_DeleteSellerReturnRule?> DeleteSellerReturnRuleAsync(
        DeleteSellerReturnRuleInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.DeleteSellerReturnRule.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.DeleteSellerReturnRule;
        });

    public Task<IItemCommitments_ItemCommitments?> ItemCommitmentsAsync(
        ItemCommitmentsInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.ItemCommitments.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.ItemCommitments;
        });

    public Task<IItemCommitmentsByIds_ItemCommitmentsByIds?> ItemCommitmentsByIdsAsync(
        ItemCommitmentsByIdsInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.ItemCommitmentsByIds.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.ItemCommitmentsByIds;
        });

    public Task<IMarkItemCommitmentsPaid_MarkItemCommitmentsPaid?> MarkItemCommitmentsPaidAsync(
        MarkItemCommitmentsPaidInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.MarkItemCommitmentsPaid.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.MarkItemCommitmentsPaid;
        });

    public Task<IMarkOrdersPaid_MarkOrdersPaid?> MarkOrdersPaidAsync(
        MarkOrdersPaidInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.MarkOrdersPaid.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.MarkOrdersPaid;
        });

    public Task<IOrderInquiry_OrderInquiry?> OrderInquiryAsync(
        OrderInquiryInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.OrderInquiry.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.OrderInquiry;
        });

    public Task<IOrderLineItemsByIds_OrderLineItemsByIds?> OrderLineItemsByIdsAsync(
        OrderLineItemsByIdsInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.OrderLineItemsByIds.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.OrderLineItemsByIds;
        });

    public Task<IOrderReturn_OrderReturn?> OrderReturnAsync(
        OrderReturnInput input,
        ExchangeAmountScenario scenario,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.OrderReturn.ExecuteAsync(input, scenario, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.OrderReturn;
        });

    public Task<IOrders_Orders?> OrdersAsync(
        OrdersInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.Orders.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.Orders;
        });

    public Task<IOrdersByIds_OrdersByIds?> OrdersByIdsAsync(
        OrdersByIdsInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.OrdersByIds.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.OrdersByIds;
        });

    public Task<IProcessOrderInquiryRefund_ProcessOrderInquiryRefund?> ProcessOrderInquiryRefundAsync(
        ProcessOrderInquiryRefundInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.ProcessOrderInquiryRefund.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.ProcessOrderInquiryRefund;
        });

    public Task<IProcessReturnRefund_ProcessReturnRefund?> ProcessReturnRefundAsync(
        ProcessReturnRefundInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.ProcessReturnRefund.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.ProcessReturnRefund;
        });

    public Task<IProposeKeepItemWithPartialRefund_ProposeKeepItemWithPartialRefund?> ProposeKeepItemWithPartialRefundAsync(
        ProposeKeepItemWithPartialRefundInput input,
        ExchangeAmountScenario scenario,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.ProposeKeepItemWithPartialRefund.ExecuteAsync(input, scenario, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.ProposeKeepItemWithPartialRefund;
        });

    public Task<IProvideRmaNumber_ProvideRmaNumber?> ProvideRmaNumberAsync(
        ProvideRmaNumberInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.ProvideRmaNumber.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.ProvideRmaNumber;
        });

    public Task<IProvideShipmentInfoForItemNotReceivedInquiry_ProvideShipmentInfoForItemNotReceivedInquiry?> ProvideShipmentInfoForItemNotReceivedInquiryAsync(
        ProvideShipmentInfoForItemNotReceivedInquiryInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.ProvideShipmentInfoForItemNotReceivedInquiry.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.ProvideShipmentInfoForItemNotReceivedInquiry;
        });

    public Task<IPurchaseQuoteLineItemsByIds_PurchaseQuoteLineItemsByIds?> PurchaseQuoteLineItemsByIdsAsync(
        PurchaseQuoteLineItemsByIdsInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.PurchaseQuoteLineItemsByIds.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.PurchaseQuoteLineItemsByIds;
        });

    public Task<IPurchaseQuotes_PurchaseQuotes?> PurchaseQuotesAsync(
        PurchaseQuotesInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.PurchaseQuotes.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.PurchaseQuotes;
        });

    public Task<IPurchaseQuotesByIds_PurchaseQuotesByIds?> PurchaseQuotesByIdsAsync(
        PurchaseQuotesByIdsInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.PurchaseQuotesByIds.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.PurchaseQuotesByIds;
        });

    public Task<IRecordReturnLabelSentOffline_RecordReturnLabelSentOffline?> RecordReturnLabelSentOfflineAsync(
        RecordReturnLabelSentOfflineInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.RecordReturnLabelSentOffline.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.RecordReturnLabelSentOffline;
        });

    public Task<IRecordReturnReceived_RecordReturnReceived?> RecordReturnReceivedAsync(
        RecordReturnReceivedInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.RecordReturnReceived.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.RecordReturnReceived;
        });

    public Task<IRequestPurchaseQuote_RequestPurchaseQuote?> RequestPurchaseQuoteAsync(
        RequestPurchaseQuoteInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.RequestPurchaseQuote.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.RequestPurchaseQuote;
        });

    public Task<IReturnRuleListingGroup_ReturnRuleListingGroup?> ReturnRuleListingGroupAsync(
        ReturnRuleListingGroupInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.ReturnRuleListingGroup.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.ReturnRuleListingGroup;
        });

    public Task<ISellerContractCancellations_SellerContractCancellations?> SellerContractCancellationsAsync(
        SellerContractCancellationsInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.SellerContractCancellations.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.SellerContractCancellations;
        });

    public Task<ISellerOrderInquiries_SellerOrderInquiries?> SellerOrderInquiriesAsync(
        SellerOrderInquiriesInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.SellerOrderInquiries.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.SellerOrderInquiries;
        });

    public Task<ISellerOrderReturns_SellerOrderReturns?> SellerOrderReturnsAsync(
        SellerOrderReturnsInput input,
        ExchangeAmountScenario scenario,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.SellerOrderReturns.ExecuteAsync(input, scenario, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.SellerOrderReturns;
        });

    public Task<ISellerReturnPreference_SellerReturnPreference?> SellerReturnPreferenceAsync(
        SellerReturnPreferenceInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.SellerReturnPreference.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.SellerReturnPreference;
        });

    public Task<ISendOrderInquiryMessage_SendOrderInquiryMessage?> SendOrderInquiryMessageAsync(
        SendOrderInquiryMessageInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.SendOrderInquiryMessage.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.SendOrderInquiryMessage;
        });

    public Task<ISendReturnMessage_SendReturnMessage?> SendReturnMessageAsync(
        SendReturnMessageInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.SendReturnMessage.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.SendReturnMessage;
        });

    public Task<ISubmitOrderInquiryReferral_SubmitOrderInquiryReferral?> SubmitOrderInquiryReferralAsync(
        SubmitOrderInquiryReferralInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.SubmitOrderInquiryReferral.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.SubmitOrderInquiryReferral;
        });

    public Task<ISubmitReturnReferral_SubmitReturnReferral?> SubmitReturnReferralAsync(
        SubmitReturnReferralInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.SubmitReturnReferral.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.SubmitReturnReferral;
        });

    public Task<IUpdateSellerReturnResponseRuleCondition_UpdateSellerReturnResponseRuleCondition?> UpdateSellerReturnResponseRuleConditionAsync(
        UpdateSellerReturnResponseRuleConditionInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.UpdateSellerReturnResponseRuleCondition.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.UpdateSellerReturnResponseRuleCondition;
        });

    public Task<IUpdateSellerReturnResponseRulesPriorities_UpdateSellerReturnResponseRulesPriorities?> UpdateSellerReturnResponseRulesPrioritiesAsync(
        UpdateSellerReturnResponseRulesPrioritiesInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.UpdateSellerReturnResponseRulesPriorities.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.UpdateSellerReturnResponseRulesPriorities;
        });

    public Task<IUpdateSellerReturnRuleStatus_UpdateSellerReturnRuleStatus?> UpdateSellerReturnRuleStatusAsync(
        UpdateSellerReturnRuleStatusInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.UpdateSellerReturnRuleStatus.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.UpdateSellerReturnRuleStatus;
        });

    public Task<IUpdateSellerReturnToAddressRuleCondition_UpdateSellerReturnToAddressRuleCondition?> UpdateSellerReturnToAddressRuleConditionAsync(
        UpdateSellerReturnToAddressRuleConditionInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.UpdateSellerReturnToAddressRuleCondition.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.UpdateSellerReturnToAddressRuleCondition;
        });

    public Task<IUpdateSellerReturnToAddressRulesPriorities_UpdateSellerReturnToAddressRulesPriorities?> UpdateSellerReturnToAddressRulesPrioritiesAsync(
        UpdateSellerReturnToAddressRulesPrioritiesInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.UpdateSellerReturnToAddressRulesPriorities.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.UpdateSellerReturnToAddressRulesPriorities;
        });

    public Task<IUpdateSellerRmaPreference_UpdateSellerRmaPreference?> UpdateSellerRmaPreferenceAsync(
        UpdateSellerRmaPreferenceInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.UpdateSellerRmaPreference.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.UpdateSellerRmaPreference;
        });

    public Task<JsonDocument> ExecuteRawUnsafeAsync(
        string query, object? variables = null, string? accessToken = null, CancellationToken ct = default)
        => _dynamic.ExecuteRawUnsafeAsync(query, variables, accessToken, ct);
}
