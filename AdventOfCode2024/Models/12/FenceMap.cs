using System.Diagnostics;
using AdventOfCode2024.Models._4;

namespace AdventOfCode2024.Models._12;

public class FenceMap(Dictionary<int, Dictionary<int, Node>> nodes, string obstructionCharacter = "#", string outOfBoundsCharacter = "@") : Map(nodes, obstructionCharacter, outOfBoundsCharacter)
{
    private Dictionary<string, long> _areaByAreaID = [];
    private Dictionary<string, long> _edgeCountByAreaID = [];
    private Dictionary<string, string> _letterByAreaID = [];

    private Dictionary<string, HashSet<string>> _connectionMap = [];
    
    public long CalculateFencePrice()
    {
        foreach (var node in GetAllNodes())
        {
            if (string.IsNullOrEmpty(node.AreaID))
            {
                node.AreaID = Guid.NewGuid().ToString();
                _connectionMap.TryAdd(node.AreaID, []);
            }
            
            // Up area for character
            _areaByAreaID.TryAdd(node.AreaID, 0);
            _areaByAreaID[node.AreaID]++;
            _letterByAreaID.TryAdd(node.AreaID, node.Letter);
            
            // Up edge count for character neighbours (that are different)
            foreach (var (_, neighbour) in node.Neighbours)
            {
                // Handle areas of same letters (detect connected areas)
                if (node.Letter == neighbour.Letter)
                {
                    if (neighbour.AreaID != null && node.AreaID != neighbour.AreaID)
                    {
                        _connectionMap.TryAdd(node.AreaID, []);
                        _connectionMap[node.AreaID].Add(neighbour.AreaID);

                        if (_connectionMap.ContainsKey(neighbour.AreaID))
                        {
                            _connectionMap.TryAdd(neighbour.AreaID, []);
                            _connectionMap[neighbour.AreaID].Add(node.AreaID);
                            
                            // Merge lists
                            foreach (var otherAreaID in _connectionMap[node.AreaID])
                                _connectionMap[neighbour.AreaID].Add(otherAreaID);
                            foreach (var otherAreaID in _connectionMap[neighbour.AreaID])
                                _connectionMap[node.AreaID].Add(otherAreaID);
                        }
                    }
                    else
                    {
                        neighbour.AreaID = node.AreaID;
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
            // Skip if already added below
            if (alreadyCheckedAreas.Contains(areaID))
                continue;
            alreadyCheckedAreas.Add(areaID);

            var totalArea = area;
            var totalEdgeCount = _edgeCountByAreaID[areaID];
            foreach (var otherAreaID in _connectionMap[areaID])
            {
                if (otherAreaID == areaID)
                    continue;
                
                // Add joined areas to current totals
                totalArea += _areaByAreaID[otherAreaID];
                totalEdgeCount += _edgeCountByAreaID[otherAreaID];
                alreadyCheckedAreas.Add(otherAreaID);
            }
            Debug.WriteLine($"A region of {_letterByAreaID[areaID]} plants with price {totalArea} * {totalEdgeCount} = { totalArea * totalEdgeCount }");
            total += totalArea * totalEdgeCount;
        }

        return total;
    }
}