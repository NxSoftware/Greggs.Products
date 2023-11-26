using System.Linq;
using Greggs.Products.Abstractions;
using Greggs.Products.Abstractions.Errors;
using Greggs.Products.Abstractions.Interfaces;
using Greggs.Products.Abstractions.Models;
using Greggs.Products.Application.Products;
using Mediator;

namespace Greggs.Products.UnitTests.Products;

public class GetProductsHandlerTests
{
    private readonly IDataAccess<Product> _dataAccessMock;
    private readonly ICurrencyConverter _currencyConverterMock;
    private readonly GetProductsHandler _subjectUnderTest;

    public GetProductsHandlerTests()
    {
        _dataAccessMock = Substitute.For<IDataAccess<Product>>();
        _dataAccessMock
            .List(null, null)
            .ReturnsForAnyArgs(callInfo =>
            {
                var pageStart = callInfo.ArgAt<int>(0);
                var pageSize = callInfo.ArgAt<int>(1);
                return TestDataCreator.Products.Skip(pageStart).Take(pageSize);
            });

        _currencyConverterMock = Substitute.For<ICurrencyConverter>();

        _subjectUnderTest = new(_dataAccessMock, _currencyConverterMock);
    }

    [Theory]
    [InlineData(0, 5)]
    [InlineData(1, 4)]
    public async Task Handler_retrieves_list_of_products_using_the_data_access_layer(
        int pageStart, int pageSize)
    {
        var request = new GetProductsRequest(pageStart, pageSize);
        var expectedProducts = TestDataCreator.Products.Skip(pageStart).Take(pageSize);

        var response = await _subjectUnderTest.Handle(request, default);

        response.IsSuccess.Should().BeTrue();
        response.Value.Should().NotBeNull()
            .And
            .BeEquivalentTo(expectedProducts);
        _dataAccessMock
            .Received(1)
            .List(pageStart, pageSize);
    }

    [Fact]
    public async Task Handler_retrieves_list_of_products_in_GBP_by_default()
    {
        var request = new GetProductsRequest();
        request.Currency.Should().Be("GBP");

        await _subjectUnderTest.Handle(request, default);

        await _currencyConverterMock
            .DidNotReceiveWithAnyArgs()
            .GetConversionRateAsync(default!, default);
    }

    [Theory]
    [InlineData("EUR")]
    [InlineData("USD")]
    public async Task Handler_retrieves_currency_from_converter_when_dealing_with_currencies_other_than_GBP(
        string currency)
    {
        var request = new GetProductsRequest(Currency: currency);
        request.Currency.Should().Be(currency);
        _currencyConverterMock
            .GetConversionRateAsync(currency, default)
            .Returns(1.2m);

        await _subjectUnderTest.Handle(request, default);

        await _currencyConverterMock
            .Received(1)
            .GetConversionRateAsync(currency, default);
    }

    [Theory]
    [InlineData(1.1)]
    [InlineData(1.2)]
    public async Task Handler_returns_products_with_prices_adjusted_using_conversion_rate(
        decimal conversionRate)
    {
        var request = new GetProductsRequest(Currency: "EUR");
        _currencyConverterMock
            .GetConversionRateAsync(request.Currency, default)
            .Returns(conversionRate);
        var expectedProducts = TestDataCreator.Products
            .Skip(request.PageStart)
            .Take(request.PageSize)
            .Select(p =>
            {
                p.PriceInPounds *= conversionRate;
                return p;
            });
        
        var response = await _subjectUnderTest.Handle(request, default);

        response.IsSuccess.Should().BeTrue();
        response.Value.Should().BeEquivalentTo(expectedProducts);
    }

    [Fact]
    public async Task Handler_returns_Error_in_case_of_failure_retrieving_conversion_rate()
    {
        var request = new GetProductsRequest(Currency: "UNKNOWN");
        _currencyConverterMock
            .GetConversionRateAsync(request.Currency, default)
            .Returns(CurrencyConversionErrors.UnknownCurrency);
        
        var response = await _subjectUnderTest.Handle(request, default);

        response.IsFailure.Should().BeTrue();
        response.Error.Should().BeEquivalentTo(CurrencyConversionErrors.UnknownCurrency);
    }
}