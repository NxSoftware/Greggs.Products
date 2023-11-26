using Greggs.Products.Abstractions;
using Greggs.Products.Abstractions.Models;
using Greggs.Products.Application.Products;
using Greggs.Products.TestData;

namespace Greggs.Products.Api.Tests.Controllers;

[UsesVerify]
public sealed class ProductControllerTests 
    : IClassFixture<GreggsApiApplicationFactory>
{
    private readonly GreggsApiApplicationFactory _factory;

    public ProductControllerTests(GreggsApiApplicationFactory factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task Successful_Get_Products_call_responds_with_200_OK()
    {
        var client = _factory.CreateClient();
        Mocks.Mediator
            .Send(Arg.Any<GetProductsRequest>())
            .ReturnsForAnyArgs(Result<IEnumerable<Product>>.Success(TestDataCreator.Products));

        var response = await client.GetAsync("/Product");

        await Verify(response);
    }

    [Fact]
    public async Task Unsuccessful_Get_Products_call_response_with_400_BadRequest()
    {
        var client = _factory.CreateClient();
        Mocks.Mediator
            .Send(Arg.Any<GetProductsRequest>())
            .ReturnsForAnyArgs(Result<IEnumerable<Product>>.Failure(
                new Error(1, "Mock Error")));

        var response = await client.GetAsync("/Product");

        await Verify(response);
    }
}