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
        var conversionRate = CurrencyConstants.DefaultConversionRate;
        if (request.Currency.Equals(CurrencyConstants.DefaultCurrency) == false)
        {
            var conversionRateResult = await _currencyConverter.GetConversionRateAsync(
                request.Currency,
                cancellationToken);
            if (conversionRateResult.IsSuccess)
            {
                conversionRate = conversionRateResult.Value;
            }
            else
            {
                return conversionRateResult.Error!;
            }
        }

        var products = _productsDataAccess
            .List(request.PageStart, request.PageSize)
            .Select(p =>
            {
                p.PriceInPounds *= conversionRate;
                return p;
            });

        return Result<IEnumerable<Product>>.Success(products);
    }
}