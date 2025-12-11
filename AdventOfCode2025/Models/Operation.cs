namespace AdventOfCode2025.Models;

public class Operation(string type, long first, long second)
{
    public string OperationType { get; set; } = type;
    public long FirstNumber { get; set; } = first;
    public long SecondNumber { get; set; } = second;
}