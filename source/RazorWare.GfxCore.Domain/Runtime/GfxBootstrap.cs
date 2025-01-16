
using System.Reflection;

using RazorWare.GfxCore.Extensibility.Logging;
using RazorWare.GfxCore.Registries;

namespace RazorWare.GfxCore.Runtime;

/*
    This is the GfxCore.Domain bootstrap implementation.
*/

/// <summary>
/// The base class for the GfxCore bootstrap.
/// </summary>
internal abstract class GfxBootstrap
{
    //  bootstrap the domain and load extensions

    //  assumption: the gfxcore.dll is in the same directory as the domain
    private const string GFX_CORE_DLL = "razorware.gfxcore.dll";
    private const string EXTENSION_PATH = "mods";

    private static string ext_path;

    //  the list of extensions
    private static readonly List<string> _extensions = new List<string>();

    private bool IsTesMode { get; init; }

    /// <summary>
    /// The registry manager
    /// </summary>
    protected readonly RegistryManager _registries;
    /// <summary>
    /// The default logger
    /// </summary>
    protected ILogger Logger { get; init; }

    /// <summary>
    /// Get the extension path.
    /// </summary>
    internal DirectoryInfo ExtensionPath => new DirectoryInfo(ext_path);
    /// <summary>
    /// Get the registries.
    /// </summary>
    internal RegistryManager Registries => _registries;

    /// <summary>
    /// Initializes the GfxCore bootstrap.
    /// </summary>
    /// <param name="testMode">If true, the bootstrap will only resolve dependencies and not initialize the extensions.</param>
    protected GfxBootstrap(bool testMode)
    {
        IsTesMode = testMode;
        _registries = new();
    }


    //  load the extensions - if testMode, only resolve dependencies,
    //  do not initialize the extensions
    internal void Initialize()
    {
        Logger.Log($"[GfxCore :: Bootstrap] Begin Extension Discovery");

        var curr_dir = string.Empty;
        Logger.Log($"{"",15}Current Directory: {curr_dir = Directory.GetCurrentDirectory()}");
        Logger.Log($"{"",15}Extension Path: {ext_path = Path.Combine(curr_dir, EXTENSION_PATH)}");

        if (!Directory.Exists(ext_path))
        {
            Logger.Log($"{"",15}Creating Extension Path: {ext_path}");
            Directory.CreateDirectory(ext_path);
        }

        //  load the extensions

        // LoadExtensions();
    }
    /// <summary>
    /// Resolves a registry by type: ex. IGfxSystem, IGfxResource
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    internal IRegistry<T> ResolveRegistry<T>()
    {
        if (!_registries.TryResolve(typeof(T), out var registry))
        {
            throw new ArgumentException($"{typeof(T)} is not found in registries");
        }

        return (IRegistry<T>)registry;
    }

    /// <summary>
    /// Loads the bootstrap concrete implementation in GfxCore
    /// </summary>
    /// <param name="testMode"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    internal static GfxBootstrap Load(bool testMode)
    {
        //  load the assembly
        var loadPath = Path.Combine(GetExecutingDirectory(), GFX_CORE_DLL);
        var assembly = Assembly.LoadFrom(loadPath);
        var type = default(Type);

        //  find the internal sealed class assignable from GfxBootstrap and should only be one
        try
        {
            type = assembly
                .GetTypes()
                .Single(t => typeof(GfxBootstrap)
                //  if single is null then this will throw an exception
                .IsAssignableFrom(t) && t.IsClass && t.IsSealed && t.IsNotPublic);
        }
        catch (Exception)
        {
            //  a valid bootstrap implementation was not found
            throw;
        }

        //  build the args
        object[] args = new object[] { testMode };
        //  find the type internal constructor
        ConstructorInfo ctor = type
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single(c => c.GetParameters().Length == 1 && c.GetParameters()[0].ParameterType == typeof(bool));
        //  return the instance            
        return ctor != null ? (GfxBootstrap)ctor.Invoke(args) : throw new InvalidOperationException("A valid bootstrap constuctor was not found.");
    }

    //  get the executing directory
    private static string GetExecutingDirectory()
    {
        var location = new Uri(Assembly.GetExecutingAssembly().Location);
        return new FileInfo(location.AbsolutePath).Directory.FullName;
    }
}