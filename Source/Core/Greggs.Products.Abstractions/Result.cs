namespace Greggs.Products.Abstractions;

// This type would realistically live in a company-wide
// package and as such is not unit tested here.
public sealed record Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public Error? Error { get; }

    private Result(T value)
    {
        Value = value;
        IsSuccess = true;
    }
    
    private Result(Error error)
    {
        Error = error;
        IsSuccess = false;
    }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(Error error) => new(error);
    public static implicit operator Result<T>(Error error) => new(error);

    public ValueTask<Result<T>> ToValueTask() => ValueTask.FromResult(this);
    
    public TOutput Match<TOutput>(Func<T, TOutput> success, Func<Error, TOutput> failure) =>
        IsSuccess 
            ? success(Value!) 
            : failure(Error!);
}