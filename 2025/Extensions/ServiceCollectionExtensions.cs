using AdventOfCode2025.Challenges;
using Microsoft.Extensions.DependencyInjection;

namespace AdventOfCode2025.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddChosenChallenge(this IServiceCollection serviceCollection, int currentAdventChallenge)
    {
        Console.WriteLine("Which challenge would you like to run?");
        Console.WriteLine($"[ {Enumerable.Range(1, currentAdventChallenge).Select(n => n.ToString()).Aggregate((a, b) => $"{a} {b}")} ]");
        var chosenChallenge = Console.ReadLine() ?? currentAdventChallenge.ToString();

        chosenChallenge = int.TryParse(chosenChallenge, out int chosenChallengeNumber) ? Math.Min(chosenChallengeNumber, currentAdventChallenge).ToString() : currentAdventChallenge.ToString();
        
        switch (chosenChallenge)
        {
            default:
                serviceCollection.AddSingleton<IChallenge, Challenge1>();
                break;
            case "2":
                serviceCollection.AddSingleton<IChallenge, Challenge2>();
                break;
            case "3":
                serviceCollection.AddSingleton<IChallenge, Challenge3>();
                break;
            case "4":
                serviceCollection.AddSingleton<IChallenge, Challenge4>();
                break;
            case "5":
                serviceCollection.AddSingleton<IChallenge, Challenge5>();
                break;
            case "6":
                serviceCollection.AddSingleton<IChallenge, Challenge6>();
                break;
            case "7":
                serviceCollection.AddSingleton<IChallenge, Challenge7>();
                break;
            case "8":
                serviceCollection.AddSingleton<IChallenge, Challenge8>();
                break;
            case "9":
                serviceCollection.AddSingleton<IChallenge, Challenge9>();
                break;
            case "10":
                serviceCollection.AddSingleton<IChallenge, Challenge10>();
                break;
            case "11":
                serviceCollection.AddSingleton<IChallenge, Challenge11>();
                break;
            case "12":
                serviceCollection.AddSingleton<IChallenge, Challenge12>();
                break;
        }
        return serviceCollection;
    }
}