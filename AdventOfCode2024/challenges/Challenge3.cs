using System.Text.RegularExpressions;
using AdventOfCode2024.Models._3;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2024.challenges;

public class Challenge3(IConfiguration config) : IChallenge
{
    private List<Operation>? _operations;
    
    public async Task ReadInput()
    {
        try
        {
            var inputFilePath = Path.Combine(config["InputFolderPath"]!, "3.txt");
            if (!File.Exists(inputFilePath)) throw new FileNotFoundException("The input file could not be found.");
            
            await using var stream = File.OpenRead(inputFilePath);
            using var reader = new StreamReader(stream);

            _operations = [];
            
            string? currentLine;
            while (!String.IsNullOrEmpty(currentLine = await reader.ReadLineAsync()))
            {
                // Parse
                var matches = Regex.Matches(currentLine, @"(mul)\((\d{1,3}),(\d{1,3})\)");
                // First match is full matched string, skip first group
                _operations.AddRange(matches.Select(m =>
                    new Operation(m.Groups[1].Value, long.Parse(m.Groups[2].Value), long.Parse(m.Groups[3].Value))));
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
        long total = 0;
        foreach (var operation in _operations!)
        {
            if (operation.OperationType == "mul")
                total += operation.FirstNumber * operation.SecondNumber;
        }
        
        return $"Part one: {total}\r\n" +
               $"Part 2: {"something else"}";
    }
}