using Sorting.Core.Engine;
using Sorting.Core.Engine.Models;

namespace Sorting.Core.Tests.Engine;

[TestFixture]
public class SortEngineTests
{
    private const string TestDataPath = "../../../TestData";
    private const string InputFilePath = $"{TestDataPath}/unsorted.txt";
    private const string OutputFilePath = $"{TestDataPath}/sorted.txt";
    
    private string _expectedResult;

    private SortEngine _sut;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _expectedResult = GetResult($"{TestDataPath}/expected-sorted.txt");
        
        _sut = new SortEngine(new ChunksGenerator(), new ChunksMerger(), new LineComparer());
    }
    
    [TestCase(1)]
    [TestCase(10)]
    [TestCase(25)]
    public async Task Run_BatchSizeSpecified_ShouldSortFileCorrectly(int batchSize)
    {
        // Act
        await _sut.RunAsync(new SortEngineInput(InputFilePath, OutputFilePath, batchSize, 1024));
        
        // Assert
        var actualResult = GetResult(OutputFilePath);
        Assert.That(actualResult, Is.EqualTo(_expectedResult));
    }
    
    [TestCase(1024)]
    [TestCase(2048)]
    [TestCase(4196)]
    public async Task Run_BufferSizeSpecified_ShouldSortFileCorrectly(int bufferSize)
    {
        // Act
        await _sut.RunAsync(new SortEngineInput(InputFilePath, OutputFilePath, 15, bufferSize));

        // Assert
        var actualResult = GetResult(OutputFilePath);
        Assert.That(actualResult, Is.EqualTo(_expectedResult));
    }

    private static string GetResult(string filePath)
    {
        using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
        using var reader = new StreamReader(fileStream);

        return reader.ReadToEnd();
    }
}