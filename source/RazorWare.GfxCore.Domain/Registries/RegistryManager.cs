
using RazorWare.GfxCore.Extensibility;

namespace RazorWare.GfxCore.Registries;

/// <summary>
/// The registry manager.
/// </summary>
public class RegistryManager : GfxRegistry<IRegistry>
{
    /// <summary>
    /// Initializes the registry manager.
    /// </summary>
    public RegistryManager() : base("Registries")
    {
        Register<IEventSourceRegistry>(typeof(IGfxEventSource), new EventSourceRegistry());
        Register<ICommandTargetRegistry>(typeof(IGfxCommandTarget), new CommandTargetRegistry());
    }

    /// <summary>
    /// Resolve a registry by type.
    /// </summary>
    /// <typeparam name="T">The registry type</typeparam>
    /// <returns>The registry</returns>
    /// <exception cref="ArgumentException"></exception>
    public T Resolve<T>() where T : class, IRegistry
    {
        T registry = default;

        if (TryResolveKey(typeof(T), out var key))
        {
            registry = Get(key) as T;
        }
        else
        {
            throw new ArgumentException($"Registry key [{key}] not found.");
        }

        return registry;
    }
    /// <summary>
    /// Determines if a registry can be resolved by type.
    /// </summary>
    /// <typeparam name="T">The registry type</typeparam>
    /// <returns>TRUE if the registry can be resolved, otherwise FALSE</returns>
    public bool CanResolve<T>() where T : class, IRegistry
    {
        return CanResolve(typeof(T));
    }
    /// <summary>
    /// Determines if a registry can be resolved by type.
    /// </summary>
    /// <param name="type">The registry type</param>
    /// <returns>TRUE if the registry can be resolved, otherwise FALSE</returns>
    public bool CanResolve(Type type)
    {
        return TryResolveKey(type, out _);
    }
}
