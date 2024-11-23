using System.Text;
using Sorting.Core.Generator.Models;

namespace Sorting.Core.Generator;

public interface ILineGenerator
{
    public string Generate(LineGeneratorInput input);
}

internal class RandomLineGenerator : ILineGenerator
{
    private const string Characters = "abcdefghijklmnoprstuwxyz";

    private readonly Random _random = new();
    
    public string Generate(LineGeneratorInput input)
    {
        var stringBuilder = new StringBuilder();

        var lineIndex = _random.Next(1, input.MaxLineIndexNumber);
        stringBuilder.Append(lineIndex);
        stringBuilder.Append(". ");
        
        var lineSize = _random.Next(1, input.LineSize);

        for (var i = 0; i < lineSize; i++)
        {
            var character = Characters[_random.Next(Characters.Length)];
            stringBuilder.Append(character);
        }

        return stringBuilder.ToString();
    }
}