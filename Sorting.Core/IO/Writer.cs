namespace Sorting.Core.IO;

public class Writer
{
    public static FileStream FileStream(string filePath) => 
        new(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
}