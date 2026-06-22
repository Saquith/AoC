using AdventOfCode2025.Challenges;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2025.UnitTests;

[TestClass]
public class Challenge3Tests
{
    private readonly IConfigurationRoot _config;

    public Challenge3Tests()
    {
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
        _config = builder.Build();
    }
    
    [TestMethod]
    [DataRow("3_example", "357", "3121910778619")]
    [DataRow("3_input", "16812", "166345822896410")]
    public async Task VerifyBasicsTest(string fileName, string expectedResultA, string expectedResultB)
    {
        var challenge = new Challenge3(_config);
        await challenge.ReadInput(fileName);

        var (a, b) = challenge.Calculate();
        Assert.AreEqual(expectedResultA, a);
        Assert.AreEqual(expectedResultB, b);
    }
}