using BenchmarkDotNet.Attributes;
using Sorting.Core.Generator;
using Sorting.Core.Generator.Models;

namespace Sorting.Benchmarks;

[MemoryDiagnoser]
public class FileGeneratorBenchmark
{
    private const int LineSize = 1000;
    private const int MaxLineIndexNumber = Int32.MaxValue;
    
    private readonly FileGenerator _fileGenerator = new(new RandomLineGenerator());
    
    
    [Benchmark]
    public async Task Generate_100MB()
    {
        const string filePath = "unsorted-100.txt";
        long fileSize = 100 * 1024 * 1024;
        await _fileGenerator.GenerateAsync(new FileGeneratorInput(filePath, fileSize, LineSize, MaxLineIndexNumber));
    }
    
    [Benchmark]
    public async Task Generate_10MB()
    {
        const string filePath = "unsorted-10.txt";
        long fileSize = 10 * 1024 * 1024;
        await _fileGenerator.GenerateAsync(new FileGeneratorInput(filePath, fileSize, LineSize, MaxLineIndexNumber));
    }
}