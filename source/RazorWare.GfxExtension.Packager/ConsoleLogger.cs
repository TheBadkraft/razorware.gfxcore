using RazorWare.GfxCore.Extensibility.Logging;

namespace RazorWare.GfxPacker.Logging;

/// <summary>
/// A basic console logger
/// </summary>
public class ConsoleLogger : ILogger
{
    /// <summary>
    /// Get the name of the GfxService
    /// </summary>
    public string Name => "ConsoleLogger";

    /// <summary>
    /// Logs a message to the console
    /// </summary>
    /// <param name="message">The message to log.</param>
    public void Log(string message)
    {
        Console.WriteLine(message);
    }
}
