using System.Diagnostics.CodeAnalysis;
using Kindred.CurrencyExchangeService.Domain.Model;
using MediatR;

namespace Kindred.CurrencyExchangeService.Application
{
    [ExcludeFromCodeCoverage]
    public class ExchangeRateServiceCommand : IRequest<CurrencyExchangeResponse>
    {
        public decimal Amount { get; set; }
        public string InputCurrency { get; set; } = string.Empty;
        public string OutputCurrency { get; set; } = string.Empty;
        public Guid CorrelationId { get; set; } = Guid.NewGuid();
    }
}
