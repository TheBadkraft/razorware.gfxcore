
using System.IO.Compression;
using System.Reflection;
using RazorWare.GfxCore.Registries;

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
    /// Determine if the package has errors.
    /// </summary>
    public bool HasErrors { get; private set; }

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

    /// <summary>
    /// Validate the assemblies in the package.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void ValidateAssemblies(IAssemblyRegistry assemblies)
    {
        var extAssembly = _manifest.Assembly;
        //  validate dependency assemblies first
        foreach (var entry in _entries.Where(a => a.FullName != extAssembly && a.FullName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)))
        {
            //  get the assembly name only because that is what the AssemblyRegistry uses
            // Assuming you have a stream that contains the assembly data
            using (Stream assemblyStream = entry.Open())
            {
                try
                {
                    // Get the AssemblyName from the stream
                    AssemblyName assemblyName = new();

                    // Print the name of the assembly
                    Console.WriteLine($"Assembly Name: {assemblyName.Name}");
                    Console.WriteLine($"Assembly Version: {assemblyName.Version}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}