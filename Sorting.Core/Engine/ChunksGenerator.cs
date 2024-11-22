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
    public const string ChunksDirectory = "chunks";
    
    public async Task<List<Chunk>> GenerateAsync(ChunksGeneratorInput input, IComparer<string> comparer)
    {
        if (!Directory.Exists(ChunksDirectory))
        {
            Directory.CreateDirectory(ChunksDirectory);
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
                var chunk = await CreateChunkAsync(chunkLines, chunks.Count, comparer);
                chunks.Add(chunk);
                
                chunkLines.Clear();
                totalBytes = 0;
            }
        }

        if (chunkLines.Count > 0)
        {
            var chunk = await CreateChunkAsync(chunkLines, chunks.Count, comparer);
            chunks.Add(chunk);
        }

        return chunks;
    }

    private async Task<Chunk> CreateChunkAsync(List<string> chunkLines, int chunkNumber, IComparer<string> comparer)
    {
        var sortedChunk = chunkLines
            .OrderBy(x => x, comparer)
            .ToList();
                
        var chunkFilePath = Path.Combine(ChunksDirectory, $"chunk_{chunkNumber}.txt");
        await File.WriteAllLinesAsync(chunkFilePath, sortedChunk);
        
        return new Chunk(chunkFilePath);
    }
    
    public List<Chunk> Generate(ChunksGeneratorInput input, IComparer<string> comparer)
    {
        if (!Directory.Exists(ChunksDirectory))
        {
            Directory.CreateDirectory(ChunksDirectory);
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
                var chunk = CreateChunk(chunkLines, chunks.Count, comparer);
                chunks.Add(chunk);
                
                chunkLines.Clear();
                totalBytes = 0;
            }
        }

        if (chunkLines.Count > 0)
        {
            var chunk = CreateChunk(chunkLines, chunks.Count, comparer);
            chunks.Add(chunk);
        }

        return chunks;
    }

    private Chunk CreateChunk(List<string> chunkLines, int chunkNumber, IComparer<string> comparer)
    {
        var sortedChunk = chunkLines
            .OrderBy(x => x, comparer)
            .ToList();
                
        var chunkFilePath = Path.Combine(ChunksDirectory, $"chunk_{chunkNumber}.txt");
        File.WriteAllLines(chunkFilePath, sortedChunk);
        
        return new Chunk(chunkFilePath);
    }
}