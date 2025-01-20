
using System.Reflection;

namespace RazorWare.GfxCore.Extensibility;

/// <summary>
/// Information about a Gfx extension.
/// </summary>
public class GfxExtensionInfo
{
    /// <summary>
    /// Get the extension unique identifier.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();
    /// <summary>
    /// The extension directory.
    /// </summary>
    public DirectoryInfo ExtensionPath { get; init; }
    /// <summary>
    /// The extension assembly.
    /// </summary>
    public Assembly Assembly { get; internal set; } = null;
    /// <summary>
    /// The extension type found decorate with the <see cref="GfxExtensionAttribute"/>.
    /// </summary>
    public Type ExtType { get; internal set; } = null;
    /// <summary>
    /// The extension metadata.
    /// </summary>
    public GfxExtensionAttribute Metadata { get; internal set; } = null;

    #region overrides
    /// <summary>
    /// Determine if two extension infos are equal.
    /// </summary>
    /// <param name="obj">The object to compare.</param>
    /// <returns>TRUE if the extension infos are equal, otherwise FALSE.</returns>
    public override bool Equals(object obj)
    {
        if (obj is GfxExtensionInfo ext)
        {
            return Id == ext.Id;
        }

        return false;
    }
    /// <summary>
    /// Get the hash code for the extension info.
    /// </summary>
    /// <returns>The hash code for the extension info.</returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    /// <summary>
    /// Get the string representation of the extension info.
    /// </summary>
    /// <returns>The string representation of the extension info.</returns>
    public override string ToString()
    {
        return $"{ExtensionPath.Name}.{ExtType.Name}";
    }
    #endregion overrides
}
