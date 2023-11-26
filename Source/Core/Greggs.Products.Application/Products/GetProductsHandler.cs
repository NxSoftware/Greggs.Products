using Greggs.Products.Abstractions;
using Greggs.Products.Abstractions.Constants;
using Greggs.Products.Abstractions.Interfaces;
using Greggs.Products.Abstractions.Models;
using Mediator;

namespace Greggs.Products.Application.Products;

public sealed class GetProductsHandler 
    : IRequestHandler<GetProductsRequest, Result<IEnumerable<Product>>>
{
    private readonly IDataAccess<Product> _productsDataAccess;
    private readonly ICurrencyConverter _currencyConverter;

    public GetProductsHandler(
        IDataAccess<Product> productsDataAccess,
        ICurrencyConverter currencyConverter)
    {
        _productsDataAccess = productsDataAccess;
        _currencyConverter = currencyConverter;
    }
    
    public async ValueTask<Result<IEnumerable<Product>>> Handle(
        GetProductsRequest request,
        CancellationToken cancellationToken)
    {
        var conversionRateResult = await GetConversionRateAsync(request.Currency, cancellationToken);
        if (conversionRateResult.IsFailure)
        {
            return conversionRateResult.Error!;
        }

        var products = _productsDataAccess
            .List(request.PageStart, request.PageSize)
            .Select(p =>
            {
                p.PriceInPounds *= conversionRateResult.Value;
                return p;
            });

        return Result<IEnumerable<Product>>.Success(products);
    }

    private async Task<Result<decimal>> GetConversionRateAsync(
        string currency,
        CancellationToken cancellationToken)
    {
        if (currency.Equals(CurrencyConstants.DefaultCurrency))
        {
            return Result<decimal>.Success(CurrencyConstants.DefaultConversionRate);
        }

        return await _currencyConverter.GetConversionRateAsync(
            currency,
            cancellationToken);
    }
}