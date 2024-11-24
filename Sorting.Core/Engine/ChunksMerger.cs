using System.Diagnostics;
using Sorting.Core.Engine.Models;
using Sorting.Core.IO;

namespace Sorting.Core.Engine;

public interface IChunksMerger
{
    Task MergeAsync(ChunksMergerInput input, IComparer<string> comparer);
    void Merge(ChunksMergerInput input, IComparer<string> comparer);
}

internal class ChunksMerger : IChunksMerger
{
    public async Task MergeAsync(ChunksMergerInput input, IComparer<string> comparer)
    {
        Console.WriteLine($"Merging chunks for run {input.SortRunId}...");
        
        var chunkReaders = input.Chunks
            .Select(chunk => new StreamReader(Reader.FileStream(chunk.FilePath)))
            .ToList();

        await using var outputFileStream = Writer.FileStream(input.OutputFilePath);
        await using var outputWriter = new StreamWriter(outputFileStream);
        
        var minLinesHeap = new PriorityQueue<StreamReader, string>(comparer);

        foreach (var chunkReader in chunkReaders)
        {
            var firstLine = await chunkReader.ReadLineAsync();
            minLinesHeap.Enqueue(chunkReader, firstLine!);
        }

        while (minLinesHeap.Count > 0)
        {
            if (minLinesHeap.TryDequeue(out var chunkReader, out var line))
            {
                await outputWriter.WriteLineAsync(line);
                
                if (!chunkReader.EndOfStream)
                {
                    var nextLine = await chunkReader.ReadLineAsync();
                    minLinesHeap.Enqueue(chunkReader, nextLine!);
                }
            }
        }
        
        foreach (var reader in chunkReaders)
        {
            reader.Dispose();
        }

        Directory.Delete(input.SortRunId, true);
    }
    
    public void Merge(ChunksMergerInput input, IComparer<string> comparer)
    {
        Console.WriteLine($"Merging chunks for run {input.SortRunId}...");
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var chunkReaders = input.Chunks
            .Select(chunk => new StreamReader(Reader.FileStream(chunk.FilePath)))
            .ToList();

        using var outputFileStream = Writer.FileStream(input.OutputFilePath);
        using var outputWriter = new StreamWriter(outputFileStream);
        
        var minLinesHeap = new PriorityQueue<StreamReader, string>(comparer);

        foreach (var chunkReader in chunkReaders)
        {
            var firstLine = chunkReader.ReadLine();
            minLinesHeap.Enqueue(chunkReader, firstLine!);
        }

        while (minLinesHeap.Count > 0)
        {
            if (minLinesHeap.TryDequeue(out var chunkReader, out var line))
            { 
                outputWriter.WriteLine(line);
                
                if (!chunkReader.EndOfStream)
                {
                    var nextLine = chunkReader.ReadLine();
                    minLinesHeap.Enqueue(chunkReader, nextLine!);
                }
            }
        }

        stopwatch.Stop();
        
        Console.WriteLine("Merging chunks elapsed Time: " + stopwatch.Elapsed.ToString(@"hh\:mm\:ss"));
        
        foreach (var reader in chunkReaders)
        {
            reader.Dispose();
        }
        
        Directory.Delete(input.SortRunId, true);
    }
}