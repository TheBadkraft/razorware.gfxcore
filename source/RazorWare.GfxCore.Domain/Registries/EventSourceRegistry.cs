
using RazorWare.GfxCore.Extensibility;

namespace RazorWare.GfxCore.Registries;

/// <summary>
/// The event registry.
/// </summary>
public class EventSourceRegistry : GfxRegistry<IGfxEventSource>, IEventSourceRegistry
{
    public EventSourceRegistry() : base(nameof(EventSourceRegistry))
    {
        Type = typeof(IEventSourceRegistry);
    }

    /// <summary>
    /// Register an event source to the framework
    /// </summary>
    /// <typeparam name="T">The event source interface</typeparam>
    /// <param name="source">The event source object</param>
    /// <param name="identifier">The event source identifier</param>
    /// <param name="tags">The event source tags</param>
    /// <returns>The event source object</returns>
    /// <exception cref="ArgumentException"></exception>
    public T Register<T>(object source, string identifier = null, params string[] tags) where T : class
    {
        if (!(source is IGfxEventSource gfxEventSource))
        {
            throw new ArgumentException($"{source.GetType().Name} is not a valid event source object.");
        }

        var key = new RegistryKey(identifier, tags)
        {
            InterfaceType = typeof(T),
            ObjectType = source.GetType()
        };

        if (!TryAdd(key, gfxEventSource))
        {
            throw new ArgumentException($"Registry key [{key}] already exists.");
        }

        return source as T;
    }
    /// <summary>
    /// Resolve an event source by type
    /// </summary>
    /// <typeparam name="T">Event source type</typeparam>
    /// <param name="identifier">[Optional] Event source identifier</param>
    /// <param name="tags">[Optional] Event source tags</param>
    /// <returns>The event source</returns>
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