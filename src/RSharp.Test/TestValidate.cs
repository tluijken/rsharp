namespace RSharp.Test;

public class TestValidate
{
    private static readonly Func<string, bool> ValidateAllDigits =
        dutchPhoneNumber => dutchPhoneNumber[1..].All(char.IsDigit);

    private static readonly Func<string, bool> ValidatePhoneNumberLength =
        dutchPhoneNumber => dutchPhoneNumber.Length is 10 or 12 or 13;

    private static readonly Func<string, bool> ValidatePhoneNumberPrefix = dutchPhoneNumber =>
        dutchPhoneNumber.StartsWith("0") || dutchPhoneNumber.StartsWith("0031") || dutchPhoneNumber.StartsWith("+31");

    private static readonly Func<string, bool> ValidateNumberWithoutPrefixLength =
        dutchPhoneNumber => SubtractPrefix(dutchPhoneNumber).Length == 9;

    private static readonly Func<string, bool> ValidateNumberWithoutPrefixIsNotZero =
        dutchPhoneNumber => !SubtractPrefix(dutchPhoneNumber).StartsWith("0");

    private static readonly Func<string, string> SubtractPrefix = dutchPhoneNumber =>
        dutchPhoneNumber.StartsWith("0031")
            ? dutchPhoneNumber[4..]
            : dutchPhoneNumber.StartsWith("+31")
                ? dutchPhoneNumber[3..]
                : dutchPhoneNumber[1..];

    [Theory]
    [InlineData("0612345678", true)]
    [InlineData("0031612345678", true)]
    [InlineData("+31612345678", true)]
    [InlineData("0012345678", false)]
    [InlineData("06123456789", false)]
    [InlineData("0612345s78", false)]
    public void TestDutchPhoneNumberValidationImperative(string dutchPhoneNumber, bool valid)
    {
        Assert.Equal(valid, ValidateDutchPhoneNumberImperative(dutchPhoneNumber));
    }

    [Theory]
    [InlineData("0612345678", true)]
    [InlineData("0031612345678", true)]
    [InlineData("+31612345678", true)]
    [InlineData("0012345678", false)]
    [InlineData("06123456789", false)]
    [InlineData("0612345s78", false)]
    public void TestDutchPhoneNumberValidationFunctional(string dutchPhoneNumber, bool valid)
    {
        Assert.Equal(valid, ValidateDutchPhoneNumberFunctional(dutchPhoneNumber));
    }

    // This is the imperative version of the validation
    // It is not very readable and it is hard to reuse the validation logic
    // It is also hard to test the individual validations
    // It is also hard to add new validations
    // Of course, i've exaggerated a bit, but you get the point
    private static bool ValidateDutchPhoneNumberImperative(string dutchPhoneNumber)
    {
        // validate length. Prefixes could be either 0, 0031 or +31
        var isValid = dutchPhoneNumber.Length is 10 or 12 or 13;
        if (!isValid) return false;
        // validate prefix 0, 0031 or +31
        isValid = dutchPhoneNumber.StartsWith("0") ||
                  dutchPhoneNumber.StartsWith("0031") ||
                  dutchPhoneNumber.StartsWith("+31");
        if (!isValid) return false;

        // validate all characters are digits (excluding the + sign)
        isValid = dutchPhoneNumber[1..].All(char.IsDigit);
        if (!isValid) return false;

        // validate numbers without prefix are 9 long
        var numberWithoutPrefix = dutchPhoneNumber.StartsWith("0031")
            ? dutchPhoneNumber[4..]
            : dutchPhoneNumber.StartsWith("+31")
                ? dutchPhoneNumber[3..]
                : dutchPhoneNumber[1..];

        isValid = numberWithoutPrefix.Length == 9;
        if (!isValid) return false;

        // The number without the prefix must not start with 0
        isValid = !numberWithoutPrefix.StartsWith("0");
        if (!isValid) return false;

        return true;
    }

    [Theory]
    [InlineData("John Doe", true)]
    [InlineData("Justin Bieber", false)]
    [InlineData("Ju", false)]
    [InlineData("This is a username that is way too long", false)]
    public void TestUserNameValidationFunctional(string userName, bool valid)
    {
        // Look at how easy it is to compose validations with anonymous functions
        // Imagine having to add a new validation, pure joy!
        var validateUserName = userName.Validate(
            un => !string.IsNullOrWhiteSpace(un),
            un => un.Length >= 3,
            un => un.Length <= 20,
            un => !un.Equals("Justin Bieber"));
        Assert.Equal(valid, validateUserName);
    }

    // By using named functions, we can read clearly what each of the validations is doing
    private static bool ValidateDutchPhoneNumberFunctional(string dutchPhoneNumber) =>
        dutchPhoneNumber.Validate(
            ValidatePhoneNumberLength,
            ValidatePhoneNumberPrefix,
            ValidateAllDigits,
            ValidateNumberWithoutPrefixLength,
            ValidateNumberWithoutPrefixIsNotZero
        );
}