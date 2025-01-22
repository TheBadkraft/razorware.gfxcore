
using System.IO.Compression;

namespace RazorWare.GfxCore.Extensibility;

/// <summary>
/// The manifest for a package.
/// </summary>
public class PackageManifest
{
    private readonly Manifest _manifest;
    private readonly List<ZipArchiveEntry> _entries = new();

    /// <summary>
    /// Get the extension info for the package.
    /// </summary>
    public GfxExtensionInfo Extension { get; init; } = new GfxExtensionInfo();

    /// <summary>
    /// Construct a new package manifest.
    /// </summary>
    /// <param name="manifest">The manifest for the package.</param>
    /// <param name="entries">The entries in the package.</param>
    public PackageManifest(Manifest manifest, IEnumerable<ZipArchiveEntry> entries)
    {
        _manifest = manifest;
        _entries.AddRange(entries);
    }

}