using Greggs.Products.Abstractions;
using Greggs.Products.Abstractions.Models;
using Mediator;

namespace Greggs.Products.Application.Products;

public sealed class GetProductsHandler 
    : IRequestHandler<GetProductsRequest, Result<IEnumerable<Product>>>
{
    private static readonly string[] Products = new[]
    {
        "Sausage Roll", "Vegan Sausage Roll", "Steak Bake", "Yum Yum", "Pink Jammie"
    };

    public ValueTask<Result<IEnumerable<Product>>> Handle(
        GetProductsRequest request,
        CancellationToken cancellationToken)
    {
        var pageSize = Math.Min(request.PageSize, Products.Length);

        var rng = new Random();

        var products = Enumerable.Range(1, pageSize).Select(index => new Product
            {
                PriceInPounds = rng.Next(0, 10),
                Name = Products[rng.Next(Products.Length)]
            })
            .ToArray();
        
        return ValueTask.FromResult(
            Result<IEnumerable<Product>>.Success(products));
    }
}