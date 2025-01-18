
using RazorWare.GfxCore.Extensibility;

namespace RazorWare.GfxCore.Registries;

/// <summary>
/// The command registry.
/// </summary>
public class CommandTargetRegistry : GfxRegistry<IGfxCommandTarget>, ICommandTargetRegistry
{
    public CommandTargetRegistry() : base(nameof(CommandTargetRegistry))
    {
        Type = typeof(ICommandTargetRegistry);
    }

    /// <summary>
    /// Register a command target.
    /// </summary>
    /// <typeparam name="T">The command target type</typeparam>
    /// <param name="target">The command target</param>
    /// <param name="identifier">[Optional] The command target identifier</param>
    /// <param name="tags">[Optional] The command target tags</param>
    /// <returns>The command target</returns>
    /// <exception cref="ArgumentException"></exception>
    public T Register<T>(object target, string identifier = null, params string[] tags) where T : class
    {
        if (!(target is IGfxCommandTarget gfxCommand))
        {
            throw new ArgumentException($"{target.GetType().Name} is not a valid command object.");
        }

        var key = new RegistryKey(identifier, tags)
        {
            InterfaceType = typeof(T),
            ObjectType = target.GetType()
        };

        if (!TryAdd(key, gfxCommand))
        {
            throw new ArgumentException($"Registry key [{key}] already exists.");
        }

        return target as T;
    }
    /// <summary>
    /// Resolve a command target.
    /// </summary>
    /// <typeparam name="T">The command target type</typeparam>
    /// <param name="identifier">[Optional] The command target identifier</param>
    /// <param name="tags">[Optional] The command target tags</param>
    /// <returns>The command target</returns>
    public T Resolve<T>(string identifier = null, params string[] tags) where T : class
    {
        T service = default;

        if (TryResolveKey(typeof(T), out var key))
        {
            service = ((object)Get(key)) as T;
        }

        return service;
    }
}