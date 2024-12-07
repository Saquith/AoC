namespace AdventOfCode2024.Models._4;

public class Map(Dictionary<int,Dictionary<int,Node>> nodes)
{
    public Dictionary<int, Dictionary<int, Node>> Nodes { get; } = nodes;
    
    public Node? this[int x, int y]
    {
        get
        {
            if (x >= 0 && x < Nodes.Count)
                if (y >= 0 && y < Nodes[x].Count)
                    return Nodes[x][y];
            
            return null;
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
}