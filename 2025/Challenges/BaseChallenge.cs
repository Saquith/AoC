using Microsoft.Extensions.Configuration;

namespace AdventOfCode2025.Challenges;

public abstract class BaseChallenge : IChallenge
{
    private string CurrentClass => GetType().Name;
    
    protected abstract Task ChallengeInput(string? fileName = null);

    protected virtual Task<string> CalculatePartA()
    {
        return Task.FromResult(string.Empty);
    }

    protected virtual Task<string> CalculatePartB()
    {
        return Task.FromResult(string.Empty);
    }

    protected virtual async Task<(string, string)> CalculateAsync()
    {
        var resultA = await CalculatePartA();
        Console.WriteLine($"[{CurrentClass}] Calculation part A done");
        var resultB = await CalculatePartB();
        Console.WriteLine($"[{CurrentClass}] Calculation part B done");

        return ($"{resultA}", $"{resultB}");
    }

    public async Task ReadInput(string? fileName = null)
    {
        await ChallengeInput(fileName);
        Console.WriteLine($"[{CurrentClass}] Input read");
    }

    public virtual (string, string) Calculate()
    {
        var calculateAsyncTask = CalculateAsync();
        Task.WhenAll(calculateAsyncTask);
        Console.WriteLine($"[{CurrentClass}] Calculation done");
        
        return calculateAsyncTask.Result;
    }
}