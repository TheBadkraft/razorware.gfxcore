
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
    private ILogger Logger { get; set; }
    /// <summary>
    /// The event pipeline
    /// </summary>
    private IEventPipeline Events { get; set; }

    internal CoreBootstrap(bool testMode) : base(testMode)
    {
        Logger.Log("[GfxCore :: Bootstrap] Registries Loaded:");
        foreach (var registry in Registries)
        {
            Logger.Log($"{"",5}{registry.Name,-25} ({Registries.CanResolve(registry.Type)})");
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="registries"><inheritdoc/></param>
    protected override void OnInitialize(RegistryManager registries)
    {
        Registries = registries;

        var services = Registries.Register<IServiceRegistry>(typeof(IGfxService), new ServiceRegistry());
        Registries.Register<ISystemRegistry>(typeof(IGfxSystem), new SystemRegistry());
        Registries.Register<IResourceRegistry>(typeof(IGfxResource), new ResourceRegistry());

        Logger = services.Register<ILogger>(new ConsoleLogger());

        var commands = Registries.Resolve<ICommandTargetRegistry>();
        Events = commands.Register<IEventPipeline>(typeof(IGfxCommandTarget), EventPipeline.Instance);
        Events.Subscribe<LogEvent>(e => Logger.Log(e.Message));

        var assemblies = Registries.Resolve<IAssemblyRegistry>();
        assemblies.RegisterDependencies(GetType().Assembly);
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
