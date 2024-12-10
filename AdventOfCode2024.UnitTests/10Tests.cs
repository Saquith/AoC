using AdventOfCode2024.Challenges;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2024.UnitTests;

[TestClass]
public class Challenge10Tests
{
    private readonly IConfigurationRoot _config;

    public Challenge10Tests()
    {
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
        _config = builder.Build();
    }
    
    [TestMethod]
    [DataRow("example10_tiny", "1", "0")]
    [DataRow("example10_small", "2", "0")]
    [DataRow("example10_unreachable9", "4", "0")]
    [DataRow("example10_multiple", "3", "0")]
    [DataRow("example10", "36", "0")]
    public async Task VerifyBasicsTest(string fileName, string expectedResultA, string expectedResultB)
    {
        var challenge = new Challenge10(_config);
        await challenge.ReadInput(fileName);

        var (a, b) = challenge.Calculate();
        Assert.AreEqual(expectedResultA, a);
        Assert.AreEqual(expectedResultB, b);
    }
}