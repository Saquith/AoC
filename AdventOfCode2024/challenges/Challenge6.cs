using AdventOfCode2024.Models._2;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2024.challenges;

public class Challenge6(IConfiguration config) : IChallenge
{
    public async Task ReadInput()
    {
        try
        {
            var inputFilePath = Path.Combine(config["InputFolderPath"]!, "6.txt");
            if (!File.Exists(inputFilePath)) throw new FileNotFoundException("The input file could not be found.");
            
            await using var stream = File.OpenRead(inputFilePath);
            using var reader = new StreamReader(stream);
            
            string? currentLine;
            while (!String.IsNullOrEmpty(currentLine = await reader.ReadLineAsync()))
            {
                // Parse
                // TODO: Parse
            }
        }
        
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.ReadLine();
        }
    }

    public async Task<string> Calculate()
    {
        // TODO: Add part one & two
        
        return $"Part one: {"something"}\r\n" +
               $"Part 2: {"something else"}";
    }
}