# ChangeLog

## 1.0.0 (09/14/2026)

**Breaking changes**
- The following operations were removed from `FulfillmentClient`:
  - `GetPaymentDisputeActivityAsync(string, string?, RequestOptions?, CancellationToken)`
- The following operations were removed from `LogisticsClient`:
  - `CreateShipmentFromQuoteAsync(CreateShipmentFromQuoteRequest, string?, RequestOptions?, CancellationToken)`
  - `DownloadShipmentLabelAsync(string, string?, RequestOptions?, CancellationToken)`
- The following operations changed signature on `FulfillmentClient`:
  - `GetOrderAsync`: `(string, string?, RequestOptions?, CancellationToken)` → `(string, string?, string?, RequestOptions?, CancellationToken)`
  - `GetOrdersAsync`: `(string?, int?, int?, string?, string?, string?, RequestOptions?, CancellationToken)` → `(string?, string?, string?, string?, string?, string?, RequestOptions?, CancellationToken)`
  - `GetPaymentDisputeSummariesAsync`: `(string?, int?, int?, string?, string?, string?, string?, string?, RequestOptions?, CancellationToken)` → `(string?, string?, string?, string?, string?, string?, string?, string?, RequestOptions?, CancellationToken)`
  - `UploadEvidenceFileAsync`: `(string, string?, RequestOptions?, CancellationToken)` → `(string, Stream, string, string?, RequestOptions?, CancellationToken)`
- The following operations changed signature on `LogisticsClient`:
  - `CreateShippingQuoteAsync`: `(ShippingQuoteRequest, string?, RequestOptions?, CancellationToken)` → `(string, ShippingQuoteRequest, string?, RequestOptions?, CancellationToken)`

- The following operations were added to `FulfillmentClient`:
  - `GetActivitiesAsync(string, string?, RequestOptions?, CancellationToken)`
- The following operations were added to `LogisticsClient`:
  - `CreateFromShippingQuoteAsync(string, CreateShipmentFromQuoteRequest, string?, RequestOptions?, CancellationToken)`
  - `DownloadLabelFileAsync(string, string?, RequestOptions?, CancellationToken)`
