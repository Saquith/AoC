using System.Diagnostics;
using AdventOfCode2024.Models._4;

namespace AdventOfCode2024.Models._6;

public class GuardMap(Dictionary<int, Dictionary<int, Node>> nodes, string outOfBoundsCharacter) : Map(nodes, outOfBoundsCharacter)
{
    private int count = 0;
    
    public void MoveGuard(Node guardNode, Direction direction)
    {
        Debug.WriteLine(ToString());
        var originalDirection = direction;

        var currentNode = guardNode;
        while (currentNode.Letter != "*")
        {
            // Mark node as visited
            this[currentNode.Y!.Value, currentNode.X!.Value]!.Letter = GetDirectionLetter(currentNode, direction);
            currentNode.FirstFollowedDirection = direction;
            
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
                this[currentNode.Y!.Value, currentNode.X!.Value]!.Letter = GetDirectionLetter(currentNode, direction);
                currentNode.FirstFollowedDirection = direction;
            }
            
            currentNode = currentNode.Neighbours[direction];
        }
        
        // Restart and check for obstruction possibilities
        // direction = originalDirection;
        // currentNode = this[guardNode.Y!.Value, guardNode.X!.Value];
        //
        // while (currentNode.Letter != "*")
        // {
        //     var nextDirection = GetNextDirection(direction);
        //     if (currentNode.Neighbours.ContainsKey(nextDirection))
        //     {
        //         var possibleLoopNode = currentNode.Neighbours[nextDirection];
        //         while (possibleLoopNode.Neighbours.ContainsKey(nextDirection) && possibleLoopNode.Neighbours[nextDirection].Letter != "*")
        //         {
        //             possibleLoopNode = possibleLoopNode.Neighbours[nextDirection];
        //             
        //             // Loop detected
        //             if (possibleLoopNode.FirstFollowedDirection == nextDirection)
        //                 break;
        //         }
        //
        //         // Check to see if we already know it intersects here, or it cannot move both ways and has to return back
        //         if (possibleLoopNode.FirstFollowedDirection == nextDirection || possibleLoopNode.Letter == "+" || possibleLoopNode.Letter == "O")
        //         {
        //             this[currentNode.Neighbours[direction].Y!.Value, currentNode.Neighbours[direction].X!.Value]!.Letter = "O";
        //             count++;
        //         }
        //     }
        //     
        //     // Keep going while possible
        //     if (currentNode.Neighbours.ContainsKey(direction))
        //     {
        //         currentNode = currentNode.Neighbours[direction];
        //         continue;
        //     }
        //     
        //     // No longer possible to move, turn 90°
        //     while (!currentNode.Neighbours.ContainsKey(direction))
        //     {
        //         direction = GetNextDirection(direction);
        //         this[currentNode.Y!.Value, currentNode.X!.Value]!.Letter = GetDirectionLetter(currentNode, direction);
        //         currentNode.FirstFollowedDirection = direction;
        //     }
        //     
        //     currentNode = currentNode.Neighbours[direction];
        // }
        
        Debug.WriteLine(count);
        Debug.WriteLine(ToString());
    }

    private string GetDirectionLetter(Node currentNode, Direction direction)
    {
        // Keep set obstructions
        if (currentNode.Letter == "O")
            return "O";
        
        switch (direction)
        {
            case Direction.Down:
            case Direction.Up:
                 return this[currentNode.Y!.Value, currentNode.X!.Value]!.Letter == "-" ? "+" : "|";
            case Direction.Right:
            case Direction.Left:
                return this[currentNode.Y!.Value, currentNode.X!.Value]!.Letter == "|" ? "+" : "-";
        }

        return "";
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