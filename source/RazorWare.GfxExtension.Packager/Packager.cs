
using System.IO.Compression;
using System.Reflection;
using System.Text.Json;

using RazorWare.GfxCore.Extensibility.Logging;
using RazorWare.GfxCore.Registries;
using RazorWare.GfxCore.Utilities;

using RazorWare.GfxPacker.Logging;

namespace RazorWare.GfxCore.Extensibility;

/// <summary>
/// The packager for the Gfx Extension.
/// </summary>
public static class Packager
{
    private static readonly AssemblyRegistry assemblies = new();

    internal const string CONFIG_JSON = "gfxconfig.json";

    internal static ILogger Logger { get; set; } = new ConsoleLogger();

    public static Config Config { get; internal set; }

    /// <summary>
    /// Generates an empty gfxpackage.json file.
    /// </summary>
    public static void GenerateEmptyManifest(string path)
    {
        path = Path.Combine(path, Manifest.PACKAGE_JSON);

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
    /// <returns>TRUE if the manifest was packed successfully; otherwise, FALSE.</returns>
    internal static bool Pack(Manifest manifest)
    {
        //  load assembly
        if (!Path.Combine(Config.Source, $"{manifest.Assembly.FileName}").ResolvePathArgs(out string sourcePath, out string file))
        {
            throw new FileNotFoundException($"Assembly not found: {manifest.Assembly}.dll");
        }
        var assembly = Assembly.LoadFrom(Path.Combine(sourcePath, file));

        if (Config.AutodetectDependencies)
        {
            assemblies.RegisterDependencies(assembly, out var deps);
            manifest.Dependencies.AddRange(deps.Select(d => new AssemblyInfo { Name = d, EntryTag = d.Name, FileName = Path.GetFileName(Assembly.Load(d).Location) }));
            assemblies.Clear();
        }

        manifest.Packed = DateTime.UtcNow;
        var packedManifest = JsonSerializer.Serialize(manifest, new JsonSerializerOptions { WriteIndented = true });
        if (Config.Destination.ResolvePathArgs(out string destPath, out _))
        {
            var outputPath = Path.Combine(destPath, $"{manifest.Name}.zip");
            using (var zipToOpen = new FileStream(outputPath, FileMode.Create))
            {
                using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
                {
                    //  add each dependency assembly
                    foreach (var dep in manifest.Dependencies)
                    {
                        var asmDependency = dep.FileName;
                        var assemblySource = Path.Combine(sourcePath, asmDependency);
                        archive.CreateEntryFromFile(assemblySource, dep.EntryTag);
                    }
                    //  add the extension assembly
                    var extensionSource = Path.Combine(sourcePath, manifest.Assembly.FileName);
                    archive.CreateEntryFromFile(extensionSource, manifest.Assembly.EntryTag);
                    //  add the packed manifest file
                    var manifestEntry = archive.CreateEntry(Manifest.PACKAGE_JSON);
                    using (StreamWriter writer = new StreamWriter(manifestEntry.Open()))
                    {
                        writer.Write(packedManifest);
                    }
                }
            }
        }
        else
        {
            Log($"Could not resolve {Config.Destination}");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Load the manifest from the specified configuration.
    /// </summary>
    /// <param name="config">The configuration to load the manifest from.</param>
    /// <param name="manifest">The manifest to load.</param>
    public static void LoadManifest(out Manifest manifest)
    {
        //  does the config ext path exist?
        if (!$"{Config.Source}/{Manifest.PACKAGE_JSON}".ResolvePathArgs(out string path, out string file))
        {
            throw new DirectoryNotFoundException($"Extension path not found: {Config.Source}");
        }

        //  load the json file from the path
        file = Path.Combine(path, file);
        string json = File.ReadAllText(file);
        //  materialize the manifest
        manifest = JsonSerializer.Deserialize<Manifest>(json);

        //  are we reading the .csproj file?
        if (Config.ReadProject)
        {
            var projDir = Path.GetFullPath(Path.Combine(path, "../../../"));
            if (!Project.Load(projDir, manifest))
            {
                Log("Could not load project file.");
            }
        }
    }

    internal static void Log(string logMessage)
    {
        if (Config.LogManifest)
        {
            Logger.Log(logMessage);
        }
    }
}
