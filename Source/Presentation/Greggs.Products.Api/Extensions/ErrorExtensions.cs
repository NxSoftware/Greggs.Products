using Greggs.Products.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Greggs.Products.Api.Extensions;

public static class ErrorExtensions
{
    public static IActionResult MapToResponse(
        this Error error,
        int statusCode,
        ControllerBase controller)
    {
        var problem = controller.ProblemDetailsFactory.CreateProblemDetails(
            controller.HttpContext,
            statusCode: statusCode,
            detail: error.Description);
        problem.Extensions["code"] = error.Code;
        
        return controller.StatusCode(statusCode, problem);
    }
}