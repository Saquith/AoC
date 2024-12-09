using Microsoft.Extensions.Configuration;

namespace AdventOfCode2024.challenges;

public class Challenge14(IConfiguration config) : IChallenge
{
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
            // TODO: Parse
        }
    }

    public (string, string) Calculate()
    {
        // TODO: Add part one & two

        long total = 0;
        long secondTotal = 0;
        return ($"{ total }",
            $"{ secondTotal }");
    }
}