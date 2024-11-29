namespace Kindred.CurrencyExchangeService.Domain.Model
{
    public class CurrencyExchangeResponse
    {
        public decimal Amount { get; set; }
        public string InputCurrency { get; set; } = string.Empty;
        public string OutputCurrency { get; set; } = string.Empty;
        public decimal Value { get; set; }
    }
}
