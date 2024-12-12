using AdventOfCode2024.Challenges;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2024.UnitTests;

[TestClass]
public class Challenge12Tests
{
    private readonly IConfigurationRoot _config;

    public Challenge12Tests()
    {
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
        _config = builder.Build();
    }
    
    [TestMethod]
    [DataRow("example12", "140", "0")]
    [DataRow("example12_same", "772", "0")]
    [DataRow("example12_large", "1930", "0")]
    public async Task VerifyBasicsTest(string fileName, string expectedResultA, string expectedResultB)
    {
        var challenge = new Challenge12(_config);
        await challenge.ReadInput(fileName);

        var (a, b) = challenge.Calculate();
        Assert.AreEqual(expectedResultA, a);
        Assert.AreEqual(expectedResultB, b);
    }
}