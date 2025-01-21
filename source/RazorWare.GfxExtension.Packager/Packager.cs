
using System.IO.Compression;
using System.Reflection;
using System.Text.Json;
using RazorWare.GfxCore.Registries;
using RazorWare.GfxCore.Utilities;

namespace RazorWare.GfxCore.Extensibility;

/// <summary>
/// The packager for the Gfx Extension.
/// </summary>
public static class Packager
{
    private static readonly AssemblyRegistry assemblies = new();

    internal const string CONFIG_JSON = "gfxconfig.json";
    internal const string PACKAGE_JSON = "gfxpackage.json";

    /// <summary>
    /// Generates an empty gfxpackage.json file.
    /// </summary>
    public static void GenerateEmptyManifest(string path)
    {
        path = Path.Combine(path, PACKAGE_JSON);

        //  generate empty gfxpackage.json
        Manifest manifest = new();
        string json = JsonSerializer.Serialize(manifest, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }
    /// <summary>
    /// Generates a default config gfxconfig.json file.
    /// </summary>
    internal static void GenerateDefaultConfig(string path, string file)
    {
        path = Path.Combine(path, (string.IsNullOrEmpty(file) ? CONFIG_JSON : file));

        //  generate default config gfxconfig.json
        Config config = new();
        string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }
    /// <summary>
    /// Pack the manifest.
    /// </summary>
    /// <param name="manifest">The manifest to pack.</param>
    internal static void Pack(Config config, Manifest manifest)
    {
        //  load assembly
        if (!Path.Combine(config.Source, $"{manifest.Assembly}.dll").ResolvePathArgs(out string sourcePath, out string file))
        {
            throw new FileNotFoundException($"Assembly not found: {manifest.Assembly}.dll");
        }
        var assembly = Assembly.LoadFrom(Path.Combine(sourcePath, file));

        if (config.AutodetectDependencies)
        {
            assemblies.RegisterDependencies(assembly);
            manifest.Dependencies.AddRange(assemblies.Select(asm => asm.Name).ToList());
            assemblies.Clear();
        }

        manifest.Packed = DateTime.UtcNow;
        var packedManifest = JsonSerializer.Serialize(manifest, new JsonSerializerOptions { WriteIndented = true });
        if (config.Destination.ResolvePathArgs(out string destPath, out _))
        {
            var outputPath = Path.Combine(destPath, $"{manifest.Name}.zip");
            using (var zipToOpen = new FileStream(outputPath, FileMode.Create))
            {
                using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
                {
                    //  add each dependency assembly
                    foreach (var dep in manifest.Dependencies)
                    {
                        var asmDependency = dep.EndsWith(".dll") ? dep : $"{dep}.dll";
                        var assemblySource = Path.Combine(sourcePath, asmDependency);
                        archive.CreateEntryFromFile(assemblySource, asmDependency);
                    }
                    //  add the extension assembly
                    var asmExtension = $"{manifest.Assembly}.dll";
                    var extensionSource = Path.Combine(sourcePath, asmExtension);
                    archive.CreateEntryFromFile(extensionSource, asmExtension);
                    //  add the packed manifest file
                    var manifestEntry = archive.CreateEntry(PACKAGE_JSON);
                    using (StreamWriter writer = new StreamWriter(manifestEntry.Open()))
                    {
                        writer.Write(packedManifest);
                    }
                }
            }
        }
        else
        {
            throw new InvalidOperationException($"Could not resolve {config.Destination}");
        }
    }
}
