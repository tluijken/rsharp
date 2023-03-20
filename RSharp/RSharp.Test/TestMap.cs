namespace RSharp.Test;

internal record SourceObject(int Id, string Name, string Description, double Value);

internal record TargetObject(Guid Id, string Name, string Description, double Value);

public class TestMap
{
    [Fact]
    public void TestMapSingleAToSingleB()
    {
        var a = new SourceObject(1, "Name", "Description", 1.0);
        var b = a.Map(source => new TargetObject(new Guid(a.Id.ToString().PadLeft(32, '0')), source.Name, source.Description, source.Value));
        Assert.NotNull(b);
        Assert.Equal(a.Description, b.Description);
        Assert.Equal(a.Name, b.Name);
        Assert.Equal(a.Value, b.Value);
        Assert.Equal(new Guid(a.Id.ToString().PadLeft(32, '0')), b.Id);
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
        var b = a.Map(source => new TargetObject(new Guid(source.Id.ToString().PadLeft(32, '0')), source.Name, source.Description, source.Value)).ToList();
        Assert.NotNull(b);
        Assert.Equal(a.Count, b.Count);
        for (var i = 0; i < a.Count; i++)
        {
            Assert.Equal(a[i].Description, b.ElementAt(i).Description);
            Assert.Equal(a[i].Name, b.ElementAt(i).Name);
            Assert.Equal(a[i].Value, b.ElementAt(i).Value);
            Assert.Equal(new Guid(a[i].Id.ToString().PadLeft(32, '0')), b.ElementAt(i).Id);
        }
    }
}