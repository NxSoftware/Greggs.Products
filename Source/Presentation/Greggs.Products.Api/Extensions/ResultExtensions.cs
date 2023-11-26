using System.Collections.Generic;
using Greggs.Products.Abstractions;
using Greggs.Products.Abstractions.Models;
using Greggs.Products.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Greggs.Products.Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult MapToResponse(
        this Result<IEnumerable<Product>> result, 
        ControllerBase controller)
    {
        return result.Match(
            products => controller.Ok(products),
            error => error.MapToResponse(400, controller));
    }
}