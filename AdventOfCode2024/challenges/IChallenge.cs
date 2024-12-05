namespace AdventOfCode2024.challenges;

public interface IChallenge
{
    Task ReadInput();
    Task<string> Calculate();
}