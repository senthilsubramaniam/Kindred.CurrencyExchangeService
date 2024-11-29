using FluentValidation;

namespace Kindred.CurrencyExchangeService.Application
{
    public  class ExchangeRateServiceCommandValidator: AbstractValidator<ExchangeRateServiceCommand>
    {
        public ExchangeRateServiceCommandValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.InputCurrency)
                .NotEmpty().WithMessage("Input currency is required.")
                .Length(3).WithMessage("Input currency should be a 3-letter code.");

            RuleFor(x => x.OutputCurrency)
                .NotEmpty().WithMessage("Output currency is required.")
                .Length(3).WithMessage("Output currency should be a 3-letter code.");
        }
    }
}
