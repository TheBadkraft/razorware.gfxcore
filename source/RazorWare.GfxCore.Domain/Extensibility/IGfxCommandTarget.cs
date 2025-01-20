
namespace RazorWare.GfxCore.Extensibility;

/// <summary>
/// The base interface for a GfxCommandTarget.
/// </summary>
public interface IGfxCommandTarget
{
    /// <summary>
    /// Get the command target name.
    /// </summary>
    string Name { get; }
}