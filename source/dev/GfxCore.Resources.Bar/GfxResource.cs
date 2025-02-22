
using RazorWare.GfxCore.Extensibility;

namespace GfxCore.Systems.Foo;


[GfxExtension(typeof(GfxResource), Type = GfxExtensionType.Resource)]
public class GfxResource : IGfxExtension
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public string Name => "GfxResource";
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public GfxVersion Version { get; } =     ;
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public List<GfxExtensionInfo> Requires { get; } = new();
}
