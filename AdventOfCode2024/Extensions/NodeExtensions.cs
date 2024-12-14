using System.Diagnostics;
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

    public static Dictionary<Direction, Node> GetNeighbours(this Node node, Direction direction = Direction.Laterals)
    {
        Direction[] laterals = [Direction.Up, Direction.Left, Direction.Right, Direction.Down];
        Direction[] diagonals = [Direction.UpLeft, Direction.UpRight, Direction.DownLeft, Direction.DownRight];

        if (direction == Direction.Laterals)
            return node.Neighbours.Where(kvp => laterals.Contains(kvp.Key)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        if (direction == Direction.Diagonals)
            return node.Neighbours.Where(kvp => diagonals.Contains(kvp.Key)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        // Direction.All
        return node.Neighbours;
    }

    public static List<Node> Explore(this Node node, Dictionary<string, long> areaByAreaId, string areaID)
    {
        var result = new List<Node> { node };
        
        node.AreaID = areaID;
        areaByAreaId.TryAdd(areaID, 0);
        areaByAreaId[areaID]++;

        // Also check diagonals, but don't run explore on diagonals (don't connect diagonal areas)
        foreach (var (_, neighbour) in node.GetNeighbours())
            if (neighbour.AreaID == null && neighbour.Letter == node.Letter)
                result.AddRange(neighbour.Explore(areaByAreaId, areaID));

        return result;
    }

    public static long GetCornerCount(this Node node)
    {
        // Detect inside corners
        int insideCornerCount = 0;
        var nonSameDiagonalNeighbours = node.GetNeighbours(Direction.Diagonals).Where(kvp => kvp.Value.Letter != node.Letter).ToDictionary();
        if (nonSameDiagonalNeighbours.Count > 0)
            foreach (var (direction, _) in nonSameDiagonalNeighbours)
                if (node.GetNeighbours().Where(kvp => direction.GetSurroundingDirections().Contains(kvp.Key)).All(kvp => kvp.Value.Letter == node.Letter))
                {
                    insideCornerCount++;
                    Debug.WriteLine(node.ToString());
                }
        
        var lateralNeighbours = node.GetNeighbours();
        var nonSameNeighbourCount = lateralNeighbours.Count(kvp => kvp.Value.Letter != node.Letter);
        switch (nonSameNeighbourCount)
        {
            case 4:
                return 4;
            case 3:
                return 2 + insideCornerCount;
            case 2:
                var (direction, _) = lateralNeighbours.FirstOrDefault(kvp => kvp.Value.Letter != node.Letter);
                lateralNeighbours.TryGetValue(direction.GetOppositeDirection(), out var oppositeNode);
                return oppositeNode != null && oppositeNode.Letter != node.Letter ? insideCornerCount : 1 + insideCornerCount;
            default:
                return insideCornerCount;
        }
    }
}