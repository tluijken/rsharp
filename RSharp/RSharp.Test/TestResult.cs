namespace RSharp.Test;

public class TestResult
{
    private static Result<int, Exception> Divide(int a, int b)
    {
        try
        {
            var result = a / b;
            return result;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
    
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
    
    [Fact]
    public void TestUnwrapOr()
    {
        var result = Divide(4, 0);
        Assert.Equal(2, result.UnwrapOr(2));
    }
    
    [Fact]
    public void TestUnwrapOrElse()
    {
        var result = Divide(4, 0);
        Assert.Equal(2, result.UnwrapOrElse(() => 2));
    }
    
    [Fact]
    public void TestUnwrapOrElseWithException()
    {
        var result = Divide(4, 0);
        Assert.Equal(2, result.UnwrapOrElse(ex => 2));
    }
    
    [Fact]
    public void TestExpect()
    {
        var result = Divide(4, 0);
        Assert.Throws<Exception>(() => result.Expect("This is an error"));
    }
}