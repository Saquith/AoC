using AdventOfCode2024.challenges;
using Microsoft.Extensions.DependencyInjection;

namespace AdventOfCode2024.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddChosenChallenge(this IServiceCollection serviceCollection)
    {
        int currentAdventChallenge = 3;
        
        Console.WriteLine("Which challenge would you like to run?");
        Console.WriteLine($"[ {Enumerable.Range(1, currentAdventChallenge).Select(n => n.ToString()).Aggregate((a, b) => $"{a} {b}")} ]");
        var chosenChallenge = Console.ReadLine();

        if (int.TryParse(chosenChallenge, out int chosenChallengeNumber))
        {
            chosenChallenge = Math.Min(chosenChallengeNumber, currentAdventChallenge).ToString();
        }
        
        switch (chosenChallenge)
        {
            default:
            case "1":
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
            case "13":
                serviceCollection.AddSingleton<IChallenge, Challenge13>();
                break;
            case "14":
                serviceCollection.AddSingleton<IChallenge, Challenge14>();
                break;
            case "15":
                serviceCollection.AddSingleton<IChallenge, Challenge15>();
                break;
            case "16":
                serviceCollection.AddSingleton<IChallenge, Challenge16>();
                break;
            case "17":
                serviceCollection.AddSingleton<IChallenge, Challenge17>();
                break;
            case "18":
                serviceCollection.AddSingleton<IChallenge, Challenge18>();
                break;
            case "19":
                serviceCollection.AddSingleton<IChallenge, Challenge19>();
                break;
            case "20":
                serviceCollection.AddSingleton<IChallenge, Challenge20>();
                break;
            case "21":
                serviceCollection.AddSingleton<IChallenge, Challenge21>();
                break;
            case "22":
                serviceCollection.AddSingleton<IChallenge, Challenge22>();
                break;
            case "23":
                serviceCollection.AddSingleton<IChallenge, Challenge23>();
                break;
            case "24":
                serviceCollection.AddSingleton<IChallenge, Challenge24>();
                break;
            case "25":
                serviceCollection.AddSingleton<IChallenge, Challenge25>();
                break;
        }
        return serviceCollection;
    }
}