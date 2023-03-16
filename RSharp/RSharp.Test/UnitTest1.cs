namespace RSharp.Test;

using Assert = AssertExtensions;

public class UnitTest1
{
    private Option<string> GetSomeValue()
    {
        return "Some value";
    }

    private Option<string> GetNoValue()
    {
        return null;
    }

    [Fact]
    public void TestSome()
    {
        var some = GetSomeValue();
        Xunit.Assert.True(some is Some<string>);
        Xunit.Assert.Equal("Some value", some);
    }

    [Fact]
    public void TestNone()
    {
        var none = GetNoValue();
        Xunit.Assert.Null(none);
    }

    [Theory]
    [InlineData("Hello world")]
    [InlineData(null)]
    public void TestSomeWithNull(string value)
    {
        var option = value.ToOption();
        option.Match(
            some: v => Xunit.Assert.Equal(value, v),
            none: () => Assert.None(option));
    }
}

public class AssertExtensions : Xunit.Assert
{
    public static void None<T>(Option<T> option)
    {
        switch (option)
        {
            case Some<T>:
                throw new Xunit.Sdk.TrueException("Expected None, but got Some", false);
            case None<T>:
                return;
            default:
                throw new Xunit.Sdk.TrueException("Expected None, but got null", null);
        }
    }
}