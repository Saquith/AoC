using System.Diagnostics;
using AdventOfCode2024.Models._4;

namespace AdventOfCode2024.Models._6;

public class GuardMap(Dictionary<int, Dictionary<int, Node>> nodes, string outOfBoundsCharacter) : Map(nodes, outOfBoundsCharacter)
{
    public bool MoveGuardToEnd(Node guardNode, Direction direction, bool simulateObstructions = true)
    {
        Debug.WriteLine(ToString());
        var originalDirection = direction;

        var currentNode = guardNode;
        while (currentNode.Letter != "*")
        {
            // Mark node as visited
            this[currentNode.Y!.Value, currentNode.X!.Value]!.Letter = GetDirectionLetter(currentNode, direction);
            currentNode.FirstFollowedDirection = direction;

            // Simulate moving to new direction & detect loops (if not already blocked)
            if (simulateObstructions && currentNode.Neighbours.ContainsKey(direction) && currentNode.Neighbours[direction].Letter != "*")
            {
                // Clone deep so it doesn't impact current run
                var clonedNodes = new Dictionary<int, Dictionary<int, Node>>();
                foreach (var (y, row) in Nodes)
                {
                    var clonedRow = new Dictionary<int, Node>();
                    foreach (var (x, node) in row)
                        clonedRow.Add(x, new Node(node.Letter, node.X, node.Y));
                    
                    clonedNodes.Add(y, clonedRow);
                }
                
                var newRouteMap = new GuardMap(clonedNodes, outOfBoundsCharacter);
                if (!newRouteMap.SimulateWithObstruction(currentNode.Neighbours[direction], guardNode, originalDirection))
                {
                    // Caused a loop, set the O.
                    this[currentNode.Neighbours[direction].Y!.Value, currentNode.Neighbours[direction].X!.Value]!.Letter = "O";
                }
            }

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
                if (this[currentNode.Y!.Value, currentNode.X!.Value]!.Neighbours.ContainsKey(direction))
                    currentNode.FirstFollowedDirection = direction;
            }
            
            currentNode = currentNode.Neighbours[direction];
        }
        Debug.WriteLine(ToString());
        return true;
    }

    private bool SimulateWithObstruction(Node obstructionNode, Node guardNode, Direction originalDirection)
    {
        // Set obstruction
        this[obstructionNode.Y!.Value, obstructionNode.X!.Value]!.Letter = "O";
        
        // Start new simulation without additional obstructions
        return MoveGuardToEnd(guardNode, originalDirection, false);
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