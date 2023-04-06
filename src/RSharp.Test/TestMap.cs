namespace RSharp.Test;

internal record SourceObject(int Id, string Name, string Description, double Value);

internal record TargetObject(Guid Id, string Name, string Description, double Value);

public class TestMap
{
    private static readonly Func<SourceObject, TargetObject> Factory = source =>
        new TargetObject(new Guid(source.Id.ToString().PadLeft(32, '0')), source.Name, source.Description,
            source.Value);

    [Fact]
    public void TestMapSingleAToSingleB()
    {
        var a = new SourceObject(1, "Name", "Description", 1.0);
        var b = a.Map(Factory);
        Assert.NotNull(b);
        Assert.True(b.IsSome());
        var result = b.Unwrap();
        CompareSourceWithMappedTarget(result, a);
    }

    [Fact]
    public void TestMapArrayAToArrayB()
    {
        var a = new List<SourceObject>
        {
            new(1, "Object 1", "This is the first element in the list", 1.0),
            new(2, "Object 2", "This is the second element in the list", 2.0),
            new(3, "Object 3", "This is the third element in the list", 3.0)
        };
        var b = a.Map(Factory);
        Assert.NotNull(b);
        Assert.True(b.All(m => m.IsSome()));
        Assert.NotNull(b);
        var targetObjects = b.Select(d => d.Unwrap()).ToList();
        Assert.Equal(a.Count, targetObjects.Count);
        targetObjects.ForEach((item, index) => CompareSourceWithMappedTarget(item, a[index]));
    }


    [Fact]
    public void TestMappingFailsSingleItem()
    {
        var a = new SourceObject(1, "Name", "Description", 1.0);
        var b = a.Map<SourceObject, TargetObject>(null!);
        Assert.NotNull(b);
        Assert.False(b.IsSome());
        Assert.Throws<Exception>(() => b.Unwrap());
    }

    [Fact]
    public void TestMappingFailsArray()
    {
        var a = new List<SourceObject>
        {
            new(-1, "Object 1", "This is the first element in the list", 1.0),
            new(2, "Object 2", "This is the second element in the list", 2.0),
            new(3, "Object 3", "This is the third element in the list", 3.0)
        };
        var b = a.Map(Factory);
        Assert.NotNull(b);
        Assert.False(b.All(m => m.IsSome()));
        
        var failed = b.Where(m => !m.IsSome()).ToList();
        Assert.Single(failed);
        Assert.Throws<Exception>(() => failed[0].Unwrap());
        
        var success = b.Where(m => m.IsSome()).ToList();
        Assert.Equal(2, success.Count);
        var targetObjects = success.ConvertAll(d => d.Unwrap());
        targetObjects.ForEach((item, index) => CompareSourceWithMappedTarget(item, a[index + 1]));
    }

    private static void CompareSourceWithMappedTarget(TargetObject b, SourceObject a)
    {
        Assert.NotNull(b);
        Assert.Equal(a.Description, b.Description);
        Assert.Equal(a.Name, b.Name);
        Assert.Equal(a.Value, b.Value);
        Assert.Equal(new Guid(a.Id.ToString().PadLeft(32, '0')), b.Id);
    }
}