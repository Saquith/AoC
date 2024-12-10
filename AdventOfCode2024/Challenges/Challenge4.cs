using AdventOfCode2024.Extensions;
using AdventOfCode2024.Models._4;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2024.Challenges;

public class Challenge4(IConfiguration config) : IChallenge
{
    private Map? _map;

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

        // Save in map so we can use indexer with guards
        _map = new Map(nodes);
    }

    public (string, string) Calculate()
    {
        _map!.SetNeighbours(Direction.Diagonals);

        var result = 0;
        foreach (var xNode in _map.GetAllNodes().Where(n => n.Letter.Equals("X") && n.Neighbours.Count > 0))
        foreach (var (direction, mNode) in xNode.Neighbours.Where(n => n.Value.Letter.Equals(xNode.GetTargetLetter())))
            result += mNode.FindXMAS(direction);

        var crossMASResult = 0;
        foreach (var aNode in _map.GetAllNodes().Where(n => n.Letter.Equals("A") && n.Neighbours.Count > 0))
        {
            var crossDirections = new List<Direction> { Direction.UpLeft, Direction.UpRight, Direction.DownLeft, Direction.DownRight };
            var crossNeighbours = aNode.Neighbours.Where(kvp => crossDirections.Contains(kvp.Key)).ToDictionary();

            if (crossNeighbours.Count(n => n.Value.Letter.Equals("M")) == 2 &&
                crossNeighbours.Count(n => n.Value.Letter.Equals("S")) == 2)
                // Do not allow MAM | SAS
                if (crossNeighbours[Direction.DownLeft].Letter != crossNeighbours[Direction.UpRight].Letter)
                    crossMASResult++;
        }

        return ($"{result}",
            $"{crossMASResult}");
    }
}