namespace Sorting.Core.Engine.Models;

public record SortEngineInput(
    string FilePath,
    string OutputFilePath,
    int NumberOfBatches,
    int BufferSize);