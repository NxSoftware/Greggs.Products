using Greggs.Products.Abstractions;

namespace Greggs.Products.ExternalServices.Tests;

public class CurrencyConverterTests
{
    private readonly CurrencyConverter _subjectUnderTest = new();

    [Fact]
    public async Task GetConversionRateAsync_returns_Error_for_unknown_currency()
    {
        var result = await _subjectUnderTest.GetConversionRateAsync(
            "USD",
            default);

        result.IsFailure.Should().BeTrue();
        result.Error
            .Should().NotBeNull()
            .And.BeEquivalentTo(new Error(
                100,
                "Unknown currency"));
    }

    [Theory]
    [InlineData("EUR", 1.11)]
    public async Task GetConversionRateAsync_returns_successful_Result_for_known_currency(
        string currency,
        decimal expectedConversionRate)
    {
        var conversionRateResult = await _subjectUnderTest.GetConversionRateAsync(
            currency,
            default);

        conversionRateResult.IsSuccess.Should().BeTrue();
        conversionRateResult.Value.Should().Be(expectedConversionRate);
    }
}