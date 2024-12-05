using AdventOfCode2024.challenges;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// Setup
var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
var config = builder.Build();

var servicesCollection = new ServiceCollection()
    // .AddSingleton<Service?>()
    .AddSingleton<IConfiguration>(config);

Console.WriteLine("Which challenge would you like to run?");
Console.WriteLine($"[ {Enumerable.Range(1, 2).Select(n => n.ToString()).Aggregate((a, b) => $"{a} {b}")} ]");
switch (Console.ReadLine())
{
    default:
    case "2":
        servicesCollection.AddSingleton<IChallenge, Challenge2>();
        break;
    case "1":
        servicesCollection.AddSingleton<IChallenge, Challenge1>();
        break;
}

var serviceProvider = servicesCollection.BuildServiceProvider();

// Run
using var scope = serviceProvider.CreateScope();
var challenge = scope.ServiceProvider.GetService<IChallenge>();
await challenge!.ReadInput();

var result = await challenge.Calculate();
Console.WriteLine($"Result to current challenge:\r\n{result}");
