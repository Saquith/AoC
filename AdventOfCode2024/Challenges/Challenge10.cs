using System.Diagnostics;
using AdventOfCode2024.Models._4;
using AdventOfCode2024.Models._10;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2024.Challenges;

public class Challenge10(IConfiguration config) : IChallenge
{
    private TrailMap? _map;

    public async Task ReadInput(string? fileName = null)
    {
        var inputFilePath = Path.Combine(config["InputFolderPath"]!, $"{fileName ?? GetType().Name.Substring(9)}.txt");
        if (!File.Exists(inputFilePath)) throw new FileNotFoundException("The input file could not be found.");

        await using var stream = File.OpenRead(inputFilePath);
        using var reader = new StreamReader(stream);

        var nodes = new Dictionary<int, Dictionary<int, Node>>();

        string? currentLine;
        var x = -1;
        while (!string.IsNullOrEmpty(currentLine = await reader.ReadLineAsync()))
        {
            // Update x coordinate after reading the new line
            x++;

            if (!nodes.ContainsKey(x))
                nodes.Add(x, new Dictionary<int, Node>());

            // Parse each letter separately as node
            for (var y = 0; y < currentLine.Length; y++)
            {
                var currentNode = new Node(currentLine[y].ToString(), x, y);
                nodes[x].Add(y, currentNode);
            }
        }

        // Save in map so we can use indexer with guards
        _map = new TrailMap(nodes);
    }

    public (string, string) Calculate()
    {
        // Allowed neighbours should be neighbours with difference -1 (e.g. 5 => 6, not the other way around)
        _map!.SetNeighbours(Direction.Laterals,
            (node, neighbour) => int.TryParse(neighbour.Letter, out var neighbourValue)
                                 && int.TryParse(node.Letter, out var nodeValue) && nodeValue - neighbourValue == -1);
        
        Debug.WriteLine(_map.ToString());

        long total = 0;
        foreach (var trailHead in _map.GetAllNodes().Where(n => n.Letter.Equals("0") && n.Neighbours.Count > 0))
        {
            total += _map.FindReachablePeaks(trailHead);
        }

        long secondTotal = 0;
        return ($"{ total }",
            $"{ secondTotal }");
    }
}