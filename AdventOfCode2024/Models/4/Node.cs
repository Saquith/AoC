namespace AdventOfCode2024.Models._4;

public class Node(string letter, int? x = null, int? y = null)
{
    private Direction _firstFollowedDirection = Direction.None;

    private bool _visited;
    public int? X = x;
    public int? Y = y;
    public string Letter { get; set; } = letter;

    public Direction FirstFollowedDirection
    {
        get => _firstFollowedDirection;
        set
        {
            if (!_visited)
            {
                _firstFollowedDirection = value;
                _visited = true;
            }
        }
    }

    public Dictionary<Direction, Node> Neighbours { get; private set; } = [];

    public string GetTargetLetter()
    {
        switch (Letter)
        {
            case "X":
                return "M";
            case "M":
                return "A";
            case "A":
                return "S";
            case "S": // Does not need to check for any neighbours
            default:
                return "";
        }
    }

    public int FindXMAS(Direction direction)
    {
        if (Letter.Equals("S"))
            return 1;

        return Neighbours.ContainsKey(direction) && Neighbours[direction].Letter.Equals(GetTargetLetter())
            ? Neighbours[direction].FindXMAS(direction)
            : 0;
    }

    public Node Clone()
    {
        var result = (Node)MemberwiseClone();
        // Can't clone neighbours, they need to be added by reference (of the new Nodes)
        result.Neighbours = [];
        return result;
    }

    public override string ToString()
    {
        var upLeft = Neighbours.ContainsKey(Direction.UpLeft) ? Neighbours[Direction.UpLeft].Letter : "?";
        var up = Neighbours.ContainsKey(Direction.Up) ? Neighbours[Direction.Up].Letter : "#";
        var upRight = Neighbours.ContainsKey(Direction.UpRight) ? Neighbours[Direction.UpRight].Letter : "?";
        var left = Neighbours.ContainsKey(Direction.Left) ? Neighbours[Direction.Left].Letter : "#";
        var right = Neighbours.ContainsKey(Direction.Right) ? Neighbours[Direction.Right].Letter : "#";
        var downLeft = Neighbours.ContainsKey(Direction.DownLeft) ? Neighbours[Direction.DownLeft].Letter : "?";
        var down = Neighbours.ContainsKey(Direction.Down) ? Neighbours[Direction.Down].Letter : "#";
        var downRight = Neighbours.ContainsKey(Direction.DownRight) ? Neighbours[Direction.DownRight].Letter : "?";

        return $"{upLeft}{up}{upRight}\r\n" +
               $"{left}{Letter}{right}\r\n" +
               $"{downLeft}{down}{downRight}";
    }
}