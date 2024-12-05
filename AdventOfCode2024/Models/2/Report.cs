using System.Diagnostics;

namespace AdventOfCode2024.Models._2;

public class Report(long[] numbers)
{
    public static Report Parse(string report)
    {
        return new Report(
            report.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse).ToArray());
    }

    public bool Validate(bool strict = true)
    {
        bool result = true;
        string? mode = null;
        long? previous = null;
        foreach (var current in numbers)
        {
            // Skip first number
            if (previous != null)
            {
                // Handle increasing or decreasing
                if (previous < current)
                {
                    if ("DESC".Equals(mode)) {
                        result = false;
                        break;
                    }
                    
                    mode = "ASC";
                }

                if (current < previous)
                {
                    if ("ASC".Equals(mode)) {
                        result = false;
                        break;
                    }
                    
                    mode = "DESC";
                }
                
                // Handle adjacency
                if (!new long[] { 1, 2, 3 }.Contains(Math.Abs(current - previous.Value)))
                {
                    result = false;
                    break;
                }
            }
            
            previous = current;
        }

        if (!result && !strict)
        {
            Debug.WriteLine(numbers.Select(n => n.ToString()).Aggregate((a, b) => $"{a} {b}"));
            for (var index = 1; index < numbers.Length + 1; index++)
            {
                var withoutOneNumberInput = numbers.Take(index - 1).Concat(numbers.Skip(index))
                    .Select(n => n.ToString()).Aggregate((a, b) => $"{a} {b}");
                
                if (Parse(withoutOneNumberInput).Validate(!strict))
                {
                    Debug.WriteLine(withoutOneNumberInput);
                    result = true;
                    break;
                }
            }
        }

        return result;
    }
}

public static class ReportExtensions
{
    public static bool IsSafe(this Report report, bool strict = true)
    {
        return report.Validate(strict);
    }
}