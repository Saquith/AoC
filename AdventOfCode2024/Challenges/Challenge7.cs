using Microsoft.Extensions.Configuration;

namespace AdventOfCode2024.Challenges;

public class Challenge7(IConfiguration config) : IChallenge
{
    private List<(string, string)> _inputs = [];

    public async Task ReadInput(string? fileName = null)
    {
        var inputFilePath = Path.Combine(config["InputFolderPath"]!, $"{fileName ?? GetType().Name.Substring(9)}.txt");
        if (!File.Exists(inputFilePath)) throw new FileNotFoundException("The input file could not be found.");

        await using var stream = File.OpenRead(inputFilePath);
        using var reader = new StreamReader(stream);

        string? currentLine;
        while (!string.IsNullOrEmpty(currentLine = await reader.ReadLineAsync()))
        {
            // Parse
            var split = currentLine.Split(":", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            _inputs.Add((split[0], split[1]));
        }
    }

    private List<string> GetPermutations(string characters, int length, string result = "")
    {
        if (length == 0) return [result];
        
        var results = new List<string>();
        foreach (var c in characters)
            results.AddRange(GetPermutations(characters, length - 1, result + c));

        return results;
    }

    private bool IsPossibleCombination(string operators, long target, List<long> numbers)
    {
        foreach (var listOfOperators in GetPermutations(operators, numbers.Count - 1))
        {
            long total = numbers[0];
            for (var index = 0; index < listOfOperators.Length; index++)
            {
                var op = listOfOperators[index];
                long number = numbers[index + 1];
                switch (op)
                {
                    case '*':
                        total *= number;
                        break;
                    case '+':
                        total += number;
                        break;
                    case '$':
                        total = long.Parse($"{total}{number}");
                        break;
                }
            }

            if (total == target)
                return true;
        }

        return false;
    }

    public (string, string) Calculate()
    {
        long total = 0;
        foreach (var input in _inputs)
        {
            var target = long.Parse(input.Item1);
            if (IsPossibleCombination("*+", target, input.Item2.Split(" ").Select(long.Parse).ToList()))
            {
                total += target;
            }
        }
        
        long secondTotal = 0;
        foreach (var input in _inputs)
        {
            var target = long.Parse(input.Item1);
            if (IsPossibleCombination("*+$", target, input.Item2.Split(" ").Select(long.Parse).ToList()))
            {
                secondTotal += target;
            }
        }

        return ($"{ total }",
            $"{ secondTotal }");
    }
}