using System.Diagnostics.CodeAnalysis;

namespace RSharp;

[SuppressMessage("ReSharper", "CA1031", Justification = "We don't know what exceptions the factory function will throw.")]
public static class MapExtensions
{
    /// <summary>
    ///     Maps a source object to a target object using a factory function.
    /// </summary>
    /// <param name="source">
    ///     The source object.
    /// </param>
    /// <param name="factory">
    ///     The factory function.
    /// </param>
    /// <typeparam name="TSource">
    ///     The type of the source object.
    /// </typeparam>
    /// <typeparam name="TTarget">
    ///     The type of the target object.
    /// </typeparam>
    /// <returns>
    ///     A result containing the target object or an exception.
    /// </returns>
    public static Result<TTarget, Exception> Map<TSource, TTarget>(this TSource source, Func<TSource, TTarget> factory)
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
    ///     Maps an enumerable of source objects to a target object using a factory function.
    /// </summary>
    /// <param name="sources">
    ///     The source objects.
    /// </param>
    /// <param name="factory">
    ///     The factory function per source object to create the target object.
    /// </param>
    /// <typeparam name="TSource">
    ///     The type of the source object.
    /// </typeparam>
    /// <typeparam name="TTarget">
    ///     The type of the target object.
    /// </typeparam>
    /// <returns>
    ///     An enumerable, containing the mapping results..
    /// </returns>
    public static IEnumerable<Result<TTarget, Exception>> Map<TSource, TTarget>(this IEnumerable<TSource> sources, Func<TSource, TTarget> factory) => sources.Select(s => s.Map(factory));
}