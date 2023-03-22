namespace RSharp;

public record Result<TResult, TException> where TException : Exception
{
    private readonly TResult? _value;
    private readonly TException? _error;

    private readonly bool _isOk;
    
    public bool IsOk() => _isOk;
    
    public bool IsErr() => !_isOk;

    private Result(TResult value)
    {
        _value = value;
        _isOk = true;
    }

    private Result(TException error)
    {
        _error = error;
        _isOk = false;
    }

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
    
    public TResult UnwrapOrElse(Func<TException, TResult> defaultValue) => 
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
        switch (_isOk)
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
