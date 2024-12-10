using System.Diagnostics;
using AdventOfCode2024.Models._9;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2024.Challenges;

public class Challenge9(IConfiguration config) : IChallenge
{
    private List<Block> _blocks = [];
    private List<Block> _chunkedBlocks = [];
    
    public async Task ReadInput(string? fileName = null)
    {
        var inputFilePath = Path.Combine(config["InputFolderPath"]!, $"{fileName ?? GetType().Name.Substring(9)}.txt");
        if (!File.Exists(inputFilePath)) throw new FileNotFoundException("The input file could not be found.");

        await using var stream = File.OpenRead(inputFilePath);
        using var reader = new StreamReader(stream);

        _blocks = [];
        _chunkedBlocks = [];
        
        int currentId = -1;
        string? currentLine = await reader.ReadLineAsync();
        if (currentLine is null) throw new FileNotFoundException("The input file could not be parsed.");
        
        for (int index = 0; index < currentLine.Length; index++)
        {
            var currentValue = int.Parse(currentLine[index].ToString());
            
            var isEmptyPart = index % 2 == 1;
            if (!isEmptyPart)
                currentId++;
            
            _chunkedBlocks.Add(new Block(isEmptyPart ? -1 : currentId, currentValue));
            for (int length = 0; length < currentValue; length++)
            {
                _blocks.Add(new Block(isEmptyPart ? -1 : currentId, 1));
            }
        }
    }

    private void RunFileCompacting(List<Block> blocks, bool allowEmptySlots = false)
    {
        var lastIndex = blocks.Count - 1;
        while (lastIndex >= 0)
        {
            // Find first empty block
            var currentBlock = blocks[lastIndex];
            if (currentBlock.Value != -1)
            {
                // Find block of size
                for (int i = 0; i < blocks.Count; i++)
                {
                    if (blocks[i].Value == -1 && blocks[i].Size >= currentBlock.Size)
                    {
                        if (blocks[i].Size > currentBlock.Size)
                        {
                            // Break up current empty block
                            var brokenUpBlock = new Block(blocks[i].Value, blocks[i].Size - currentBlock.Size);
                            blocks.Insert(i + 1, brokenUpBlock);
                            blocks[i].Size = currentBlock.Size;
                        }
                        
                        // Swap end block to first fitting block
                        (blocks[i], blocks[lastIndex]) = (blocks[lastIndex], blocks[i]);
                    }
                }
            }

            lastIndex--;
        }
        
        // Move first item to last
        var firstBlock = blocks[0];
        if (firstBlock.Value == -1)
        {
            blocks.RemoveAt(0);
            blocks.Add(firstBlock);
        }

        // Aggregate using brackets if the ID is larger than one character
        var stringBlocks = blocks.Select(b => b.ToString()).Aggregate("", (current, s) => current + (s.Length > 1 ? $"[{s}]" : s));
        Debug.WriteLine(stringBlocks);
        if (!allowEmptySlots && stringBlocks.TrimEnd('.').Contains('.'))
            Debug.WriteLine("Detected issues");
    }

    private long CalculateChecksum(List<Block> blocks)
    {
        long checksum = 0;
        for (int i = 0; i < blocks.Count; i++)
        {
            var currentBlock = blocks[i];
            if (currentBlock.Value != -1)
            {
                checksum += i * currentBlock.Value;
            }
        }

        return checksum;
    }

    public (string, string) Calculate()
    {
        RunFileCompacting(_blocks);
        var checksum = CalculateChecksum(_blocks);

        RunFileCompacting(_chunkedBlocks, true);
        long secondChecksum = CalculateChecksum(_chunkedBlocks);

        return ($"{ checksum }",
            $"{ secondChecksum }");
    }
}