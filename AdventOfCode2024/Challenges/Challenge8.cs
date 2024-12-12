using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2024.Challenges;

public class Challenge8(IConfiguration config) : IChallenge
{
    private HashSet<char> _characterMap = [];
    private Dictionary<char, HashSet<(int, int)>> _maps = [];
    private (int, int) _bounds;
    
    public async Task ReadInput(string? fileName = null)
    {
        var inputFilePath = Path.Combine(config["InputFolderPath"]!, $"{fileName ?? GetType().Name.Substring(9)}.txt");
        if (!File.Exists(inputFilePath)) throw new FileNotFoundException("The input file could not be found.");

        await using var stream = File.OpenRead(inputFilePath);
        using var reader = new StreamReader(stream);

        string? currentLine;
        var y = -1;
        var width = 0;
        while (!string.IsNullOrEmpty(currentLine = await reader.ReadLineAsync()))
        {
            width = Math.Max(width, currentLine.Length - 1);
            y++;

            for (var x = 0; x < currentLine.ToCharArray().Length; x++)
            {
                var c = currentLine.ToCharArray()[x];
                if (c != '.')
                    _characterMap.Add(c);
                else
                    continue;

                if (!_maps.ContainsKey(c))
                    _maps.Add(c, new HashSet<(int, int)>());
                _maps[c].Add((x, y));
            }
        }
        _bounds = (width, y);
    }

    public (string, string) Calculate()
    {
        HashSet<(int, int)> antiNodeCoordinates = [];
        foreach (var (c, coordinates) in _maps)
        {
            foreach (var antenna in coordinates)
            {
                foreach (var otherAntenna in coordinates)
                {
                    if (antenna == otherAntenna)
                        continue;

                    var difference = (antenna.Item1 - otherAntenna.Item1, antenna.Item2 - otherAntenna.Item2);

                    // While calculated coordinate is in bounds
                    var calculatedCoordinate = (difference.Item1 + antenna.Item1, difference.Item2 + antenna.Item2);

                    // Only save the antiNode if it is between bounds
                    if (calculatedCoordinate.Item1 >= 0 && calculatedCoordinate.Item1 <= _bounds.Item1
                        && calculatedCoordinate.Item2 >= 0 && calculatedCoordinate.Item2 <= _bounds.Item2)
                    {
                        antiNodeCoordinates.Add(calculatedCoordinate);
                    }
                }
            }
        }
        
        HashSet<(int, int)> antiNodeCoordinatesLinear = [];
        foreach (var (c, coordinates) in _maps)
        {
            foreach (var antenna in coordinates)
            {
                foreach (var otherAntenna in coordinates)
                {
                    if (antenna == otherAntenna)
                        continue;
                    
                    // All pairs of antennae are antiNodes
                    var currentCoordinate = (antenna.Item1, antenna.Item2);
                    antiNodeCoordinatesLinear.Add(currentCoordinate);
                    
                    var difference = (antenna.Item1 - otherAntenna.Item1, antenna.Item2 - otherAntenna.Item2);

                    // While calculated coordinate is in bounds
                    while (currentCoordinate.Item1 >= 0 && currentCoordinate.Item1 <= _bounds.Item1 
                            && currentCoordinate.Item2 >= 0 && currentCoordinate.Item2 <= _bounds.Item2)
                    {
                        var calculatedCoordinate = (difference.Item1 + currentCoordinate.Item1, difference.Item2 + currentCoordinate.Item2);

                        // Only save the antiNode if it is between bounds
                        if (calculatedCoordinate.Item1 >= 0 && calculatedCoordinate.Item1 <= _bounds.Item1
                            && calculatedCoordinate.Item2 >= 0 && calculatedCoordinate.Item2 <= _bounds.Item2)
                        {
                            antiNodeCoordinatesLinear.Add(calculatedCoordinate);
                        }
                        
                        currentCoordinate = (calculatedCoordinate.Item1, calculatedCoordinate.Item2);
                    }
                }
            }
        }
        
        Debug.WriteLine(GenerateMap(_maps, _bounds, antiNodeCoordinates));
        Debug.WriteLine(GenerateMap(_maps, _bounds, antiNodeCoordinatesLinear));
        
        return ($"{ antiNodeCoordinates.Count }",
            $"{ antiNodeCoordinatesLinear.Count }");
    }

    private string GenerateMap(Dictionary<char, HashSet<(int, int)>> maps, (int, int) bounds, HashSet<(int, int)> antiNodeCoordinates)
    {
        var result = "";
        for (var y = 0; y <= bounds.Item1; y++)
        {
            for (int x = 0; x <= bounds.Item2; x++)
            {
                var mapCharacter = antiNodeCoordinates.Contains((x, y)) ? '#' : '.';
                foreach (var (antenna, coordinates) in maps)
                {
                    if (coordinates.Contains((x, y)))
                        mapCharacter = antenna;
                }

                result += mapCharacter;
            }

            result += "\r\n";
        }

        return result;
    }
}