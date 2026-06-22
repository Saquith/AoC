using System.Text;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2025.Challenges;

public class Challenge3(IConfiguration config) : IChallenge
{
    private List<string>? _batteryBanks;

    public async Task ReadInput(string? fileName = null)
    {
        var inputFilePath = Path.Combine(config["InputFolderPath"]!, $"{fileName ?? GetType().Name.Substring(9)}.txt");
        if (!File.Exists(inputFilePath)) throw new FileNotFoundException("The input file could not be found.");

        await using var stream = File.OpenRead(inputFilePath);
        using var reader = new StreamReader(stream);

        _batteryBanks = [];

        string? currentLine;
        while (!string.IsNullOrEmpty(currentLine = await reader.ReadLineAsync()))
        {
            // Parse
            _batteryBanks.Add(currentLine);
        }
    }

    public (string, string) Calculate()
    {
        const int CUTOFF_INDEX_FROM_END = 1;
        const int CUTOFF_INDEX_FROM_END_PART_TWO = 11;
        
        long total = 0;
        foreach (var batteryBank in _batteryBanks!)
        {
            var highestIndex = GetHighestJoltage(batteryBank, CUTOFF_INDEX_FROM_END);
            
            // Greedy implementation
            var secondJoltageNumber = batteryBank.Substring(highestIndex + 1).Select(s => int.Parse(s.ToString())).Max();
            var largestJoltage = $"{batteryBank[highestIndex]}{secondJoltageNumber}";
            Console.WriteLine($"{batteryBank} : {largestJoltage}");
            total += long.Parse(largestJoltage);
        }

        long secondOutput = 0;
        foreach (var batteryBank in _batteryBanks!)
        {
            // Check list until no longer possible to get higher amounts
            var highestIndex = GetHighestJoltage(batteryBank, CUTOFF_INDEX_FROM_END_PART_TWO);
            
            // Find best continuation of joltage
            var currentIndex = highestIndex + 1;
            var indices = new List<int> { highestIndex };
            while (highestIndex != currentIndex && currentIndex < batteryBank.Length - (CUTOFF_INDEX_FROM_END_PART_TWO - indices.Count))
            {
                var newIndex = GetHighestJoltage(batteryBank, CUTOFF_INDEX_FROM_END_PART_TWO - indices.Count, currentIndex);
                if (newIndex == -1)
                    break;
                
                indices.Add(newIndex);
                currentIndex = newIndex + 1;
            }

            // Add rest of joltages due to cutoff
            var largestJoltage = new StringBuilder();
            foreach (var index in indices.Take(CUTOFF_INDEX_FROM_END_PART_TWO + 1))
                largestJoltage.Append(batteryBank[index]);
            
            Console.WriteLine($"{batteryBank} : {largestJoltage}");
            secondOutput += long.Parse(largestJoltage.ToString());
        }

        return ($"{total}",
            $"{secondOutput}");
    }

    private int GetHighestJoltage(string batteryBankJoltages, int cutoffIndex, int startIndex = 0)
    {
        int highestJoltage = 0, highestIndex = -1;
        for (int i = startIndex; i < batteryBankJoltages.Length - cutoffIndex && i < batteryBankJoltages.Length; i++)
        {
            var currentJoltage = int.Parse(batteryBankJoltages[i].ToString());
            if (currentJoltage > highestJoltage)
            {
                highestJoltage = currentJoltage;
                highestIndex = i;
            }
        }

        return highestIndex;
    }
}