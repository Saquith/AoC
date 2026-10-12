using AdventOfCode2024.Models._2;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2024.Challenges;

public class Challenge2(IConfiguration config) : IChallenge
{
    private List<Report>? _reports;

    public async Task ReadInput(string? fileName = null)
    {
        var inputFilePath = Path.Combine(config["InputFolderPath"]!, $"{fileName ?? GetType().Name.Substring(9)}.txt");
        if (!File.Exists(inputFilePath)) throw new FileNotFoundException("The input file could not be found.");

        await using var stream = File.OpenRead(inputFilePath);
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

        return ($"Safe reports: {strictCount}",
            $"Dampener safe reports: {count}");
    }
}