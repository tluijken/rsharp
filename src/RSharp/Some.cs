namespace RSharp;

public record Some<T>(T Value) : Option<T>
{
    public static implicit operator Some<T>(T value) => new(value);

    public static implicit operator T(Some<T> @this) => @this.Value;

    public virtual bool Equals(Some<T>? other) => other is not null && EqualityComparer<T>.Default.Equals(Value, other.Value);

    public override int GetHashCode() => Value is not null ? EqualityComparer<T>.Default.GetHashCode(Value) : 0;
}

public record None<T> : Option<T>
{
    public static implicit operator None<T>(T value) => new();
}

public record Option<T>
{
    public static implicit operator Option<T>(T value) => value is null ? new None<T>() : new Some<T>(value);
}

public static class OptionExtensions
{
    public static Option<T> ToOption<T>(this T value) => value;

    // create the match extension method
    public static void Match<T>(this Option<T> option, Action<T> some, Action none)
    {
        switch (option)
        {
            case Some<T> someOption:
                some(someOption.Value);
                break;
            default:
                none();
                break;
        }
    }
}