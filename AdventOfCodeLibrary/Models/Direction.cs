namespace AdventOfCodeLibrary.Models;

public enum Direction
{
    All,
    // Neighbour directions
    Diagonals,
    Laterals,
    // Empty direction for non-visit detection
    None,
    // Self for single node, not used usually
    Self,
    // All directions around
    Up,
    Left,
    Right,
    Down,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight
}