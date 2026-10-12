namespace AdventOfCode2024.Models._9;

public class Block(int value, int size)
{
    public int Value { get; } = value;
    public int Size { get; set; } = size;

    public override string ToString()
    {
        var result = "";
        for (int i = 0; i < Size; i++)
        {
            result += Value == -1 ? "." : Value.ToString();
        }
        return result;
    }
}