using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Kindred.CurrencyExchangeService.Application.Services;
using Kindred.CurrencyExchangeService.Application;
using Kindred.CurrencyExchangeService.Domain.Model;

public class ExchangeRateServiceHandlerTests
{
    private readonly Mock<IExchangeRateService> _mockExchangeRateService;
    private readonly Mock<ILogger<ExchangeRateServiceHandler>> _mockLogger;
    private readonly ExchangeRateServiceHandler _handler;

    public ExchangeRateServiceHandlerTests()
    {
        _mockExchangeRateService = new Mock<IExchangeRateService>();
        _mockLogger = new Mock<ILogger<ExchangeRateServiceHandler>>();
        _handler = new ExchangeRateServiceHandler(_mockExchangeRateService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Currencies_Are_Invalid()
    {
        // Arrange
        var request = new ExchangeRateServiceCommand
        {
            CorrelationId = Guid.NewGuid(),
            Amount = 100,
            InputCurrency = "USD",
            OutputCurrency = "XXX"
        };

        var exchangeData = new ExchangeRateResponse
        {
            Rates = new Dictionary<string, decimal>
            {
                { "USD", 1 },
                { "EUR", 1.5m }
            }
        };

        _mockExchangeRateService.Setup(x => x.GetExchangeRatesAsync(request.CorrelationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exchangeData);

        // Act& Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
        exception.Message.Should().Contain("Invalid currency codes.");

        //_mockLogger.Verify(log => log.LogError(It.IsAny<Exception>(), It.Is<string>(s => s.Contains("Invalid currency codes."))), Times.Exactly(2));

    }

    [Fact]
    public async Task Handle_Should_Return_Converted_Amount_When_Currencies_Are_Valid()
    {
        // Arrange
        var request = new ExchangeRateServiceCommand
        {
            CorrelationId = Guid.NewGuid(),
            Amount = 100,
            InputCurrency = "USD",
            OutputCurrency = "EUR"
        };

        var exchangeData = new ExchangeRateResponse
        {
            Rates = new Dictionary<string, decimal>
            {
                { "USD", 1.0m },
                { "EUR", 0.85m }
            }
        };

        _mockExchangeRateService.Setup(x => x.GetExchangeRatesAsync(request.CorrelationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exchangeData);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // assert
        result.Should().NotBeNull();
        result.Amount.Should().Be(100);
        result.InputCurrency.Should().Be("USD");
        result.OutputCurrency.Should().Be("EUR");
        result.Value.Should().Be(85.0m);  // (100 / 1.0) * 0.85 = 85
        _mockLogger.Verify(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) =>
                    v.ToString().Contains($"About to fetch Exchange Rates.")),
                null,
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Log_Error_When_Fetching_Exchange_Rates_Fails()
    {
        // Arrange
        var request = new ExchangeRateServiceCommand
        {
            CorrelationId = Guid.NewGuid(),
            Amount = 100,
            InputCurrency = "USD",
            OutputCurrency = "EUR"
        };

        _mockExchangeRateService.Setup(x => x.GetExchangeRatesAsync(request.CorrelationId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Service unavailable"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
        exception.Message.Should().Be("Service unavailable");

    }
}
