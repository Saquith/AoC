using AdventOfCode2024.Models._4;

namespace AdventOfCode2024.Extensions;

public static class DirectionExtensions
{
    public static bool IsDiagonal(this Direction direction)
    {
        switch (direction)
        {
            case Direction.Diagonals:
            case Direction.UpLeft:
            case Direction.UpRight:
            case Direction.DownLeft:
            case Direction.DownRight:
                return true;
            default:
                return false;
        }
    }
    
    public static Direction GetOppositeDirection(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            Direction.Down => Direction.Up,
            Direction.UpLeft => Direction.DownRight,
            Direction.UpRight => Direction.DownLeft,
            Direction.DownLeft => Direction.UpRight,
            Direction.DownRight => Direction.UpLeft,
            Direction.Self => Direction.Self,
            _ => Direction.None
        };
    }
    
    public static Direction[] GetSurroundingDirections(this Direction direction)
    {
        return direction switch
        {
            Direction.UpLeft => [Direction.Up, Direction.Left],
            Direction.UpRight => [Direction.Up, Direction.Right],
            Direction.DownLeft => [Direction.Down, Direction.Left],
            Direction.DownRight => [Direction.Down, Direction.Right],
            _ => []
        };
    }

    public static Direction GetNextDirection(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Right,
            Direction.Left => Direction.Up,
            Direction.Right => Direction.Down,
            Direction.Down => Direction.Left,
            _ => Direction.None
        };
    }
}