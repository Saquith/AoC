namespace AdventOfCodeLibrary.Challenges;

public interface IChallenge
{
    Task ReadInput(string filePath);
    (string, string) Calculate();
}