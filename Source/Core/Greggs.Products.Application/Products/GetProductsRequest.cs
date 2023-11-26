using Greggs.Products.Abstractions;
using Greggs.Products.Abstractions.Models;
using Mediator;

namespace Greggs.Products.Application.Products;

public readonly record struct GetProductsRequest(
        int PageStart = 0,
        int PageSize = 0) 
    : IRequest<Result<IEnumerable<Product>>>;