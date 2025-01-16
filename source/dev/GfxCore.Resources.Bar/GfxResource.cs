
using RazorWare.GfxCore.Extensibility;

namespace GfxCore.Systems.Foo;


[GfxExtension(typeof(GfxResource))]
public class GfxResource : IGfxExtension
{
    //  this class is our resource entry point
    public string Name => "GfxResource";
}
