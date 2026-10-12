using System.Diagnostics;
using AdventOfCodeLibrary.Challenges;
using AdventOfCodeLibrary.Models;

namespace AdventOfCode2024.Challenges;

public class Challenge12 : IChallenge
{
    private FenceMap? _map;

    public async Task ReadInput(string filePath)
    {
        await using var stream = File.OpenRead(filePath);
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
        _map!.SetNeighbours(Direction.Diagonals);
        
        long fencePricePerNode = _map.CalculateFencePricePerNode();
        Debug.WriteLine(_map.ToString());

        // Reset input
        _map!.EmptyAreaIDs();
        long fencePricePerSide = _map.CalculateFencePricePerSide();
        return ($"{ fencePricePerNode }",
            $"{ fencePricePerSide }");
    }
}