using System.Diagnostics;
using AdventOfCode2024.Models._4;

namespace AdventOfCode2024.Models._6;

public class GuardMap(Dictionary<int, Dictionary<int, Node>> nodes, string outOfBoundsCharacter) : Map(nodes, outOfBoundsCharacter)
{
    private Dictionary<int, Dictionary<int, Node>> _originalNodes;
    private List<(int, int)> _foundObstacleLocations;

    public bool GuardCanFindMapEdge(Node guardNode, Direction originalDirection, bool simulateObstructions = true)
    {
        _originalNodes = Clone(Nodes);
        _foundObstacleLocations = [];
        var direction = originalDirection;

        var currentNode = guardNode;
        while (currentNode.Letter != outOfBoundsCharacter)
        {
            // Check for loops
            if (currentNode.FirstFollowedDirection == direction)
            {
                Debug.WriteLine("Early exit - loop found");
                return false;
            }

            // Mark node as visited
            this[currentNode.Y!.Value, currentNode.X!.Value]!.Letter = GetDirectionLetter(currentNode, direction);
            if (currentNode.Neighbours.ContainsKey(direction))
                currentNode.FirstFollowedDirection = direction;

            // Simulate moving to new direction & detect loops (if not already blocked)
            if (simulateObstructions && currentNode.Neighbours.ContainsKey(direction) && currentNode.Neighbours[direction].Letter != outOfBoundsCharacter)
            {
                var newRouteMap = new GuardMap(Clone(_originalNodes), outOfBoundsCharacter);
                if (newRouteMap.DetectLoopSimulatingWithObstruction(currentNode.Neighbours[direction], guardNode, originalDirection))
                {
                    // Caused a loop, add to obstacle locations.
                    Debug.WriteLine(newRouteMap.ToString());
                    _foundObstacleLocations.Add((currentNode.Neighbours[direction].Y!.Value, currentNode.Neighbours[direction].X!.Value));
                }
            }
            
            // if ((currentNode.Neighbours.ContainsKey(direction) && currentNode.Neighbours[direction].Letter == outOfBoundsCharacter) || currentNode.Letter == outOfBoundsCharacter)
            //     Console.WriteLine("something is amiss");

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
            }
            
            currentNode = currentNode.Neighbours[direction];
        }
        
        if (!(currentNode.X >= 0 || currentNode.X < _originalNodes[0].Count) || !(currentNode.Y >= 0 || currentNode.Y < _originalNodes.Count))
            Debug.WriteLine($"Exit found! {currentNode.Letter} Incorrect coordinates: [{currentNode.X!.Value},{currentNode.Y!.Value}]");
        
        // Print the route
        Debug.WriteLine(ToString());

        if (simulateObstructions)
        {
            // Set the obstacles
            foreach (var location in _foundObstacleLocations)
                this[location.Item1, location.Item2]!.Letter = "O";

            // Print the new map
            Console.WriteLine(ToString());
        }

        return true;
    }

    private Dictionary<int, Dictionary<int, Node>> Clone(Dictionary<int, Dictionary<int, Node>> originalNodes)
    {
        var result = new Dictionary<int, Dictionary<int, Node>>();
        foreach (var (y, row) in originalNodes)
        {
            var originalRowNodes = new Dictionary<int, Node>();
            foreach (var (x, node) in row)
            {
                var newNode = new Node(node.Letter, node.X, node.Y);
                originalRowNodes.Add(x, newNode);
            }

            result.Add(y, originalRowNodes);
        }
        
        // We have to reset the neighbours, otherwise the old neighbour nodes are copied onto the new set
        foreach (var (y, row) in result)
        {
            foreach (var (x, node) in row)
            {
                // Set all traversable neighbours
                if (x > 0)
                {
                    var leftNeighbour = result[y][x - 1];
                    if (leftNeighbour != null && leftNeighbour.Letter != "#")
                        node.Neighbours.Add(Direction.Left, leftNeighbour);
                }
                else
                {
                    node.Neighbours.Add(Direction.Left, new Node(outOfBoundsCharacter, x - 1, y));
                }
                
                if (x < row.Count - 1)
                {
                    var rightNeighbour = result[y][x + 1];
                    if (rightNeighbour != null && rightNeighbour.Letter != "#")
                        node.Neighbours.Add(Direction.Right, rightNeighbour);
                }
                else
                {
                    node.Neighbours.Add(Direction.Right, new Node(outOfBoundsCharacter, x + 1, y));
                }
                
                if (y > 0)
                {
                    var upNeighbour = result[y - 1][x];
                    if (upNeighbour != null && upNeighbour.Letter != "#")
                        node.Neighbours.Add(Direction.Up, upNeighbour);
                }
                else
                {
                    node.Neighbours.Add(Direction.Up, new Node(outOfBoundsCharacter, x, y - 1));
                }

                if (y < result.Count - 1)
                {
                    var downNeighbour = result[y + 1][x];
                    if (downNeighbour != null && downNeighbour.Letter != "#")
                        node.Neighbours.Add(Direction.Down, downNeighbour);
                }
                else
                {
                    node.Neighbours.Add(Direction.Down, new Node(outOfBoundsCharacter, x, y + 1));
                }
            }
        }

        return result;
    }

    private bool DetectLoopSimulatingWithObstruction(Node obstructionNode, Node guardNode, Direction originalDirection)
    {
        if (obstructionNode.X == guardNode.X && obstructionNode.Y == guardNode.Y)
            return false;
        
        // Set obstruction
        this[obstructionNode.Y!.Value, obstructionNode.X!.Value]!.Letter = "#";
        
        // Remove obstruction from possible neighbours
        var leftNeighbour = this[obstructionNode.Y!.Value, obstructionNode.X!.Value - 1];
        if (leftNeighbour != null && leftNeighbour.Neighbours.ContainsKey(Direction.Right))
            leftNeighbour.Neighbours.Remove(Direction.Right);
        var rightNeighbour = this[obstructionNode.Y!.Value, obstructionNode.X!.Value + 1];
        if (rightNeighbour != null && rightNeighbour.Neighbours.ContainsKey(Direction.Left))
            rightNeighbour.Neighbours.Remove(Direction.Left);
        var upNeighbour = this[obstructionNode.Y!.Value - 1, obstructionNode.X!.Value];
        if (upNeighbour != null && upNeighbour.Neighbours.ContainsKey(Direction.Down))
            upNeighbour.Neighbours.Remove(Direction.Down);
        var downNeighbour = this[obstructionNode.Y!.Value + 1, obstructionNode.X!.Value];
        if (downNeighbour != null && downNeighbour.Neighbours.ContainsKey(Direction.Up))
            downNeighbour.Neighbours.Remove(Direction.Up);
        
        // Start new simulation without additional obstructions
        var direction = originalDirection;
        return !GuardCanFindMapEdge(this[guardNode.Y!.Value, guardNode.X!.Value]!, direction, false);
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