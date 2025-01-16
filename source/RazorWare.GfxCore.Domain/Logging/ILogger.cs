
namespace RazorWare.GfxCore.Extensibility.Logging;

/// <summary>
/// Logger interface
/// </summary>
public interface ILogger
{
    /// <summary>
    /// Log message
    /// </summary>
    /// <param name="message">The message to log.</param>
    void Log(string message);
}
