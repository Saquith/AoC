using System.Diagnostics;

namespace AdventOfCode2024.Models._4;

public class Map(Dictionary<int, Dictionary<int, Node>> nodes, string? outOfBoundsCharacter = null)
{
    public Dictionary<int, Dictionary<int, Node>> Nodes { get; } = nodes;

    public List<Node> GetAllNodes()
    {
        var result = new List<Node>();
        foreach (var (_, row) in Nodes)
            foreach (var (_, node) in row)
                result.Add(node);

        return result;
    }
    
    public Node? this[int y, int x]
    {
        get
        {
            if (y >= 0 && y < Nodes.Count)
                if (x >= 0 && x < Nodes[y].Count)
                    return Nodes[y][x];
            
            return string.IsNullOrEmpty(outOfBoundsCharacter) ? null : new Node(outOfBoundsCharacter);
        }
    }

    public static Direction GetDirectionsFromCoordinates(int x, int y)
    {
        switch (x)
        {
            case -1:
                switch (y)
                {
                    case -1: return Direction.DownLeft;
                    case 0: return Direction.Left;
                    case 1: return Direction.UpLeft;
                }
                break;
            case 0:
                switch (y)
                {
                    case -1: return Direction.Up;
                    case 0: return Direction.Self; // Should never matter, but will be filtered out with target letter
                    case 1: return Direction.Down;
                }
                break;
            case 1:
                switch (y)
                {
                    case -1: return Direction.DownRight;
                    case 0: return Direction.Right;
                    case 1: return Direction.UpRight;
                }
                break;
        }

        return Direction.None; // Should never occur
    }
    
    public override string ToString()
    {
        var result = "";
        foreach (var (_, row) in Nodes)
        {
            var rowResult = "";
            foreach (var (_, node) in row)
            {
                rowResult += $"{node.Letter}";
            }
            result += $"{rowResult}\r\n";
        }

        return result;
    }
}