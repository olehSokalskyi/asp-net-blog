namespace Application.Common;

public readonly struct Result<TValue, TError>
{
    private readonly TValue? _value;
    private readonly TError? _error;

    public bool IsError { get; }
    public bool IsSuccess => !IsError;

    private Result(TValue value)
    {
        IsError = false;
        _value = value;
        _error = default;
    }

    private Result(TError error)
    {
        IsError = true;
        _value = default;
        _error = error;
    }

    public static implicit operator Result<TValue, TError>(TValue value) => new(value);
    public static implicit operator Result<TValue, TError>(TError error) => new(error);

    public TResult Match<TResult>(
        Func<TValue, TResult> success,
        Func<TError, TResult> failure) =>
        IsSuccess ? success(_value!) : failure(_error!);
}