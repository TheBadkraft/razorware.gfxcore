
using RazorWare.GfxCore.Extensibility.Logging;

namespace RazorWare.GfxCore.Logging;

/// <summary>
/// A basic file logger.
/// </summary>
public class FileLogger : ILogger
{
    private readonly string _fileName;

    /// <summary>
    /// Create a new instance of the <see cref="FileLogger"/> class.
    /// </summary>
    /// <param name="fileName">The name of the log file.</param>
    /// <param name="truncate">Whether to truncate the log file. [Default] TRUE</param>
    /// <exception cref="ArgumentNullException"></exception>
    public FileLogger(string fileName, bool truncate = true)
    {
        _fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        using (StreamWriter writer = new StreamWriter(_fileName, !truncate))
        {
            //  write the header
            writer.WriteLine($"GfxCore Log: {DateTime.Now}");
        }
    }

    /// <summary>
    /// Log a message to the file.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public void Log(string message)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(_fileName, true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to log file: {ex.Message}");
        }
    }
}