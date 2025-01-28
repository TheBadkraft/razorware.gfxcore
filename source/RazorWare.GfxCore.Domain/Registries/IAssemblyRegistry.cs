
using System.Reflection;

namespace RazorWare.GfxCore.Registries;

/// <summary>
/// Assembly registry interface
/// </summary>
public interface IAssemblyRegistry : IRegistry<AssemblyName>
{
    /// <summary>
    /// Register an assembly to the framework
    /// </summary>
    /// <param name="assembly">The assembly object</param>
    /// <param name="identifier">[Optional] The assembly identifier</param>
    /// <param name="tags">[Optional] The assembly tags</param>
    /// <returns>The assembly name object</returns>
    /// <exception cref="ArgumentException"></exception>
    AssemblyName Register(object assembly, string identifier, params string[] tags);
    /// <summary>
    /// Register an assembly to the framework
    /// </summary>
    /// <param name="assembly">The assembly object</param>
    /// <param name="identifier">[Optional] The assembly identifier</param>
    /// <param name="tags">[Optional] The assembly tags</param>
    /// <returns>The assembly name object</returns>
    AssemblyName Register(Assembly assembly, string identifier = null, params string[] tags);
    /// <summary>
    /// Resolve an assembly by identifier.
    /// </summary>
    /// <param name="identifier">The assembly identifier</param>
    /// <param name="tags">[Optional] The assembly tags</param>
    /// <returns>The assembly</returns>
    AssemblyName Resolve(string identifier, params string[] tags);
    /// <summary>
    /// Resolve an assembly by identifier.
    /// </summary>
    /// <param name="identifier">The assembly identifier</param>
    /// <param name="tags">[Optional] The assembly tags</param>
    /// <returns>The assembly</returns>
    Assembly ResolveAssembly(string identifier, params string[] tags);
    /// <summary>
    /// Resolve an assembly by name.
    /// </summary>
    /// <param name="assemblyName">The assembly name</param>
    /// <returns>The assembly</returns>
    Assembly ResolveAssembly(AssemblyName assemblyName);
    /// <summary>
    /// Register an assembly's dependencies
    /// </summary>
    /// <remarks>
    /// Set auto-register TRUE to automatically register the 
    /// assembly's .Net dependencies.
    /// </remarks>
    /// <param name="assembly">The assembly to inspect</param>
    /// <param name="autoRegister">Autoregister .net dependencies</param>
    void RegisterDependencies(Assembly assembly, bool autoRegister = false);
}
