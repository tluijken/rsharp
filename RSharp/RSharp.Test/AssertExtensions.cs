using Xunit.Sdk;

namespace RSharp.Test;

internal sealed class AssertExtensions : Assert
{
    public static void None<T>(Option<T> option)
    {
        switch (option)
        {
            case Some<T>:
                throw new TrueException("Expected None, but got Some", false);
            case None<T>:
                return;
            default:
                throw new TrueException("Expected None, but got null", null);
        }
    }
}