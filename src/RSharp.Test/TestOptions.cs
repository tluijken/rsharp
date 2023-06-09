namespace RSharp.Test;

public class TestOptions
{
    private static Option<string> GetSomeValue() => "Some value";

    private static Option<string> GetNoValue() => null!;

    [Fact]
    public void TestSome()
    {
        var some = GetSomeValue();
        Assert.True(some.IsSome());
        Assert.Equal("Some value", some);
    }

    [Fact]
    public void TestNone()
    {
        var none = GetNoValue();
        Assert.True(none.IsNone());
    }

    [Theory]
    [InlineData("Hello world")]
    [InlineData(null)]
    public void TestSomeWithNull(string value)
    {
        var option = value.ToOption();
        option.Match(
            v => Assert.Equal(value, v),
            () => Assert.True(option.IsNone()));
    }

    [Fact]
    public void TestHashCodes()
    {
        var some = GetSomeValue();
        Assert.Equal("Some value".GetHashCode(), some.GetHashCode());
    }
}