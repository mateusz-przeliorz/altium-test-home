namespace Sorting.Core.Generator.Models;

public record FileGeneratorInput(
    string OutputFilePath,
    long Bytes,
    int MaxLineSize,
    int MaxLineIndexNumber);