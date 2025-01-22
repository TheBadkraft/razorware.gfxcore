using System;

namespace RazorWare.GfxCore.Extensibility;

/// <summary>
/// Represents a GfxCore extension interface.
/// </summary>
public interface IGfxExtension
{
    /// <summary>
    /// The extension name.
    /// </summary>
    string Name { get; }
    /// <summary>
    /// The extension version
    /// </summary>
    Version Version { get; }
    /// <summary>
    /// Collection of required extensions
    /// </summary>
    List<GfxExtensionInfo> Requires { get; }
}
