using Greggs.Products.Abstractions;
using Greggs.Products.Abstractions.Interfaces;
using Greggs.Products.Abstractions.Models;
using Mediator;

namespace Greggs.Products.Application.Products;

public sealed class GetProductsHandler 
    : IRequestHandler<GetProductsRequest, Result<IEnumerable<Product>>>
{
    private readonly IDataAccess<Product> _productsDataAccess;

    public GetProductsHandler(IDataAccess<Product> productsDataAccess)
    {
        _productsDataAccess = productsDataAccess;
    }
    
    public ValueTask<Result<IEnumerable<Product>>> Handle(
        GetProductsRequest request,
        CancellationToken cancellationToken)
    {
        var products = _productsDataAccess.List(
            request.PageStart,
            request.PageSize);
        
        return Result<IEnumerable<Product>>
            .Success(products)
            .ToValueTask();
    }
}