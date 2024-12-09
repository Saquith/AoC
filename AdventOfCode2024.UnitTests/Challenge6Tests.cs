using AdventOfCode2024.challenges;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2024.UnitTests;

[TestClass]
public class Challenge6Tests
{
    private readonly IConfigurationRoot _config;

    public Challenge6Tests()
    {
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
        _config = builder.Build();
    }
    
    [TestMethod]
    [DataRow("box", "1", "0")]
    [DataRow("boxWithOpening", "2", "1")]
    public async Task VerifyBasicsTest(string fileName, string expectedResultA, string expectedResultB)
    {
        var challenge = new Challenge6(_config);
        await challenge.ReadInput(fileName);

        var (a, b) = challenge.Calculate();
        Assert.AreEqual(expectedResultA, a);
        Assert.AreEqual(expectedResultB, b);
    }
}