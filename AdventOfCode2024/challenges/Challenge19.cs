
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2024.challenges;

public class Challenge19(IConfiguration config) : IChallenge
{
    public async Task ReadInput()
    {
        try
        {
            var inputFilePath = Path.Combine(config["InputFolderPath"]!, "19.txt");
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

    public string Calculate()
    {
        // TODO: Add part one & two
        
        return $"Part one: {"something"}\r\n" +
               $"Part 2: {"something else"}";
    }
}