using RSharp.Test.Models;
using Xunit.Abstractions;

namespace RSharp.Test;

public class TestTee
{
    private readonly Person _person;
    private readonly ITestOutputHelper _testOutputHelper;

    public TestTee(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _person = new Person
        {
            FirstName = "John",
            LastName = "Doe",
            Age = 42,
            Spouse = new Person
            {
                FirstName = "Jane",
                LastName = null, // You bet this 'not null' property can be a null reference, see how the runtime treats you!
                Age = 41
            }
        };
    }

    [Fact]
    public void TestTeeImperative()
    {
        var spouse = _person.Spouse;
        if (spouse is not null)
        {
            _testOutputHelper.WriteLine($"{spouse.FirstName} is {spouse.Age} years old");
            if (spouse.FirstName is not null)
            {
                if (spouse.FirstName.Length > 0) Assert.Equal("Jane", spouse.FirstName);
            }
            else
            {
                throw new Exception("Spouse's first name is null");
            }
        }
        else
        {
            _testOutputHelper.WriteLine("No spouse");
        }
    }

    [Fact]
    public void TestTeeFunctional()
    {
        // Let's use Tee to print the spouse's name and age to the console in the middle of the Map chain.
        // We can place the Tee call anywhere in the chain and it will be executed of the value is Some.
        var spouseFirstName = _person
            .Map(a => a.Spouse)
            .Tee(d => _testOutputHelper.WriteLine(d switch
            {
                Some<Person> spouse => $"{spouse.Value.FirstName} is {spouse.Value.Age} years old",
                _ => "No spouse"
            }))
            .Map(spouse => spouse.FirstName);

        Assert.Equal("Jane", spouseFirstName);
    }
}