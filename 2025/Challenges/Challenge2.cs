using AdventOfCode2025.Models;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2025.Challenges;

public class Challenge2(IConfiguration config) : BaseChallenge
{
    private List<(long, long)>? _ranges;

    protected override async Task ChallengeInput(string? fileName = null)
    {
        var inputFilePath = Path.Combine(config["InputFolderPath"]!, $"{fileName ?? GetType().Name.Substring(9)}.txt");
        if (!File.Exists(inputFilePath)) throw new FileNotFoundException("The input file could not be found.");

        await using var stream = File.OpenRead(inputFilePath);
        using var reader = new StreamReader(stream);

        _ranges = [];

        string? currentLine;
        while (!string.IsNullOrEmpty(currentLine = await reader.ReadLineAsync()))
        {
            foreach (var rangeString in currentLine.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                var split = rangeString.Split('-', StringSplitOptions.RemoveEmptyEntries);
                _ranges.Add((long.Parse(split[0]), long.Parse(split[1])));
            }
        }
    }

    public override (string, string) Calculate()
    {
        var invalidIds = new HashSet<string>();
        long invalidHalfIdSum = 0;
        long invalidIdSum = 0;
        foreach (var t in _ranges!)
        {
            for (var i = t.Item1; i <= t.Item2; i++)
            {
                // First pass
                var id = i.ToString();
                for (var divider = 1; divider < id.Length; divider++)
                {
                    var possibleRepeatingSequence = id.Take(divider);
                    if (possibleRepeatingSequence.SequenceEqual(id.Skip(divider)))
                    {
                        invalidIds.Add(id);

                        invalidHalfIdSum += long.Parse(id);
                        break;
                    }
                }
                
                // Second pass
                if (invalidIds.Contains(id))
                {
                    invalidIdSum += long.Parse(id);
                    continue;
                }
                
                for (var divider = 1; divider <= id.Length / 2; divider++)
                {
                    var possibleRepeatingSequence = id.Substring(0, divider);
                    var currentSequence = "";
                    for (var c = 0; c < id.Length / divider; c++)
                        currentSequence += possibleRepeatingSequence;
                    
                    invalidIds.Add(currentSequence);
                    if (currentSequence.Equals(id))
                    {
                        invalidIdSum += long.Parse(id);
                        break;
                    }
                }
            }
        }
        
        return ($"{invalidHalfIdSum}", $"{invalidIdSum}");
    }
}