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
    /// The default package manifest file name.
    /// </summary>
    public const string PACKAGE_JSON = "gfxpackage.json";

    /// <summary>
    /// Get the package name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = "GfxExtension Name";
    /// <summary>
    /// Get the package version.
    /// </summary>
    [JsonPropertyName("version")]
    public string Version { get; set; } = "1.0.0";
    /// <summary>
    /// Get the package description.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = "A description for your extension";
    /// <summary>
    /// Get the package author.
    /// </summary>
    [JsonPropertyName("author")]
    public string Author { get; set; } = "Your name or organization";
    /// <summary>
    /// Get the package assembly.
    /// </summary>
    [JsonPropertyName("assembly")]
    public string Assembly { get; set; } = "AssemblyName";
    /// <summary>
    /// Get decorated the entry class name.
    /// </summary>
    /// <remarks>
    /// This class is decorated with the GfxExtension attribute.
    /// </remarks>
    [JsonPropertyName("entry_class")]
    public string EntryClass { get; set; } = "DecoratedClassName";
    /// <summary>
    /// Get the package dependencies.
    /// </summary>
    [JsonPropertyName("dependencies")]
    public List<string> Dependencies { get; set; } = new();
    /// <summary>
    /// Get the package assembly checksum.
    /// </summary>
    /// <remarks>
    /// Auto-generated
    /// </remarks>
    [JsonPropertyName("checksum")]
    public string Checksum { get; set; } = "";
    /// <summary>
    /// Get the package packed timestamp.
    /// </summary>
    /// <remarks>
    /// Auto-generated when pakcing the extension.
    /// </remarks>
    [JsonPropertyName("packed")]
    public DateTime Packed { get; set; } = new DateTime(1776, 7, 4);
    /// <summary>
    /// The package identifier.
    /// </summary>
    /// <remarks>
    /// Optional; used as a means to uniquely identify your extension.
    /// </remarks>
    [JsonPropertyName("identifier")]
    public string Identifier { get; set; } = "my_extension";
    /// <summary>
    /// Package tags.
    /// </summary>
    /// <remarks>
    /// Optional; used to categorize your extension.
    /// </remarks>
    [JsonPropertyName("tags")]
    public List<string> Tags { get; set; } = new();

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
