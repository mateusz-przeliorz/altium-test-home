using System.Text;
using Sorting.Core.Generator.Models;

namespace Sorting.Core.Generator;

public interface ILineGenerator
{
    public string Generate();
}

internal class RandomLineGenerator : ILineGenerator
{
    private const string Characters = "abcdefghijklmnoprstuwxyz";
    private const int LineSize = 2000;

    private readonly Random _random = new();
    
    public string Generate()
    {
        var stringBuilder = new StringBuilder();

        var lineIndex = _random.Next(1, Int32.MaxValue);
        stringBuilder.Append(lineIndex);
        stringBuilder.Append(". ");
        
        for (var i = 0; i < LineSize; i++)
        {
            var character = Characters[_random.Next(Characters.Length)];
            stringBuilder.Append(character);
        }

        return stringBuilder.ToString();
    }
}