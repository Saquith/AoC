namespace AdventOfCode2024.Models._4;

public class Map(Dictionary<int, Dictionary<int, Node>> nodes, string? obstructionCharacter = "#", string? outOfBoundsCharacter = null)
{
    protected Dictionary<int, Dictionary<int, Node>> Nodes { get; } = nodes;

    protected Node? this[int y, int x]
    {
        get
        {
            if (y >= 0 && y < Nodes.Count)
                if (x >= 0 && x < Nodes[y].Count)
                    return Nodes[y][x];

            return string.IsNullOrEmpty(outOfBoundsCharacter) ? null : new Node(outOfBoundsCharacter, x, y, obstructionCharacter);
        }
    }

    public List<Node> GetAllNodes()
    {
        var result = new List<Node>();
        foreach (var (_, row) in Nodes)
        foreach (var (_, node) in row)
            result.Add(node);

        return result;
    }

    private static Direction GetDirectionsFromCoordinates(int x, int y)
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

    /// <summary>
    /// Sets neighbours by reference
    /// </summary>
    /// <param name="direction">Supported Directions: Laterals (default) & Diagonals</param>
    /// <param name="action"></param>
    public void SetNeighbours(Direction direction, Func<Node, Node, bool>? action = null)
    {
        action ??= (_, _) => true;

        if (direction == Direction.Diagonals)
            foreach (var (x, row) in Nodes)
            foreach (var (y, node) in row)
                // Loop neighbours (safety checks are done within map)
                for (var i = -1; i <= 1; i++)
                for (var j = -1; j <= 1; j++)
                {
                    // Skip self
                    if (i == 0 && j == 0)
                        continue;
                    var currentNeighbour = this[x + i, y + j];
                    if (currentNeighbour != null && action(node, currentNeighbour))
                        node.Neighbours.Add(GetDirectionsFromCoordinates(i, j), currentNeighbour);
                }
        else
            foreach (var (y, row) in Nodes)
            foreach (var (x, node) in row)
            {
                // Set all traversable neighbours (safety checks are done within map)
                var leftNeighbour = this[y, x - 1];
                if (leftNeighbour != null && leftNeighbour.Letter != obstructionCharacter && action(node, leftNeighbour))
                    node.Neighbours.Add(Direction.Left, leftNeighbour);
                var rightNeighbour = this[y, x + 1];
                if (rightNeighbour != null && rightNeighbour.Letter != obstructionCharacter && action(node, rightNeighbour))
                    node.Neighbours.Add(Direction.Right, rightNeighbour);
                var upNeighbour = this[y - 1, x];
                if (upNeighbour != null && upNeighbour.Letter != obstructionCharacter && action(node, upNeighbour))
                    node.Neighbours.Add(Direction.Up, upNeighbour);
                
                var downNeighbour = this[y + 1, x];
                if (downNeighbour != null && downNeighbour.Letter != obstructionCharacter && action(node, downNeighbour))
                    node.Neighbours.Add(Direction.Down, downNeighbour);
            }
    }
}