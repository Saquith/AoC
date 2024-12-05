using AdventOfCode2024.Models._2;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2024.challenges;

public class Challenge2(IConfiguration config) : IChallenge
{
    private List<Report>? _reports;
    
    public async Task ReadInput()
    {
        try
        {
            var inputFilePath = Path.Combine(config["InputFolderPath"]!, "2.txt");
            if (!File.Exists(inputFilePath)) throw new FileNotFoundException("The input file could not be found.");
            
            await using var stream = File.OpenRead(inputFilePath);
            using var reader = new StreamReader(stream);

            _reports = [];
            
            string? currentLine;
            while (!String.IsNullOrEmpty(currentLine = await reader.ReadLineAsync()))
            {
                // Parse
                 _reports.Add(Report.Parse(currentLine));
            }
        }
        
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.ReadLine();
        }
    }

    public async Task<string> Calculate()
    {
        var strictCount = _reports!.Count(r => r.IsSafe());
        var count = _reports!.Count(r => r.IsSafe(strict: false));
        
        return $"Safe reports: {strictCount}\r\n" +
               $"Dampener sae reports: {count}";
    }
}