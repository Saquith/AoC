namespace AdventOfCode2024.Models._4;

public class Node(string letter)
{
    public Guid Id { get; set; } = new();
    public string Letter { get; } = letter;
    public Dictionary<Direction, Node> Neighbours { get; } = [];

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
                return null;
        }
    }

    public int FindXMAS(Direction direction)
    {
        if (Letter.Equals("S"))
            return 1;
        
        return Neighbours.ContainsKey(direction) ? Neighbours[direction].FindXMAS(direction) : 0;
    }
}