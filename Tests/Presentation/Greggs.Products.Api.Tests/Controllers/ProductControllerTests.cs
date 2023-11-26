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
    
    [Theory]
    [InlineData(0, 5)]
    [InlineData(5, 5)]
    public async Task Successful_Get_Products_call_responds_with_200_OK(
        int pageStart,
        int pageSize)
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync($"/Product?pageStart={pageStart}&pageSize={pageSize}");

        await Verify(response)
            .UseParameters(pageStart, pageSize);
    }

    [Fact]
    public async Task Unsuccessful_Get_Products_call_response_with_400_BadRequest()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/Product?currency=UNKNOWN");

        await Verify(response);
    }
}