using RazorWare.GfxCore.Extensibility;

namespace RazorWare.GfxCore.Registries;

/// <summary>
/// Command target registry interface
/// </summary>
public interface ICommandTargetRegistry : IRegistry<IGfxCommandTarget>
{
    /// <summary>
    /// Resolve a command target by type.
    /// </summary>
    /// <typeparam name="T">The command target type</typeparam>
    /// <param name="identifier">[Optional] The command target identifier</param>
    /// <param name="tags">[Optional] The command target tags</param>
    /// <returns>The command target</returns>
    T Resolve<T>(string identifier = null, params string[] tags) where T : class;
}
