using System.Diagnostics;
using AdventOfCode2024.challenges;
using AdventOfCode2024.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// Setup
var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
var config = builder.Build();

var serviceCollection = new ServiceCollection()
    // .AddSingleton<Service?>()
    .AddSingleton<IConfiguration>(config)
    .AddChosenChallenge(6);

// Run
long startTime = Stopwatch.GetTimestamp();

using var scope = serviceCollection.BuildServiceProvider().CreateScope();
var challenge = scope.ServiceProvider.GetService<IChallenge>();
await challenge!.ReadInput();

var result = challenge.Calculate();

TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
Console.WriteLine($"Result (in {elapsedTime:c}) to current challenge:\r\n{result}");
