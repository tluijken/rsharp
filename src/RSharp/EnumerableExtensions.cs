namespace RSharp;

public static class EnumerableExtensions
{
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T, int> action) =>
        source.Select((item, index) =>
        {
            action(item, index);
            return item;
        });

    public static IEnumerable<TResult> ForEach<T, TResult>(this IEnumerable<T> source, Func<T, int, TResult> action) =>
        source.Select(action);
}