using System.ComponentModel.DataAnnotations;

namespace Kindred.CurrencyExchangeService.Application.Configuration
{    public class ExchangeRateApiOptions
    {
        [Required]
        public string ApiUrl { get; set; } = string.Empty;
    }
}
