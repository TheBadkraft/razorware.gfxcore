
using System.IO.Compression;
using System.Reflection;
using System.Text.Json;
using RazorWare.GfxCore.Events;
using RazorWare.GfxCore.Registries;

namespace RazorWare.GfxCore.Extensibility;

public class ExtensionLoader
{
    private const string EXTENSION_PATH = "mods";

    private static string ext_path;

    //  the list of extensions
    private static readonly List<PackageManifest> _extPackages = new();

    private RegistryManager Registries { get; init; }

    private IEventPipeline Events { get; init; }

    /// <summary>
    /// Get the extension path.
    /// </summary>
    internal DirectoryInfo ExtensionPath => new DirectoryInfo(ext_path);


    public ExtensionLoader(RegistryManager registries)
    {
        Registries = registries;
        var registry = Registries.Resolve<ICommandTargetRegistry>();
        Events = registry.Resolve<IEventPipeline>();
    }

    /// <summary>
    /// Open the package and extract the manifest.
    /// </summary>
    /// <param name="pkgFile">The package file to open.</param>
    /// <param name="pkgManifest">The package manifest.</param>
    /// <returns>TRUE if the package was opened successfully, otherwise FALSE.</returns>
    public bool OpenPackage(FileInfo pkgFile, out PackageManifest pkgManifest)
    {
        //  obtain the package file stream
        using var fs = pkgFile.OpenRead();
        //  open the package archive
        using var archive = new ZipArchive(fs, ZipArchiveMode.Read, false);
        //  find the manifest file ("gfxpackage.json")
        var manifest_entry = archive.GetEntry(Manifest.PACKAGE_JSON);
        if (manifest_entry == null)
        {
            Log($"{"",13}Invalid Package :: Manifest Not Found");
            pkgManifest = null;
            return false;
        }
        //  deserialize the manifest file
        Manifest manifest = JsonSerializer.Deserialize<Manifest>(manifest_entry.Open());
        if (manifest == null)
        {
            Log($"{"",13}Invalid Package :: Manifest Invalid");
            pkgManifest = null;
            return false;
        }
        //  create the package manifest
        //  exclude the manifest file from the entries
        pkgManifest = new PackageManifest(manifest, archive.Entries.Where(e => e != manifest_entry));

        return true;
    }

    //  load the extensions - if testMode, only resolve dependencies,
    //  do not initialize the extensions
    internal void DiscoverExtensions()
    {
        Log($"[GfxCore :: Bootstrap] Begin Extension Discovery");

        var curr_dir = string.Empty;
        Log($"{"",5}{"Current Directory:",-25} {curr_dir = Directory.GetCurrentDirectory()}");
        Log($"{"",5}{"Extension Path:",-25} {ext_path = Path.Combine(curr_dir, EXTENSION_PATH)}");

        if (!Directory.Exists(ext_path))
        {
            Log($"{"",5}Creating Extension Path: {ext_path}");
            Directory.CreateDirectory(ext_path);
        }

        EnumerateExtensions();
        //  if no extensions found, we are done
        if (!_extPackages.Any())
        {
            Log($"{"",5}{"No Extensions Found",-25}");
            goto DiscoverComplete;
        }

    // LoadExtensions();

    DiscoverComplete:
        Log($"[GfxCore :: Bootstrap] Extension Discovery Complete");
    }

    //  iterate through the extension path:
    //   - the zip packages are the extensions
    //   - each package contains:
    //      - a manifest file (gfxpackage.json)
    //      - the actual extension assembly (dll)
    //      - any other resources required by the extension (e.g. images, assemblies, etc.)
    private void EnumerateExtensions()
    {
        Log($"{"",5}{"Enumerating Extensions:",-25}");
        //  not expecting directories ... just "*.zip" files
        foreach (var pkg in ExtensionPath.GetFiles("*.zip"))
        {
            Log($"{"",9}{"Found Package:",-25} {pkg.Name}");
            //  extract the package
            if (OpenPackage(pkg, out PackageManifest pkgManifest))
            {
                //  add the package to the list of extensions
                _extPackages.Add(pkgManifest);
                //  validate manifest file

            }
            //  check dependencies (if any)
            //  load the assembly
            //  ... and ...
            //  register assemblies (AssemblyRegistry)
            //  register extensions (ExtensionRegistry)
        }
    }

    private void Log(string message)
    {
        Events.Raise(new LogEvent(message));
    }
}
