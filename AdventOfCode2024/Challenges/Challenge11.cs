using AdventOfCode2024.Models._11;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2024.Challenges;

public class Challenge11(IConfiguration config) : IChallenge
{
    private Tree<Stone> _tree;
    
    public async Task ReadInput(string? fileName = null)
    {
        var inputFilePath = Path.Combine(config["InputFolderPath"]!, $"{fileName ?? GetType().Name.Substring(9)}.txt");
        if (!File.Exists(inputFilePath)) throw new FileNotFoundException("The input file could not be found.");

        _tree = new();
        
        await using var stream = File.OpenRead(inputFilePath);
        using var reader = new StreamReader(stream);

        string? currentLine;
        while (!string.IsNullOrEmpty(currentLine = await reader.ReadLineAsync()))
        {
            // Parse
            var split = currentLine.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            foreach (var stoneValue in split)
            {
                _tree.Add(new Stone(stoneValue));
            }
        }
    }

    public (string, string) Calculate()
    {
        
        

        long secondTotal = 0;
        return ($"{ _tree.Count }",
            $"{ secondTotal }");
    }
}