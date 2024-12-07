﻿
using AdventOfCode2024.Models._4;
using AdventOfCode2024.Models._6;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2024.challenges;

public class Challenge6(IConfiguration config) : IChallenge
{
    private GuardMap? _map;

    public async Task ReadInput()
    {
        try
        {
            var inputFilePath = Path.Combine(config["InputFolderPath"]!, "6.txt");
            if (!File.Exists(inputFilePath)) throw new FileNotFoundException("The input file could not be found.");
            
            await using var stream = File.OpenRead(inputFilePath);
            using var reader = new StreamReader(stream);

            var nodes = new Dictionary<int, Dictionary<int, Node>>();
            
            string? currentLine;
            int y = -1;
            while (!String.IsNullOrEmpty(currentLine = await reader.ReadLineAsync()))
            {
                // Update y coordinate after reading the new line
                y++;
                
                if (!nodes.ContainsKey(y))
                    nodes.Add(y, new Dictionary<int, Node>());
                
                // Parse each letter separately as node
                for (int x = 0; x < currentLine.Length; x++)
                {
                    var currentNode = new Node(currentLine[x].ToString(), x, y);
                    nodes[y].Add(x, currentNode);
                }
            }

            // Save in map so we can use indexer with guards
            _map = new GuardMap(nodes, "*");
        }
        
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.ReadLine();
        }
    }

    public string Calculate()
    {
        var guardCoordinates = (-1, -1);
        foreach (var (y, row) in _map!.Nodes)
        {
            foreach (var (x, node) in row)
            {
                // Set all traversible neighbours (safety checks are done within map)
                var leftNeighbour = _map[y, x - 1];
                if (leftNeighbour != null && leftNeighbour.Letter != "#")
                    node.Neighbours.Add(Direction.Left, leftNeighbour);
                var rightNeighbour = _map[y, x + 1];
                if (rightNeighbour != null && rightNeighbour.Letter != "#")
                    node.Neighbours.Add(Direction.Right, rightNeighbour);
                var upNeighbour = _map[y - 1, x];
                if (upNeighbour != null && upNeighbour.Letter != "#")
                    node.Neighbours.Add(Direction.Up, upNeighbour);
                var downNeighbour = _map[y + 1, x];
                if (downNeighbour != null && downNeighbour.Letter != "#")
                    node.Neighbours.Add(Direction.Down, downNeighbour);
                
                // Keep guard coordinates when found
                if (node.Letter == "^")
                    guardCoordinates = (y, x);
            }
        }

        var guardNode = _map[guardCoordinates.Item1, guardCoordinates.Item2];
        _map.MoveGuard(guardNode!, Direction.Up);
        
        var countLetters = new[] { "|", "-", "+", "O" };
        return $"Part one: { _map.GetAllNodes().Count(n => countLetters.Contains(n.Letter)) }\r\n" +
               $"Part 2: { _map.GetAllNodes().Count(n => n.Letter == "O") }";
    }
}