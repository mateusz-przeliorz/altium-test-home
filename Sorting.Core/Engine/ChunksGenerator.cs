using System.Text;
using Sorting.Core.Engine.Models;
using Sorting.Core.IO;

namespace Sorting.Core.Engine;

public interface IChunksGenerator
{
    Task<List<Chunk>> GenerateAsync(ChunksGeneratorInput input, IComparer<string> comparer);
    List<Chunk> Generate(ChunksGeneratorInput input, IComparer<string> comparer);
}

internal class ChunksGenerator : IChunksGenerator
{
    private const string ChunksDirectory = "chunks";
    
    public async Task<List<Chunk>> GenerateAsync(ChunksGeneratorInput input, IComparer<string> comparer)
    {
        Console.WriteLine($"Generating chunks for run {input.SortRunId}...");

        var chunksPath = Path.Combine(input.SortRunId, ChunksDirectory);
        if (!Directory.Exists(chunksPath))
        {
            Directory.CreateDirectory(chunksPath);
        }
        
        var chunks = new List<Chunk>();
        var chunkLines = new List<string>();
        var totalBytes = 0;
        
        var fileSizeInBytes = new FileInfo(input.FilePath).Length;
        var batchSize = fileSizeInBytes / input.NumberOfBatches;

        await using var fileStream = Reader.FileStream(input.FilePath);
        using var reader = new StreamReader(fileStream, bufferSize: input.BufferSize);
        
        while (await reader.ReadLineAsync() is { } line)
        {
            totalBytes += Encoding.UTF8.GetByteCount(line);
                
            chunkLines.Add(line);
                
            if (totalBytes > batchSize)
            {
                var chunk = await CreateChunkAsync(chunkLines, chunksPath, chunks.Count, comparer);
                chunks.Add(chunk);
                
                chunkLines.Clear();
                totalBytes = 0;
            }
        }

        if (chunkLines.Count > 0)
        {
            var chunk = await CreateChunkAsync(chunkLines, chunksPath, chunks.Count, comparer);
            chunks.Add(chunk);
        }

        return chunks;
    }

    private async Task<Chunk> CreateChunkAsync(List<string> chunkLines, string chunksPath,
        int chunkNumber, IComparer<string> comparer)
    {
        var sortedChunk = chunkLines
            .OrderBy(x => x, comparer)
            .ToList();
                
        var chunkFilePath = Path.Combine(chunksPath, $"chunk_{chunkNumber}.txt");
        await File.WriteAllLinesAsync(chunkFilePath, sortedChunk);
        
        return new Chunk(chunkFilePath);
    }
    
    public List<Chunk> Generate(ChunksGeneratorInput input, IComparer<string> comparer)
    {
        Console.WriteLine($"Generating chunks for run {input.SortRunId}...");

        var chunksPath = Path.Combine(input.SortRunId, ChunksDirectory);
        if (!Directory.Exists(chunksPath))
        {
            Directory.CreateDirectory(chunksPath);
        }
        
        var chunks = new List<Chunk>();
        var chunkLines = new List<string>();
        var totalBytes = 0;
        
        var fileSizeInBytes = new FileInfo(input.FilePath).Length;
        var batchSize = fileSizeInBytes / input.NumberOfBatches;

        using var fileStream = Reader.FileStream(input.FilePath);
        using var reader = new StreamReader(fileStream, bufferSize: input.BufferSize);
        
        while (reader.ReadLine() is { } line)
        {
            totalBytes += Encoding.UTF8.GetByteCount(line);
                
            chunkLines.Add(line);
                
            if (totalBytes > batchSize)
            {
                var chunk = CreateChunk(chunkLines, chunksPath, chunks.Count, comparer);
                chunks.Add(chunk);
                
                chunkLines.Clear();
                totalBytes = 0;
            }
        }

        if (chunkLines.Count > 0)
        {
            var chunk = CreateChunk(chunkLines, chunksPath, chunks.Count, comparer);
            chunks.Add(chunk);
        }

        return chunks;
    }

    private Chunk CreateChunk(List<string> chunkLines, string chunksPath, int chunkNumber, IComparer<string> comparer)
    {
        var sortedChunk = chunkLines
            .OrderBy(x => x, comparer)
            .ToList();
                
        var chunkFilePath = Path.Combine(chunksPath, $"chunk_{chunkNumber}.txt");
        File.WriteAllLines(chunkFilePath, sortedChunk);
        Console.WriteLine(chunkFilePath);
        return new Chunk(chunkFilePath);
    }
}