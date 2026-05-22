using System.Net.Http.Json;

namespace Library;

public record ExchangeRate(string FromCurrency, string ToCurrency, double Rate, DateTime Timestamp);

/// <summary>
/// Exchange-rate service used to demonstrate HttpClient mocking (example 18).
/// </summary>
public class ExchangeRateService(HttpClient httpClient)
{
    public async Task<ExchangeRate?> GetRateAsync(string from, string to)
    {
        var response = await httpClient.GetAsync($"/rates?from={from}&to={to}");
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<ExchangeRate>();
    }

    public async Task<double> ConvertAsync(double amount, string from, string to)
    {
        var rate = await GetRateAsync(from, to);
        if (rate is null)
            throw new InvalidOperationException($"Could not fetch exchange rate for {from} -> {to}.");
        return amount * rate.Rate;
    }
}
