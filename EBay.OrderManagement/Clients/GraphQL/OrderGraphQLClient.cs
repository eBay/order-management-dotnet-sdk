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
using EBay.OrderManagement.Exceptions;
using EBay.OrderManagement.Generated.GraphQL;
using StrawberryShake;
using System.Net;
using System.Text.Json;

namespace EBay.OrderManagement.Clients.GraphQL;

public sealed class OrderGraphQLClient
{
    private readonly IOrderManagementGraphQLClient? _client;
    private readonly DynamicGraphQLClient _dynamic;
    private readonly ITokenContext _tokenContext;

    internal OrderGraphQLClient(IOrderManagementGraphQLClient? client, DynamicGraphQLClient dynamic, ITokenContext tokenContext)
        => (_client, _dynamic, _tokenContext) = (client, dynamic, tokenContext);

    private void EnsureTypedClientAvailable()
    {
        if (_client is null)
            throw new NotSupportedException(
                "Typed GraphQL operations require DI. Use ExecuteRawUnsafeAsync for standalone mode.");
    }

    // ── Orders ────────────────────────────────────────────────────────────────

    public Task<IOrders_Orders?> OrdersAsync(
        OrdersInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.Orders.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.Orders;
        });

    public Task<IOrdersByIds_OrdersByIds?> OrdersByIdsAsync(
        OrdersByIdsInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.OrdersByIds.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.OrdersByIds;
        });

    public Task<IMarkOrdersPaid_MarkOrdersPaid?> MarkOrdersPaidAsync(
        MarkOrdersPaidInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.MarkOrdersPaid.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.MarkOrdersPaid;
        });

    public Task<IBulkUpdateOrderTags_BulkUpdateOrderTags?> BulkUpdateOrderTagsAsync(
        BulkUpdateOrderTagsInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.BulkUpdateOrderTags.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.BulkUpdateOrderTags;
        });

    public Task<IOrderLineItemsByIds_OrderLineItemsByIds?> OrderLineItemsByIdsAsync(
        OrderLineItemsByIdsInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.OrderLineItemsByIds.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.OrderLineItemsByIds;
        });

    // ── Cancellations ─────────────────────────────────────────────────────────

    public Task<ISellerContractCancellations_SellerContractCancellations?> SellerContractCancellationsAsync(
        SellerContractCancellationsInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.SellerContractCancellations.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.SellerContractCancellations;
        });

    public Task<IContractCancellation_ContractCancellation?> ContractCancellationAsync(
        ContractCancellationInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.ContractCancellation.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.ContractCancellation;
        });

    public Task<ICreateOrderCancellation_CreateOrderCancellation?> CreateOrderCancellationAsync(
        CreateOrderCancellationInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.CreateOrderCancellation.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.CreateOrderCancellation;
        });

    public Task<IApproveContractCancellation_ApproveContractCancellation?> ApproveContractCancellationAsync(
        ApproveContractCancellationInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.ApproveContractCancellation.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.ApproveContractCancellation;
        });

    public Task<IDeclineContractCancellation_DeclineContractCancellation?> DeclineContractCancellationAsync(
        DeclineContractCancellationInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.DeclineContractCancellation.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.DeclineContractCancellation;
        });

    // ── Returns ───────────────────────────────────────────────────────────────

    public Task<IOrderReturn_OrderReturn?> OrderReturnAsync(
        OrderReturnInput input, ExchangeAmountScenario scenario, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.OrderReturn.ExecuteAsync(input, scenario, ct));
            EnsureNoErrors(result);
            return result.Data?.OrderReturn;
        });

    public Task<ISellerOrderReturns_SellerOrderReturns?> SellerOrderReturnsAsync(
        SellerOrderReturnsInput input, ExchangeAmountScenario scenario, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.SellerOrderReturns.ExecuteAsync(input, scenario, ct));
            EnsureNoErrors(result);
            return result.Data?.SellerOrderReturns;
        });

    public Task<IApproveReturn_ApproveReturn?> ApproveReturnAsync(
        ApproveReturnInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.ApproveReturn.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.ApproveReturn;
        });

    public Task<IDeclineReturn_DeclineReturn?> DeclineReturnAsync(
        DeclineReturnInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.DeclineReturn.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.DeclineReturn;
        });

    public Task<IProcessReturnRefund_ProcessReturnRefund?> ProcessReturnRefundAsync(
        ProcessReturnRefundInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.ProcessReturnRefund.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.ProcessReturnRefund;
        });

    public Task<ICreateInStoreReturn_CreateInStoreReturn?> CreateInStoreReturnAsync(
        CreateInStoreReturnInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.CreateInStoreReturn.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.CreateInStoreReturn;
        });

    public Task<IAttachReturnShipmentLabel_AttachReturnShipmentLabel?> AttachReturnShipmentLabelAsync(
        AttachReturnShipmentLabelInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.AttachReturnShipmentLabel.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.AttachReturnShipmentLabel;
        });

    public Task<IRecordReturnLabelSentOffline_RecordReturnLabelSentOffline?> RecordReturnLabelSentOfflineAsync(
        RecordReturnLabelSentOfflineInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.RecordReturnLabelSentOffline.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.RecordReturnLabelSentOffline;
        });

    public Task<IRecordReturnReceived_RecordReturnReceived?> RecordReturnReceivedAsync(
        RecordReturnReceivedInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.RecordReturnReceived.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.RecordReturnReceived;
        });

    public Task<ISendReturnMessage_SendReturnMessage?> SendReturnMessageAsync(
        SendReturnMessageInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.SendReturnMessage.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.SendReturnMessage;
        });

    public Task<ISubmitReturnReferral_SubmitReturnReferral?> SubmitReturnReferralAsync(
        SubmitReturnReferralInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.SubmitReturnReferral.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.SubmitReturnReferral;
        });

    public Task<IProposeKeepItemWithPartialRefund_ProposeKeepItemWithPartialRefund?> ProposeKeepItemWithPartialRefundAsync(
        ProposeKeepItemWithPartialRefundInput input, ExchangeAmountScenario scenario, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.ProposeKeepItemWithPartialRefund.ExecuteAsync(input, scenario, ct));
            EnsureNoErrors(result);
            return result.Data?.ProposeKeepItemWithPartialRefund;
        });

    public Task<IProvideRmaNumber_ProvideRmaNumber?> ProvideRmaNumberAsync(
        ProvideRmaNumberInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.ProvideRmaNumber.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.ProvideRmaNumber;
        });

    // ── Return Rules ──────────────────────────────────────────────────────────

    public Task<IReturnRuleListingGroup_ReturnRuleListingGroup?> ReturnRuleListingGroupAsync(
        ReturnRuleListingGroupInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.ReturnRuleListingGroup.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.ReturnRuleListingGroup;
        });

    public Task<ICreateReturnRuleListingGroup_CreateReturnRuleListingGroup?> CreateReturnRuleListingGroupAsync(
        CreateReturnRuleListingGroupInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.CreateReturnRuleListingGroup.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.CreateReturnRuleListingGroup;
        });

    public Task<IDeleteReturnRuleListingGroup_DeleteReturnRuleListingGroup?> DeleteReturnRuleListingGroupAsync(
        DeleteReturnRuleListingGroupInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.DeleteReturnRuleListingGroup.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.DeleteReturnRuleListingGroup;
        });

    public Task<IAddListingsToReturnRuleListingGroup_AddListingsToReturnRuleListingGroup?> AddListingsToReturnRuleListingGroupAsync(
        AddListingsToReturnRuleListingGroupInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.AddListingsToReturnRuleListingGroup.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.AddListingsToReturnRuleListingGroup;
        });

    public Task<ISellerReturnPreference_SellerReturnPreference?> SellerReturnPreferenceAsync(
        SellerReturnPreferenceInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.SellerReturnPreference.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.SellerReturnPreference;
        });

    public Task<IAppendSellerReturnResponseRule_AppendSellerReturnResponseRule?> AppendSellerReturnResponseRuleAsync(
        AppendSellerReturnResponseRuleInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.AppendSellerReturnResponseRule.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.AppendSellerReturnResponseRule;
        });

    public Task<IAppendSellerReturnToAddressRule_AppendSellerReturnToAddressRule?> AppendSellerReturnToAddressRuleAsync(
        AppendSellerReturnToAddressRuleInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.AppendSellerReturnToAddressRule.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.AppendSellerReturnToAddressRule;
        });

    public Task<IUpdateSellerReturnResponseRuleCondition_UpdateSellerReturnResponseRuleCondition?> UpdateSellerReturnResponseRuleConditionAsync(
        UpdateSellerReturnResponseRuleConditionInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.UpdateSellerReturnResponseRuleCondition.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.UpdateSellerReturnResponseRuleCondition;
        });

    public Task<IUpdateSellerReturnResponseRulesPriorities_UpdateSellerReturnResponseRulesPriorities?> UpdateSellerReturnResponseRulesPrioritiesAsync(
        UpdateSellerReturnResponseRulesPrioritiesInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.UpdateSellerReturnResponseRulesPriorities.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.UpdateSellerReturnResponseRulesPriorities;
        });

    public Task<IUpdateSellerReturnToAddressRuleCondition_UpdateSellerReturnToAddressRuleCondition?> UpdateSellerReturnToAddressRuleConditionAsync(
        UpdateSellerReturnToAddressRuleConditionInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.UpdateSellerReturnToAddressRuleCondition.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.UpdateSellerReturnToAddressRuleCondition;
        });

    public Task<IUpdateSellerReturnToAddressRulesPriorities_UpdateSellerReturnToAddressRulesPriorities?> UpdateSellerReturnToAddressRulesPrioritiesAsync(
        UpdateSellerReturnToAddressRulesPrioritiesInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.UpdateSellerReturnToAddressRulesPriorities.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.UpdateSellerReturnToAddressRulesPriorities;
        });

    public Task<IUpdateSellerReturnRuleStatus_UpdateSellerReturnRuleStatus?> UpdateSellerReturnRuleStatusAsync(
        UpdateSellerReturnRuleStatusInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.UpdateSellerReturnRuleStatus.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.UpdateSellerReturnRuleStatus;
        });

    public Task<IDeleteSellerReturnRule_DeleteSellerReturnRule?> DeleteSellerReturnRuleAsync(
        DeleteSellerReturnRuleInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.DeleteSellerReturnRule.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.DeleteSellerReturnRule;
        });

    public Task<IUpdateSellerRmaPreference_UpdateSellerRmaPreference?> UpdateSellerRmaPreferenceAsync(
        UpdateSellerRmaPreferenceInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.UpdateSellerRmaPreference.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.UpdateSellerRmaPreference;
        });

    // ── Order Inquiries ───────────────────────────────────────────────────────

    public Task<IOrderInquiry_OrderInquiry?> OrderInquiryAsync(
        OrderInquiryInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.OrderInquiry.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.OrderInquiry;
        });

    public Task<ISellerOrderInquiries_SellerOrderInquiries?> SellerOrderInquiriesAsync(
        SellerOrderInquiriesInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.SellerOrderInquiries.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.SellerOrderInquiries;
        });

    public Task<ISendOrderInquiryMessage_SendOrderInquiryMessage?> SendOrderInquiryMessageAsync(
        SendOrderInquiryMessageInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.SendOrderInquiryMessage.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.SendOrderInquiryMessage;
        });

    public Task<IProcessOrderInquiryRefund_ProcessOrderInquiryRefund?> ProcessOrderInquiryRefundAsync(
        ProcessOrderInquiryRefundInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.ProcessOrderInquiryRefund.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.ProcessOrderInquiryRefund;
        });

    public Task<ISubmitOrderInquiryReferral_SubmitOrderInquiryReferral?> SubmitOrderInquiryReferralAsync(
        SubmitOrderInquiryReferralInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.SubmitOrderInquiryReferral.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.SubmitOrderInquiryReferral;
        });

    public Task<IProvideShipmentInfoForItemNotReceivedInquiry_ProvideShipmentInfoForItemNotReceivedInquiry?> ProvideShipmentInfoForItemNotReceivedInquiryAsync(
        ProvideShipmentInfoForItemNotReceivedInquiryInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.ProvideShipmentInfoForItemNotReceivedInquiry.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.ProvideShipmentInfoForItemNotReceivedInquiry;
        });

    // ── Item Commitments ──────────────────────────────────────────────────────

    public Task<IItemCommitments_ItemCommitments?> ItemCommitmentsAsync(
        ItemCommitmentsInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.ItemCommitments.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.ItemCommitments;
        });

    public Task<IItemCommitmentsByIds_ItemCommitmentsByIds?> ItemCommitmentsByIdsAsync(
        ItemCommitmentsByIdsInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.ItemCommitmentsByIds.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.ItemCommitmentsByIds;
        });

    public Task<IMarkItemCommitmentsPaid_MarkItemCommitmentsPaid?> MarkItemCommitmentsPaidAsync(
        MarkItemCommitmentsPaidInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.MarkItemCommitmentsPaid.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.MarkItemCommitmentsPaid;
        });

    public Task<IBulkUpdateItemCommitmentTags_BulkUpdateItemCommitmentTags?> BulkUpdateItemCommitmentTagsAsync(
        BulkUpdateItemCommitmentTagsInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.BulkUpdateItemCommitmentTags.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.BulkUpdateItemCommitmentTags;
        });

    public Task<ICreateItemCommitmentCancellation_CreateItemCommitmentCancellation?> CreateItemCommitmentCancellationAsync(
        CreateItemCommitmentCancellationInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.CreateItemCommitmentCancellation.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.CreateItemCommitmentCancellation;
        });

    // ── Purchase Quotes ───────────────────────────────────────────────────────

    public Task<IPurchaseQuotes_PurchaseQuotes?> PurchaseQuotesAsync(
        PurchaseQuotesInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.PurchaseQuotes.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.PurchaseQuotes;
        });

    public Task<IPurchaseQuotesByIds_PurchaseQuotesByIds?> PurchaseQuotesByIdsAsync(
        PurchaseQuotesByIdsInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.PurchaseQuotesByIds.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.PurchaseQuotesByIds;
        });

    public Task<IPurchaseQuoteLineItemsByIds_PurchaseQuoteLineItemsByIds?> PurchaseQuoteLineItemsByIdsAsync(
        PurchaseQuoteLineItemsByIdsInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.PurchaseQuoteLineItemsByIds.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.PurchaseQuoteLineItemsByIds;
        });

    public Task<ICreatePurchaseQuote_CreatePurchaseQuote?> CreatePurchaseQuoteAsync(
        CreatePurchaseQuoteInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.CreatePurchaseQuote.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.CreatePurchaseQuote;
        });

    public Task<ICreatePurchaseQuoteCancellation_CreatePurchaseQuoteCancellation?> CreatePurchaseQuoteCancellationAsync(
        CreatePurchaseQuoteCancellationInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.CreatePurchaseQuoteCancellation.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.CreatePurchaseQuoteCancellation;
        });

    public Task<IBulkUpdatePurchaseQuotes_BulkUpdatePurchaseQuotes?> BulkUpdatePurchaseQuotesAsync(
        BulkUpdatePurchaseQuotesInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.BulkUpdatePurchaseQuotes.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.BulkUpdatePurchaseQuotes;
        });

    public Task<IRequestPurchaseQuote_RequestPurchaseQuote?> RequestPurchaseQuoteAsync(
        RequestPurchaseQuoteInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.RequestPurchaseQuote.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.RequestPurchaseQuote;
        });

    public Task<ICommitToPurchase_CommitToPurchase?> CommitToPurchaseAsync(
        CommitToPurchaseInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.CommitToPurchase.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.CommitToPurchase;
        });

    // ── Dynamic escape hatch ──────────────────────────────────────────────────

    /// <summary>
    /// Executes an arbitrary GraphQL query string against the API.
    /// <para>
    /// <b>WARNING — injection risk.</b> Never interpolate user-supplied input directly into
    /// <paramref name="query"/>. Always pass dynamic values via <paramref name="variables"/>
    /// so the server handles escaping. Treat this method like a raw SQL string: safe only
    /// with static, developer-controlled query text.
    /// </para>
    /// </summary>
    public Task<JsonDocument> ExecuteRawUnsafeAsync(
        string query, object? variables = null, string? accessToken = null, CancellationToken ct = default)
        => _dynamic.ExecuteRawUnsafeAsync(query, variables, accessToken, ct);

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static async Task<IOperationResult<T>> ExecuteAsync<T>(Func<Task<IOperationResult<T>>> call)
        where T : class
    {
        try { return await call(); }
        catch (Exception ex) when (ex is not SdkException) { throw WrapException(ex); }
    }

    private static SdkException WrapException(Exception ex) => ex switch
    {
        HttpRequestException { StatusCode: HttpStatusCode.Unauthorized } => new UnauthorizedException(ex),
        HttpRequestException { StatusCode: { } code } http =>
            new SdkException($"GraphQL request failed with HTTP {(int)code}: {http.Message}", ex),
        HttpRequestException http =>
            new SdkException($"GraphQL request failed (network error): {http.Message} — check that GraphQLBaseUrl is reachable.", ex),
        TaskCanceledException =>
            new SdkException("GraphQL request timed out — check that GraphQLBaseUrl is reachable and the network is accessible.", ex),
        InvalidOperationException { Message: var msg } when msg.Contains("execution attempt") =>
            new SdkException("GraphQL request returned no data; the endpoint may be unreachable or returned an unexpected response — check GraphQLBaseUrl.", ex),
        _ => new SdkException($"GraphQL request failed: {ex.Message}", ex)
    };

    private static void EnsureNoErrors<T>(IOperationResult<T> result) where T : class
    {
        if (result.Errors is { Count: > 0 })
        {
            var messages = result.Errors
                .Select(e => e.Message ?? e.Code ?? "unknown error")
                .ToArray();
            throw new GraphQLException(messages);
        }
    }
}
