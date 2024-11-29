using FluentValidation;
using FluentValidation.AspNetCore;
using Kindred.CurrencyExchangeService.Application;
using Kindred.CurrencyExchangeService.Application.Configuration;
using Kindred.CurrencyExchangeService.Application.Services; 

namespace Kindred.CurrencyExchangeService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient<IExchangeRateService, ExchangeRateService>();
            builder.Services.Configure<ExchangeRateApiOptions>(builder.Configuration.GetSection("ExchangeRateApi"));
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ExchangeRateServiceHandler>());
            builder.Services.AddHttpClient<ExchangeRateService>(); 
            builder.Services.AddScoped<IValidator<ExchangeRateServiceCommand>, ExchangeRateServiceCommandValidator>();

            /***            
            Can configure Redis to cache the currency rates until the next update.

            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = ConfigurationOptions.Parse("localhost:6379", true);
                return ConnectionMultiplexer.Connect(configuration);
            });
            ***/

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
