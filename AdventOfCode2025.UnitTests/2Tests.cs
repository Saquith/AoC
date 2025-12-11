using AdventOfCode2025.Challenges;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2025.UnitTests;

[TestClass]
public class Challenge2Tests
{
    private readonly IConfigurationRoot _config;

    public Challenge2Tests()
    {
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
        _config = builder.Build();
    }
    
    [TestMethod]
    [DataRow("2_example", "1227775554", "4174379265")]
    [DataRow("2_input", "19386344315", "34421651192")]
    public async Task VerifyBasicsTest(string fileName, string expectedResultA, string expectedResultB)
    {
        var challenge = new Challenge2(_config);
        await challenge.ReadInput(fileName);

        var (a, b) = challenge.Calculate();
        Assert.AreEqual(expectedResultA, a);
        Assert.AreEqual(expectedResultB, b);
    }
}