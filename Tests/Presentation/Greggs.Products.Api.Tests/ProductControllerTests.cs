using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Greggs.Products.Api.Tests;

public class ProductControllerTests 
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ProductControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task Get_Products_responds_with_200_OK()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/Product");

        response.Should().Be200Ok();
    }
}