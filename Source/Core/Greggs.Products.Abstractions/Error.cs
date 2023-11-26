namespace Greggs.Products.Abstractions;

// This type would realistically live in a company-wide
// package and as such is not unit tested here.
public sealed record Error(int Code, string Description)
{
    public static readonly Error None = new(0, string.Empty);
}