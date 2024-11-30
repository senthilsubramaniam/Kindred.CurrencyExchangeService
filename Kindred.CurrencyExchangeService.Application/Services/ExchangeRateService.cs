using Kindred.CurrencyExchangeService.Application.Configuration;
using Kindred.CurrencyExchangeService.Domain.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;

namespace Kindred.CurrencyExchangeService.Application.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExchangeRateService> _logger;
        private readonly string _apiUrl;
        private const string CacheKey = "ExchangeRates";

        public ExchangeRateService(HttpClient httpClient,
            IOptions<ExchangeRateApiOptions> options,
            ILogger<ExchangeRateService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiUrl = options.Value.ApiUrl;
        }

        public async Task<ExchangeRateResponse> GetExchangeRatesAsync(Guid correlationId, CancellationToken cancellationToken)
        {
            var cachedExchangeRate =  await GetCachedExchangeRatesIfNotExpired();
            if (cachedExchangeRate != null)
            {
                return cachedExchangeRate;
            }

            var content = await GetLiveExchangeRate(correlationId, cancellationToken); // To get live Exchange rates

            // var content = GetStubData(); // Used Stub for Testing.

            var exchangeData = JsonSerializer.Deserialize<ExchangeRateResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (exchangeData?.Rates == null)
            {
                const string? invalidExchangeRateData = "Invalid exchange rate data.";
                _logger.LogError($"TraceId:{correlationId}:: {nameof(ExchangeRateService)}::{invalidExchangeRateData}");
                throw new Exception($"{invalidExchangeRateData}");
            }

            await UpdateCacheAsync(exchangeData);

            return exchangeData;

        }

        private async Task<ExchangeRateResponse?> GetCachedExchangeRatesIfNotExpired()
        {
            var cachedRates = await GetCachedExchangeRateTillNextUpdateAsync();

            if (!string.IsNullOrEmpty(cachedRates))
            {
                var cachedExchangeRates = JsonSerializer.Deserialize<ExchangeRateResponse>(cachedRates!);
                if (cachedExchangeRates != null)
                {
                    return cachedExchangeRates;
                }
            }

            return null;
        }

        private async Task<string> GetLiveExchangeRate(Guid correlationId, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(_apiUrl, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                const string? failedToFetchExchangeRates = "Failed to fetch exchange rates.";
                _logger.LogError($"TraceId:{correlationId}:: {nameof(ExchangeRateService)}::{failedToFetchExchangeRates}");
                throw new Exception($"{failedToFetchExchangeRates}");
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return content;
        }

        public Task<RedisValue> GetCachedExchangeRateTillNextUpdateAsync()
        {
            //Get value from Redis for the CacheKey 
            return Task.FromResult(new RedisValue(string.Empty));
        }

        private Task UpdateCacheAsync(ExchangeRateResponse exchangeData)
        {
            // Cache rates with the expiry
            var cacheDuration = exchangeData.TimeNextUpdateUnix > 0
                ? DateTimeOffset.FromUnixTimeSeconds(exchangeData.TimeNextUpdateUnix) - DateTime.UtcNow
                : TimeSpan.FromHours(1); // Set for Expiry  so that the next time the Rates are pulled from Exchange;
            // Update Redis with the Exchange Rates.
            return Task.CompletedTask;
        }

        private string GetStubData()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(Directory.GetParent(currentDirectory)?.FullName, "Kindred.CurrencyExchangeService.Application", "Stub",
                "ExchangeRates.json");
            return File.ReadAllText(filePath);
        }
    }
}
