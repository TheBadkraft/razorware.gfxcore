
using Microsoft.Win32;
using RazorWare.GfxCore.Events;
using RazorWare.GfxCore.Extensibility;
using RazorWare.GfxCore.Extensibility.Logging;
using RazorWare.GfxCore.Logging;
using RazorWare.GfxCore.Registries;

namespace RazorWare.GfxCore.Runtime;

/*
    The GfxCore bootstrap implementation initializes the core systems and instantiates the core registries.
*/

/// <summary>
/// The core bootstrap implementation for GfxCore.
/// </summary>
internal sealed class CoreBootstrap : GfxBootstrap
{
    internal static RegistryManager Registries { get; private set; }

    /// <summary>
    /// The default logger
    /// </summary>
    private ILogger Logger { get; init; }

    internal CoreBootstrap(bool testMode) : base(testMode)
    {
        var services = Registries.Register<IServiceRegistry>(typeof(IGfxService), new ServiceRegistry());
        Registries.Register<ISystemRegistry>(typeof(IGfxSystem), new SystemRegistry());
        Registries.Register<IResourceRegistry>(typeof(IGfxResource), new ResourceRegistry());

        if (!Registries.CanResolve<IEventSourceRegistry>())
        {
            throw new InvalidOperationException("Event registry not found");
        }

        Logger = services.Register<ILogger>(new ConsoleLogger());
        Logger.Log("[GfxCore :: Bootstrap] Registries Loaded:");
        foreach (var registry in Registries)
        {
            Logger.Log($"{"",15}{registry.Name,-25} ({Registries.CanResolve(registry.Type)})");
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="registries"><inheritdoc/></param>
    protected override void OnLoad(RegistryManager registries)
    {
        Registries = registries;
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="message"><inheritdoc/></param>
    protected override void Log(string message)
    {
        Logger.Log(message);
    }
}
