using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Library;
using Xunit;

namespace Tests;

// --------------------------------------------------------------------------
// Custom HttpMessageHandler to simulate HTTP responses without a real server
// --------------------------------------------------------------------------

/// <summary>
/// A fake HttpMessageHandler that returns a pre-configured response.
/// This lets us test HttpClient-consuming services without network calls.
/// </summary>
internal sealed class FakeHttpMessageHandler(HttpResponseMessage response) : HttpMessageHandler
{
    public HttpRequestMessage? LastRequest { get; private set; }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        LastRequest = request;
        return Task.FromResult(response);
    }
}

// --------------------------------------------------------------------------
// Tests
// --------------------------------------------------------------------------

/// <summary>
/// Example 18 — HttpClient Tests
///
/// The key pattern: inject a custom HttpMessageHandler so no real HTTP call is made.
///   - FakeHttpMessageHandler returns whatever response we configure
///   - We can inspect LastRequest to verify the correct URL was called
///   - Test both success and failure (non-2xx) status codes
/// </summary>
public class HttpClientTests
{
    private static ExchangeRateService BuildService(HttpResponseMessage response, out FakeHttpMessageHandler handler)
    {
        handler = new FakeHttpMessageHandler(response);
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://api.fake-rates.io") };
        return new ExchangeRateService(httpClient);
    }

    [Fact]
    public async Task GetRateAsync_SuccessResponse_ReturnsExchangeRate()
    {
        var rate = new ExchangeRate("USD", "EUR", 0.92, DateTime.UtcNow);
        var json = JsonSerializer.Serialize(rate);

        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
        };

        var service = BuildService(response, out _);
        var result  = await service.GetRateAsync("USD", "EUR");

        Assert.NotNull(result);
        Assert.Equal("USD", result.FromCurrency);
        Assert.Equal("EUR", result.ToCurrency);
        Assert.Equal(0.92, result.Rate, precision: 4);
    }

    [Fact]
    public async Task GetRateAsync_NotFoundResponse_ReturnsNull()
    {
        var response = new HttpResponseMessage(HttpStatusCode.NotFound);
        var service  = BuildService(response, out _);

        var result = await service.GetRateAsync("XYZ", "ABC");

        Assert.Null(result);
    }

    [Fact]
    public async Task ConvertAsync_ValidRate_ReturnsConvertedAmount()
    {
        var rate = new ExchangeRate("USD", "BRL", 5.0, DateTime.UtcNow);
        var json = JsonSerializer.Serialize(rate);
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
        };

        var service = BuildService(response, out _);
        double converted = await service.ConvertAsync(100, "USD", "BRL");

        Assert.Equal(500.0, converted);
    }

    [Fact]
    public async Task ConvertAsync_ApiUnavailable_ThrowsInvalidOperationException()
    {
        var response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
        var service  = BuildService(response, out _);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.ConvertAsync(100, "USD", "GBP"));
    }

    [Fact]
    public async Task GetRateAsync_CallsCorrectUrl()
    {
        var rate = new ExchangeRate("USD", "JPY", 150.0, DateTime.UtcNow);
        var json = JsonSerializer.Serialize(rate);
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
        };

        var service = BuildService(response, out var handler);
        await service.GetRateAsync("USD", "JPY");

        // Verify the correct query parameters were sent
        var requestUri = handler.LastRequest?.RequestUri?.ToString() ?? "";
        Assert.Contains("from=USD", requestUri);
        Assert.Contains("to=JPY",  requestUri);
    }
}
