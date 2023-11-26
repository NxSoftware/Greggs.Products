using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Greggs.Products.Api.Extensions;
using Greggs.Products.Application.Products;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProductController> _logger;

    public ProductController(
        IMediator mediator,
        ILogger<ProductController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        int pageStart = 0,
        int pageSize = 5)
    {
        var request = new GetProductsRequest(pageStart, pageSize);
        var result = await _mediator.Send(request, HttpContext.RequestAborted);

        return result.MapToResponse(this);
    }
}