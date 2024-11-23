namespace Sorting.Core.Engine.Models;

public record ChunksGeneratorInput(
    string FilePath,
    int NumberOfBatches,
    int BufferSize,
    string SortRunId);