using Kindred.CurrencyExchangeService.Domain.Model;
using StackExchange.Redis;

namespace Kindred.CurrencyExchangeService.Application.Services
{
    public interface IExchangeRateService
    {
        Task<ExchangeRateResponse> GetExchangeRatesAsync(Guid correlationId,CancellationToken cancellationToken);

        Task<RedisValue> GetCachedExchangeRateTillNextUpdateAsync(); // Added just Redis caching.
    }
}
