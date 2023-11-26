using Greggs.Products.Abstractions;
using Greggs.Products.Abstractions.Errors;
using Greggs.Products.Abstractions.Interfaces;

namespace Greggs.Products.ExternalServices;

public class CurrencyConverter : ICurrencyConverter
{
    private static IReadOnlyDictionary<string, decimal> ConversionRates = new Dictionary<string, decimal>
    {
        { "EUR", 1.11m },
    };
    
    public Task<Result<decimal>> GetConversionRateAsync(string toCurrency, CancellationToken cancellationToken)
    {
        Result<decimal> result; 
        if (ConversionRates.TryGetValue(toCurrency, out var conversionRate))
        {
            result = conversionRate;
        }
        else
        {
            result = CurrencyConversionErrors.UnknownCurrency;
        }
        
        return Task.FromResult(result);
    }
}