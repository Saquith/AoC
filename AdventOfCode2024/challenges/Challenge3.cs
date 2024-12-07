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
                var matches = Regex.Matches(currentLine, @"((don't|do)\(\))|(mul)\((\d{1,3}),(\d{1,3})\)");
                foreach (Match match in matches)
                {
                    // First match is full matched string, skip first group
                    var operation = !String.IsNullOrEmpty(match.Groups[2].Value) ? new Operation(match.Groups[2].Value, 0, 0)
                        : new Operation(match.Groups[3].Value, long.Parse(match.Groups[4].Value), long.Parse(match.Groups[5].Value));
                    
                    _operations.Add(operation);
                }
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
        long total = 0;
        foreach (var operation in _operations!)
        {
            if (operation.OperationType == "mul")
                total += operation.FirstNumber * operation.SecondNumber;
        }

        var enabled = true;
        long enabledMultiplicationsTotal = 0;
        foreach (var operation in _operations!)
        {
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
        }
        
        return $"Part one: {total}\r\n" +
               $"Part 2: {enabledMultiplicationsTotal}";
    }
}