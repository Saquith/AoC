﻿namespace AdventOfCode2024.Models._4;

public class Node(string letter, int? x = null, int? y = null)
{
    private bool _visited = false;
    public int? X = x;
    public int? Y = y;
    private Direction _firstFollowedDirection = Direction.None;
    public string Letter { get; set; } = letter;

    public Direction FirstFollowedDirection
    {
        get => _firstFollowedDirection;
        set { if (!_visited) {
            _firstFollowedDirection = value;
            _visited = true;
        } }
    }

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
                return "";
        }
    }

    public int FindXMAS(Direction direction)
    {
        if (Letter.Equals("S"))
            return 1;
        
        return Neighbours.ContainsKey(direction) && Neighbours[direction].Letter.Equals(GetTargetLetter()) ?  Neighbours[direction].FindXMAS(direction) : 0;
    }

    public override string ToString()
    {
        var upLeft = Neighbours.ContainsKey(Direction.UpLeft) ? Neighbours[Direction.UpLeft].Letter : ".";
        var upRight = Neighbours.ContainsKey(Direction.UpRight) ? Neighbours[Direction.UpRight].Letter : ".";
        var downLeft = Neighbours.ContainsKey(Direction.DownLeft) ? Neighbours[Direction.DownLeft].Letter : ".";
        var downRight = Neighbours.ContainsKey(Direction.DownRight) ? Neighbours[Direction.DownRight].Letter : ".";
        
        return $"{upLeft}.{upRight}\r\n" +
            $".{Letter}.\r\n" +
            $"{downLeft}.{downRight}";
    }
}