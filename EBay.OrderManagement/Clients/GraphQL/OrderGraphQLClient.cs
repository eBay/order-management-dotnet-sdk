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
    private readonly GraphQLExecutor _executor;
    private readonly DynamicGraphQLClient _dynamic;
    private readonly ITokenContext _tokenContext;

    internal OrderGraphQLClient(
        GraphQLExecutor executor,
        DynamicGraphQLClient dynamic,
        ITokenContext tokenContext)
        => (_executor, _dynamic, _tokenContext) = (executor, dynamic, tokenContext);

    public Task<IAddListingsToReturnRuleListingGroup_AddListingsToReturnRuleListingGroup?> AddListingsToReturnRuleListingGroupAsync(
        AddListingsToReturnRuleListingGroupInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                AddListingsToReturnRuleListingGroupOperation.OperationName,
                AddListingsToReturnRuleListingGroupOperation.Document,
                writer => AddListingsToReturnRuleListingGroupOperation.WriteVariables(writer, input),
                AddListingsToReturnRuleListingGroupOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.AddListingsToReturnRuleListingGroup;
        });

    public Task<IAppendSellerReturnResponseRule_AppendSellerReturnResponseRule?> AppendSellerReturnResponseRuleAsync(
        AppendSellerReturnResponseRuleInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                AppendSellerReturnResponseRuleOperation.OperationName,
                AppendSellerReturnResponseRuleOperation.Document,
                writer => AppendSellerReturnResponseRuleOperation.WriteVariables(writer, input),
                AppendSellerReturnResponseRuleOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.AppendSellerReturnResponseRule;
        });

    public Task<IAppendSellerReturnToAddressRule_AppendSellerReturnToAddressRule?> AppendSellerReturnToAddressRuleAsync(
        AppendSellerReturnToAddressRuleInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                AppendSellerReturnToAddressRuleOperation.OperationName,
                AppendSellerReturnToAddressRuleOperation.Document,
                writer => AppendSellerReturnToAddressRuleOperation.WriteVariables(writer, input),
                AppendSellerReturnToAddressRuleOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.AppendSellerReturnToAddressRule;
        });

    public Task<IApproveContractCancellation_ApproveContractCancellation?> ApproveContractCancellationAsync(
        ApproveContractCancellationInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                ApproveContractCancellationOperation.OperationName,
                ApproveContractCancellationOperation.Document,
                writer => ApproveContractCancellationOperation.WriteVariables(writer, input),
                ApproveContractCancellationOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.ApproveContractCancellation;
        });

    public Task<IApproveReturn_ApproveReturn?> ApproveReturnAsync(
        ApproveReturnInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                ApproveReturnOperation.OperationName,
                ApproveReturnOperation.Document,
                writer => ApproveReturnOperation.WriteVariables(writer, input),
                ApproveReturnOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.ApproveReturn;
        });

    public Task<IAttachReturnShipmentLabel_AttachReturnShipmentLabel?> AttachReturnShipmentLabelAsync(
        AttachReturnShipmentLabelInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                AttachReturnShipmentLabelOperation.OperationName,
                AttachReturnShipmentLabelOperation.Document,
                writer => AttachReturnShipmentLabelOperation.WriteVariables(writer, input),
                AttachReturnShipmentLabelOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.AttachReturnShipmentLabel;
        });

    public Task<IBulkUpdateItemCommitmentTags_BulkUpdateItemCommitmentTags?> BulkUpdateItemCommitmentTagsAsync(
        BulkUpdateItemCommitmentTagsInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                BulkUpdateItemCommitmentTagsOperation.OperationName,
                BulkUpdateItemCommitmentTagsOperation.Document,
                writer => BulkUpdateItemCommitmentTagsOperation.WriteVariables(writer, input),
                BulkUpdateItemCommitmentTagsOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.BulkUpdateItemCommitmentTags;
        });

    public Task<IBulkUpdateOrderTags_BulkUpdateOrderTags?> BulkUpdateOrderTagsAsync(
        BulkUpdateOrderTagsInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                BulkUpdateOrderTagsOperation.OperationName,
                BulkUpdateOrderTagsOperation.Document,
                writer => BulkUpdateOrderTagsOperation.WriteVariables(writer, input),
                BulkUpdateOrderTagsOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.BulkUpdateOrderTags;
        });

    public Task<IBulkUpdatePurchaseQuotes_BulkUpdatePurchaseQuotes?> BulkUpdatePurchaseQuotesAsync(
        BulkUpdatePurchaseQuotesInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                BulkUpdatePurchaseQuotesOperation.OperationName,
                BulkUpdatePurchaseQuotesOperation.Document,
                writer => BulkUpdatePurchaseQuotesOperation.WriteVariables(writer, input),
                BulkUpdatePurchaseQuotesOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.BulkUpdatePurchaseQuotes;
        });

    public Task<ICommitToPurchase_CommitToPurchase?> CommitToPurchaseAsync(
        CommitToPurchaseInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                CommitToPurchaseOperation.OperationName,
                CommitToPurchaseOperation.Document,
                writer => CommitToPurchaseOperation.WriteVariables(writer, input),
                CommitToPurchaseOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.CommitToPurchase;
        });

    public Task<IContractCancellation_ContractCancellation?> ContractCancellationAsync(
        ContractCancellationInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                ContractCancellationOperation.OperationName,
                ContractCancellationOperation.Document,
                writer => ContractCancellationOperation.WriteVariables(writer, input),
                ContractCancellationOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.ContractCancellation;
        });

    public Task<ICreateInStoreReturn_CreateInStoreReturn?> CreateInStoreReturnAsync(
        CreateInStoreReturnInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                CreateInStoreReturnOperation.OperationName,
                CreateInStoreReturnOperation.Document,
                writer => CreateInStoreReturnOperation.WriteVariables(writer, input),
                CreateInStoreReturnOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.CreateInStoreReturn;
        });

    public Task<ICreateItemCommitmentCancellation_CreateItemCommitmentCancellation?> CreateItemCommitmentCancellationAsync(
        CreateItemCommitmentCancellationInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                CreateItemCommitmentCancellationOperation.OperationName,
                CreateItemCommitmentCancellationOperation.Document,
                writer => CreateItemCommitmentCancellationOperation.WriteVariables(writer, input),
                CreateItemCommitmentCancellationOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.CreateItemCommitmentCancellation;
        });

    public Task<ICreateOrderCancellation_CreateOrderCancellation?> CreateOrderCancellationAsync(
        CreateOrderCancellationInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                CreateOrderCancellationOperation.OperationName,
                CreateOrderCancellationOperation.Document,
                writer => CreateOrderCancellationOperation.WriteVariables(writer, input),
                CreateOrderCancellationOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.CreateOrderCancellation;
        });

    public Task<ICreatePurchaseQuote_CreatePurchaseQuote?> CreatePurchaseQuoteAsync(
        CreatePurchaseQuoteInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                CreatePurchaseQuoteOperation.OperationName,
                CreatePurchaseQuoteOperation.Document,
                writer => CreatePurchaseQuoteOperation.WriteVariables(writer, input),
                CreatePurchaseQuoteOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.CreatePurchaseQuote;
        });

    public Task<ICreatePurchaseQuoteCancellation_CreatePurchaseQuoteCancellation?> CreatePurchaseQuoteCancellationAsync(
        CreatePurchaseQuoteCancellationInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                CreatePurchaseQuoteCancellationOperation.OperationName,
                CreatePurchaseQuoteCancellationOperation.Document,
                writer => CreatePurchaseQuoteCancellationOperation.WriteVariables(writer, input),
                CreatePurchaseQuoteCancellationOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.CreatePurchaseQuoteCancellation;
        });

    public Task<ICreateReturnRuleListingGroup_CreateReturnRuleListingGroup?> CreateReturnRuleListingGroupAsync(
        CreateReturnRuleListingGroupInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                CreateReturnRuleListingGroupOperation.OperationName,
                CreateReturnRuleListingGroupOperation.Document,
                writer => CreateReturnRuleListingGroupOperation.WriteVariables(writer, input),
                CreateReturnRuleListingGroupOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.CreateReturnRuleListingGroup;
        });

    public Task<IDeclineContractCancellation_DeclineContractCancellation?> DeclineContractCancellationAsync(
        DeclineContractCancellationInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                DeclineContractCancellationOperation.OperationName,
                DeclineContractCancellationOperation.Document,
                writer => DeclineContractCancellationOperation.WriteVariables(writer, input),
                DeclineContractCancellationOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.DeclineContractCancellation;
        });

    public Task<IDeclineReturn_DeclineReturn?> DeclineReturnAsync(
        DeclineReturnInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                DeclineReturnOperation.OperationName,
                DeclineReturnOperation.Document,
                writer => DeclineReturnOperation.WriteVariables(writer, input),
                DeclineReturnOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.DeclineReturn;
        });

    public Task<IDeleteReturnRuleListingGroup_DeleteReturnRuleListingGroup?> DeleteReturnRuleListingGroupAsync(
        DeleteReturnRuleListingGroupInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                DeleteReturnRuleListingGroupOperation.OperationName,
                DeleteReturnRuleListingGroupOperation.Document,
                writer => DeleteReturnRuleListingGroupOperation.WriteVariables(writer, input),
                DeleteReturnRuleListingGroupOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.DeleteReturnRuleListingGroup;
        });

    public Task<IDeleteSellerReturnRule_DeleteSellerReturnRule?> DeleteSellerReturnRuleAsync(
        DeleteSellerReturnRuleInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                DeleteSellerReturnRuleOperation.OperationName,
                DeleteSellerReturnRuleOperation.Document,
                writer => DeleteSellerReturnRuleOperation.WriteVariables(writer, input),
                DeleteSellerReturnRuleOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.DeleteSellerReturnRule;
        });

    public Task<IItemCommitments_ItemCommitments?> ItemCommitmentsAsync(
        ItemCommitmentsInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                ItemCommitmentsOperation.OperationName,
                ItemCommitmentsOperation.Document,
                writer => ItemCommitmentsOperation.WriteVariables(writer, input),
                ItemCommitmentsOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.ItemCommitments;
        });

    public Task<IItemCommitmentsByIds_ItemCommitmentsByIds?> ItemCommitmentsByIdsAsync(
        ItemCommitmentsByIdsInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                ItemCommitmentsByIdsOperation.OperationName,
                ItemCommitmentsByIdsOperation.Document,
                writer => ItemCommitmentsByIdsOperation.WriteVariables(writer, input),
                ItemCommitmentsByIdsOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.ItemCommitmentsByIds;
        });

    public Task<IMarkItemCommitmentsPaid_MarkItemCommitmentsPaid?> MarkItemCommitmentsPaidAsync(
        MarkItemCommitmentsPaidInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                MarkItemCommitmentsPaidOperation.OperationName,
                MarkItemCommitmentsPaidOperation.Document,
                writer => MarkItemCommitmentsPaidOperation.WriteVariables(writer, input),
                MarkItemCommitmentsPaidOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.MarkItemCommitmentsPaid;
        });

    public Task<IMarkOrdersPaid_MarkOrdersPaid?> MarkOrdersPaidAsync(
        MarkOrdersPaidInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                MarkOrdersPaidOperation.OperationName,
                MarkOrdersPaidOperation.Document,
                writer => MarkOrdersPaidOperation.WriteVariables(writer, input),
                MarkOrdersPaidOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.MarkOrdersPaid;
        });

    public Task<IOrderInquiry_OrderInquiry?> OrderInquiryAsync(
        OrderInquiryInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                OrderInquiryOperation.OperationName,
                OrderInquiryOperation.Document,
                writer => OrderInquiryOperation.WriteVariables(writer, input),
                OrderInquiryOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.OrderInquiry;
        });

    public Task<IOrderLineItemsByIds_OrderLineItemsByIds?> OrderLineItemsByIdsAsync(
        OrderLineItemsByIdsInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                OrderLineItemsByIdsOperation.OperationName,
                OrderLineItemsByIdsOperation.Document,
                writer => OrderLineItemsByIdsOperation.WriteVariables(writer, input),
                OrderLineItemsByIdsOperation.Parse,
                ct)).ConfigureAwait(false);
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
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                OrderReturnOperation.OperationName,
                OrderReturnOperation.Document,
                writer => OrderReturnOperation.WriteVariables(writer, input, scenario),
                OrderReturnOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.OrderReturn;
        });

    public Task<IOrders_Orders?> OrdersAsync(
        OrdersInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                OrdersOperation.OperationName,
                OrdersOperation.Document,
                writer => OrdersOperation.WriteVariables(writer, input),
                OrdersOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.Orders;
        });

    public Task<IOrdersByIds_OrdersByIds?> OrdersByIdsAsync(
        OrdersByIdsInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                OrdersByIdsOperation.OperationName,
                OrdersByIdsOperation.Document,
                writer => OrdersByIdsOperation.WriteVariables(writer, input),
                OrdersByIdsOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.OrdersByIds;
        });

    public Task<IProcessOrderInquiryRefund_ProcessOrderInquiryRefund?> ProcessOrderInquiryRefundAsync(
        ProcessOrderInquiryRefundInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                ProcessOrderInquiryRefundOperation.OperationName,
                ProcessOrderInquiryRefundOperation.Document,
                writer => ProcessOrderInquiryRefundOperation.WriteVariables(writer, input),
                ProcessOrderInquiryRefundOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.ProcessOrderInquiryRefund;
        });

    public Task<IProcessReturnRefund_ProcessReturnRefund?> ProcessReturnRefundAsync(
        ProcessReturnRefundInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                ProcessReturnRefundOperation.OperationName,
                ProcessReturnRefundOperation.Document,
                writer => ProcessReturnRefundOperation.WriteVariables(writer, input),
                ProcessReturnRefundOperation.Parse,
                ct)).ConfigureAwait(false);
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
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                ProposeKeepItemWithPartialRefundOperation.OperationName,
                ProposeKeepItemWithPartialRefundOperation.Document,
                writer => ProposeKeepItemWithPartialRefundOperation.WriteVariables(writer, input, scenario),
                ProposeKeepItemWithPartialRefundOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.ProposeKeepItemWithPartialRefund;
        });

    public Task<IProvideRmaNumber_ProvideRmaNumber?> ProvideRmaNumberAsync(
        ProvideRmaNumberInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                ProvideRmaNumberOperation.OperationName,
                ProvideRmaNumberOperation.Document,
                writer => ProvideRmaNumberOperation.WriteVariables(writer, input),
                ProvideRmaNumberOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.ProvideRmaNumber;
        });

    public Task<IProvideShipmentInfoForItemNotReceivedInquiry_ProvideShipmentInfoForItemNotReceivedInquiry?> ProvideShipmentInfoForItemNotReceivedInquiryAsync(
        ProvideShipmentInfoForItemNotReceivedInquiryInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                ProvideShipmentInfoForItemNotReceivedInquiryOperation.OperationName,
                ProvideShipmentInfoForItemNotReceivedInquiryOperation.Document,
                writer => ProvideShipmentInfoForItemNotReceivedInquiryOperation.WriteVariables(writer, input),
                ProvideShipmentInfoForItemNotReceivedInquiryOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.ProvideShipmentInfoForItemNotReceivedInquiry;
        });

    public Task<IPurchaseQuoteLineItemsByIds_PurchaseQuoteLineItemsByIds?> PurchaseQuoteLineItemsByIdsAsync(
        PurchaseQuoteLineItemsByIdsInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                PurchaseQuoteLineItemsByIdsOperation.OperationName,
                PurchaseQuoteLineItemsByIdsOperation.Document,
                writer => PurchaseQuoteLineItemsByIdsOperation.WriteVariables(writer, input),
                PurchaseQuoteLineItemsByIdsOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.PurchaseQuoteLineItemsByIds;
        });

    public Task<IPurchaseQuotes_PurchaseQuotes?> PurchaseQuotesAsync(
        PurchaseQuotesInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                PurchaseQuotesOperation.OperationName,
                PurchaseQuotesOperation.Document,
                writer => PurchaseQuotesOperation.WriteVariables(writer, input),
                PurchaseQuotesOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.PurchaseQuotes;
        });

    public Task<IPurchaseQuotesByIds_PurchaseQuotesByIds?> PurchaseQuotesByIdsAsync(
        PurchaseQuotesByIdsInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                PurchaseQuotesByIdsOperation.OperationName,
                PurchaseQuotesByIdsOperation.Document,
                writer => PurchaseQuotesByIdsOperation.WriteVariables(writer, input),
                PurchaseQuotesByIdsOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.PurchaseQuotesByIds;
        });

    public Task<IRecordReturnLabelSentOffline_RecordReturnLabelSentOffline?> RecordReturnLabelSentOfflineAsync(
        RecordReturnLabelSentOfflineInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                RecordReturnLabelSentOfflineOperation.OperationName,
                RecordReturnLabelSentOfflineOperation.Document,
                writer => RecordReturnLabelSentOfflineOperation.WriteVariables(writer, input),
                RecordReturnLabelSentOfflineOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.RecordReturnLabelSentOffline;
        });

    public Task<IRecordReturnReceived_RecordReturnReceived?> RecordReturnReceivedAsync(
        RecordReturnReceivedInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                RecordReturnReceivedOperation.OperationName,
                RecordReturnReceivedOperation.Document,
                writer => RecordReturnReceivedOperation.WriteVariables(writer, input),
                RecordReturnReceivedOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.RecordReturnReceived;
        });

    public Task<IRequestPurchaseQuote_RequestPurchaseQuote?> RequestPurchaseQuoteAsync(
        RequestPurchaseQuoteInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                RequestPurchaseQuoteOperation.OperationName,
                RequestPurchaseQuoteOperation.Document,
                writer => RequestPurchaseQuoteOperation.WriteVariables(writer, input),
                RequestPurchaseQuoteOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.RequestPurchaseQuote;
        });

    public Task<IReturnRuleListingGroup_ReturnRuleListingGroup?> ReturnRuleListingGroupAsync(
        ReturnRuleListingGroupInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                ReturnRuleListingGroupOperation.OperationName,
                ReturnRuleListingGroupOperation.Document,
                writer => ReturnRuleListingGroupOperation.WriteVariables(writer, input),
                ReturnRuleListingGroupOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.ReturnRuleListingGroup;
        });

    public Task<ISellerContractCancellations_SellerContractCancellations?> SellerContractCancellationsAsync(
        SellerContractCancellationsInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                SellerContractCancellationsOperation.OperationName,
                SellerContractCancellationsOperation.Document,
                writer => SellerContractCancellationsOperation.WriteVariables(writer, input),
                SellerContractCancellationsOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.SellerContractCancellations;
        });

    public Task<ISellerOrderInquiries_SellerOrderInquiries?> SellerOrderInquiriesAsync(
        SellerOrderInquiriesInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                SellerOrderInquiriesOperation.OperationName,
                SellerOrderInquiriesOperation.Document,
                writer => SellerOrderInquiriesOperation.WriteVariables(writer, input),
                SellerOrderInquiriesOperation.Parse,
                ct)).ConfigureAwait(false);
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
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                SellerOrderReturnsOperation.OperationName,
                SellerOrderReturnsOperation.Document,
                writer => SellerOrderReturnsOperation.WriteVariables(writer, input, scenario),
                SellerOrderReturnsOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.SellerOrderReturns;
        });

    public Task<ISellerReturnPreference_SellerReturnPreference?> SellerReturnPreferenceAsync(
        SellerReturnPreferenceInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                SellerReturnPreferenceOperation.OperationName,
                SellerReturnPreferenceOperation.Document,
                writer => SellerReturnPreferenceOperation.WriteVariables(writer, input),
                SellerReturnPreferenceOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.SellerReturnPreference;
        });

    public Task<ISendOrderInquiryMessage_SendOrderInquiryMessage?> SendOrderInquiryMessageAsync(
        SendOrderInquiryMessageInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                SendOrderInquiryMessageOperation.OperationName,
                SendOrderInquiryMessageOperation.Document,
                writer => SendOrderInquiryMessageOperation.WriteVariables(writer, input),
                SendOrderInquiryMessageOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.SendOrderInquiryMessage;
        });

    public Task<ISendReturnMessage_SendReturnMessage?> SendReturnMessageAsync(
        SendReturnMessageInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                SendReturnMessageOperation.OperationName,
                SendReturnMessageOperation.Document,
                writer => SendReturnMessageOperation.WriteVariables(writer, input),
                SendReturnMessageOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.SendReturnMessage;
        });

    public Task<ISubmitOrderInquiryReferral_SubmitOrderInquiryReferral?> SubmitOrderInquiryReferralAsync(
        SubmitOrderInquiryReferralInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                SubmitOrderInquiryReferralOperation.OperationName,
                SubmitOrderInquiryReferralOperation.Document,
                writer => SubmitOrderInquiryReferralOperation.WriteVariables(writer, input),
                SubmitOrderInquiryReferralOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.SubmitOrderInquiryReferral;
        });

    public Task<ISubmitReturnReferral_SubmitReturnReferral?> SubmitReturnReferralAsync(
        SubmitReturnReferralInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                SubmitReturnReferralOperation.OperationName,
                SubmitReturnReferralOperation.Document,
                writer => SubmitReturnReferralOperation.WriteVariables(writer, input),
                SubmitReturnReferralOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.SubmitReturnReferral;
        });

    public Task<IUpdateSellerReturnResponseRuleCondition_UpdateSellerReturnResponseRuleCondition?> UpdateSellerReturnResponseRuleConditionAsync(
        UpdateSellerReturnResponseRuleConditionInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                UpdateSellerReturnResponseRuleConditionOperation.OperationName,
                UpdateSellerReturnResponseRuleConditionOperation.Document,
                writer => UpdateSellerReturnResponseRuleConditionOperation.WriteVariables(writer, input),
                UpdateSellerReturnResponseRuleConditionOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.UpdateSellerReturnResponseRuleCondition;
        });

    public Task<IUpdateSellerReturnResponseRulesPriorities_UpdateSellerReturnResponseRulesPriorities?> UpdateSellerReturnResponseRulesPrioritiesAsync(
        UpdateSellerReturnResponseRulesPrioritiesInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                UpdateSellerReturnResponseRulesPrioritiesOperation.OperationName,
                UpdateSellerReturnResponseRulesPrioritiesOperation.Document,
                writer => UpdateSellerReturnResponseRulesPrioritiesOperation.WriteVariables(writer, input),
                UpdateSellerReturnResponseRulesPrioritiesOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.UpdateSellerReturnResponseRulesPriorities;
        });

    public Task<IUpdateSellerReturnRuleStatus_UpdateSellerReturnRuleStatus?> UpdateSellerReturnRuleStatusAsync(
        UpdateSellerReturnRuleStatusInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                UpdateSellerReturnRuleStatusOperation.OperationName,
                UpdateSellerReturnRuleStatusOperation.Document,
                writer => UpdateSellerReturnRuleStatusOperation.WriteVariables(writer, input),
                UpdateSellerReturnRuleStatusOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.UpdateSellerReturnRuleStatus;
        });

    public Task<IUpdateSellerReturnToAddressRuleCondition_UpdateSellerReturnToAddressRuleCondition?> UpdateSellerReturnToAddressRuleConditionAsync(
        UpdateSellerReturnToAddressRuleConditionInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                UpdateSellerReturnToAddressRuleConditionOperation.OperationName,
                UpdateSellerReturnToAddressRuleConditionOperation.Document,
                writer => UpdateSellerReturnToAddressRuleConditionOperation.WriteVariables(writer, input),
                UpdateSellerReturnToAddressRuleConditionOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.UpdateSellerReturnToAddressRuleCondition;
        });

    public Task<IUpdateSellerReturnToAddressRulesPriorities_UpdateSellerReturnToAddressRulesPriorities?> UpdateSellerReturnToAddressRulesPrioritiesAsync(
        UpdateSellerReturnToAddressRulesPrioritiesInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                UpdateSellerReturnToAddressRulesPrioritiesOperation.OperationName,
                UpdateSellerReturnToAddressRulesPrioritiesOperation.Document,
                writer => UpdateSellerReturnToAddressRulesPrioritiesOperation.WriteVariables(writer, input),
                UpdateSellerReturnToAddressRulesPrioritiesOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.UpdateSellerReturnToAddressRulesPriorities;
        });

    public Task<IUpdateSellerRmaPreference_UpdateSellerRmaPreference?> UpdateSellerRmaPreferenceAsync(
        UpdateSellerRmaPreferenceInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                UpdateSellerRmaPreferenceOperation.OperationName,
                UpdateSellerRmaPreferenceOperation.Document,
                writer => UpdateSellerRmaPreferenceOperation.WriteVariables(writer, input),
                UpdateSellerRmaPreferenceOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.UpdateSellerRmaPreference;
        });

    public Task<JsonDocument> ExecuteRawUnsafeAsync(
        string query, object? variables = null, string? accessToken = null, CancellationToken ct = default)
        => _dynamic.ExecuteRawUnsafeAsync(query, variables, accessToken, ct);
}
