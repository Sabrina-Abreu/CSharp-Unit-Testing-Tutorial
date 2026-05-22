using Library;
using Xunit;

namespace Tests;

/// <summary>
/// Example 02 — String Assertions
///
/// xUnit provides string-specific helpers:
///   - Assert.Equal with StringComparison for case-insensitive checks
///   - Assert.Contains / Assert.DoesNotContain
///   - Assert.StartsWith / Assert.EndsWith
///   - Assert.Matches (regex)
///   - Assert.Empty / Assert.NotEmpty
/// </summary>
public class StringTests
{
    private readonly StringHelper _helper = new();

    [Fact]
    public void Reverse_NormalString_ReturnsReversed()
    {
        string result = _helper.Reverse("hello");
        Assert.Equal("olleh", result);
    }

    [Fact]
    public void Reverse_SingleChar_ReturnsSame()
    {
        Assert.Equal("x", _helper.Reverse("x"));
    }

    [Fact]
    public void IsPalindrome_Racecar_ReturnsTrue()
    {
        Assert.True(_helper.IsPalindrome("racecar"));
    }

    [Fact]
    public void IsPalindrome_WithSpaces_IgnoresSpaces()
    {
        Assert.True(_helper.IsPalindrome("A man a plan a canal Panama"));
    }

    [Fact]
    public void IsPalindrome_RegularWord_ReturnsFalse()
    {
        Assert.False(_helper.IsPalindrome("hello"));
    }

    [Fact]
    public void ToPascalCase_LowercaseSentence_CapitalizesEachWord()
    {
        string result = _helper.ToPascalCase("hello world");
        Assert.Equal("HelloWorld", result);
        Assert.StartsWith("H", result);
    }

    [Fact]
    public void CountWords_MultiWordString_ReturnsCorrectCount()
    {
        int count = _helper.CountWords("the quick brown fox");
        Assert.Equal(4, count);
    }

    [Fact]
    public void CountWords_EmptyString_ReturnsZero()
    {
        int count = _helper.CountWords("   ");
        Assert.Equal(0, count);
    }

    [Fact]
    public void MaskEmail_ValidEmail_MasksLocalPart()
    {
        string result = _helper.MaskEmail("john@example.com");
        Assert.Contains("@example.com", result);
        Assert.StartsWith("j", result);
        Assert.DoesNotContain("ohn", result);  // middle is masked
    }

    [Fact]
    public void Truncate_LongString_AppendEllipsis()
    {
        string result = _helper.Truncate("Hello, World!", 5);
        Assert.EndsWith("...", result);
        Assert.Equal("Hello...", result);
    }

    [Fact]
    public void Truncate_ShortString_ReturnsOriginal()
    {
        string result = _helper.Truncate("Hi", 10);
        Assert.Equal("Hi", result);
    }

    [Fact]
    public void Reverse_CaseInsensitiveComparison()
    {
        string result = _helper.Reverse("ABC");
        // "CBA" should equal "cba" when ignoring case
        Assert.Equal("cba", result, ignoreCase: true);
    }
}
