﻿using CommandLine;
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
        await fileGenerator.GenerateAsync(new FileGeneratorInput(g.FilePath, g.Bytes, g.MaxLineSize, g.MaxNumber));
    });

// await Parser.Default
//     .ParseArguments<GenerateOptions, SortOptions>(args)
//     .WithParsedAsync<SortOptions>(async s =>
//     {
//         await sortEngine.RunAsync(new SortEngineInput(s.FilePath, s.OutputFilePath, s.NumberOfBatches, s.BufferSize));
//     });
    
    
Parser.Default
    .ParseArguments<GenerateOptions, SortOptions>(args)
    .WithParsed<SortOptions>(s =>
    {
        sortEngine.Run(new SortEngineInput(s.FilePath, s.OutputFilePath, s.NumberOfBatches, s.BufferSize));
    });