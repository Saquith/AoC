using System.Diagnostics;
using AdventOfCodeLibrary.Challenges;
using AdventOfCodeLibrary.Models;

namespace AdventOfCode2024.Challenges;

public class Challenge10 : IChallenge
{
    private TrailMap? _map;

    public async Task ReadInput(string filePath)
    {
        await using var stream = File.OpenRead(filePath);
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

        long totalDistinct = 0;
        long totalRoutes = 0;
        foreach (var trailHead in _map.GetAllNodes().Where(n => n.Letter.Equals("0") && n.Neighbours.Count > 0))
        {
            var (currentTotalDistinct, currentTotalRoutes) = _map.FindReachablePeaks(trailHead);
            totalDistinct += currentTotalDistinct;
            totalRoutes += currentTotalRoutes;
        }

        return ($"{ totalDistinct }",
            $"{ totalRoutes }");
    }
}