using System.Diagnostics;
using AdventOfCode2024.Models._12;
using AdventOfCode2024.Models._4;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2024.Challenges;

public class Challenge12(IConfiguration config) : IChallenge
{
    private FenceMap? _map;

    public async Task ReadInput(string? fileName = null)
    {
        var inputFilePath = Path.Combine(config["InputFolderPath"]!, $"{fileName ?? GetType().Name.Substring(9)}.txt");
        if (!File.Exists(inputFilePath)) throw new FileNotFoundException("The input file could not be found.");

        await using var stream = File.OpenRead(inputFilePath);
        using var reader = new StreamReader(stream);
        
        var nodes = new Dictionary<int, Dictionary<int, Node>>();

        string? currentLine;
        var y = -1;
        while (!string.IsNullOrEmpty(currentLine = await reader.ReadLineAsync()))
        {
            // Update x coordinate after reading the new line
            y++;

            if (!nodes.ContainsKey(y))
                nodes.Add(y, new Dictionary<int, Node>());

            // Parse each letter separately as node
            for (var x = 0; x < currentLine.Length; x++)
            {
                var currentNode = new Node(currentLine[x].ToString(), x, y);
                nodes[y].Add(x, currentNode);
            }
        }
        
        _map = new FenceMap(nodes);
    }

    public (string, string) Calculate()
    {
        _map!.SetNeighbours(Direction.Laterals);
        
        long totalPrice = _map.CalculateFencePricePerNode();
        Debug.WriteLine(_map.ToString());

        long secondTotal = 0;
        return ($"{ totalPrice }",
            $"{ secondTotal }");
    }
}