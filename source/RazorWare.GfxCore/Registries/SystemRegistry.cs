using RazorWare.GfxCore.Extensibility;

namespace RazorWare.GfxCore.Registries;

/// <summary>
/// The system registry.
/// </summary>
public class SystemRegistry : GfxRegistry<IGfxSystem>, ISystemRegistry
{
    public SystemRegistry() : base(nameof(SystemRegistry))
    {
        Type = typeof(ISystemRegistry);
    }
}
