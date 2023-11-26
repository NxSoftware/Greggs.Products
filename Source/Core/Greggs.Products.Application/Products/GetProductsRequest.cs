using Greggs.Products.Abstractions;
using Greggs.Products.Abstractions.Models;
using Mediator;

namespace Greggs.Products.Application.Products;

public sealed record GetProductsRequest(
        int PageStart = 0,
        int PageSize = 5,
        string Currency = "GBP") 
    : IRequest<Result<IEnumerable<Product>>>;