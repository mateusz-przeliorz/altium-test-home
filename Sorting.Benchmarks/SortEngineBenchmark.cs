using BenchmarkDotNet.Attributes;
using Sorting.Core.Engine;
using Sorting.Core.Engine.Models;

namespace Sorting.Benchmarks;

[MemoryDiagnoser]
public class SortEngineBenchmark
{ 
    private const string Input = "benchmark-10-mb.txt";
    private const string Output = "benchmark-sorted-10-mb.txt";
    private const int DefaultBufferSize = 1024;
    private const int DefaultBatchSize = 15;
    
    private readonly SortEngine _sortEngine = new(new ChunksGenerator(), new ChunksMerger(), new LineComparer());

    [Benchmark]
    public async Task RunAsync_NumberOfBatches_5()
    {
        await _sortEngine.RunAsync(new SortEngineInput(Input, Output, 5, DefaultBufferSize));
    }
    
    [Benchmark]
    public async Task RunAsync_NumberOfBatches_10()
    {
        await _sortEngine.RunAsync(new SortEngineInput(Input, Output, 10, DefaultBufferSize));
    }
    
    [Benchmark]
    public async Task RunAsync_NumberOfBatches_15()
    {
        await _sortEngine.RunAsync(new SortEngineInput(Input, Output, 15, DefaultBufferSize));
    }
    
    [Benchmark]
    public async Task RunAsync_NumberOfBatches_20()
    {
        await _sortEngine.RunAsync(new SortEngineInput(Input, Output, 20, DefaultBufferSize));
    }
    
    [Benchmark]
    public async Task RunAsync_BufferSize_1024()
    {
        await _sortEngine.RunAsync(new SortEngineInput(Input, Output, DefaultBatchSize, 1024));
    }
    
    [Benchmark]
    public async Task RunAsync_BufferSize_2048()
    {
        await _sortEngine.RunAsync(new SortEngineInput(Input, Output, DefaultBatchSize, 2048));
    }
        
    [Benchmark]
    public async Task RunAsync_BufferSize_4096()
    {
        await _sortEngine.RunAsync(new SortEngineInput(Input, Output, DefaultBatchSize, 4096));
    }
        
    [Benchmark]
    public async Task RunAsync_BufferSize_8192()
    {
        await _sortEngine.RunAsync(new SortEngineInput(Input, Output, DefaultBatchSize, 8192));
    }
    
    [Benchmark]
    public void Run_NumberOfBatches_5()
    {
        _sortEngine.Run(new SortEngineInput(Input, Output, 5, DefaultBufferSize));
    }
    
    [Benchmark]
    public void Run_NumberOfBatches_10()
    {
        _sortEngine.Run(new SortEngineInput(Input, Output, 10, DefaultBufferSize));
    }
    
    [Benchmark]
    public void Run_NumberOfBatches_15()
    {
        _sortEngine.Run(new SortEngineInput(Input, Output, 15, DefaultBufferSize));
    }
    
    [Benchmark]
    public void Run_NumberOfBatches_20()
    {
        _sortEngine.Run(new SortEngineInput(Input, Output, 20, DefaultBufferSize));
    }
    
    [Benchmark]
    public void Run_BufferSize_1024()
    {
        _sortEngine.Run(new SortEngineInput(Input, Output, DefaultBatchSize, 1024));
    }
    
    [Benchmark]
    public void Run_BufferSize_2048()
    {
        _sortEngine.Run(new SortEngineInput(Input, Output, DefaultBatchSize, 2048));
    }
        
    [Benchmark]
    public void Run_BufferSize_4096()
    {
        _sortEngine.Run(new SortEngineInput(Input, Output, DefaultBatchSize, 4096));
    }
        
    [Benchmark]
    public void Run_BufferSize_8192()
    {
        _sortEngine.Run(new SortEngineInput(Input, Output, DefaultBatchSize, 8192));
    }
}