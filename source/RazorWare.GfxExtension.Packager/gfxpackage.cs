
using RazorWare.GfxCore.Extensibility;
using RazorWare.GfxCore.Logging;
using RazorWare.GfxCore.Utilities;

const string EXECUTING_DIR = "./";

Console.WriteLine("Gfx Extension Packager!");

string path, file;
Config config = null;
Manifest manifest = null;

/*  1/22/2025
    Adding several features/enhancements
    - gfxconfig.json:
    [X] - set log_manifest true && log_file with a file name, the 
          packer will output all logging to that file
    [X] - set log_manifest true and no log_file, logging goes to 
          the console
    [ ] - read_csproj setting to read the .csproj file for assembly 
          version, etc.
    - gfxpackage.json:
    [ ] - adding AssemblyInfo class for extension class and 
          dependencies
    [ ] - AssemblyInfo will contain a string AssemblyName as well 
          as the package entry name
    ---------------------------------------------------------------
*/

switch (args[0])
{
    case "-p":
        Console.Write("Packing extension: ");
        //  TODO: singc ResolvePathArgs return whether the directory path exists, implement check
        _ = EXECUTING_DIR.ResolvePathArgs(out path, out file);
        Config.Load(path, out config);
        //  if log_manifest is true and log_file is set, log to file
        if (config.LogManifest && !string.IsNullOrEmpty(config.LogFile))
        {
            Packager.Logger = new FileLogger(config.LogFile);
        }
        Packager.Config = config;
        Packager.Log($"{config.Source} ...");
        Packager.LoadManifest(out manifest);
        Console.Write($"{manifest.Name} v{manifest.Version} ");

        var isPacked = Packager.Pack(manifest);

        if (isPacked)
        {
            Packager.Log("Packed successfully.");
            Packager.Log($"Manifest:\n{manifest}");
        }
        else
        {
            Packager.Log("Packing failed.");
        }

        Console.WriteLine($"\t[{(isPacked ? "SUCCESS" : "FAILURE")}]");

        break;
    case "-u":
        Console.WriteLine("Unpacking...");

        break;
    case "-g":  //  generate empty manifest
        //  TODO: singc ResolvePathArgs return whether the directory path exists, implement check
        _ = args.Length > 1 ? args[1].ResolvePathArgs(out path, out file) : string.Empty.ResolvePathArgs(out path, out file);
        Console.WriteLine($"Generating empty manifest: {Path.Combine(path, Manifest.PACKAGE_JSON)} ...");

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
