using System.Diagnostics;
using AdventOfCode2024.Models._4;

namespace AdventOfCode2024.Models._6;

public class GuardMap(Dictionary<int, Dictionary<int, Node>> nodes, string outOfBoundsCharacter) : Map(nodes, outOfBoundsCharacter)
{
    public void MoveGuard(Node guardNode, Direction direction)
    {
        Debug.WriteLine(ToString());

        var currentNode = guardNode;
        while (currentNode.Letter != "*")
        {
            // Mark node as visited
            this[currentNode.Y!.Value, currentNode.X!.Value]!.Letter = "X";
            
            // Keep going while possible
            if (currentNode.Neighbours.ContainsKey(direction))
            {
                currentNode = currentNode.Neighbours[direction];
                continue;
            }
            
            // No longer possible to move, turn 90°
            while (!currentNode.Neighbours.ContainsKey(direction))
            {
                direction = GetNextDirection(direction);
            }
            
            currentNode = currentNode.Neighbours[direction];
        }
        
        Debug.WriteLine(ToString());
    }

    private Direction GetNextDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Down:
                return Direction.Left;
            case Direction.Left:
                return Direction.Up;
            case Direction.Up:
                return Direction.Right;
            case Direction.Right:
                return Direction.Down;
        }
        
        return Direction.None;
    }
}