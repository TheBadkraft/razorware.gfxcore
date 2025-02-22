
using RazorWare.GfxCore.Extensibility;
using RazorWare.GfxCore.Registries;

namespace RazorWare.GfxCore.Runtime;

/// <summary>
/// The framework instance.
/// </summary>
public class Framework
{
    private static readonly Singleton<Framework> _singleton = new(() => new());

    public static Framework Instance => _singleton.Value;
    public RegistryManager Registries { get; private set; }

    // Private constructor to prevent instantiation
    private Framework()
    {
        Registries = new RegistryManager();
    }

    /*
        Resolving extensions is a multi-step process in which we look at the extension information 
        and determine if we have the necessary dependencies to load the extension.

        In general:
        1. resource extensions are loaded first (they will generall have few if any dependencies)
        2. services are loaded next (they will generally have dependencies on resource extensions)
        3. systems and SDK extensions are loaded last (they can have dependencies on services and resources)
    */
    internal static void ResolveExtensions(IReadOnlyCollection<GfxExtensionInfo> extensions)
    {
        var gfxExtensions = Instance.Registries.Resolve<IExtensionRegistry>();

        //  resolve resources
        var resources = extensions.Where(e => e.Metadata.Type == GfxExtensionType.Resource).ToList();
        foreach (var resource in resources)
        {
            //  the resource type should implement IGfxExtension
            if (resource.ExtType.IsAssignableTo(typeof(IGfxExtension)))
            {
                //  create an instance of the resource
                var gfxExtension = Activator.CreateInstance(resource.ExtType) as IGfxExtension;
                if (gfxExtension != null)
                {
                    //  if there are any requires, resolve them
                    if (gfxExtension.Requires.Any())
                    {
                        //  resolve the required extensions
                        foreach (var r in gfxExtension.Requires)
                        {
                            //  1. is the required extension in the registry?
                            if (!r.IsRegistered())
                            {
                                //  2. if not, is the required extension in the list of extensions?
                                var required = extensions.FirstOrDefault(e => e.Name == r.Name && e.Version == r.Version);
                                if (required != null)
                                {
                                    //  3. can we find and load it?
                                    ResolveExtensions(new[] { required });
                                }
                            }
                            //  2. if not, is the required extension in the list of extensions?

                            //  3. can we find and load it?}
                        }
                        //  add the resource to the registry
                        gfxExtensions.Register<IGfxExtension>(resource.ExtType, gfxExtension, resource.Assembly.Name, $"{gfxExtension.Version}");
                    }
                }
            }
        }
    }
