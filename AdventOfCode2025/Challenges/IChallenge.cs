namespace AdventOfCode2025.Challenges;

public interface IChallenge
{
    Task ReadInput(string? fileName = null);
    (string, string) Calculate();
}