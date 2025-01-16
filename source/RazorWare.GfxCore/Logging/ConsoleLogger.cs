
using RazorWare.GfxCore.Extensibility;
using RazorWare.GfxCore.Extensibility.Logging;

namespace RazorWare.GfxCore.Logging;

/// <summary>
/// A basic console logger
/// </summary>
public class ConsoleLogger : ILogger, IGfxService
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
