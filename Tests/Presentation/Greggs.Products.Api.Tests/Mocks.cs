using Mediator;

namespace Greggs.Products.Api.Tests;

public static class Mocks
{
    public static readonly IMediator Mediator = Substitute.For<IMediator>();
}