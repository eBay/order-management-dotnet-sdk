using System.Net;
using System.Text;
using EBay.OrderManagement.Configuration;
using Xunit;

namespace EBay.OrderManagement.Tests;

public sealed class FileTransferTests
{
    [Fact]
    public async Task File_downloads_return_the_file_content()
    {
        // Regression: download methods returned Task and the file was discarded.
        var handler = new FileHandler(_ => new ByteArrayContent(Encoding.UTF8.GetBytes("%PDF-label")));
        using var client = new OrderManagementClient(new OrderManagementConfiguration { AccessToken = "t" }, handler);

        using var file = await client.Logistics.DownloadLabelFileAsync("shipment-1");

        using var reader = new StreamReader(file!);
        Assert.Equal("%PDF-label", await reader.ReadToEndAsync());
        Assert.EndsWith("/shipment/shipment-1/download_label_file", handler.LastUri!.AbsolutePath);
    }

    [Fact]
    public async Task Uploads_send_multipart_and_leave_the_callers_stream_open()
    {
        // Regression: the multipart content was disposed and closed the caller's stream.
        var handler = new FileHandler(_ => new StringContent("""{"fileId":"file-1"}""", Encoding.UTF8, "application/json"));
        using var client = new OrderManagementClient(new OrderManagementConfiguration { AccessToken = "t" }, handler);
        using var evidence = new MemoryStream(Encoding.UTF8.GetBytes("evidence-bytes"));

        var uploaded = await client.Fulfillment.UploadEvidenceFileAsync("dispute-1", evidence, "receipt.jpg");

        Assert.Equal("file-1", uploaded?.FileId);
        Assert.True(evidence.CanRead);
        Assert.StartsWith("multipart/form-data", handler.LastContentType);
        Assert.Contains("evidence-bytes", handler.LastBody);
    }

    private sealed class FileHandler(Func<HttpRequestMessage, HttpContent> respond) : HttpMessageHandler
    {
        public Uri? LastUri { get; private set; }
        public string? LastContentType { get; private set; }
        public string LastBody { get; private set; } = "";

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastUri = request.RequestUri;
            LastContentType = request.Content?.Headers.ContentType?.ToString();
            LastBody = request.Content is null ? "" : await request.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = respond(request), RequestMessage = request };
        }
    }
}
