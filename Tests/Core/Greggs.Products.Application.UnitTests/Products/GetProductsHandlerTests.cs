using System.Linq;
using Greggs.Products.Abstractions.Interfaces;
using Greggs.Products.Abstractions.Models;
using Greggs.Products.Application.Products;

namespace Greggs.Products.UnitTests.Products;

public class GetProductsHandlerTests
{
    private readonly IDataAccess<Product> _dataAccessMock;

    public GetProductsHandlerTests()
    {
        _dataAccessMock = Substitute.For<IDataAccess<Product>>();
        _dataAccessMock
            .List(null, null)
            .ReturnsForAnyArgs(callInfo =>
            {
                var pageStart = callInfo.ArgAt<int>(0);
                var pageSize = callInfo.ArgAt<int>(1);
                return TestDataCreator.Products.Skip(pageStart).Take(pageSize);
            });
    }

    [Theory]
    [InlineData(0, 5)]
    [InlineData(1, 4)]
    public async Task Handler_retrieves_list_of_products_using_the_data_access_layer(
        int pageStart, int pageSize)
    {
        var handler = new GetProductsHandler(_dataAccessMock);
        var request = new GetProductsRequest(pageStart, pageSize);
        var expectedProducts = TestDataCreator.Products.Skip(pageStart).Take(pageSize);

        var response = await handler.Handle(request, default);

        response.IsSuccess.Should().BeTrue();
        response.Value.Should().NotBeNull()
            .And
            .BeEquivalentTo(expectedProducts);
        _dataAccessMock
            .Received(1)
            .List(pageStart, pageSize);
    }
}