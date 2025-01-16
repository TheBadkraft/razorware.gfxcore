
using RazorWare.GfxCore.Extensibility;
using RazorWare.GfxCore.Extensibility.Logging;
using RazorWare.GfxCore.Logging;
using RazorWare.GfxCore.Registries;

namespace RazorWare.GfxCore.Runtime;

/*
    The GfxCore bootstrap implementation initializes the core systems.
*/

internal sealed class CoreBootstrap : GfxBootstrap
{
    internal CoreBootstrap(bool testMode) : base(testMode)
    {
        var services = _registries.Register<IServiceRegistry>(typeof(IGfxService), new ServiceRegistry());
        _registries.Register<ISystemRegistry>(typeof(IGfxSystem), new SystemRegistry());
        _registries.Register<IResourceRegistry>(typeof(IGfxResource), new ResourceRegistry());


        Logger = services.Register<ILogger>(new ConsoleLogger());
        Logger.Log("GfxCore loaded ...");
    }
}
