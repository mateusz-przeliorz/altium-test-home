using CommandLine;

namespace Sorting.Console;

[Verb("generate", HelpText = "Generate file")]
public class GenerateOptions
{
    private const string DefaultFilePath = "unsorted.txt";
    private const long DefaultSizeInBytes = 1000000000;
    
    [Option('b', "bytes", Required = false, Default = DefaultSizeInBytes, HelpText = "Size in bytes.")]
    public long Bytes { get; set; }
    
    [Option('o', "output-file-path", Required = false, Default = DefaultFilePath, HelpText = "Expected output file path.")]
    public string OutputFilePath { get; set; } = null!;
}

[Verb("sort", HelpText = "Sort file")]
public class SortOptions
{
    [Option('i', "input-file-path", Required = true, HelpText = "Unsorted input file path.")]
    public string FilePath { get; set; } = null!;
    
    [Option('o', "output", Required = false, Default = "sorted.txt", HelpText = "Sorted output file path.")]
    public string OutputFilePath { get; set; } = null!;
    
    [Option('b', "batches", Required = false, Default = 10, HelpText = "Number of batches.")]
    public int NumberOfBatches { get; set; }
    
    [Option('s', "buffer-size", Required = false, Default = 2048, HelpText = "Buffer size.")]
    public int BufferSize { get; set; }
}