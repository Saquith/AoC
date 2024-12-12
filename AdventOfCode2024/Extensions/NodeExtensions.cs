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
}