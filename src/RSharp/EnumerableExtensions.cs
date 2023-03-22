namespace RSharp;

public static class EnumerableExtensions
{
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T, int> action) =>
        source.Select((item, index) =>
        {
            action(item, index);
            return item;
        });
}