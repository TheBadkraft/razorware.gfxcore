using System.Text.Json;
using System.Text.Json.Serialization;

namespace RazorWare.GfxCore.Extensibility;

/// <summary>
/// The configuration for the packager.
/// </summary>
public class Config
{
    /// <summary>
    /// The source directory to search for files to pack.
    /// </summary>
    [JsonPropertyName("source")]
    public string Source { get; set; } = "./";
    /// <summary>
    /// The destination directory to pack files to.
    /// </summary>
    [JsonPropertyName("destination")]
    public string Destination { get; set; } = "./";
    /// <summary>
    /// Determine whether to log manifest information.
    /// </summary>
    /// <remarks>
    /// Log output to the console unless a log file is provided.
    /// </remarks>
    [JsonPropertyName("log_mainifest")]
    public bool LogManifest { get; set; } = true;
    /// <summary>
    /// [Optional] The log file to write log output to.
    /// </summary>
    [JsonPropertyName("log_file")]
    public string LogFile { get; set; } = string.Empty;
    /// <summary>
    /// Determine whether to autodetect dependencies on packing.
    /// </summary>
    [JsonPropertyName("autodetect_dependencies")]
    public bool AutodetectDependencies { get; set; } = false;

    /// <summary>
    /// Load the configuration from the specified path.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="config"></param>
    /// <exception cref="NotImplementedException"></exception>
    public static void Load(string path, out Config config)
    {
        //  if the path is empty, throw an exception
        if (string.IsNullOrEmpty(path))
        {
            throw new NotImplementedException($"The {nameof(path)} is empty.");
        }
        //  if the directory does not exist, throw an exception
        if (!Directory.Exists(path))
        {
            throw new NotImplementedException($"Directory ({path}) not found.");
        }
        //  load the json file from the path
        string json = File.ReadAllText(Path.Combine(path, Packager.CONFIG_JSON));
        //  materialize the config object
        config = JsonSerializer.Deserialize<Config>(json);
    }
}
