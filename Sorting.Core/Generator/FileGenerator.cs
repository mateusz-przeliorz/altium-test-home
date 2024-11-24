using System.Text;
using Sorting.Core.Generator.Models;
using Sorting.Core.IO;

namespace Sorting.Core.Generator;

public interface IFileGenerator
{
    Task GenerateAsync(FileGeneratorInput input);
}

internal class FileGenerator : IFileGenerator
{
    private const int LinesBufferSize = 1024;
        
    private readonly ILineGenerator _lineGenerator;

    public FileGenerator(ILineGenerator lineGenerator)
    {
        _lineGenerator = lineGenerator;
    }
    
    public async Task GenerateAsync(FileGeneratorInput input)
    {
        Console.WriteLine("Generating file...");

        await using var fileStream = Writer.FileStream(input.OutputFilePath);
        await using var writer = new StreamWriter(fileStream);
        
        long totalBytes = 0;
        var linesBuffer = new StringBuilder(LinesBufferSize);

        while (totalBytes < input.Bytes)
        {
            var line = _lineGenerator.Generate();
            linesBuffer.AppendLine(line);

            totalBytes += Encoding.UTF8.GetByteCount(line + Environment.NewLine);
                
            if (linesBuffer.Length > LinesBufferSize)
            {
                await writer.WriteAsync(linesBuffer.ToString());
                linesBuffer.Clear();
            }
        }

        if (linesBuffer.Length > 0)
        {
            await writer.WriteAsync(linesBuffer.ToString());
            linesBuffer.Clear();
        }
        
        Console.WriteLine($"Saved file in {input.OutputFilePath}");
    }
}