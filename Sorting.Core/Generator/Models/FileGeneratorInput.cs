namespace Sorting.Core.Generator.Models;

public record FileGeneratorInput(
    string FilePath,
    long Bytes,
    int LineSize,
    int MaxLineIndexNumber);