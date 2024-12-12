namespace AdventOfCode2024.Models._4;

public enum Direction
{
    // Neighbour directions
    Diagonals,
    Laterals,
    // Empty direction for non-visit detection
    None,
    // Self for single node, not used usually
    Self,
    // All directions around
    Up,
    Down,
    Left,
    Right,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight
}