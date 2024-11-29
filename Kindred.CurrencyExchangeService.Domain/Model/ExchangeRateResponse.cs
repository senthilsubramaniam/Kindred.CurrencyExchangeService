using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kindred.CurrencyExchangeService.Domain.Model
{
    public class ExchangeRateResponse
    {
        public string Result { get; set; } = string.Empty;
        public Dictionary<string, decimal> Rates { get; set; } = new();

        [JsonPropertyName("time_next_update_unix")]
        public long TimeNextUpdateUnix { get; set; }

        [JsonPropertyName("time_last_update_unix")]
        public long TimeLastUpdateUnix { get; set; }
    }
}
