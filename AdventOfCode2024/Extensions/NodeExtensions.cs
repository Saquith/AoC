using AdventOfCode2024.Models._4;

namespace AdventOfCode2024.Extensions;

public static class NodeExtensions
{
    public static string GetTargetLetter(this Node node)
    {
        switch (node.Letter)
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

    public static int FindXMAS(this Node node, Direction direction)
    {
        if (node.Letter.Equals("S"))
            return 1;

        return node.Neighbours.ContainsKey(direction) && node.Neighbours[direction].Letter.Equals(node.GetTargetLetter())
            ? node.Neighbours[direction].FindXMAS(direction)
            : 0;
    }

    public static List<Node> Explore(this Node node, Dictionary<string, long> areaByAreaId, string areaID)
    {
        var result = new List<Node> { node };
        
        node.AreaID = areaID;
        areaByAreaId.TryAdd(areaID, 0);
        areaByAreaId[areaID]++;
        
        foreach (var (_, neighbour) in node.Neighbours)
            if (neighbour.AreaID == null && neighbour.Letter == node.Letter)
                result.AddRange(neighbour.Explore(areaByAreaId, areaID));

        return result;
    }
    
    /// <summary>
    /// This method is not yet suited for diagonals
    /// </summary>
    public static long GetCornerCount(this Node node)
    {
        var nonSameNeighbourCount = node.Neighbours.Count(kvp => kvp.Value.Letter != node.Letter);
        switch (nonSameNeighbourCount)
        {
            case 4:
                return 4;
            case 3:
                return 2;
            case 2:
                var (direction, _) = node.Neighbours.FirstOrDefault(kvp => kvp.Value.Letter != node.Letter);
                node.Neighbours.TryGetValue(GetOppositeDirection(direction), out var oppositeNode);
                return oppositeNode != null && oppositeNode.Letter != node.Letter ? 0 : 1;
            default:
                return 0;
        }
    }

    private static Direction GetOppositeDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
            case Direction.UpRight:
                return Direction.DownLeft;
            case Direction.DownLeft:
                return Direction.UpRight;
            case Direction.UpLeft:
                return Direction.DownRight;
            case Direction.DownRight:
                return Direction.UpLeft;
            case Direction.Self:
                return Direction.Self;
            default:
                return Direction.None;
        }
    }
}