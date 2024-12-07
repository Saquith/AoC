using AdventOfCode2024.Extensions;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2024.challenges;

public class Challenge1(IConfiguration config) : IChallenge
{
    private List<long>? _leftList;
    private List<long>? _rightList;
    private Dictionary<long, long>? _tracker;
    private static object _lock = new();

    public async Task ReadInput()
    {
        try
        {
            var inputFilePath = Path.Combine(config["InputFolderPath"]!, "1.txt");
            if (!File.Exists(inputFilePath)) throw new FileNotFoundException("The input file could not be found.");
            
            await using var stream = File.OpenRead(inputFilePath);
            using var reader = new StreamReader(stream);

            _leftList = [];
            _rightList = [];
            _tracker = [];

            string? currentLine;
            while (!String.IsNullOrEmpty(currentLine = await reader.ReadLineAsync()))
            {
                // Parse
                var split = currentLine.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(long.Parse).ToArray();
                
                _leftList.AddSorted(split[0]);
                _rightList.AddSorted(split[1]);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.ReadLine();
        }
    }

    public string Calculate()
    {
        if (_leftList == null) throw new ArgumentNullException(nameof(_leftList));
        if (_rightList == null) throw new ArgumentNullException(nameof(_rightList));
        
        long differenceTotal = 0;
        long similarityTotal = 0;
        Parallel.For(0, _leftList.Count, i =>
        {
            var difference = Math.Abs(_leftList[i] - _rightList[i]);
            Interlocked.Add(ref differenceTotal, difference);

            // Lock to ensure no conflicts when increasing the tracker in parallel
            lock (_lock)
            {
                if (!_tracker!.ContainsKey(_rightList[i]))
                    _tracker!.Add(_rightList[i], 0);

                _tracker![_rightList[i]]++;
            }
        });

        foreach (var key in _leftList)
            similarityTotal += key * _tracker!.GetValueOrDefault(key, 0);
        
        return $"Total difference: {differenceTotal}\r\n" +
               $"Total similarity score: {similarityTotal}";
    }
}