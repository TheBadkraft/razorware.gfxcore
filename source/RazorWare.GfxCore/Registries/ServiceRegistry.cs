using RazorWare.GfxCore.Extensibility;

namespace RazorWare.GfxCore.Registries;

/// <summary>
/// The service registry.
/// </summary>
public class ServiceRegistry : GfxRegistry<IGfxService>, IServiceRegistry
{
    public ServiceRegistry() : base("ServiceRegistry") { }

    /// <summary>
    /// Register a service to the framework
    /// </summary>
    /// <typeparam name="T">The service interface</typeparam>
    /// <param name="service">The service object</param>
    /// <param name="identifier">The service identifier</param>
    /// <param name="tags">The service tags</param>
    /// <returns><The service object/returns>
    public T Register<T>(object service, string identifier = null, params string[] tags) where T : class
    {
        if (!(service is IGfxService gfxService))
        {
            throw new ArgumentException($"{service.GetType().Name} is not a valid service object.");
        }

        var key = new RegistryKey(identifier, tags)
        {
            InterfaceType = typeof(T),
            ObjectType = service.GetType()
        };

        if (!TryAdd(key, gfxService))
        {
            throw new ArgumentException($"Registry key [{key}] already exists.");
        }

        return service as T;
    }
    /// <summary>
    /// Resolve a service by type
    /// </summary>
    /// <typeparam name="T">Service type</typeparam>
    /// <param name="identifier">[Optional] Service identifier</param>
    /// <param name="tags">[Optional] Service tags</param>
    /// <returns>The service</returns>
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

/// <summary>
/// The system registry.
/// </summary>
public class SystemRegistry : GfxRegistry<IGfxSystem>, ISystemRegistry
{
    public SystemRegistry() : base("SystemRegistry") { }
}

/// <summary>
/// The resource registry.
/// </summary>
public class ResourceRegistry : GfxRegistry<IGfxResource>, IResourceRegistry
{
    public ResourceRegistry() : base("ResourceRegistry") { }
}