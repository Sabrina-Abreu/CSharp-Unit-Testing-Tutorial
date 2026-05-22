namespace Library;

/// <summary>
/// Validators used to demonstrate boolean/guard tests and null tests.
/// </summary>
public class PasswordValidator
{
    private const string SpecialChars = "!@#$%^&*";

    public bool IsValid(string? password)
    {
        if (string.IsNullOrEmpty(password)) return false;
        return password.Length >= 8
            && password.Any(char.IsUpper)
            && password.Any(char.IsLower)
            && password.Any(char.IsDigit)
            && password.Any(c => SpecialChars.Contains(c));
    }

    public string GetStrength(string password)
    {
        int score = 0;
        if (password.Length >= 8) score++;
        if (password.Length >= 12) score++;
        if (password.Any(char.IsUpper)) score++;
        if (password.Any(char.IsDigit)) score++;
        if (password.Any(c => SpecialChars.Contains(c))) score++;
        return score switch { >= 5 => "Strong", >= 3 => "Medium", _ => "Weak" };
    }
}

public class AgeValidator
{
    public bool IsAdult(int age) => age >= 18;
    public bool IsValidAge(int age) => age >= 0 && age <= 150;

    public string GetCategory(int age) => age switch
    {
        < 0 => throw new ArgumentOutOfRangeException(nameof(age), "Age cannot be negative."),
        < 13 => "Child",
        < 18 => "Teenager",
        < 65 => "Adult",
        _ => "Senior"
    };
}

public class EmailValidator
{
    public bool IsValid(string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        var parts = email.Split('@');
        return parts.Length == 2 && parts[0].Length > 0 && parts[1].Contains('.');
    }
}
