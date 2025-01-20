
using System.Text.Json;

namespace RazorWare.GfxCore.Extensibility;

/// <summary>
/// The packager for the Gfx Extension.
/// </summary>
public static class Packager
{
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
}
