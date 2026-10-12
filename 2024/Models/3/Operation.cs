namespace AdventOfCode2024.Models._3;

public class Operation(string type, long first, long second)
{
    public string OperationType { get; set; } = type;
    public long FirstNumber { get; set; } = first;
    public long SecondNumber { get; set; } = second;
}