using System.Diagnostics.CodeAnalysis;

namespace RSharp;

[SuppressMessage("CodeAnalysis", "CA1815", Justification = "This is a value type.")]
[SuppressMessage("CodeAnalysis", "CA2231", Justification = "This is a value type.")]
public readonly struct Result<TResult> : IEquatable<Result<TResult>>
{
    private readonly Exception? _error;

    private readonly bool _isOk;
    private readonly TResult? _value;

    private Result(TResult value)
    {
        _value = value;
        _isOk = true;
    }

    private Result(Exception error)
    {
        _error = error;
        _isOk = false;
    }

    public bool IsOk() => _isOk;

    public bool IsErr() => !_isOk;

    public TResult Unwrap() =>
        _isOk switch
        {
            true => _value!,
            _ => throw _error!
        };

    public TResult UnwrapOr(TResult defaultValue) =>
        _isOk switch
        {
            true => _value!,
            _ => defaultValue
        };

    public TResult UnwrapOrElse(Func<TResult> defaultValue) =>
        _isOk switch
        {
            true => _value!,
            _ => defaultValue()
        };

    public TResult UnwrapOrElse(Func<Exception, TResult> defaultValue) =>
        _isOk switch
        {
            true => _value!,
            _ => defaultValue(_error!)
        };

    public TResult Expect(string message) =>
        _isOk switch
        {
            true => _value!,
            _ => throw new Exception(message)
        };

    public static implicit operator Result<TResult>(TResult value) => new(value);

    public static implicit operator Result<TResult>(Exception error) => new(error);

    public static implicit operator Result<TResult>(Option<TResult> option) =>
        option switch
        {
            TResult some => some,
            _ => throw new Exception("Option is None")
        };

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public void Match(Action<TResult> Ok, Action<Exception> Err)
    {
        switch (_isOk)
        {
            case true:
                Ok(_value!);
                break;
            default:
                Err(_error!);
                break;
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public Result<TTarget> Match<TTarget>(Func<object, Result<TTarget>> Ok, Func<Exception, Exception> Err) =>
        _isOk switch
        {
            true => Ok(_value!),
            _ => Err(_error!)
        };

    public override bool Equals(object? obj) => obj is Result<TResult> other && Equals(other);

    public bool Equals(Result<TResult> other) => Equals(_error, other._error) && _isOk == other._isOk &&
                                                 EqualityComparer<TResult?>.Default.Equals(_value, other._value);

    public override int GetHashCode() => HashCode.Combine(_error, _isOk, _value);
}