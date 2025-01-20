
using RazorWare.GfxCore.Extensibility;

namespace RazorWare.GfxCore.Registries;

/// <summary>
/// The extension registry.
/// </summary>
public class ExtensionRegistry : GfxRegistry<IGfxExtension>, IExtensionRegistry
{
    public ExtensionRegistry() : base(nameof(ExtensionRegistry))
    {
        Type = typeof(IExtensionRegistry);
    }

    /// <summary>
    /// Register an extension.
    /// </summary>
    /// <typeparam name="T">The extension type</typeparam>
    /// <param name="extension">The extension</param>
    /// <param name="identifier">[Optional] The extension identifier</param>
    /// <param name="tags">[Optional] The extension tags</param>
    /// <returns>The extension</returns>
    /// <exception cref="ArgumentException"></exception>
    public T Register<T>(object extension, string identifier = null, params string[] tags) where T : class
    {
        if (!(extension is IGfxExtension gfxExtension))
        {
            throw new ArgumentException($"{extension.GetType().Name} is not a valid extension.");
        }

        var key = new RegistryKey(identifier, tags)
        {
            InterfaceType = typeof(T),
            ObjectType = extension.GetType()
        };

        if (!TryAdd(key, gfxExtension))
        {
            throw new ArgumentException($"Registry key [{key}] already exists.");
        }

        return extension as T;
    }
    /// <summary>
    /// Resolve an extension.
    /// </summary>
    /// <typeparam name="T">The extension type</typeparam>
    /// <param name="identifier">[Optional] The extension identifier</param>
    /// <param name="tags">[Optional] The extension tags</param>
    /// <returns>The extension</returns>
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