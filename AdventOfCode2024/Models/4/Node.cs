namespace AdventOfCode2024.Models._4;

public class Node(string letter)
{
    public Guid Id { get; set; } = new();
    public string Letter { get; } = letter;
    public List<Node> Neighbours { get; } = [];
}