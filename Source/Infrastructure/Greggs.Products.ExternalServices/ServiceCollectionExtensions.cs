using Greggs.Products.Abstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Greggs.Products.ExternalServices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExternalServices(this IServiceCollection services)
    {
        services.AddSingleton<ICurrencyConverter, CurrencyConverter>();
        
        return services;
    }
}