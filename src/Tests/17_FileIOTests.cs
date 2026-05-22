using Library;
using Xunit;

namespace Tests;

/// <summary>
/// Example 17 — File I/O Tests
///
/// File system tests need careful cleanup to stay isolated:
///   - Use Path.GetTempPath() / Path.GetTempFileName() for temp files
///   - Always delete files in IDisposable.Dispose() or a finally block
///   - Test both happy path (file exists) and error path (file missing)
/// </summary>
public class FileIOTests : IDisposable
{
    private readonly FileProcessor _processor = new();
    private readonly string _tempFile;

    public FileIOTests()
    {
        // A unique temp file per test instance
        _tempFile = Path.Combine(Path.GetTempPath(), $"fileio_test_{Guid.NewGuid():N}.txt");
    }

    public void Dispose()
    {
        if (File.Exists(_tempFile)) File.Delete(_tempFile);
    }

    [Fact]
    public void WriteLines_FileIsCreated()
    {
        _processor.WriteLines(_tempFile, ["Hello", "World"]);
        Assert.True(File.Exists(_tempFile));
    }

    [Fact]
    public void ReadLines_FileDoesNotExist_ThrowsFileNotFoundException()
    {
        Assert.Throws<FileNotFoundException>(
            () => _processor.ReadLines("/nonexistent/path/file.txt"));
    }

    [Fact]
    public void WriteAndRead_ContentRoundTrips()
    {
        var original = new[] { "Alpha", "Beta", "Gamma" };
        _processor.WriteLines(_tempFile, original);

        var result = _processor.ReadLines(_tempFile);

        Assert.Equal(original, result);
    }

    [Fact]
    public void AppendLine_AddsNewLineToExistingFile()
    {
        _processor.WriteLines(_tempFile, ["First line"]);
        _processor.AppendLine(_tempFile, "Second line");

        var lines = _processor.ReadLines(_tempFile);
        Assert.Equal(2, lines.Length);
        Assert.Equal("First line",  lines[0]);
        Assert.Equal("Second line", lines[1]);
    }

    [Fact]
    public void CountLines_MatchesWrittenCount()
    {
        _processor.WriteLines(_tempFile, ["a", "b", "c", "d", "e"]);
        Assert.Equal(5, _processor.CountLines(_tempFile));
    }

    [Fact]
    public void WordFrequency_CountsWordsCorrectly()
    {
        _processor.WriteLines(_tempFile, ["the cat sat on the mat", "the cat"]);

        var freq = _processor.WordFrequency(_tempFile);

        Assert.Equal(3, freq["the"]);
        Assert.Equal(2, freq["cat"]);
        Assert.Equal(1, freq["mat"]);
    }

    [Fact]
    public void FileExists_AfterDelete_ReturnsFalse()
    {
        _processor.WriteLines(_tempFile, ["data"]);
        Assert.True(_processor.FileExists(_tempFile));

        _processor.DeleteFile(_tempFile);
        Assert.False(_processor.FileExists(_tempFile));
    }
}
