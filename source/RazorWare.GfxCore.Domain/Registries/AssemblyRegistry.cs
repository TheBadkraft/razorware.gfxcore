using System.Reflection;

namespace RazorWare.GfxCore.Registries;

/// <summary>
/// The assembly registry.
/// </summary>
public class AssemblyRegistry : GfxRegistry<AssemblyName>, IAssemblyRegistry
{
    /// <summary>
    /// Initializes the assembly registry.
    /// </summary>
    public AssemblyRegistry() : base(nameof(AssemblyRegistry))
    {
        Type = typeof(IAssemblyRegistry);
    }

    /// <summary>
    /// Register an assembly to the framework
    /// </summary>
    /// <typeparam name="T">The assembly type</typeparam>
    /// <param name="assembly">The assembly object</param>
    /// <param name="identifier">[Optional] The assembly identifier</param>
    /// <param name="tags">[Optional] The assembly tags</param>
    /// <returns>The assembly object</returns>
    public T Register<T>(object assembly, string identifier = null, params string[] tags) where T : class
    {
        identifier = identifier ?? assembly.GetType().Name;
        return Register(assembly, identifier, tags) as T;
    }
    /// <summary>
    /// Register an assembly to the framework
    /// </summary>
    /// <param name="assembly">The assembly object</param>
    /// <param name="identifier">[Optional] The assembly identifier</param>
    /// <param name="tags">[Optional] The assembly tags</param>
    /// <returns>The assembly object</returns>
    /// <exception cref="ArgumentException"></exception>
    public AssemblyName Register(object assembly, string identifier, params string[] tags)
    {
        if (!(assembly is AssemblyName gfxAssembly))
        {
            throw new ArgumentException($"{assembly.GetType().Name} is not a valid assembly object.");
        }

        var key = new RegistryKey(identifier, tags)
        {
            InterfaceType = assembly.GetType(),
            ObjectType = assembly.GetType()
        };

        if (!TryAdd(key, gfxAssembly))
        {
            throw new ArgumentException($"Registry key [{key}] already exists.");
        }

        return assembly as AssemblyName;
    }
    /// <summary>
    /// Register an assembly to the framework
    /// </summary>
    /// <param name="assembly">The assembly object</param>
    /// <param name="identifier">[Optional] The assembly identifier</param>
    /// <param name="tags">[Optional] The assembly tags</param>
    /// <returns>The assembly name object</returns>
    public AssemblyName Register(Assembly assembly, string identifier = null, params string[] tags)
    {
        var asmName = assembly.GetName();
        return Register(asmName, asmName.Name, tags);
    }
    /// <summary>
    /// Resolve an assembly by type
    /// </summary>
    /// <typeparam name="T">Assembly type</typeparam>
    /// <param name="identifier">[Optional] Assembly identifier</param>
    /// <param name="tags">[Optional] Assembly tags</param>
    /// <returns>The assembly</returns>
    public T Resolve<T>(string identifier = null, params string[] tags) where T : class
    {
        T service = default;

        if (TryResolveKey(typeof(T), out var key))
        {
            service = ((object)Get(key)) as T;
        }

        return service;
    }
    /// <summary>
    /// Resolve an assembly by identifier
    /// </summary>
    /// <param name="identifier">The assembly identifier</param>
    /// <param name="tags">The assembly tags</param>
    /// <returns>The assembly name</returns>
    public AssemblyName Resolve(string identifier, params string[] tags)
    {
        AssemblyName assembly = default;
        if (TryResolveKey(identifier, tags, out var key))
        {
            assembly = Get(key) as AssemblyName;
        }

        return assembly;
    }
    /// <summary>
    /// Register an assembly's dependencies
    /// </summary>
    /// <remarks>
    /// This method is used to register an assembly's dependencies 
    /// to the framework. .Net assemblies are not registered by 
    /// default.
    /// <para>
    /// Set auto-register TRUE to automatically register the 
    /// assembly's .Net dependencies.
    /// </para>
    /// </remarks>
    /// <param name="assembly">The assembly to inspect</param>
    /// <param name="autoRegister">Autoregister .Net dependencies</param>
    public void RegisterDependencies(Assembly assembly, bool autoRegister = false)
    {
        foreach (AssemblyName dependency in assembly.GetReferencedAssemblies())
        {
            // Skip .NET assemblies
            if (!IsNetAssembly(dependency.Name) && !TryResolve(dependency.Name, out _))
            {
                Register(dependency, dependency.FullName);
            }
        }
    }

    //  determine if the assembly is a .NET assembly
    private static bool IsNetAssembly(string assemblyName)
    {
        // This method checks if the assembly name starts with common .NET prefixes
        return assemblyName.StartsWith("System.", StringComparison.OrdinalIgnoreCase) ||
               assemblyName.StartsWith("Microsoft.", StringComparison.OrdinalIgnoreCase);
    }

}