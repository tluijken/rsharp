namespace RSharp;

public static class FunctionalExtensions
{
    public static void Match<T>(Option<T> option, Action<T> some, Action none)
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

    // Notice that 'All' will also return early as soon as one of the validations fails
    public static bool Validate<T>(this T @this, params Func<T, bool>[] predicates) => predicates.All(p => p(@this));
    

    /// <summary>
    ///     Performs the specified action on the given value and returns the value.
    /// </summary>
    /// <param name="this">
    ///    The value to perform the action on.
    /// </param>
    /// <param name="action">
    ///    The action to perform on the value.
    /// </param>
    /// <typeparam name="T">
    ///   The type of the value.
    /// </typeparam>
    /// <returns>
    ///  The value.
    /// </returns>
    public static T Tee<T>(this T @this, Action<T> action)
    {
        if (@this is not null && @this is not None<T>)
            action(@this);
        return @this;
    }
}