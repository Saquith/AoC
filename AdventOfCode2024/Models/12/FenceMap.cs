using System.Diagnostics;
using AdventOfCode2024.Extensions;
using AdventOfCode2024.Models._4;

namespace AdventOfCode2024.Models._12;

public class FenceMap(Dictionary<int, Dictionary<int, Node>> nodes, string obstructionCharacter = "#", string outOfBoundsCharacter = "@") : Map(nodes, obstructionCharacter, outOfBoundsCharacter)
{
    private Dictionary<string, long> _areaByAreaID = [];
    private Dictionary<string, long> _edgeCountByAreaID = [];
    private Dictionary<string, string> _letterByAreaID = [];

    private Dictionary<string, HashSet<string>> _connectionMap = [];
    
    public long CalculateFencePricePerNode()
    {
        foreach (var node in GetAllNodes())
        {
            if (string.IsNullOrEmpty(node.AreaID))
            {
                string? areaID = null;
                var neighboursWithSameLetters = node.GetNeighbours().Where(kvp => kvp.Value.Letter == node.Letter && kvp.Value.AreaID != null);
                foreach (var (d, neighbour) in neighboursWithSameLetters)
                    areaID ??= neighbour.AreaID;
                
                node.AreaID = areaID ?? Guid.NewGuid().ToString();
                _connectionMap.TryAdd(node.AreaID, []);
                _connectionMap[node.AreaID].Add(node.AreaID);
            }
            
            // Up area for character
            _areaByAreaID.TryAdd(node.AreaID, 0);
            _areaByAreaID[node.AreaID]++;
            _letterByAreaID.TryAdd(node.AreaID, node.Letter);
            
            // Up edge count for character neighbours (that are different)
            foreach (var (direction, neighbour) in node.GetNeighbours())
            {
                // Handle areas of same letters (detect connected areas)
                if (node.Letter == neighbour.Letter)
                {
                    if (neighbour.AreaID == null)
                        neighbour.AreaID = node.AreaID;
                    else
                    {
                        // Merge lists
                        foreach (var otherAreaID in _connectionMap[node.AreaID])
                            _connectionMap[neighbour.AreaID].Add(otherAreaID);
                        foreach (var otherAreaID in _connectionMap[neighbour.AreaID])
                            _connectionMap[node.AreaID].Add(otherAreaID);
                    }
                }
                else
                {
                    _edgeCountByAreaID.TryAdd(node.AreaID, 0);
                    _edgeCountByAreaID[node.AreaID]++;
                }
            }
        }

        // Calculate total for each area
        HashSet<string> alreadyCheckedAreas = [];
        long total = 0;
        foreach (var (areaID, area) in _areaByAreaID)
        {
            long totalArea = 0;
            long totalEdgeCount = 0;

            // All hope abandon ye who enter here
            HashSet<string> currentListOfConnections = [];
            foreach (var id in _connectionMap[areaID])
                foreach (var id2 in _connectionMap[id])
                    currentListOfConnections.Add(id2);
            HashSet<string> connectedConnections = [];
            foreach (var id in currentListOfConnections)
                foreach (var id2 in _connectionMap[id])
                    connectedConnections.Add(id2);
            currentListOfConnections = [];
            foreach (var id in connectedConnections)
                foreach (var id2 in _connectionMap[id])
                    currentListOfConnections.Add(id2);
            
            // Calculate all total areas
            foreach (var otherAreaID in currentListOfConnections)
            {
                if (alreadyCheckedAreas.Contains(otherAreaID))
                    continue;
                
                // Add joined areas to current totals
                totalArea += _areaByAreaID[otherAreaID];
                totalEdgeCount += _edgeCountByAreaID[otherAreaID];
                alreadyCheckedAreas.Add(otherAreaID);
            }

            if (totalArea > 0)
            {
                Debug.WriteLine($"A region of {_letterByAreaID[areaID]} plants with price {totalArea} * {totalEdgeCount} = {totalArea * totalEdgeCount}");
                total += totalArea * totalEdgeCount;
            }
        }

        return total;
    }

    public long CalculateFencePricePerSide()
    {
        _areaByAreaID = [];
        List<List<Node>> nodeAreas = [];
        foreach (var currentNode in GetAllNodes())
        {
            // TODO: Allow only navigation for Laterals, but request Diagonals so diagonal can find corners as well
            if (currentNode.AreaID == null)
                nodeAreas.Add(currentNode.Explore(_areaByAreaID, Guid.NewGuid().ToString()));
        }

        // Calculate total for each area
        long total = 0;
        foreach (var list in nodeAreas)
        {
            // Check corners, and count those for the easiest count
            long cornerTotal = 0;
            foreach (var corner in list.Select(n => n.GetCornerCount()))
                cornerTotal += corner;

            // Get random first node to take areaID
            var firstNode = list[0];
            total += _areaByAreaID[firstNode.AreaID!] * cornerTotal;
            Debug.WriteLine($"A region of {firstNode.Letter} plants with price {_areaByAreaID[firstNode.AreaID!]} * {cornerTotal} = {_areaByAreaID[firstNode.AreaID!] * cornerTotal}");
        }

        return total;
    }

    public override string ToString()
    {
        var result = "";
        foreach (var (_, row) in Nodes)
        {
            var rowResult = "";
            foreach (var (_, node) in row)
            {
                rowResult += $"{node.Letter}{node.AreaID![0]}";
            }
            result += $"{rowResult}\r\n";
        }

        return result;
    }

    public void EmptyAreaIDs()
    {
        foreach (var node in GetAllNodes())
            node.AreaID = null;
    }
}