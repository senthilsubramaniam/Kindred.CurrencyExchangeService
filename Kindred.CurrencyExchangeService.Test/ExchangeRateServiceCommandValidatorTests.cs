using FluentValidation.TestHelper;
using Kindred.CurrencyExchangeService.Application;
using Xunit;

public class ExchangeRateServiceCommandValidatorTests
{
    private readonly ExchangeRateServiceCommandValidator _validator;

    public ExchangeRateServiceCommandValidatorTests()
    {
        _validator = new ExchangeRateServiceCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Amount_Is_Zero_Or_Less()
    {
        // Arrange
        var command = new ExchangeRateServiceCommand { Amount = 0 };

        //Act
        var result = _validator.TestValidate(command);

        // Act &Assert
        result.ShouldHaveValidationErrorFor(x => x.Amount);
    }

    [Fact]
    public void Should_Have_Error_When_Amount_Is_Less_Than_Zero()
    {
        //TODO
    }

    [Fact]
    public void Should_Not_Have_Error_When_Amount_Is_Greater_Than_Zero()
    {
        // Arrange
        var command = new ExchangeRateServiceCommand { Amount = 1 };

        //Act
        var result = _validator.TestValidate(command);

        // Act & Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Amount);
    }

    [Fact]
    public void Should_Have_Error_When_InputCurrency_Is_Empty()
    {
        // Arrange
        var command = new ExchangeRateServiceCommand { InputCurrency = string.Empty };

        //Act
        var result = _validator.TestValidate(command);

        // Act & Assert
        result.ShouldHaveValidationErrorFor(x => x.InputCurrency);
    }

    [Fact]
    public void Should_Have_Error_When_InputCurrency_Is_Not_3_Letters()
    {
        //TODO
    }

    [Fact]
    public void Should_Not_Have_Error_When_InputCurrency_Is_Valid_3_Letters()
    {
        //TODO
    }

    [Fact]
    public void Should_Have_Error_When_OutputCurrency_Is_Empty()
    {
        // Arrange
        var command = new ExchangeRateServiceCommand { OutputCurrency = string.Empty };


        //Act
        var result = _validator.TestValidate(command);

        // Act & Assert
        result.ShouldHaveValidationErrorFor(x => x.OutputCurrency);
    }

    [Fact]
    public void Should_Have_Error_When_OutputCurrency_Is_Not_3_Letters()
    {
        // Arrange
        var command = new ExchangeRateServiceCommand { OutputCurrency = "EU" };


        //Act
        var result = _validator.TestValidate(command);

        // Act & Assert
        result.ShouldHaveValidationErrorFor(x => x.OutputCurrency);
    }

    [Fact]
    public void Should_Not_Have_Error_When_OutputCurrency_Is_Valid_3_Letters()
    {
        // Arrange
        var command = new ExchangeRateServiceCommand { OutputCurrency = "EUR" };

        //Act
        var result = _validator.TestValidate(command);

        // Act & Assert
        result.ShouldNotHaveValidationErrorFor(x => x.OutputCurrency);
    }
}