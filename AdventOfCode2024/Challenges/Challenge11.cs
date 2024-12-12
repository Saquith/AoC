using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2024.Challenges;

public class Challenge11(IConfiguration config) : IChallenge
{
    private Dictionary<string, long> _countByValues = [];

    public async Task ReadInput(string? fileName = null)
    {
        var inputFilePath = Path.Combine(config["InputFolderPath"]!, $"{fileName ?? GetType().Name.Substring(9)}.txt");
        if (!File.Exists(inputFilePath)) throw new FileNotFoundException("The input file could not be found.");

        await using var stream = File.OpenRead(inputFilePath);
        using var reader = new StreamReader(stream);

        string? currentLine;
        while (!string.IsNullOrEmpty(currentLine = await reader.ReadLineAsync()))
        {
            // Parse
            var split = currentLine.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            foreach (var value in split)
            {
                _countByValues.TryAdd(value, 0);
                _countByValues[value]++;
            }
            
        }
    }

    public long RunBlinks(int runSimulationCount)
    {
        long amountOfStones = 0;
        for (var c = 0; c < runSimulationCount; c++)
        {
            Debug.WriteLine($"Run #{c}");
            Dictionary<string, long> nextCountByValues = [];
            foreach (var (value, count) in _countByValues)
            {
                if (c == 0)
                    amountOfStones += count;
            
                if (value == "0")
                {
                    nextCountByValues.TryAdd("1", 0);
                    nextCountByValues["1"] += count;
                }
                else if (value.Length % 2 == 0)
                {
                    var firstPart = value.Substring(0, value.Length / 2);
                    nextCountByValues.TryAdd(firstPart, 0);
                    nextCountByValues[firstPart] += count;
                    
                    var secondPart = value.Substring(value.Length / 2).TrimStart('0');
                    if (secondPart.Length == 0)
                        secondPart = "0";
                    nextCountByValues.TryAdd(secondPart, 0);
                    nextCountByValues[secondPart] += count;
                    amountOfStones += count;
                }
                else
                {
                    var newValue = $"{long.Parse(value) * 2024}";
                    nextCountByValues.TryAdd(newValue, 0);
                    nextCountByValues[newValue] += count;
                }
            }
            
            _countByValues = nextCountByValues;
        }
        
        Debug.WriteLine(amountOfStones);
        return amountOfStones;
    }

    public (string, string) Calculate()
    {
        var originalInput = _countByValues;
        var run25StoneCount = RunBlinks(25);
        
        _countByValues = originalInput;
        var run75StoneCount = RunBlinks(75);
        
        return ($"{ run25StoneCount }",
            $"{ run75StoneCount }");
    }
}