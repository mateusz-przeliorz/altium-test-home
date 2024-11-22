using BenchmarkDotNet.Running;
using Sorting.Benchmarks;

BenchmarkRunner.Run<SortEngineBenchmark>();
// BenchmarkRunner.Run<FileGeneratorBenchmark>();