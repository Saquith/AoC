using AdventOfCode2025.Extensions;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2025.Challenges;

public class Challenge1(IConfiguration config) : BaseChallenge
{
    private List<int>? _operations;

    protected override async Task ChallengeInput(string? fileName = null)
    {
        var inputFilePath = Path.Combine(config["InputFolderPath"]!, $"{fileName ?? GetType().Name.Substring(9)}.txt");
        if (!File.Exists(inputFilePath)) throw new FileNotFoundException("The input file could not be found.");

        await using var stream = File.OpenRead(inputFilePath);
        using var reader = new StreamReader(stream);

        _operations = [];

        string? currentLine;
        while (!string.IsNullOrEmpty(currentLine = await reader.ReadLineAsync()))
        {
            // Parse
            var rotation = currentLine[0];
            var rotationCount = int.Parse(currentLine[1..]);
            
            _operations.Add(rotation.Equals('L') ? -rotationCount : rotationCount);
        }
    }

    public override (string, string) Calculate()
    {
        int zeroCount = 0;
        int zeroPassCount = 0;

        var previousLocation = 50;
        foreach (var i in _operations!)
        {
            var rotation = i;
            if ((previousLocation + rotation) % 100 == 0)
                zeroCount++;

            var zeroesFound = 0;
            var fullRevolutionsInRotation = Math.Abs(rotation / 100);
            zeroesFound += fullRevolutionsInRotation;
            rotation %= 100;
            
            var firstHundredth = previousLocation / 100;
            var location = previousLocation + rotation;
            var secondHundredth = location / 100;
            var difference = Math.Abs(firstHundredth - secondHundredth);
            zeroesFound += difference;
            if (location <= 0 && previousLocation != 0)
                zeroesFound++;
            
            zeroPassCount += zeroesFound;
            
            location %= 100;
            if (location < 0)
                location += 100;
            
            Console.WriteLine($"{rotation} {location} ({zeroesFound}); total: {zeroPassCount}");
            previousLocation += rotation;
            previousLocation %= 100;
            if (previousLocation < 0)
                previousLocation += 100;
        }
        
        return ($"{zeroCount}", $"{zeroPassCount}");
    }
}