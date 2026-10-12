
using AdventOfCodeLibrary.Challenges;
using AdventOfCodeLibrary.Models;

namespace AdventOfCode2024.Challenges;

public class Challenge2 : IChallenge
{
    private List<Report>? _reports;

    public async Task ReadInput(string filePath)
    {
        await using var stream = File.OpenRead(filePath);
        using var reader = new StreamReader(stream);

        _reports = [];

        string? currentLine;
        while (!string.IsNullOrEmpty(currentLine = await reader.ReadLineAsync()))
        {
            // Parse
            _reports.Add(Report.Parse(currentLine));
        }
    }

    public (string, string) Calculate()
    {
        var strictCount = _reports!.Count(r => r.IsSafe());
        var count = _reports!.Count(r => r.IsSafe(false));

        return ($"{strictCount}", $"{count}");
    }
}