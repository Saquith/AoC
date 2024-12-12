﻿using System.Diagnostics;
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
                var currentNode = new Node(currentLine[y].ToString());
                nodes[x].Add(y, currentNode);
            }
        }
        
        _map = new FenceMap(nodes);
    }

    public (string, string) Calculate()
    {
        _map!.SetNeighbours(Direction.Laterals);
        
        long totalPrice = _map.CalculateFencePrice();
        Debug.WriteLine(_map.ToString());

        long secondTotal = 0;
        return ($"{ totalPrice }",
            $"{ secondTotal }");
    }
}