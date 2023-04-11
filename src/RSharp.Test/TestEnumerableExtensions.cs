namespace RSharp.Test;

public class TestEnumerableExtensions
{
    private readonly int[] _ages = { 22, 20, 29, 66, 42, 74, 15, 33, 45, 6 };

    private readonly string[] _names =
        { "John", "Paul", "George", "Ringo", "Pete", "John", "Paul", "George", "Ringo", "Pete" };

    [Fact]
    public void TestForEachFunction()
    {
        // Iterate over the the numbers 0 to 10, and select a new Person object for each iteration based on the index
        var result = Enumerable.Range(0, 10).ForEach((_, index) => new Person(_names[index], _ages[index]));

        // Now we enumerate the result, so the action should be executed
        Assert.Equal(10, result.Count());
        Enumerable.Range(0, 10).Select(i => new Person(_names[i], _ages[i])).ForEach((item, index) =>
        {
            Assert.Equal(item.Name, result.ElementAt(index).Name);
            Assert.Equal(item.Age, result.ElementAt(index).Age);
        });
    }

    [Fact]
    public void TestForEachAction()
    {
        // Iterate over the the numbers 0 to 10, and select a new Person object for each iteration based on the index
        var persons = new List<Person>();
        var enumerableResult = Enumerable.Range(0, 10)
            .ForEach((_, index) => persons.Add(new Person(_names[index], _ages[index])));

        // The result is an IEnumerable, so the action should not be executed yet
        Assert.Empty(persons);

        // Now we enumerate the result, so the action should be executed
        Assert.Equal(10, enumerableResult.Count());
        Assert.Equal(10, persons.Count);
        Enumerable.Range(0, 10).Select(i => new Person(_names[i], _ages[i])).ForEach((item, index) =>
        {
            Assert.Equal(item.Name, persons.ElementAt(index).Name);
            Assert.Equal(item.Age, persons.ElementAt(index).Age);
        });
    }

    private record Person(string Name, int Age);
}