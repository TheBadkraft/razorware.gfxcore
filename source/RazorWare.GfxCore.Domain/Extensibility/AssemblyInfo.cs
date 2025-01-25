
using System.Reflection;
using System.Text.Json.Serialization;

using RazorWare.GfxCore.Serialization.Json;

namespace RazorWare.GfxCore.Extensibility;

/// <summary>
/// The assembly information for the extension.
/// </summary>
public class AssemblyInfo
{
    /// <summary>
    /// The name of the assembly.
    /// </summary>
    [JsonPropertyName("assembly_name")]
    [JsonConverter(typeof(AssemblyNameConverter))]
    public AssemblyName Name { get; init; } = new();
    /// <summary>
    /// The assembly identifier.
    /// </summary>
    [JsonPropertyName("entry_tag")]
    public string EntryTag { get; init; } = "GfxExtension";
    /// <summary>
    /// The file name of the assembly.
    /// </summary>
    [JsonPropertyName("file_name")]
    public string FileName { get; init; } = "GfxExtension.dll";

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
        return $"Assembly: {Name.Name} v{Name.Version} ({EntryTag} :: {FileName})";
    }
}
