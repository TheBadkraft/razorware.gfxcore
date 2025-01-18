using RazorWare.GfxCore.Extensibility;

namespace RazorWare.GfxCore.Registries;

/// <summary>
/// The resource registry.
/// </summary>
public class ResourceRegistry : GfxRegistry<IGfxResource>, IResourceRegistry
{
    public ResourceRegistry() : base(nameof(ResourceRegistry))
    {
        Type = typeof(IResourceRegistry);
    }
}