using AdventOfCode2024.challenges;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// Setup
var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
var config = builder.Build();

var servicesCollection = new ServiceCollection()
    // .AddSingleton<Service?>()
    .AddSingleton<IChallenge, Challenge2>()
    .AddSingleton<IConfiguration>(config);

var serviceProvider = servicesCollection.BuildServiceProvider();

// Run
using var scope = serviceProvider.CreateScope();
var challenge = scope.ServiceProvider.GetService<IChallenge>();
await challenge!.ReadInput();

var result = await challenge.Calculate();
Console.WriteLine($"Result to current challenge:\r\n{result}");

// Console.WriteLine("Press enter to exit..");
// Console.ReadLine();