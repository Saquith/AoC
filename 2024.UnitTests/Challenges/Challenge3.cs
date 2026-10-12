using System.Text.RegularExpressions;
using AdventOfCodeLibrary.Challenges;
using AdventOfCodeLibrary.Models;

namespace AdventOfCode2024.Challenges;

public class Challenge3 : IChallenge
{
    private List<Operation>? _operations;

    public async Task ReadInput(string filePath)
    {
        await using var stream = File.OpenRead(filePath);
        using var reader = new StreamReader(stream);

        _operations = [];

        string? currentLine;
        while (!string.IsNullOrEmpty(currentLine = await reader.ReadLineAsync()))
        {
            // Parse
            var matches = Regex.Matches(currentLine, @"((don't|do)\(\))|(mul)\((\d{1,3}),(\d{1,3})\)");
            foreach (Match match in matches)
            {
                // First match is full matched string, skip first group
                var operation = !string.IsNullOrEmpty(match.Groups[2].Value)
                    ? new Operation(match.Groups[2].Value, 0, 0)
                    : new Operation(match.Groups[3].Value, long.Parse(match.Groups[4].Value),
                        long.Parse(match.Groups[5].Value));

                _operations.Add(operation);
            }
        }
    }

    public (string, string) Calculate()
    {
        long total = 0;
        foreach (var operation in _operations!)
            if (operation.OperationType == "mul")
                total += operation.FirstNumber * operation.SecondNumber;

        var enabled = true;
        long enabledMultiplicationsTotal = 0;
        foreach (var operation in _operations!)
            switch (operation.OperationType)
            {
                case "mul":
                    if (enabled)
                        enabledMultiplicationsTotal += operation.FirstNumber * operation.SecondNumber;
                    break;
                case "do":
                    enabled = true;
                    break;
                case "don't":
                    enabled = false;
                    break;
            }

        return ($"{total}", $"{enabledMultiplicationsTotal}");
    }
}