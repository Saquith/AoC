
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2024.challenges;

public class Challenge5(IConfiguration config) : IChallenge
{
    private Dictionary<int, List<int>>? _ordering;
    private List<int[]>? _validUpdates;

    public async Task ReadInput()
    {
        try
        {
            var inputFilePath = Path.Combine(config["InputFolderPath"]!, "5.txt");
            if (!File.Exists(inputFilePath)) throw new FileNotFoundException("The input file could not be found.");
            
            await using var stream = File.OpenRead(inputFilePath);
            using var reader = new StreamReader(stream);

            _ordering = new Dictionary<int, List<int>>();
            
            string? currentLine;
            while (!String.IsNullOrEmpty(currentLine = await reader.ReadLineAsync()))
            {
                // Parse ordering
                var split = currentLine.Split("|").Select(int.Parse).ToArray();
                if (!_ordering.ContainsKey(split[0]))
                    _ordering.Add(split[0], []);
                
                _ordering[split[0]].Add(split[1]);
            }
            Debug.WriteLine($"Found {_ordering.Values.Select(list => list.Count).Aggregate((a, b) => a + b)} order rules");

            _validUpdates = [];
            
            while (!String.IsNullOrEmpty(currentLine = await reader.ReadLineAsync()))
            {
                Debug.WriteLine($"Checking line {currentLine}");
                
                // Parse update
                var split = currentLine.Split(",").Select(int.Parse).ToArray();

                var valid = true;
                for (var index = 0; index < split.Length; index++)
                {
                    var currentNumber = split[index];
                    if (_ordering.ContainsKey(currentNumber))
                    {
                        // Ensure ordering does not block previous numbers
                        for (var i = 0; i < index; i++)
                            if (_ordering[currentNumber].Contains(split[i]))
                                valid = false;
                    }
                }
                
                if (valid)
                    _validUpdates.Add(split);
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
        var result = 0;
        foreach (var list in _validUpdates!)
        {
            result += list[list.Length / 2];
        }
        
        return $"Part one: { result }\r\n" +
               $"Part 2: {"something else"}";
    }
}