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
    public async Task Get_Products_responds_with_200_OK()
    {
        var client = _factory.CreateClient();
        Mocks.Mediator
            .Send(Arg.Any<GetProductsRequest>())
            .ReturnsForAnyArgs(Result<IEnumerable<Product>>.Success(TestDataCreator.Products));

        var response = await client.GetAsync("/Product");

        await Verify(response);
    }
}