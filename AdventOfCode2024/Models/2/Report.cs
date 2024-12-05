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
        string? mode = null;
        long? previous = null;
        var hasIssue = false;
        foreach (var current in numbers)
        {
            // Skip first number
            if (previous != null)
            {
                // Handle increasing or decreasing
                if (previous < current)
                {
                    if ("DESC".Equals(mode))
                        if (!strict && !hasIssue)
                            hasIssue = true;
                        else
                            return false;
                    
                    mode = "ASC";
                }

                if (current < previous)
                {
                    if ("ASC".Equals(mode))
                        if (!strict && !hasIssue)
                            hasIssue = true;
                        else
                            return false;
                    
                    mode = "DESC";
                }
                
                // Handle adjacency
                if (!new long[] { 1, 2, 3 }.Contains(Math.Abs(current - previous.Value)))
                {
                    return false;
                }
            }            
            
            previous = current;
        }

        return mode != null && previous != null;
    }
}

public static class ReportExtensions
{
    public static bool IsSafe(this Report report, bool strict = true)
    {
        return report.Validate(strict);
    }
}