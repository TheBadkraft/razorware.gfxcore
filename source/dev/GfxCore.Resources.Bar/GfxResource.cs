
using RazorWare.GfxCore.Extensibility;

namespace GfxCore.Systems.Foo;


[GfxExtension(typeof(GfxResource))]
public class GfxResource : IGfxExtension
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public string Name => "GfxResource";
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public Version Version => new(1, 0, 0);
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public List<GfxExtensionInfo> Requires { get; }
}
