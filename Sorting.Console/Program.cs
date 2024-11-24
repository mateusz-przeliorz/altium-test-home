using CommandLine;
using Sorting.Console;
using Sorting.Core.Engine;
using Sorting.Core.Engine.Models;
using Sorting.Core.Generator;
using Sorting.Core.Generator.Models;

ILineGenerator lineGenerator = new RandomLineGenerator();
IFileGenerator fileGenerator = new FileGenerator(lineGenerator);

IChunksGenerator chunksGenerator = new ChunksGenerator();
IChunksMerger chunksMerger = new ChunksMerger();
IComparer<string> comparer = new LineComparer();
ISortEngine sortEngine = new SortEngine(chunksGenerator, chunksMerger, comparer);

await Parser.Default
    .ParseArguments<GenerateOptions, SortOptions>(args)
    .WithParsedAsync<GenerateOptions>(async g =>
    {
        await fileGenerator.GenerateAsync(new FileGeneratorInput(g.OutputFilePath, g.Bytes));
    });

// Due to huge amount of I/O operations async approach seems to be slower.
// Very likely due to tasks based context switching. I have decided to keep async implementation
// in case the application would be used by multiple users, and non-blocking async approach is necessary.

// I could not achieve expected results 1GB/1min for files with line length lower than 2000 characters.
// Tested on SSD: ~30 seconds (line length ~2000 characters)
// Tested on HDD: ~1 minute (line length ~2000 characters)

Parser.Default
    .ParseArguments<GenerateOptions, SortOptions>(args)
    .WithParsed<SortOptions>(s =>
    {
        sortEngine.Run(new SortEngineInput(s.FilePath, s.OutputFilePath, s.NumberOfBatches, s.BufferSize));
    });