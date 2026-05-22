namespace Library;

/// <summary>
/// File processor used in setup/teardown (ex 15) and file I/O tests (ex 17).
/// </summary>
public class FileProcessor
{
    public void WriteLines(string filePath, IEnumerable<string> lines) =>
        File.WriteAllLines(filePath, lines);

    public string[] ReadLines(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");
        return File.ReadAllLines(filePath);
    }

    public int CountLines(string filePath) => ReadLines(filePath).Length;

    public Dictionary<string, int> WordFrequency(string filePath)
    {
        var words = ReadLines(filePath)
            .SelectMany(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Select(w => w.ToLowerInvariant().Trim('.', ',', '!', '?'));

        var freq = new Dictionary<string, int>();
        foreach (var word in words)
            freq[word] = freq.GetValueOrDefault(word) + 1;
        return freq;
    }

    public void AppendLine(string filePath, string line) =>
        File.AppendAllText(filePath, line + Environment.NewLine);

    public bool FileExists(string filePath) => File.Exists(filePath);
    public void DeleteFile(string filePath) => File.Delete(filePath);
}
