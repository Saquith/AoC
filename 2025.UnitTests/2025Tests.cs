using AdventOfCodeLibrary.Challenges;
using Microsoft.Extensions.Configuration;

namespace AdventOfCodeLibrary.UnitTests;

[TestClass]
public class ChallengeTests
{
    private readonly IConfigurationRoot _config;

    public ChallengeTests()
    {
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
        _config = builder.Build();
    }
    
    [TestMethod]
    [DataRow("example", "357", "3121910778619")]
    [DataRow("input", "16812", "166345822896410")]
    public async Task VerifyChallenge3Test(string fileName, string expectedResultA, string expectedResultB)
    {
        var challenge = new Challenge3();
        var inputFilePath = Path.Combine(_config["InputFolderPath"]!, $"{challenge.CurrentClass.Substring("Challenge".Length)}/{fileName}.txt");
        
        await challenge.ReadInput(inputFilePath);

        var (a, b) = challenge.Calculate();
        Assert.AreEqual(expectedResultA, a);
        Assert.AreEqual(expectedResultB, b);
    }
    
    [TestMethod]
    [DataRow("example", "1227775554", "4174379265")]
    [DataRow("input", "19386344315", "34421651192")]
    public async Task VerifyChallenge2Test(string fileName, string expectedResultA, string expectedResultB)
    {
        var challenge = new Challenge2();
        var inputFilePath = Path.Combine(_config["InputFolderPath"]!, $"{challenge.CurrentClass.Substring("Challenge".Length)}/{fileName}.txt");
        
        await challenge.ReadInput(inputFilePath);

        var (a, b) = challenge.Calculate();
        Assert.AreEqual(expectedResultA, a);
        Assert.AreEqual(expectedResultB, b);
    }
    
    [TestMethod]
    [DataRow("example", "3", "6")]
    [DataRow("example2", "1", "10")]
    [DataRow("example3", "2", "2")]
    [DataRow("example4", "1", "1")]
    [DataRow("example5", "1", "1")]
    [DataRow("example6", "1", "2")]
    [DataRow("input", "1120", "6554")]
    public async Task VerifyChallenge1Test(string fileName, string expectedResultA, string expectedResultB)
    {
        var challenge = new Challenge1();
        var inputFilePath = Path.Combine(_config["InputFolderPath"]!, $"{challenge.CurrentClass.Substring("Challenge".Length)}/{fileName}.txt");
        
        await challenge.ReadInput(inputFilePath);

        var (a, b) = challenge.Calculate();
        Assert.AreEqual(expectedResultA, a);
        Assert.AreEqual(expectedResultB, b);
    }
}