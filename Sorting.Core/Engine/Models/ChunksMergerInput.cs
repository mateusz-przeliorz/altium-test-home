namespace Sorting.Core.Engine.Models;

public record ChunksMergerInput(
    List<Chunk> Chunks,
    string OutputFilePath,
    string SortRunId);