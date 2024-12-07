
using AdventOfCode2024.Models._4;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2024.challenges;

public class Challenge4(IConfiguration config) : IChallenge
{
    private Map? _map;

    public async Task ReadInput()
    {
        try
        {
            var inputFilePath = Path.Combine(config["InputFolderPath"]!, "4.txt");
            if (!File.Exists(inputFilePath)) throw new FileNotFoundException("The input file could not be found.");
            
            await using var stream = File.OpenRead(inputFilePath);
            using var reader = new StreamReader(stream);

            var nodes = new Dictionary<int, Dictionary<int, Node>>();
            
            string? currentLine;
            int x = -1;
            while (!String.IsNullOrEmpty(currentLine = await reader.ReadLineAsync()))
            {
                // Update x coordinate after reading the new line
                x++;
                
                if (!nodes.ContainsKey(x))
                    nodes.Add(x, new Dictionary<int, Node>());
                
                // Parse each letter separately as node
                for (int y = 0; y < currentLine.Length; y++)
                {
                    var currentNode = new Node(currentLine[y].ToString());
                    nodes[x].Add(y, currentNode);
                }
            }

            // Save in map so we can use indexer with guards
            _map = new Map(nodes);
        }
        
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.ReadLine();
        }
    }

    public async Task<string> Calculate()
    {
        foreach (var (x, row) in _map!.Nodes)
        {
            foreach (var (y, node) in row)
            {
                // Find (correct) neighbours for all nodes
                string targetLetter = node.GetTargetLetter();

                // Loop neighbours (safety checks are done within map)
                for (int i = -1; i <= 1; i++)
                    for (int j = -1; j <= 1; j++)
                    {
                        var currentNeighbour = _map[x + i, y + j];
                        if (currentNeighbour != null)
                            node.Neighbours.Add(Map.GetDirectionsFromCoordinates(i, j), currentNeighbour);
                    }
            }
        }

        var result = 0;
        foreach (var xNode in _map.GetAllNodes().Where(n => n.Letter.Equals("X") && n.Neighbours.Count > 0))
        {
            foreach (var (direction, mNode) in xNode.Neighbours.Where(n => n.Value.Letter.Equals(xNode.GetTargetLetter())))
                result += mNode.FindXMAS(direction);
        }

        var crossMASResult = 0;
        foreach (var aNode in _map.GetAllNodes().Where(n => n.Letter.Equals("A") && n.Neighbours.Count > 0))
        {
            var crossDirections = new List<Direction> { Direction.UpLeft, Direction.UpRight, Direction.DownLeft, Direction.DownRight };
            var crossNeighbours = aNode.Neighbours.Where(kvp => crossDirections.Contains(kvp.Key)).ToDictionary();

            if (crossNeighbours.Count(n => n.Value.Letter.Equals("M")) == 2 && crossNeighbours.Count(n => n.Value.Letter.Equals("S")) == 2)
            {
                // Do not allow MAM | SAS
                if (crossNeighbours[Direction.DownLeft].Letter != crossNeighbours[Direction.UpRight].Letter)
                    crossMASResult++;
            }
        }
        
        return $"Part one: { result }\r\n" +
               $"Part 2: { crossMASResult }";
    }
}