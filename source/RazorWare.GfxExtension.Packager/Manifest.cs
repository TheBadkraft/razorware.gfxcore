using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using RazorWare.GfxCore.Utilities;

namespace RazorWare.GfxCore.Extensibility;

/// <summary>
/// The manifest for the extension package.
/// </summary>
public class Manifest
{
    /// <summary>
    /// Get the package name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }
    /// <summary>
    /// Get the package version.
    /// </summary>
    [JsonPropertyName("version")]
    public string Version { get; set; }
    /// <summary>
    /// Get the package description.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; }
    /// <summary>
    /// Get the package author.
    /// </summary>
    [JsonPropertyName("author")]
    public string Author { get; set; }
    /// <summary>
    /// Get the package assembly.
    /// </summary>
    [JsonPropertyName("assembly")]
    public string Assembly { get; set; }
    /// <summary>
    /// Get the package dependencies.
    /// </summary>
    [JsonPropertyName("dependencies")]
    public List<string> Dependencies { get; set; } = new();
    /// <summary>
    /// Get the package assembly checksum.
    /// </summary>
    [JsonPropertyName("checksum")]
    public string Checksum { get; set; }
    /// <summary>
    /// Get the package packed timestamp.
    /// </summary>
    [JsonPropertyName("packed")]
    public DateTime Packed { get; set; }

    /// <summary>
    /// Load the manifest from the specified configuration.
    /// </summary>
    /// <param name="config">The configuration to load the manifest from.</param>
    /// <param name="manifest">The manifest to load.</param>
    public static void Load(Config config, out Manifest manifest)
    {
        //  does the config ext path exist?
        if (!$"{config.Source}/{Packager.PACKAGE_JSON}".ResolvePathArgs(out string path, out string file))
        {
            throw new DirectoryNotFoundException($"Extension path not found: {config.Source}");
        }

        //  load the json file from the path
        file = Path.Combine(path, file);
        string json = File.ReadAllText(file);
        //  materialize the manifest
        manifest = JsonSerializer.Deserialize<Manifest>(json);
    }

    /// <summary>
    /// Get the string representation of the manifest information.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{"",5}{"Name",-12}: {Name}");
        sb.AppendLine($"{"",5}{"Packed",-12}: {Packed}");
        sb.AppendLine($"{"",5}{"Version",-12}: {Version}");
        sb.AppendLine($"{"",5}{"Description",-12}: {Description}");
        sb.AppendLine($"{"",5}{"Author",-12}: {Author}");
        sb.AppendLine($"{"",5}{"Assembly",-12}: {Assembly}");
        sb.AppendLine($"{"",5}{"Dependencies",-12}:");
        foreach (var dep in Dependencies)
        {
            sb.AppendLine($"{"",9}{dep}");
        }
        sb.AppendLine($"{"",5}{"Checksum",-12}: {Checksum}");

        return sb.ToString();
    }
}
