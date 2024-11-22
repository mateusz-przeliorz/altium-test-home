// using Sorting.Core.Engine.Models;
// using Sorting.Core.IO;
//
// namespace Sorting.Core.Engine;
//
// public interface IChunksMerger
// {
//     void Merge(ChunksMergerInput input, IComparer<string> comparer);
// }
//
// internal class ChunksMerger : IChunksMerger
// {
//     public void Merge(ChunksMergerInput input, IComparer<string> comparer)
//     {
//         var chunkReaders = input.Chunks
//             .Select(chunk => new StreamReader(Reader.FileStream(chunk.FilePath)))
//             .ToList();
//         
//         using var outputFileStream = Writer.FileStream(input.OutputFilePath);
//         using var outputWriter = new StreamWriter(outputFileStream);
//         
//         var heap = new SortedDictionary<string, Queue<StreamReader>>(comparer);
//
//         foreach (var chunkReader in chunkReaders)
//         {
//             UpdateHeap(heap, chunkReader);
//         }
//
//         while (heap.Count > 0)
//         {
//             var currentLine = heap.Keys.First();
//             outputWriter.WriteLine(currentLine);
//             
//             var chunkReadersWithCurrentLine = heap[currentLine];
//             var chunkReaderWithCurrentLine = chunkReadersWithCurrentLine.Dequeue();
//             
//             UpdateHeap(heap, chunkReaderWithCurrentLine);
//             
//             if (chunkReadersWithCurrentLine.Count == 0)
//             {
//                 heap.Remove(currentLine);
//             }
//         }
//         
//         foreach (var chunk in input.Chunks)
//         {
//             File.Delete(chunk.FilePath);
//         }
//
//         foreach (var reader in chunkReaders)
//         {
//             reader.Dispose();
//         }
//     }
//
//     private void UpdateHeap(SortedDictionary<string, Queue<StreamReader>> heap, StreamReader chunkReader)
//     {
//         if (!chunkReader.EndOfStream)
//         {
//             var line = chunkReader.ReadLine()!;
//             if (!heap.ContainsKey(line))
//             {
//                 heap[line] = new Queue<StreamReader>();
//             }
//             heap[line].Enqueue(chunkReader);
//         }
//     }
// }