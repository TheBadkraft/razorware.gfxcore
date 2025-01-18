using RazorWare.GfxCore.Extensibility;

namespace RazorWare.GfxCore.Registries;

/// <summary>
/// Service registry interface
/// </summary>
public interface IServiceRegistry : IRegistry<IGfxService>
{
    /// <summary>
    /// Register a service to the framework
    /// </summary>
    /// <typeparam name="T">The service interface</typeparam>
    /// <param name="service">The service object</param>
    /// <param name="identifier">The service identifier</param>
    /// <param name="tags">The service tags</param>
    /// <returns><The service object/returns>
    T Register<T>(object service, string identifier = null, params string[] tags) where T : class;
    /// <summary>
    /// Resolve a service by type
    /// </summary>
    /// <typeparam name="T">Service type</typeparam>
    /// <param name="identifier">[Optional] Service identifier</param>
    /// <param name="tags">[Optional] Service tags</param>
    /// <returns>The service</returns>
    T Resolve<T>(string identifier = null, params string[] tags) where T : class;
}

/// <summary>
/// System registry interface
/// </summary>
public interface ISystemRegistry : IRegistry<IGfxSystem> { }

/// <summary>
/// Resource registry interface
/// </summary>
public interface IResourceRegistry : IRegistry<IGfxResource> { }

/// <summary>
/// Event source registry interface
/// </summary>
public interface IEventSourceRegistry : IRegistry<IGfxEventSource> { }

/// <summary>
/// Command target registry interface
/// </summary>
public interface ICommandTargetRegistry : IRegistry<IGfxCommandTarget> { }