namespace Greggs.Products.Abstractions.Interfaces;

public interface ICurrencyConverter
{
    Task<Result<decimal>> GetConversionRateAsync(string toCurrency, CancellationToken cancellationToken);
}