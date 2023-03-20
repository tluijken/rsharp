namespace RSharp;

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
    ///     The target object.
    /// </returns>
    public static TTarget Map<TSource, TTarget>(this TSource source, Func<TSource, TTarget> factory) => factory(source);

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
    ///     An enumerable of target objects.
    /// </returns>
    public static IEnumerable<TTarget> Map<TSource, TTarget>(this IEnumerable<TSource> sources, Func<TSource, TTarget> factory) => sources.Select(factory);
}