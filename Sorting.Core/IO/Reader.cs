namespace Sorting.Core.IO;

internal class Reader
{
    public static FileStream FileStream(string filePath) => 
        new(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
}