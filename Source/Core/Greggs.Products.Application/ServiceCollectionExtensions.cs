using Microsoft.Extensions.DependencyInjection;

namespace Greggs.Products.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediator(o =>
        {
            o.ServiceLifetime = ServiceLifetime.Scoped;
        });

        return services;
    }
}