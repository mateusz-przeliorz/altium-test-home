using System.Diagnostics;
using Sorting.Core.Engine.Models;

namespace Sorting.Core.Engine;

public interface ISortEngine
{
    Task RunAsync(SortEngineInput input);
    void Run(SortEngineInput input);
}

internal class SortEngine : ISortEngine
{
    private readonly Guid _sortRunId = Guid.NewGuid();
    
    private readonly IChunksGenerator _chunksGenerator;
    private readonly IChunksMerger _chunksMerger;
    private readonly IComparer<string> _comparer;
    
    public SortEngine(
        IChunksGenerator chunksGenerator,
        IChunksMerger chunksMerger,
        IComparer<string> comparer)
    {
        _chunksGenerator = chunksGenerator;
        _chunksMerger = chunksMerger;
        _comparer = comparer;
    }
    
    public async Task RunAsync(SortEngineInput input)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        
        var sortedChunks = await _chunksGenerator.GenerateAsync(
            new ChunksGeneratorInput(input.FilePath, input.NumberOfBatches, input.BufferSize, _sortRunId.ToString()),
            _comparer);
        
        await _chunksMerger.MergeAsync(
            new ChunksMergerInput(sortedChunks, input.OutputFilePath, _sortRunId.ToString()),
            _comparer);

        stopwatch.Stop();
        
        Console.WriteLine("Elapsed Time: " + stopwatch.Elapsed.ToString(@"hh\:mm\:ss"));
        Console.WriteLine($"Saved file in {input.OutputFilePath}.");
    }
    
    public void Run(SortEngineInput input)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        
        var sortedChunks = _chunksGenerator.Generate(
            new ChunksGeneratorInput(input.FilePath, input.NumberOfBatches, input.BufferSize, _sortRunId.ToString()),
            _comparer);
        
        _chunksMerger.Merge(
            new ChunksMergerInput(sortedChunks, input.OutputFilePath, _sortRunId.ToString()),
            _comparer);

        stopwatch.Stop();
        
        Console.WriteLine("Elapsed Time: " + stopwatch.Elapsed.ToString(@"hh\:mm\:ss"));
        Console.WriteLine($"Saved file in {input.OutputFilePath}.");
    }
}