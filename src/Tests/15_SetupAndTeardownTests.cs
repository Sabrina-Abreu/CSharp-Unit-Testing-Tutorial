using Library;
using Xunit;

namespace Tests;

// --------------------------------------------------------------------------
// Shared fixture — expensive setup created once per test class
// --------------------------------------------------------------------------

/// <summary>
/// A shared fixture creates a temporary directory once for all tests in the class
/// and deletes it in Dispose — just like a database transaction rollback.
/// </summary>
public sealed class TempDirectoryFixture : IDisposable
{
    public string DirectoryPath { get; } =
        Path.Combine(Path.GetTempPath(), "xunit_" + Guid.NewGuid().ToString("N"));

    public TempDirectoryFixture() => Directory.CreateDirectory(DirectoryPath);

    public void Dispose() => Directory.Delete(DirectoryPath, recursive: true);
}

// --------------------------------------------------------------------------
// Test class using setup (ctor) and teardown (IDisposable)
// --------------------------------------------------------------------------

/// <summary>
/// Example 15 — Setup and Teardown
///
/// xUnit does NOT have [SetUp]/[TearDown] like NUnit.
/// Instead it uses standard C# idioms:
///   - Constructor     : per-test setup (runs before each [Fact])
///   - IDisposable     : per-test teardown (runs after each [Fact])
///   - IClassFixture&lt;T&gt;: shared setup/teardown for all tests in a class
/// </summary>
public class SetupAndTeardownTests : IClassFixture<TempDirectoryFixture>, IDisposable
{
    private readonly TempDirectoryFixture _fixture;
    private readonly FileProcessor _processor;
    private readonly string _testFile;

    // Constructor = per-test setup
    public SetupAndTeardownTests(TempDirectoryFixture fixture)
    {
        _fixture   = fixture;
        _processor = new FileProcessor();
        // Each test gets its own unique file name inside the shared temp directory
        _testFile  = Path.Combine(_fixture.DirectoryPath, $"test_{Guid.NewGuid():N}.txt");
    }

    // IDisposable.Dispose = per-test teardown
    public void Dispose()
    {
        if (File.Exists(_testFile))
            File.Delete(_testFile);
    }

    [Fact]
    public void WriteLines_ThenReadBack_ReturnsSameContent()
    {
        var lines = new[] { "Line 1", "Line 2", "Line 3" };
        _processor.WriteLines(_testFile, lines);

        var result = _processor.ReadLines(_testFile);

        Assert.Equal(lines, result);
    }

    [Fact]
    public void CountLines_AfterWriting_ReturnsCorrectCount()
    {
        _processor.WriteLines(_testFile, ["a", "b", "c", "d"]);
        Assert.Equal(4, _processor.CountLines(_testFile));
    }

    [Fact]
    public void AppendLine_AddsToExistingFile()
    {
        _processor.WriteLines(_testFile, ["First"]);
        _processor.AppendLine(_testFile, "Second");

        var lines = _processor.ReadLines(_testFile);
        Assert.Equal(2, lines.Length);
        Assert.Equal("Second", lines[1]);
    }

    [Fact]
    public void SharedDirectory_ExistsThroughoutAllTests()
    {
        // The fixture's directory is created once and shared
        Assert.True(Directory.Exists(_fixture.DirectoryPath));
    }
}
