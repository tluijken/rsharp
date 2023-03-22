namespace RSharp.Test;

public class TestResult
{
    private static Result<int, Exception> Divide(int a, int b) =>
        b == 0
            ? new DivideByZeroException("Cannot divide by zero")
            : a / b;

    [Fact]
    public void TestImplicitOk()
    {
        Result<int, Exception> result = 2;
        Assert.True(result.IsOk());
        Assert.Equal(2, result.Unwrap());
    }
    
    [Fact]
    public void TestImplicitErr()
    {
        Result<int, Exception> result = new DivideByZeroException();
        Assert.True(result.IsErr());
        Assert.Throws<DivideByZeroException>(() => result.Unwrap());
    }

    [Theory]
    [InlineData(4, 2, 2)]
    [InlineData(4, 0, 0)]
    public void TestMatch(int a, int b, int expected)
    {
        var result = Divide(a, b);
        result.Match(
            v => Assert.Equal(expected, v),
            ex => Assert.IsType<DivideByZeroException>(ex));
    }
    
    [Fact]
    public void TestOk()
    {
        var result = Divide(4, 2);
        Assert.True(result.IsOk());
        Assert.Equal(2, result.Unwrap());
    }
    
    [Fact]
    public void TestErr()
    {
        var result = Divide(4, 0);
        Assert.True(result.IsErr());
        Assert.Throws<DivideByZeroException>(() => result.Unwrap());
    }
    
    [Theory]
    [InlineData(4, 2, 2)]
    [InlineData(4, 0, 0)]
    public void TestUnwrapOr(int a, int b, int expected)
    {
        var result = Divide(a, b);
        Assert.Equal(expected, result.UnwrapOr(0));
    }
    
    [Theory]
    [InlineData(4, 2, 2)]
    [InlineData(4, 0, 0)]
    public void TestUnwrapOrElse(int a, int b, int expected)
    {
        var result = Divide(a, b);
        Assert.Equal(expected, result.UnwrapOrElse(() => 0));
    }

    [Theory]
    [InlineData(4, 2, 2)]
    [InlineData(4, 0, 2)]
    public void TestUnwrapOrElseWithException(int a, int b, int expected)
    {
        var result = Divide(a, b);
        Assert.Equal(expected, result.UnwrapOrElse(ex => 2));
    }
    
    [Theory]
    [InlineData(4, 2, 2)]
    [InlineData(4, 0, 2)]
    public void TestExpect(int a, int b, int expected)
    {
        var result = Divide(a, b);
        result.Match(
            r => Assert.Equal(expected, r),
            _ => Assert.Throws<Exception>(() => result.Expect("This is an error")));
    }
}