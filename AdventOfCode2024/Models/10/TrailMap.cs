using System.Diagnostics;
using AdventOfCode2024.Models._4;

namespace AdventOfCode2024.Models._10;

public class TrailMap(Dictionary<int, Dictionary<int, Node>> nodes, string obstructionCharacter = ".", string outOfBoundsCharacter = "*") : Map(nodes, obstructionCharacter, outOfBoundsCharacter)
{
    private HashSet<(int, int)> _reachedPeaks = [];

    public int FindReachablePeaks(Node trailHead)
    {
        _reachedPeaks = [];
        
        FindPeaks(trailHead);

        return _reachedPeaks.Count;
    }

    private void FindPeaks(Node currentNode)
    {
        if (currentNode.Letter == "9")
            _reachedPeaks.Add((currentNode.X!.Value, currentNode.Y!.Value));

        if (currentNode.Neighbours.Count == 0)
            return;
        
        foreach (var (_, neighbour) in currentNode.Neighbours)
        {
            FindPeaks(neighbour);
        }
    }
}