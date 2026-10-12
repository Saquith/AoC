using AdventOfCode2024.Challenges;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2024.UnitTests;

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
    [DataRow("example12", "140", "80")]
    [DataRow("example12_same", "772", "436")]
    [DataRow("example12_large", "1930", "1206")]
    [DataRow("example12_input", "1304764", "811148")]
    [DataRow("example12_2_input", "692", "236")]
    [DataRow("example12_2_example", "1184", "368")]
    [DataRow("example12_large", "1930", "1206")]
    public async Task VerifyChallenge12Test(string fileName, string expectedResultA, string expectedResultB)
    {
        var challenge = new Challenge12();
        var inputFilePath = Path.Combine(_config["InputFolderPath"]!, $"{fileName}.txt");

        await challenge.ReadInput(inputFilePath);

        var (a, b) = challenge.Calculate();
        Assert.AreEqual(expectedResultA, a);
        Assert.AreEqual(expectedResultB, b);
    }
    
    [TestMethod]
    [DataRow("example11", "55312", "65601038650482")]
    public async Task VerifyChallenge11Test(string fileName, string expectedResultA, string expectedResultB)
    {
        var challenge = new Challenge11();
        var inputFilePath = Path.Combine(_config["InputFolderPath"]!, $"{fileName}.txt");

        await challenge.ReadInput(inputFilePath);

        var (a, b) = challenge.Calculate();
        Assert.AreEqual(expectedResultA, a);
        Assert.AreEqual(expectedResultB, b);
    }
    
    [TestMethod]
    [DataRow("example10_tiny", "1", "16")]
    [DataRow("example10_small", "2", "2")]
    [DataRow("example10_unreachable9", "4", "13")]
    [DataRow("example10_multiple", "3", "3")]
    [DataRow("example10", "36", "81")]
    [DataRow("example10_singleLowScore", "1", "3")]
    [DataRow("example10_singleHighScore", "2", "227")]
    public async Task VerifyChallenge10Test(string fileName, string expectedResultA, string expectedResultB)
    {
        var challenge = new Challenge10();
        var inputFilePath = Path.Combine(_config["InputFolderPath"]!, $"{fileName}.txt");

        await challenge.ReadInput(inputFilePath);

        var (a, b) = challenge.Calculate();
        Assert.AreEqual(expectedResultA, a);
        Assert.AreEqual(expectedResultB, b);
    }
    
    [TestMethod]
    [DataRow("example9", "1928", "2858")]
    public async Task VerifyChallenge9Test(string fileName, string expectedResultA, string expectedResultB)
    {
        var challenge = new Challenge9();
        var inputFilePath = Path.Combine(_config["InputFolderPath"]!, $"{fileName}.txt");

        await challenge.ReadInput(inputFilePath);

        var (a, b) = challenge.Calculate();
        Assert.AreEqual(expectedResultA, a);
        Assert.AreEqual(expectedResultB, b);
    }
    
    [TestMethod]
    [DataRow("example8", "14", "34")]
    public async Task VerifyChallenge8Test(string fileName, string expectedResultA, string expectedResultB)
    {
        var challenge = new Challenge8();
        var inputFilePath = Path.Combine(_config["InputFolderPath"]!, $"{fileName}.txt");

        await challenge.ReadInput(inputFilePath);

        var (a, b) = challenge.Calculate();
        Assert.AreEqual(expectedResultA, a);
        Assert.AreEqual(expectedResultB, b);
    }
    
    [TestMethod]
    [DataRow("example7", "3749", "11387")]
    public async Task VerifyChallenge7Test(string fileName, string expectedResultA, string expectedResultB)
    {
        var challenge = new Challenge7();
        var inputFilePath = Path.Combine(_config["InputFolderPath"]!, $"{fileName}.txt");

        await challenge.ReadInput(inputFilePath);

        var (a, b) = challenge.Calculate();
        Assert.AreEqual(expectedResultA, a);
        Assert.AreEqual(expectedResultB, b);
    }
    
    [TestMethod]
    [DataRow("box", "1", "0")]
    [DataRow("boxWithOpening", "2", "1")]
    public async Task VerifyChallenge6Test(string fileName, string expectedResultA, string expectedResultB)
    {
        var challenge = new Challenge6();
        var inputFilePath = Path.Combine(_config["InputFolderPath"]!, $"{fileName}.txt");

        await challenge.ReadInput(inputFilePath);

        var (a, b) = challenge.Calculate();
        Assert.AreEqual(expectedResultA, a);
        Assert.AreEqual(expectedResultB, b);
    }

    [TestMethod]
    [DataRow("example5", "143", "123")]
    [DataRow("input5", "4924", "6085")]
    public async Task VerifyChallenge5Test(string fileName, string expectedResultA, string expectedResultB)
    {
        var challenge = new Challenge5();
        var inputFilePath = Path.Combine(_config["InputFolderPath"]!, $"{fileName}.txt");
        
        await challenge.ReadInput(inputFilePath);
    
        var (a, b) = challenge.Calculate();
        Assert.AreEqual(expectedResultA, a);
        Assert.AreEqual(expectedResultB, b);
    }
    
    [TestMethod]
    [DataRow("example4", "18", "9")]
    [DataRow("input4", "2654", "1990")]
    public async Task VerifyChallenge4Test(string fileName, string expectedResultA, string expectedResultB)
    {
        var challenge = new Challenge4();
        var inputFilePath = Path.Combine(_config["InputFolderPath"]!, $"{fileName}.txt");
        
        await challenge.ReadInput(inputFilePath);
    
        var (a, b) = challenge.Calculate();
        Assert.AreEqual(expectedResultA, a);
        Assert.AreEqual(expectedResultB, b);
    }

    [TestMethod]
    [DataRow("example3", "161", "48")]
    [DataRow("input3", "174336360", "88802350")]
    public async Task VerifyChallenge3Test(string fileName, string expectedResultA, string expectedResultB)
    {
        var challenge = new Challenge3();
        var inputFilePath = Path.Combine(_config["InputFolderPath"]!, $"{fileName}.txt");
        
        await challenge.ReadInput(inputFilePath);
    
        var (a, b) = challenge.Calculate();
        Assert.AreEqual(expectedResultA, a);
        Assert.AreEqual(expectedResultB, b);
    }
    
    [TestMethod]
    [DataRow("example2", "2", "4")]
    [DataRow("input2", "624", "658")]
    public async Task VerifyChallenge2Test(string fileName, string expectedResultA, string expectedResultB)
    {
        var challenge = new Challenge2();
        var inputFilePath = Path.Combine(_config["InputFolderPath"]!, $"{fileName}.txt");
        
        await challenge.ReadInput(inputFilePath);
    
        var (a, b) = challenge.Calculate();
        Assert.AreEqual(expectedResultA, a);
        Assert.AreEqual(expectedResultB, b);
    }
    
    [TestMethod]
    [DataRow("example", "11", "31")]
    [DataRow("input", "765748", "27732508")]  
    public async Task VerifyChallenge1Test(string fileName, string expectedResultA, string expectedResultB)
    {
        var challenge = new Challenge1();
        var inputFilePath = Path.Combine(_config["InputFolderPath"]!, $"{fileName}.txt");
        
        await challenge.ReadInput(inputFilePath);
    
        var (a, b) = challenge.Calculate();
        Assert.AreEqual(expectedResultA, a);
        Assert.AreEqual(expectedResultB, b);
    }
}