namespace Library;

/// <summary>
/// Utility class for string operations, used to demonstrate string and custom-assertion tests.
/// </summary>
public class StringHelper
{
    public string Reverse(string input)
    {
        ArgumentNullException.ThrowIfNull(input);
        return new string(input.Reverse().ToArray());
    }

    public bool IsPalindrome(string input)
    {
        ArgumentNullException.ThrowIfNull(input);
        var clean = input.ToLowerInvariant().Replace(" ", "");
        return clean == new string(clean.Reverse().ToArray());
    }

    public string ToPascalCase(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input;
        return string.Concat(
            input.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                 .Select(w => char.ToUpper(w[0]) + w[1..].ToLower()));
    }

    public int CountWords(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return 0;
        return input.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
    }

    public string MaskEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return email;
        var atIndex = email.IndexOf('@');
        if (atIndex <= 1) return email;
        return email[0] + new string('*', atIndex - 1) + email[atIndex..];
    }

    public string Truncate(string input, int maxLength)
    {
        if (string.IsNullOrEmpty(input)) return input;
        return input.Length <= maxLength ? input : input[..maxLength] + "...";
    }
}
