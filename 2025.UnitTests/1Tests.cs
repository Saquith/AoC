using AdventOfCode2025.Challenges;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2025.UnitTests;

[TestClass]
public class Challenge1Tests
{
    private readonly IConfigurationRoot _config;

    public Challenge1Tests()
    {
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
        _config = builder.Build();
    }
    
    [TestMethod]
    [DataRow("1_example", "3", "6")]
    [DataRow("1_example2", "1", "10")]
    [DataRow("1_example3", "2", "2")]
    [DataRow("1_example4", "1", "1")]
    [DataRow("1_example5", "1", "1")]
    [DataRow("1_example6", "1", "2")]
    [DataRow("1_input", "1120", "6554")]
    public async Task VerifyBasicsTest(string fileName, string expectedResultA, string expectedResultB)
    {
        var challenge = new Challenge1(_config);
        await challenge.ReadInput(fileName);

        var (a, b) = challenge.Calculate();
        Assert.AreEqual(expectedResultA, a);
        Assert.AreEqual(expectedResultB, b);
    }
}