namespace RSharp;

/// <summary>
///     Represents a <see cref="Option{T}" /> instance that has a value.
/// </summary>
/// <param name="Value"></param>
/// <typeparam name="T"></typeparam>
public record Some<T>(T Value) : Option<T>
{
    /// <summary>
    ///     Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="other">
    ///     The object to compare with the current object.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />
    ///     .
    /// </returns>
    public virtual bool Equals(Some<T>? other) =>
        other is not null && EqualityComparer<T>.Default.Equals(Value, other.Value);

    /// <summary>
    ///     Implicitly converts a value to a <see cref="Some{T}" />.
    /// </summary>
    /// <param name="value">
    ///     The value to convert.
    /// </param>
    /// <returns>
    ///     The <see cref="Some{T}" />.
    /// </returns>
    public static implicit operator Some<T>(T value) => new(value);

    /// <summary>
    ///     Implicitly converts a <see cref="Some{T}" /> to a value.
    /// </summary>
    /// <param name="this">
    ///     The <see cref="Some{T}" /> to convert.
    /// </param>
    /// <returns>
    ///     The value.
    /// </returns>
    public static implicit operator T(Some<T> @this) => @this.Value;

    /// <summary>
    ///     Gets the hash code for this instance.
    /// </summary>
    /// <returns>
    ///     A 32-bit signed integer that is the hash code for this instance.
    /// </returns>
    public override int GetHashCode() => Value is not null ? EqualityComparer<T>.Default.GetHashCode(Value) : 0;
}

/// <summary>
///     Represents a <see cref="Option{T}" /> instance that does not have a value.
/// </summary>
/// <typeparam name="T"></typeparam>
public record None<T> : Option<T>
{
    /// <summary>
    ///     Implicitly converts a <see cref="None{T}" /> to a value.
    /// </summary>
    /// <param name="_">
    ///     The <see cref="None{T}" /> to convert.
    /// </param>
    /// <returns></returns>
    public static implicit operator None<T>(T _) => new();
}

/// <summary>
///     Represents a value that may or may not be present.
/// </summary>
/// <typeparam name="T">
///     The type of the value.
/// </typeparam>
public record Option<T>
{
    /// <summary>
    ///     Implicitly converts a value to either a <see cref="Some{T}" /> or a <see cref="None{T}" /> depending on whether the
    ///     value is <see langword="null" />.
    /// </summary>
    /// <param name="value">
    ///     The value to convert.
    /// </param>
    /// <returns>
    ///     The <see cref="Some{T}" /> or <see cref="None{T}" />.
    /// </returns>
    public static implicit operator Option<T>(T value) => value is null ? new None<T>() : new Some<T>(value);
}

/// <summary>
///     Provides extension methods for <see cref="Option{T}" />.
/// </summary>
public static class OptionExtensions
{
    /// <summary>
    ///     Matches the <see cref="Option{T}" />. value and executes the specified <paramref name="some" /> or
    ///     <paramref name="none" /> action, based on whether the <see cref="Option{T}" /> is a <see cref="Some{T}" /> or a
    ///     <see cref="None{T}" />.
    /// </summary>
    /// <param name="option">
    ///     The <see cref="Option{T}" /> containing the value to match.
    /// </param>
    /// <param name="some">
    ///     The action to execute if the <see cref="Option{T}" /> is a <see cref="Some{T}" />.
    /// </param>
    /// <param name="none">
    ///     The action to execute if the <see cref="Option{T}" /> is a <see cref="None{T}" />.
    /// </param>
    /// <typeparam name="T">
    ///     The type of the value.
    /// </typeparam>
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

    /// <summary>
    ///     Converts a value to an <see cref="Option{T}" />.
    /// </summary>
    /// <param name="value">
    ///     The value to convert.
    /// </param>
    /// <typeparam name="T">
    ///     The type of the value.
    /// </typeparam>
    /// <returns>
    ///     The <see cref="Option{T}" />.
    /// </returns>
    public static Option<T> ToOption<T>(this T @this) => @this;

    /// <summary>
    ///     Converts an <see cref="Option{T}" /> to a value.
    /// </summary>
    /// <param name="this">
    ///     The <see cref="Option{T}" /> to convert.
    /// </param>
    /// <typeparam name="T">
    ///     The type of the value.
    /// </typeparam>
    /// <returns>
    ///     The value.
    /// </returns>
    /// <exception cref="Exception">
    ///     Thrown if the <see cref="Option{T}" /> is a <see cref="None{T}" />.
    /// </exception>
    public static T Unwrap<T>(this Option<T> @this) => @this switch
    {
        Some<T> some => some.Value,
        _ => throw new Exception("There was no value to unwrap")
    };

    /// <summary>
    ///     Converts an <see cref="Option{T}" /> to a value or returns the specified <paramref name="defaultValue" />.
    /// </summary>
    /// <param name="this">
    ///     The <see cref="Option{T}" /> to convert.
    /// </param>
    /// <param name="defaultValue">
    ///     The default value to return if the <see cref="Option{T}" /> is a <see cref="None{T}" />.
    /// </param>
    /// <typeparam name="T">
    ///     The type of the value.
    /// </typeparam>
    /// <returns>
    ///     The value or the <paramref name="defaultValue" />.
    /// </returns>
    public static T UnwrapOr<T>(this Option<T> @this, T defaultValue) => @this switch
    {
        Some<T> some => some.Value,
        _ => defaultValue
    };

    /// <summary>
    ///     Indicates whether the <see cref="Option{T}" /> contains a value.
    /// </summary>
    /// <param name="this">
    ///     The <see cref="Option{T}" /> to check.
    /// </param>
    /// <typeparam name="T">
    ///     The type of the value.
    /// </typeparam>
    /// <returns>
    ///     <see langword="true" /> if the <see cref="Option{T}" /> is a <see cref="Some{T}" />; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    public static bool IsSome<T>(this Option<T> @this) => @this is Some<T>;
    
    /// <summary>
    ///     Performs the specified action on the <see cref="Option{T}" /> if it is a <see cref="Some{T}" />.
    /// </summary>
    /// <param name="this">
    ///    The <see cref="Option{T}" />.
    /// </param>
    /// <param name="action">
    ///    The action to perform.
    /// </param>
    /// <typeparam name="T">
    ///   The type of the value.
    /// </typeparam>
    /// <returns>
    ///     The <see cref="Option{T}" />.
    /// </returns>
    public static Option<T> Tee<T>(this Option<T> @this, Action<Option<T>> action)
    {
        action(@this);
        return @this;
    }
}