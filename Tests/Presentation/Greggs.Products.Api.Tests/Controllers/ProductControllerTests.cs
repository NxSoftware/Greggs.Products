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

        var response = await client.GetAsync("/Product?pageStart=0&pageSize=5");

        await Verify(response);
    }

    [Fact]
    public async Task Unsuccessful_Get_Products_call_response_with_400_BadRequest()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/Product?currency=UNKNOWN");

        await Verify(response);
    }
}