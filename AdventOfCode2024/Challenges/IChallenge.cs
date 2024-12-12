namespace AdventOfCode2024.Challenges;

public interface IChallenge
{
    Task ReadInput(string? fileName = null);
    (string, string) Calculate();
}