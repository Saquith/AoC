
using AdventOfCode2024.Models._4;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2024.challenges;

public class Challenge4(IConfiguration config) : IChallenge
{
    private Dictionary<int,Dictionary<int,Node>>? _nodes;

    public async Task ReadInput()
    {
        try
        {
            var inputFilePath = Path.Combine(config["InputFolderPath"]!, "4.txt");
            if (!File.Exists(inputFilePath)) throw new FileNotFoundException("The input file could not be found.");
            
            await using var stream = File.OpenRead(inputFilePath);
            using var reader = new StreamReader(stream);

            _nodes = new Dictionary<int, Dictionary<int, Node>>();
            
            string? currentLine;
            int x = -1;
            while (!String.IsNullOrEmpty(currentLine = await reader.ReadLineAsync()))
            {
                // Update x coordinate after reading the new line
                x++;
                
                if (!_nodes.ContainsKey(x))
                    _nodes.Add(x, new Dictionary<int, Node>());
                
                for (int y = 0; y < currentLine.Length; y++)
                {
                    // Parse
                    var currentNode = new Node(currentLine[y].ToString());
                    if (!_nodes[x].ContainsKey(y))
                        _nodes[x].Add(y, currentNode);
                }
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
        var map = "";
        foreach (var (x, row) in _nodes)
        {
            var currentRow = "";
            foreach (var (y, node) in row)
            {
                currentRow += node.Letter;
            }

            map += $"\r\n{currentRow}";
        }
        
        return $"Part one: {map}\r\n" +
               $"Part 2: {"something else"}";
    }
}