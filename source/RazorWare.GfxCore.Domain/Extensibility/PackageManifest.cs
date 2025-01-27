
using System.IO.Compression;
using System.Reflection;
using System.Text.Json;

using RazorWare.GfxCore.Events;
using RazorWare.GfxCore.Registries;

namespace RazorWare.GfxCore.Extensibility;

/// <summary>
/// The manifest for a package.
/// </summary>
public class PackageManifest
{
    private static IEventPipeline Events { get; set; }

    private readonly Manifest _manifest;

    private List<ZipArchiveEntry> PkgEntries { get; init; } = new();

    /// <summary>
    /// Get the extension info for the package.
    /// </summary>
    public GfxExtensionInfo Extension { get; init; } = new GfxExtensionInfo();
    /// <summary>
    /// Determine if the package has errors.
    /// </summary>
    public bool HasErrors { get; private set; }
    /// <summary>
    /// Get the extension assembly name
    /// </summary>
    public AssemblyName ExtAssembly => _manifest.Assembly.Name;

    /// <summary>
    /// Construct a new package manifest.
    /// </summary>
    /// <param name="manifest">The manifest for the package.</param>
    /// <param name="entries">The entries in the package.</param>
    private PackageManifest(Manifest manifest)
    {
        _manifest = manifest;
    }

    /// <summary>
    /// Open the package and extract the manifest.
    /// </summary>
    /// <param name="pkgFile">The package file to open.</param>
    /// <param name="pkgManifest">The package manifest.</param>
    /// <returns>TRUE if the package was opened successfully, otherwise FALSE.</returns>
    public static bool Unpack(RegistryManager registries, FileInfo pkgFile, out PackageManifest pkgManifest)
    {
        var registry = registries.Resolve<ICommandTargetRegistry>();
        Events = registry.Resolve<IEventPipeline>();

        //  obtain the package file stream
        using var fs = pkgFile.OpenRead();
        //  open the package archive
        using var archive = new ZipArchive(fs, ZipArchiveMode.Read, false);
        //  open and validate the package
        if (OpenPackage(out pkgManifest, archive) && pkgManifest.ValidateAssemblies())
        {
            var assemblies = registries.Resolve<IAssemblyRegistry>();
            //  register the extension assembly
            pkgManifest.RegisterExtensionAssembly(assemblies);
            //  register the dependency assemblies
            pkgManifest.RegisterDependencyAssemblies(assemblies);
        }

        return true;
    }

    /// <summary>
    /// open and extract the package contents
    /// </summary>
    /// <returns>TRUE if the package was opened successfully, otherwise FALSE.</returns>
    private static bool OpenPackage(out PackageManifest pkgManifest, ZipArchive archive)
    {
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
        pkgManifest = new PackageManifest(manifest);
        pkgManifest.PkgEntries.AddRange(archive.Entries.Where(e => e.FullName != Manifest.PACKAGE_JSON));

        return true;
    }

    private static void Log(string message)
    {
        Events.Raise(new LogEvent(message));
    }

    /// <summary>
    /// Validate the assemblies in the package.
    /// </summary>
    /// <returns>TRUE if the assemblies are valid, otherwise FALSE.</returns>
    private bool ValidateAssemblies()
    {
        /*      
                What do we do to validate the manifest assemblies?
                We are not loading them here, we are just validating them
                1. iterate dependencies and make sure the zip archive entry exists
                2. make sure the extension zip archive entry exists

                We check these by looking at the AssemblyInfo objects in the manifest
         */
        var ext = _manifest.Assembly;
        var deps = _manifest.Dependencies;

        //  check if the extension assembly and dependencies are in the package
        //  TODO: add logging for missing dependencies and add error conditions
        return PkgEntries.Any(e => e.FullName == ext.EntryTag) &&
            deps.All(d => PkgEntries.Any(e => e.FullName == d.EntryTag));
    }
    /// <summary>
    /// Register the dependency assemblies.
    /// </summary>
    private void RegisterDependencyAssemblies(IAssemblyRegistry assemblies)
    {
        Assembly assembly = null;
        var ext = _manifest.Assembly;
        var extDependencies = _manifest.Dependencies;
        //  validate dependency assemblies first - get entry from assembly info
        foreach (var dep in extDependencies)
        {
            //  get the assembly name only because that is what the AssemblyRegistry uses
            var depAssembly = dep.Name;
            if (assemblies.TryResolve(depAssembly.Name, out _))
            {
                //  if we have the dependency registered, then we continue
                continue;
            }

            if (TryLoadAssembly(dep, out assembly))
            {
                //  register the assembly
                assemblies.Register(assembly, dep.EntryTag);
            }
        }
    }
    /// <summary>
    /// Register the extension assembly.
    /// </summary>
    private void RegisterExtensionAssembly(IAssemblyRegistry assemblies)
    {
        Assembly assembly = null;
        var ext = _manifest.Assembly;
        var extAssembly = ext.Name;

        //  validate the extension assembly
        if (assemblies.TryResolve(extAssembly.Name, out _))
        {
            //  if we have the extension registered, then we continue ... 
            //  ... but this should not happen because we are validating the extension
            Console.WriteLine($"[*** WARNING ***]   Extension {extAssembly.Name} already registered.");

            return;
        }
        //  load the extension assembly
        if (TryLoadAssembly(ext, out assembly))
        {
            //  register the extension assembly
            assemblies.Register(extAssembly, ext.EntryTag);
        }
    }


    //  load the assembly from the package
    private bool TryLoadAssembly(AssemblyInfo asmInfo, out Assembly assembly)
    {
        var pkgEntry = PkgEntries.FirstOrDefault(a => a.FullName == asmInfo.EntryTag);
        assembly = null;

        using (Stream asmStream = pkgEntry.Open())
        {
            try
            {
                using (var memory = new MemoryStream())
                {
                    asmStream.CopyTo(memory);
                    //  load the assembly
                    assembly = Assembly.Load(memory.ToArray());
                }

                // Print the name of the assembly
                Console.WriteLine($"Assembly Name: {assembly.GetName().Name}");
                Console.WriteLine($"Assembly Version: {assembly.GetName().Version}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        return assembly != null;
    }
}