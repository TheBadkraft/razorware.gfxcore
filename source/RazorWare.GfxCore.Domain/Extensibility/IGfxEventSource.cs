
namespace RazorWare.GfxCore.Extensibility;

/// <summary>
/// The base interface for a GfxSystem.
/// </summary>
public interface IGfxEventSource
{
    /// <summary>
    /// Get the event source name.
    /// </summary>
    string Name { get; }
}
