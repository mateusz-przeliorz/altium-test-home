namespace Sorting.Core.Engine;

internal class LineComparer : IComparer<string>
{
    private const string Separator = ". ";
    
    public int Compare(string? x, string? y)
    {
        var xLineParts = SplitLine(x!);
        var yLineParts = SplitLine(y!);

        var stringComparison = string.Compare(xLineParts.String, yLineParts.String, StringComparison.OrdinalIgnoreCase);
        if (stringComparison != 0)
        {
            return stringComparison;
        }

        return xLineParts.Number.CompareTo(yLineParts.Number);
    }

    private static (int Number, string String) SplitLine(string line)
    {
        var parts = line.Split(Separator, 2);
        if (parts.Length != 2)
        {
            throw new FormatException($"Invalid format: line should contain number and string. Invalid line: {line}.");
        }

        if (!int.TryParse(parts[0], out var number))
        {
            throw new FormatException($"Invalid format: line does not start with a number. Invalid line: {line}.");
        }
        
        return (number, parts[1]);
    }
}