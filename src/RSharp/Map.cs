using System.Diagnostics.CodeAnalysis;

namespace RSharp;

[SuppressMessage("ReSharper", "CA1031",
    Justification = "We don't know what exceptions the factory function will throw.")]
public static class MapExtensions
{
    /// <summary>
    ///     Try to create a new instance of TTarget using the factory function.
    /// </summary>
    /// <param name="source">
    ///     The source value.
    /// </param>
    /// <param name="factory">
    ///     The factory function to convert the source value to a new instance of TTarget.
    /// </param>
    /// <typeparam name="TSource">
    ///     The type of the source value.
    /// </typeparam>
    /// <typeparam name="TTarget">
    ///     The type of the target value.
    /// </typeparam>
    /// <returns>
    ///     A new instance of TTarget if the factory function succeeds, otherwise a None.
    /// </returns>
    public static Result<TTarget> Map<TSource, TTarget>(this TSource source,
        Func<TSource, Result<TTarget>> factory)
    {
        try
        {
            return factory(source);
        }
        catch (Exception e)
        {
            return e;
        }
    }

    /// <summary>
    ///     Try to create a new instance of TTarget using the factory function.
    /// </summary>
    /// <param name="source">
    ///     The source value.
    /// </param>
    /// <param name="factory">
    ///     The factory function to convert the source value to a new instance of TTarget.
    /// </param>
    /// <typeparam name="TSource">
    ///     The type of the source value.
    /// </typeparam>
    /// <typeparam name="TTarget">
    ///     The type of the target value.
    /// </typeparam>
    /// <returns>
    ///     A new instance of TTarget if the factory function succeeds, otherwise a None.
    /// </returns>
    public static Result<TTarget> Map<TSource, TTarget>(this Result<TSource> source,
        Func<object, Result<TTarget>> factory)
    {
        var x = source.Match(
            val => val.Map(factory),
            e => e);
        return x;
    }

    /// <summary>
    ///     Try to create a new instance of TTarget using the factory function for each source value.
    /// </summary>
    /// <param name="sources">
    ///     The source values.
    /// </param>
    /// <param name="factory">
    ///     The factory function to convert the source value to a new instance of TTarget.
    /// </param>
    /// <typeparam name="TSource">
    ///     The type of the source value.
    /// </typeparam>
    /// <typeparam name="TTarget">
    ///     The type of the target value.
    /// </typeparam>
    /// <returns>
    ///     A new instance of TTarget if the factory function succeeds, otherwise a None per element in the source collection.
    /// </returns>
    public static IEnumerable<Result<TTarget>> Map<TSource, TTarget>(this IEnumerable<TSource> sources,
        Func<TSource, Result<TTarget>> factory) => sources.Select(s => s.Map(factory));

    // Add map with options
    /// <summary>
    ///     Try to create a new instance of TTarget using the factory function.
    /// </summary>
    /// <param name="source">
    ///     The source value.
    /// </param>
    /// <param name="factory">
    ///     The factory function to convert the source value to a new instance of TTarget.
    /// </param>
    /// <typeparam name="TSource">
    ///     The type of the source value.
    /// </typeparam>
    /// <typeparam name="TTarget">
    ///     The type of the target value.
    /// </typeparam>
    /// <returns>
    ///     A new instance of TTarget if the factory function succeeds, otherwise a None.
    /// </returns>
    public static Option<TTarget> Map<TSource, TTarget>(this TSource source, Func<TSource, TTarget> factory) =>
        source.ToOption().Map(factory);

    /// <summary>
    ///     Try to create a new instance of TTarget using the factory function.
    /// </summary>
    /// <param name="source">
    ///     The source value.
    /// </param>
    /// <param name="factory">
    ///     The factory function to convert the source value to a new instance of TTarget.
    /// </param>
    /// <typeparam name="TSource">
    ///     The type of the source value.
    /// </typeparam>
    /// <typeparam name="TTarget">
    ///     The type of the target value.
    /// </typeparam>
    /// <returns>
    ///     A new instance of TTarget if the factory function succeeds, otherwise a None.
    /// </returns>
    public static Option<TTarget> Map<TSource, TTarget>(this Option<TSource> source, Func<TSource, TTarget> factory) =>
        source.IsSome() switch
        {
            true => TryCreate(() => factory(source.Value!)),
            false => Option<TTarget>.None
        };

    /// <summary>
    ///     Try to create a new instance of TTarget using the factory function for each source value.
    /// </summary>
    /// <param name="sources">
    ///     The source values.
    /// </param>
    /// <param name="factory">
    ///     The factory function to convert the source value to a new instance of TTarget.
    /// </param>
    /// <typeparam name="TSource">
    ///     The type of the source value.
    /// </typeparam>
    /// <typeparam name="TTarget">
    ///     The type of the target value.
    /// </typeparam>
    /// <returns>
    ///     A new instance of TTarget if the factory function succeeds, otherwise a None per element in the source collection.
    /// </returns>
    public static IEnumerable<Option<TTarget>> Map<TSource, TTarget>(this IEnumerable<TSource> sources,
        Func<TSource, TTarget> factory) => sources.Select(s => s.Map(factory));

    /// <summary>
    ///     Try to create a new instance of TTarget using the factory function.
    /// </summary>
    /// <param name="func">
    ///     The factory function to create a new instance of TTarget.
    /// </param>
    /// <typeparam name="T">
    ///     The type of the target value.
    /// </typeparam>
    /// <returns></returns>
    private static Option<T> TryCreate<T>(Func<T> func)
    {
        try
        {
            return Option<T>.Some(func());
        }
        catch
        {
            return Option<T>.None;
        }
    }
}