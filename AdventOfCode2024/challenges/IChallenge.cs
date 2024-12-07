namespace AdventOfCode2024.challenges;

public interface IChallenge
{
    Task ReadInput();
    string Calculate();
}