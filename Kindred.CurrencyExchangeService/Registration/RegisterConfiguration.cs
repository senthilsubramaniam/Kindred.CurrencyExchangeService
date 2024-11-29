using Kindred.CurrencyExchangeService.Application.Configuration;

namespace Kindred.CurrencyExchangeService.Registration
{
    public static class RegisterConfiguration
    {
        public static void RegisterOptions(this IServiceCollection serviceCollection,IConfiguration configuration)
        {
            serviceCollection.AddOptions<ExchangeRateApiOptions>()
                .Bind(configuration.GetSection("Application"))
                .ValidateDataAnnotations();
        }
    }
}
