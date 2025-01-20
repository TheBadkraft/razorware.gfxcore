
using System.Reflection;

using RazorWare.GfxCore.Events;
using RazorWare.GfxCore.Registries;

namespace RazorWare.GfxCore.Extensibility;

public class ExtensionLoader
{
    private const string EXTENSION_PATH = "mods";

    private static string ext_path;

    //  the list of extensions
    private static readonly List<GfxExtensionInfo> _extensions = new();

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
        if (!_extensions.Any())
        {
            Log($"{"",5}{"No Extensions Found",-25}");
            return;
        }

        // LoadExtensions();

        Log($"[GfxCore :: Bootstrap] Extension Discovery Complete");
    }

    //  iterate through the extension path:
    //   - each extension is a directory
    //   - each extension directory contains an assembly (dll)
    //   - at least one assembly in the directory contains a class 
    //     decorated with the GfxExtensionAttribute
    private void EnumerateExtensions()
    {
        foreach (var dir in Directory.GetDirectories(ext_path))
        {
            //  does the path contain an assembly (dll)?
            if (!Directory.GetFiles(dir, "*.dll").Any())
            {
                continue;
            }
            //  find the assembly with a class decorated with the GfxExtensionAttribute
            var info = new GfxExtensionInfo
            {
                ExtensionPath = new(dir)
            };
            if (!LocateExtensionAssembly(info))
            {
                continue;
            }

            _extensions.Add(info);
            Log($"{"",5}{"Extension Found:",-25} {info}");
        }
    }
    //  locate the extension assembly; one per directory
    //  let's go ahead and materialize the attribute
    private bool LocateExtensionAssembly(GfxExtensionInfo info)
    {
        /*
            All we are doing is finding the first assembly in the directory 
            that has a class decorated with the GfxExtensionAttribute. If 
            there is more than one assembly in the directory, we will only 
            load the first one found.

            This is a simple implementation and lacks the robustness of 
            a full extension system. This is a good starting point.
        */
        var files = info.ExtensionPath.GetFiles("*.dll");

        foreach (var file in files)
        {
            try
            {
                var assembly = Assembly.LoadFrom(file.FullName);
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if ((info.Metadata = type.GetCustomAttribute<GfxExtensionAttribute>()) != null)
                    {
                        info.Assembly = assembly;
                        info.ExtType = type;

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"{"",5}{"Error Loading Assembly:",-25} {file}");
                Log($"{"",10}{ex.Message}");
            }

            //  if info.Assembly != null, we found the assembly
            if (info.Assembly != null)
            {
                break;
            }
        }

        return info.Assembly != null;
    }

    private void Log(string message)
    {
        Events.Raise(new LogEvent(message));
    }
}
