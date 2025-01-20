
namespace RazorWare.GfxCore.Registries;

/// <summary>
/// Generic registry interface
/// </summary>
/// <typeparam name="TRegType"></typeparam>
public interface IRegistry<TRegType> : IEnumerable<TRegType>, IRegistry
{
    /// <summary>
    /// Register a registry item
    /// </summary>
    /// <typeparam name="T">Interface type</typeparam>
    /// <param name="type">Item type</param>
    /// <param name="regItem">The item</param>
    /// <param name="identifier">Item identifier</param>
    /// <param name="tags">Item tags</param>
    /// <returns>The registry item</returns>
    /// <exception cref="ArgumentException"></exception>
    T Register<T>(Type regType, object regObj, string identifier = null, params string[] tags) where T : class;
    /// <summary>
    /// Try to resolve registry item by type
    /// </summary>
    /// <param name="type">The registry type</param>
    /// <param name="registry">The registry</param>
    /// <returns>TRUE if the registry is found, FALSE otherwise</returns>
    bool TryResolve(Type type, out TRegType registry);
    /// <summary>
    /// Try to resolve registry item by identifier
    /// </summary>
    /// <param name="identifier">The registry identifier</param>
    /// <param name="registry">The registry</param>
    /// <returns>TRUE if the registry is found, FALSE otherwise</returns>
    bool TryResolve(string identifier, out TRegType registry);
}

/// <summary>
/// Registry interface
/// </summary>
public interface IRegistry
{
    /// <summary>
    /// The registry name
    /// </summary>
    string Name { get; }
    /// <summary>
    /// The registry type
    /// </summary>
    Type Type { get; }
}