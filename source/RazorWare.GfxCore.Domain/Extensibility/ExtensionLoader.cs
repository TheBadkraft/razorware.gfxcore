
using System.Reflection;
using RazorWare.GfxCore.Events;
using RazorWare.GfxCore.Registries;
using RazorWare.GfxCore.Utilities;

namespace RazorWare.GfxCore.Extensibility;

public class ExtensionLoader
{
    private const string EXTENSION_PATH = "mods";

    private static string ext_path;

    //  the list of GfxExtensionInfo objects
    private static readonly List<GfxExtensionInfo> _extensions = new();

    private RegistryManager Registries { get; init; }

    private IEventPipeline Events { get; init; }

    /// <summary>
    /// Get the extension path.
    /// </summary>
    internal DirectoryInfo ExtensionPath => new DirectoryInfo(ext_path);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="registries"></param>
    public ExtensionLoader(RegistryManager registries)
    {
        Registries = registries;
        var registry = Registries.Resolve<ICommandTargetRegistry>();
        Events = registry.Resolve<IEventPipeline>();
    }

    /// <summary>
    /// Load the <see cref="GfxExtensionInfo"> objects
    /// </summary>
    /// <returns>A read-only collection of GfxExtensionInfo objects</returns>
    internal IReadOnlyCollection<GfxExtensionInfo> LoadExtensions()
    {
        DiscoverExtensions(out List<PackageManifest> packages);
        IExtensionRegistry extensions = null;

        extensions = Registries.Resolve<IExtensionRegistry>();
        List<GfxExtensionInfo> extInfos = new();
        //  validate package manifests
        foreach (var pkgManifest in packages)
        {
            //  load the extension
            extInfos.Add(LoadExtensionInfo(pkgManifest));
        }

        return extInfos;
    }

    //  load the extensions
    private void DiscoverExtensions(out List<PackageManifest> packages)
    {
        Log($"[GfxCore :: Bootstrap] Begin Extension Discovery");

        var curr_dir = string.Empty;
        Log($"{"",5}{"Current Directory:",-25} {curr_dir = Directory.GetCurrentDirectory()}");
        Log($"{"",5}{"Extension Path:",-25} {ext_path = Path.Combine(curr_dir, EXTENSION_PATH)}");

        if (!ext_path.ResolvePathArgs(out ext_path, out _))
        {
            Log($"{"",5}Creating Extension Path: {ext_path}");
            Directory.CreateDirectory(ext_path);
        }

        EnumerateExtensions(out packages);
        //  if no extensions found, we are done
        if (!packages.Any())
        {
            Log($"{"",5}{"No Extensions Found",-25}");
        }

        Log($"[GfxCore :: Bootstrap] Extension Discovery Complete");
    }

    //  iterate through the extension path:
    //   - the zip packages are the extensions
    //   - each package contains:
    //      - a manifest file (gfxpackage.json)
    //      - the actual extension assembly (dll)
    //      - any other resources required by the extension (e.g. images, assemblies, etc.)
    private void EnumerateExtensions(out List<PackageManifest> packages)
    {
        packages = new();
        Log($"{"",5}{"Enumerating Extensions:",-25}");
        //  not expecting directories ... just "*.zip" files
        foreach (var pkg in ExtensionPath.GetFiles("*.zip"))
        {
            Log($"{"",9}{"Found Package:",-25} {pkg.Name}");
            //  extract the package
            if (PackageManifest.Unpack(Registries, pkg, out PackageManifest pkgManifest))
            {
                //  add the package to the list of extensions
                packages.Add(pkgManifest);
            }
        }
    }
    //  load the extension info
    private GfxExtensionInfo LoadExtensionInfo(PackageManifest manifest)
    {
        var assemblies = Registries.Resolve<IAssemblyRegistry>();
        var extensions = Registries.Resolve<IExtensionRegistry>();

        var ext = manifest.ExtAssembly;
        var extAssembly = ext.Name;

        //  get the assembly - I don't like iterating through all assemblies every time we want to get an assembly
        var assembly = assemblies.ResolveAssembly(ext.Name);
        if (assembly == null)
        {
            Log($"[*** ERROR ***]   Extension {ext.Name} not loaded into the application domain.");
            return null;
        }
        //  get the extension type
        var extType = assembly.GetTypes().FirstOrDefault(t => t.GetCustomAttribute<GfxExtensionAttribute>() != null);

        if (extType == null)
        {
            Log($"[*** ERROR ***]   Extension {ext.Name} does not contain a GfxExtensionAttribute.");
            return null;
        }

        //  register the extension
        return new()
        {
            Assembly = ext,
            ExtType = extType,
            Metadata = extType.GetCustomAttribute<GfxExtensionAttribute>()
        };
    }
    //  log a message
    private void Log(string message)
    {
        Events.Raise(new LogEvent(message));
    }
}
