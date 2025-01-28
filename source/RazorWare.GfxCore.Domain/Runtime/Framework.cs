
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

}
