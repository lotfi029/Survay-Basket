using Microsoft.AspNetCore.Identity;

namespace Survay_Basket.API.Abstractions;

public class Result
{
    public Result(bool isSucess, Error error)
    {
        if ((isSucess && error != Error.None) || (!isSucess && error == Error.None))
            throw new InvalidOperationException();

        IsSuccess = isSucess;
        Error = error;

    }
    public bool IsSuccess { get; }
    public bool IsFailer => !IsSuccess;
    public Error Error { get; } = default!;

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

}

public class Result<TValue>(TValue? value, bool isSuccess, Error error) : Result(isSuccess, error)
{
    private readonly TValue? _value = value;

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Failure result cannot have value");
}
