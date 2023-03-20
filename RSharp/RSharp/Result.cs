namespace RSharp;

public record Result<TResult, TException> where TException : Exception
{
    private readonly TResult? _value;
    private readonly TException? _error;

    public bool IsOk { get; }
    public bool IsErr => !IsOk;

    private Result(TResult value)
    {
        _value = value;
        IsOk = true;
    }

    private Result(TException error)
    {
        _error = error;
        IsOk = false;
    }

    public TResult Unwrap() =>
        IsOk switch
        {
            true => _value!,
            _ => throw _error!
        };

    public TResult UnwrapOr(TResult defaultValue) =>
        IsOk switch
        {
            true => _value!,
            _ => defaultValue
        };
    
    public TResult UnwrapOrElse(Func<TResult> defaultValue) =>
        IsOk switch
        {
            true => _value!,
            _ => defaultValue()
        };
    
    public TResult UnwrapOrElse(Func<TException, TResult> defaultValue) => 
        IsOk switch
        {
            true => _value!,
            _ => defaultValue(_error!)
        };
    
    public TResult Expect(string message) =>  
        IsOk switch
        {
            true => _value!,
            _ => throw new Exception(message)
        };
    
    public static implicit operator Result<TResult, TException>(TResult value) => new(value);
    public static implicit operator Result<TResult, TException>(TException error) => new(error);
    public static implicit operator Result<TResult, TException>(Option<TResult> option) => 
        option switch
        {
            Some<TResult> some => some.Value,
            None<TResult> _ => throw new Exception("Option is None"),
            _ => throw new Exception("Option is None")
        };

    public void Match(Action<TResult> okAction, Action<TException> errorAction)
    {
        switch (IsOk)
        {
            case true:
                okAction(_value!);
                break;
            default:
                errorAction(_error!);
                break;
        }
    }
}
