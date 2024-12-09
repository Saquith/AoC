using System.Diagnostics;
using AdventOfCode2024.Models._4;

namespace AdventOfCode2024.Models._6;

public class GuardMap(Dictionary<int, Dictionary<int, Node>> nodes, string outOfBoundsCharacter)
    : Map(nodes, outOfBoundsCharacter)
{
    private readonly object _lock = new();
    private long _counter;
    private List<(int, int)> _foundObstacleLocations;
    private Dictionary<int, Dictionary<int, Node>> _originalNodes;
    private long _simulationCount;
    private List<Task> _tasks;
    private TimeSpan _totalCopyDuration;

    public bool GuardCanFindMapEdge(Node guardNode, Direction originalDirection, bool simulateObstructions = true)
    {
        _originalNodes = Clone(Nodes);
        _foundObstacleLocations = [];
        _tasks = [];
        _counter = 0;
        _simulationCount = 0;
        _totalCopyDuration = TimeSpan.Zero;
        var direction = originalDirection;

        var currentNode = guardNode;
        while (currentNode.Letter != outOfBoundsCharacter)
        {
            // Check for loops
            if (currentNode.FirstFollowedDirection == direction ||
                currentNode.Neighbours.Count == 0 ||
                _counter > _originalNodes.Count * _originalNodes[0].Count * _originalNodes.Count)
            {
                Debug.WriteLine($"Early exit - loop found (counter @ {_counter})");
                return false;
            }

            _counter++;

            // Simulate moving to new direction & detect loops (if not already blocked)
            if (simulateObstructions && currentNode.Neighbours.ContainsKey(direction) &&
                currentNode.Neighbours[direction].Letter != outOfBoundsCharacter)
            {
                // Clone using the current position and set to cut work for threads
                var startTime = Stopwatch.GetTimestamp();
                var scopedNodes = Clone(Nodes);
                _totalCopyDuration += Stopwatch.GetElapsedTime(startTime);
                var scopedNode = currentNode;
                var scopedDirection = direction;
                _tasks.Add(Task.Run(() =>
                {
                    var obstacleLocation = (scopedNode.Neighbours[scopedDirection].Y!.Value,
                        scopedNode.Neighbours[scopedDirection].X!.Value);

                    // Don't run duplicate obstruction checks
                    var newRouteMap = new GuardMap(scopedNodes, outOfBoundsCharacter);
                    if (scopedNode.Neighbours[scopedDirection].FirstFollowedDirection == Direction.None
                        && newRouteMap.DetectLoopSimulatingWithObstruction(scopedNode, scopedDirection))
                    {
                        _simulationCount++;

                        // Caused a loop, add to obstacle locations.
                        lock (_lock)
                        {
                            _foundObstacleLocations.Add(obstacleLocation);
                        }
                    }
                }));
            }

            // Mark node as visited
            if (currentNode.Neighbours.ContainsKey(direction))
            {
                this[currentNode.Y!.Value, currentNode.X!.Value]!.Letter = GetDirectionLetter(currentNode, direction);
                currentNode.FirstFollowedDirection = direction;
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
            }
        }

        if (!(currentNode.X >= 0 || currentNode.X < _originalNodes[0].Count) ||
            !(currentNode.Y >= 0 || currentNode.Y < _originalNodes.Count))
            Debug.WriteLine(
                $"Exit found! {currentNode.Letter} Incorrect coordinates: [{currentNode.X!.Value},{currentNode.Y!.Value}]");

        // Print the route
        Debug.WriteLine(ToString());

        if (simulateObstructions)
        {
            Task.WaitAll(_tasks.ToArray());

            // Set the obstacles (set lock to satisfy the compiler)
            lock (_lock)
            {
                foreach (var location in _foundObstacleLocations)
                    if (location.Item1 != -1 && location.Item1 != guardNode.Y && location.Item2 != guardNode.X)
                        this[location.Item1, location.Item2]!.Letter = "O";
            }

            // Print the new map
            Debug.WriteLine(ToString());
            Debug.WriteLine(_simulationCount);
            Debug.WriteLine($"Time wasted copying: {_totalCopyDuration:c}");
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
                if (node.FirstFollowedDirection != Direction.None)
                    newNode.FirstFollowedDirection = node.FirstFollowedDirection;

                originalRowNodes.Add(x, newNode);
            }

            result.Add(y, originalRowNodes);
        }

        // We have to reset the neighbours, otherwise the old neighbour nodes are copied onto the new set
        foreach (var (y, row) in result)
        foreach (var (x, node) in row)
        {
            // Set all traversable neighbours
            if (x > 0)
            {
                var leftNeighbour = result[y][x - 1];
                if (leftNeighbour.Letter != "#")
                    node.Neighbours.Add(Direction.Left, leftNeighbour);
            }
            else
            {
                node.Neighbours.Add(Direction.Left, new Node(outOfBoundsCharacter, x - 1, y));
            }

            if (x < row.Count - 1)
            {
                var rightNeighbour = result[y][x + 1];
                if (rightNeighbour.Letter != "#")
                    node.Neighbours.Add(Direction.Right, rightNeighbour);
            }
            else
            {
                node.Neighbours.Add(Direction.Right, new Node(outOfBoundsCharacter, x + 1, y));
            }

            if (y > 0)
            {
                var upNeighbour = result[y - 1][x];
                if (upNeighbour.Letter != "#")
                    node.Neighbours.Add(Direction.Up, upNeighbour);
            }
            else
            {
                node.Neighbours.Add(Direction.Up, new Node(outOfBoundsCharacter, x, y - 1));
            }

            if (y < result.Count - 1)
            {
                var downNeighbour = result[y + 1][x];
                if (downNeighbour.Letter != "#")
                    node.Neighbours.Add(Direction.Down, downNeighbour);
            }
            else
            {
                node.Neighbours.Add(Direction.Down, new Node(outOfBoundsCharacter, x, y + 1));
            }
        }

        return result;
    }

    private bool DetectLoopSimulatingWithObstruction(Node currentNode, Direction direction)
    {
        // Set obstruction
        var obstructionNode = currentNode.Neighbours[direction];
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
        var runDirection = direction;
        return !GuardCanFindMapEdge(this[currentNode.Y!.Value, currentNode.X!.Value]!, runDirection, false);
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