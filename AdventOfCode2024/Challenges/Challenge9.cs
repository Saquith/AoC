using System.Diagnostics;
using AdventOfCode2024.Models._9;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2024.Challenges;

public class Challenge9(IConfiguration config) : IChallenge
{
    private List<Block> _blocks = [];
    private List<Block> _chunkedBlocks = [];
    private int _currentId = -1;
    private const bool DebugShowSubsteps = false;
    
    public async Task ReadInput(string? fileName = null)
    {
        var inputFilePath = Path.Combine(config["InputFolderPath"]!, $"{fileName ?? GetType().Name.Substring(9)}.txt");
        if (!File.Exists(inputFilePath)) throw new FileNotFoundException("The input file could not be found.");

        await using var stream = File.OpenRead(inputFilePath);
        using var reader = new StreamReader(stream);

        _blocks = [];
        _chunkedBlocks = [];
        _currentId = -1;
        string? currentLine = await reader.ReadLineAsync();
        if (currentLine is null) throw new FileNotFoundException("The input file could not be parsed.");
        
        for (int index = 0; index < currentLine.Length; index++)
        {
            var currentValue = int.Parse(currentLine[index].ToString());
            
            var isEmptyPart = index % 2 == 1;
            if (!isEmptyPart)
                _currentId++;
            
            // Don't add empty (size 0) blocks
            if (currentValue > 0)
                _chunkedBlocks.Add(new Block(isEmptyPart ? -1 : _currentId, currentValue));
            
            for (int length = 0; length < currentValue; length++)
            {
                _blocks.Add(new Block(isEmptyPart ? -1 : _currentId, 1));
            }
        }
    }

    private void RunFileCompacting(List<Block> blocks)
    {
        var lastIndex = blocks.Count - 1;
        while (lastIndex >= 0)
        {
            // Find first non-empty block
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
                            var newSize = blocks[i].Size - currentBlock.Size;
                            var brokenUpBlock = new Block(blocks[i].Value, newSize);
                            
                            // Adding a new item means increasing the index 
                            blocks.Insert(i + 1, brokenUpBlock);
                            
                            // Update size
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
        if (stringBlocks.TrimEnd('.').Contains('.'))
            Debug.WriteLine("Detected issues");
    }

    private void RunFileCompactingWithEmptyBlocks(List<Block> blocks)
    {
        HashSet<int> consideredBlockIds = [];
        var stringBlocks = "";

        if (DebugShowSubsteps)
        {
            stringBlocks = blocks.Select(b => b.ToString()).Aggregate("", (current, s) => current + (s.Length > 1 ? $"[{s}]" : s));
            Debug.WriteLine(stringBlocks.Where(c => c != '[' && c != ']').Select(c => c.ToString()).Aggregate((a, b) => $"{a}{b}"));
        }

        int lastConsideredBlockIndex = blocks.Count - 1;
        while (consideredBlockIds.Count < _currentId)
        {
            // Ensure double empty blocks are merged
            for (var index = 0; index < blocks.Count - 1; index++)
            {
                var block = blocks[index];
                var nextBlock = blocks[index + 1];
                if (block.Value == -1 && nextBlock.Value == -1)
                {
                    block.Size += nextBlock.Size;
                    blocks.RemoveAt(index + 1);
                    if (index < lastConsideredBlockIndex)
                        lastConsideredBlockIndex--;
                    index--;
                }
            }

            // FindLastIndex, but start at the previously considered index instead of the end of the list
            if (lastConsideredBlockIndex > blocks.Count - 1)
                lastConsideredBlockIndex = blocks.Count - 1;
            for (int i = lastConsideredBlockIndex; i >= 0; i--)
            {
                if (blocks[i].Value != -1 && !consideredBlockIds.Contains(blocks[i].Value))
                {
                    lastConsideredBlockIndex = i;
                    break;
                }
            }
            if (lastConsideredBlockIndex == -1)
            {
                // Mark size as depleted
                continue;
            }
            var lastConsideredBlock = blocks[lastConsideredBlockIndex];
            consideredBlockIds.Add(lastConsideredBlock.Value);
            
            var firstFittingEmptyIndex = blocks.FindIndex(b => b.Value == -1 && b.Size >= lastConsideredBlock.Size);
            if (firstFittingEmptyIndex == -1)
            {
                // Mark size as depleted
                continue;
            }
            var firstFittingEmptyBlock = blocks[firstFittingEmptyIndex];

            // Break up current empty block
            var newSize = firstFittingEmptyBlock.Size - lastConsideredBlock.Size;
            var brokenUpBlock = new Block(firstFittingEmptyBlock.Value, newSize);

            // Swap only when swapping ahead
            if (lastConsideredBlockIndex < firstFittingEmptyIndex)
                continue;
            
            if (newSize > 0)
                firstFittingEmptyBlock.Size = lastConsideredBlock.Size;
            
            (blocks[firstFittingEmptyIndex], blocks[lastConsideredBlockIndex]) = (blocks[lastConsideredBlockIndex], blocks[firstFittingEmptyIndex]);

            if (newSize > 0)
                blocks.Insert(firstFittingEmptyIndex + 1, brokenUpBlock);

            if (DebugShowSubsteps)
            {
                stringBlocks = blocks.Select(b => b.ToString()).Aggregate("", (current, s) => current + (s.Length > 1 ? $"[{s}]" : s));
                Debug.WriteLine(stringBlocks.Where(c => c != '[' && c != ']').Select(c => c.ToString()).Aggregate((a, b) => $"{a}{b}"));
                Debug.WriteLine(consideredBlockIds.Select(id => id.ToString()).Aggregate((a, b) => $"{a}, {b}"));
            }

            Debug.WriteLine(consideredBlockIds.Last().ToString());
        }

        // Aggregate using brackets if the ID is larger than one character
        stringBlocks = blocks.Select(b => b.ToString()).Aggregate("", (current, s) => current + (s.Length > 1 ? $"[{s}]" : s));
        Debug.WriteLine(stringBlocks.Where(c => c != '[' && c != ']').Select(c => c.ToString()).Aggregate((a, b) => $"{a}{b}"));
        Debug.WriteLine(consideredBlockIds.Select(id => id.ToString()).Aggregate((a, b) => $"{a}, {b}"));
    }

    private long CalculateChecksum(List<Block> blocks)
    {
        long checksum = 0;
        int offset = 0;
        for (int i = 0; i < blocks.Count; i++)
        {
            var currentBlock = blocks[i];
            if (currentBlock.Value != -1)
            {
                for (int j = 0; j < currentBlock.Size; j++)
                {
                    if (j > 0)
                        offset++;
                    checksum += (i + offset) * currentBlock.Value;
                }
            }
            else
            {
                if (currentBlock.Size > 1)
                    offset += currentBlock.Size - 1;
            }
        }

        return checksum;
    }

    public (string, string) Calculate()
    {
        RunFileCompacting(_blocks);
        var checksum = CalculateChecksum(_blocks);

        RunFileCompactingWithEmptyBlocks(_chunkedBlocks);
        long secondChecksum = CalculateChecksum(_chunkedBlocks);

        return ($"{ checksum }",
            $"{ secondChecksum }");
    }
}