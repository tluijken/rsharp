namespace RSharp;

public class Some<T> : Option<T>
{
    public Some(T value)
    {
        Value = value;
    }

    public T Value { get; }
    public static implicit operator Some<T>(T value) => new(value);
    public static implicit operator T(Some<T> @this) => @this.Value;

    public override bool Equals(object? obj)
    {
        return obj is Some<T> some && EqualityComparer<T>.Default.Equals(Value, some.Value);
    }

    protected bool Equals(Some<T> other)
    {
        return EqualityComparer<T>.Default.Equals(Value, other.Value);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<T>.Default.GetHashCode(Value);
    }
}

public class None<T> : Option<T>
{
    public static implicit operator None<T>(T value) => new();

    public static object? ToObject(None<T> none) => null;
}

public class Option<T>
{
    public static implicit operator Option<T>(T value) => value is null ? new None<T>() : new Some<T>(value);
}

public static class OptionExtensions
{
    public static Option<T> ToOption<T>(this T value) => value;
    // create the match extension method
    public static void Match<T>(this Option<T> option, Action<T> some, Action none)
    {
        if (option is Some<T> someOption)
        {
            some(someOption.Value);
        }
        else
        {
            none();
        }
    }
}
