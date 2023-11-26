using Greggs.Products.Abstractions;
using Greggs.Products.Abstractions.Models;
using Greggs.Products.Application.Products;
using Mediator;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Greggs.Products.Api.Tests;

public sealed class GreggsApiApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(ConfigureServices);
    }

    private void ConfigureServices(IServiceCollection services)
    {
        var mediatorService = services.First(s => s.ServiceType == typeof(IMediator));
        services.Remove(mediatorService);
        services.AddSingleton(Mocks.Mediator);
    }
}