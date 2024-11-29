using Kindred.CurrencyExchangeService.Application.Services;
using Kindred.CurrencyExchangeService.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Kindred.CurrencyExchangeService.Application
{
    public class ExchangeRateServiceHandler : IRequestHandler<ExchangeRateServiceCommand, CurrencyExchangeResponse>
    {
        private readonly IExchangeRateService _exchangeRateService;
        private readonly ILogger<ExchangeRateServiceHandler> _logger;

        public ExchangeRateServiceHandler(IExchangeRateService exchangeRateService,ILogger<ExchangeRateServiceHandler> logger)
        {
            _exchangeRateService = exchangeRateService;
            _logger = logger;
        }

        public async Task<CurrencyExchangeResponse> Handle(ExchangeRateServiceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"TraceId:{request.CorrelationId}:: {nameof(ExchangeRateServiceHandler)}:: About to fetch Exchange Rates.");

                var exchangeData = await _exchangeRateService.GetExchangeRatesAsync(request.CorrelationId, cancellationToken);

                ValidateRequestCurrencies(request, exchangeData);

                return GetCurrencyExchangeResponse(request, exchangeData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"TraceId:{request.CorrelationId}:: {nameof(ExchangeRateServiceHandler)}::Failed to get rates from Exchange.");
                throw;
            }
        }

        private void ValidateRequestCurrencies(ExchangeRateServiceCommand request, ExchangeRateResponse exchangeData)
        {
            if (!exchangeData.Rates.ContainsKey(request.InputCurrency))
            {
                _logger.LogError($"TraceId:{request.CorrelationId}:: {nameof(ExchangeRateServiceHandler)}::Invalid currency codes.");

                throw new Exception($"Invalid currency codes. : {request.InputCurrency}");
            }

            if (!exchangeData.Rates.ContainsKey(request.OutputCurrency))
            {
                _logger.LogError($"TraceId:{request.CorrelationId}:: {nameof(ExchangeRateServiceHandler)}::Invalid currency codes.");

                throw new Exception($"Invalid currency codes. : {request.OutputCurrency}");
            }
        }

        private static CurrencyExchangeResponse GetCurrencyExchangeResponse(ExchangeRateServiceCommand request,
            ExchangeRateResponse exchangeData)
        {
            var inputRate = exchangeData.Rates[request.InputCurrency];
            var outputRate = exchangeData.Rates[request.OutputCurrency];
            var convertedValue = (request.Amount / inputRate) * outputRate;
            var roundedAmount = Math.Round(convertedValue, 2, MidpointRounding.AwayFromZero);

            var response = new CurrencyExchangeResponse
            {
                Amount = request.Amount,
                InputCurrency = request.InputCurrency,
                OutputCurrency = request.OutputCurrency,
                Value = roundedAmount
            };

            return response;
        }
    }
}
