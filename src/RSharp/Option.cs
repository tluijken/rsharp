using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace RSharp;

/// <summary>
///     Represents a <see cref="Option{T}" /> instance that has a value.
/// </summary>
/// <typeparam name="T"></typeparam>
[SuppressMessage("CodeAnalysis", "CA1815", Justification = "This is a value type.")]
[SuppressMessage("CodeAnalysis", "CA2231", Justification = "This is a value type.")]
[SuppressMessage("CodeAnalysis", "CA1000", Justification = "This is a value type.")]
public readonly struct Option<T> : IEquatable<Option<T>>
{
    /// <summary>
    ///     Creates a new instance of <see cref="Option{T}" />.
    /// </summary>
    /// <param name="value">
    ///     The value to bind to the <see cref="Option{T}" />.
    /// </param>
    private Option(T value)
    {
        ArgumentNullException.ThrowIfNull(value);
        Value = value;
        _isSome = true;
    }

    internal readonly T? Value;
    private readonly bool _isSome;

    /// <summary>
    ///     Construct an Option of T in a Some state
    /// </summary>
    /// <param name="value">Value to bind, must be non-null</param>
    /// <returns>Option of A</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Some(T value) =>
        value is null
            ? throw new ArgumentNullException(nameof(value))
            : new Option<T>(value);

    /// <summary>
    ///     Construct an Option of T in a None state
    /// </summary>
    [SuppressMessage("CodeAnalysis", "CA1805", Justification = "We want this.")]
    public static readonly Option<T> None = default;

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
    public bool Equals(Option<T>? other) =>
        other is not null && EqualityComparer<T>.Default.Equals(Value, other.Value);

    /// <summary>
    ///     Implicitly converts a value to a <see cref="Option{T}" />.
    /// </summary>
    /// <param name="value">
    ///     The value to convert.
    /// </param>
    /// <returns>
    ///     The <see cref="Option{T}" />.
    /// </returns>
    public static implicit operator Option<T>(T value) =>
        value is null || EqualityComparer<T>.Default.Equals(value, default) ? None : Some(value);

    /// <summary>
    ///     Implicitly converts a <see cref="Option{T}" /> to a value.
    /// </summary>
    /// <param name="this">
    ///     The <see cref="Option{T}" /> to convert.
    /// </param>
    /// <returns>
    ///     The value.
    /// </returns>
    public static implicit operator T(Option<T> @this) => @this.Value ??
                                                          throw new InvalidOperationException(
                                                              "Cannot implicitly convert None to a value.");

    public override bool Equals(object? obj) => obj is Option<T> other && Equals(other);

    public override int GetHashCode() => Value?.GetHashCode() ?? 0;

    public bool Equals(Option<T> other) => EqualityComparer<T>.Default.Equals(Value, other.Value);

    /// <summary>
    ///     Indicates whether the <see cref="Option{T}" /> contains a value.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the value.
    /// </typeparam>
    /// <returns>
    ///     <see langword="true" /> if the <see cref="Option{T}" /> is has a value; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    [Pure]
    public bool IsSome() => _isSome;

    /// <summary>
    ///     Is the option in a None state
    /// </summary>
    [Pure]
    public bool IsNone() => !_isSome;
}

/// <summary>
///     Provides extension methods for <see cref="Option{T}" />.
/// </summary>
public static class OptionExtensions
{
    /// <summary>
    ///     Matches the <see cref="Option{T}" />. value and executes the specified <paramref name="some" /> or
    ///     <paramref name="none" /> action, based on whether the <see cref="Option{T}" /> has a value or not.
    /// </summary>
    /// <param name="option">
    ///     The <see cref="Option{T}" /> containing the value to match.
    /// </param>
    /// <param name="some">
    ///     The action to execute if the <see cref="Option{T}" /> is has a value.
    /// </param>
    /// <param name="none">
    ///     The action to execute if the <see cref="Option{T}" /> is has no valur.
    /// </param>
    /// <typeparam name="T">
    ///     The type of the value.
    /// </typeparam>
    public static void Match<T>(this Option<T> option, Action<T> some, Action none)
    {
        if (option.IsSome())
            some(option.Value!);
        else
            none();
    }

    /// <summary>
    ///     Converts a value to an <see cref="Option{T}" />.
    /// </summary>
    /// <param name="this">
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
    ///     Thrown if the <see cref="Option{T}" /> has no value to unwrap.
    /// </exception>
    public static T Unwrap<T>(this Option<T> @this) => @this switch
    {
        T some => some,
        _ => throw new Exception("There was no value to unwrap")
    };

    /// <summary>
    ///     Converts an <see cref="Option{T}" /> to a value or returns the specified <paramref name="defaultValue" />.
    /// </summary>
    /// <param name="this">
    ///     The <see cref="Option{T}" /> to convert.
    /// </param>
    /// <param name="defaultValue">
    ///     The default value to return if the <see cref="Option{T}" /> is has no value.
    /// </param>
    /// <typeparam name="T">
    ///     The type of the value.
    /// </typeparam>
    /// <returns>
    ///     The value or the <paramref name="defaultValue" />.
    /// </returns>
    public static T UnwrapOr<T>(this Option<T> @this, T defaultValue) => @this switch
    {
        T some => some,
        _ => defaultValue
    };

    /// <summary>
    ///     Performs the specified action on the <see cref="Option{T}" /> if it has a value.
    /// </summary>
    /// <param name="this">
    ///     The <see cref="Option{T}" />.
    /// </param>
    /// <param name="action">
    ///     The action to perform.
    /// </param>
    /// <typeparam name="T">
    ///     The type of the value.
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