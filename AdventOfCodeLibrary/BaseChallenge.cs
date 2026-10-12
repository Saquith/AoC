using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("2025.UnitTests")]
namespace AdventOfCodeLibrary.Challenges;

public abstract class BaseChallenge : IChallenge
{
    internal string CurrentClass => GetType().Name;
    
    protected abstract Task ChallengeInput(string filePath);

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

    public async Task ReadInput(string filePath)
    {
        await ChallengeInput(filePath);
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