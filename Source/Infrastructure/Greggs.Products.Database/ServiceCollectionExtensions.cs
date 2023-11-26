using Greggs.Products.Abstractions.Interfaces;
using Greggs.Products.Abstractions.Models;
using Greggs.Products.Database.DataAccess;
using Microsoft.Extensions.DependencyInjection;

namespace Greggs.Products.Database;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddScoped<IDataAccess<Product>, ProductAccess>();

        return services;
    }
}