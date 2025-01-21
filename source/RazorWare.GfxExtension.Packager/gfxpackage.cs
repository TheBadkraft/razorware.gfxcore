
using RazorWare.GfxCore.Extensibility;
using RazorWare.GfxCore.Utilities;

const string EXECUTING_DIR = "./";

Console.WriteLine("Gfx Extension Packager!");

string path, file;
Config config = null;
Manifest manifest = null;

switch (args[0])
{
    case "-p":
        Console.Write("Packing extension: ");
        //  TODO: singc ResolvePathArgs return whether the directory path exists, implement check
        _ = EXECUTING_DIR.ResolvePathArgs(out path, out file);
        Config.Load(path, out config);
        Console.WriteLine($"{config.Source} ...");
        Manifest.Load(config, out manifest);
        Packager.Pack(config, manifest);

        Console.WriteLine($"{manifest}");

        break;
    case "-u":
        Console.WriteLine("Unpacking...");

        break;
    case "-g":  //  generate empty manifest
        //  TODO: singc ResolvePathArgs return whether the directory path exists, implement check
        _ = args.Length > 1 ? args[1].ResolvePathArgs(out path, out file) : string.Empty.ResolvePathArgs(out path, out file);
        Console.WriteLine($"Generating empty manifest: {Path.Combine(path, Packager.PACKAGE_JSON)} ...");

        Packager.GenerateEmptyManifest(path);

        break;
    case "-c":  //  generate default config
        //  TODO: singc ResolvePathArgs return whether the directory path exists, implement check
        _ = args.Length > 1 ? args[1].ResolvePathArgs(out path, out file) : "".ResolvePathArgs(out path, out file);
        Console.WriteLine($"Generating default config: {Path.Combine(path, (string.IsNullOrEmpty(file) ? Packager.CONFIG_JSON : file))} ...");

        Packager.GenerateDefaultConfig(path, file);

        break;
    case "-gU": //  update manifest at path (required)
        //  TODO: ... implement update manifest
        Console.WriteLine("Update manifest is not implemented.");

        break;
    case "-h":  //  output options
    default:
        Console.WriteLine("Options:");
        Console.WriteLine("  -p  Pack extension");
        Console.WriteLine("  -u  Unpack extension");
        Console.WriteLine("  -g [path] Generate empty manifest");
        Console.WriteLine("  : [path] (optional) path to generate manifest");
        Console.WriteLine("  -c [path] Generate default config");
        Console.WriteLine("  : [path] (optional) path to generate config");

        break;
}
