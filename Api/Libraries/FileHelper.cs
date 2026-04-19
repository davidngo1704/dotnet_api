namespace Api.Libraries;

public class FileHelper
{
    public static void WriteText(string path, string text)
    {
        var directory = Path.GetDirectoryName(path);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory!);
        }

        File.WriteAllText(path, text);
    }
    public static string ReadFromFile(string filePath)
    {
        try
        {
            return File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            // Handle exceptions as needed
            Console.WriteLine($"Error reading from file: {ex.Message}");
            return string.Empty;
        }
    }
}
