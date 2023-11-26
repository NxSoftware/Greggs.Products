namespace Greggs.Products.Abstractions.Errors;

public static class CurrencyConversionErrors
{
    public static readonly Error UnknownCurrency = new Error(100, "Unknown currency");
}