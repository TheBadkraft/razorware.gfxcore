
using System.Reflection;
using RazorWare.GfxCore.Events;
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

    private readonly RegistryManager _registries;

    /// <summary>
    /// Determine if bootstrap in test mode
    /// </summary>
    /// <remarks>
    /// In test mode the bootstrap will only resolve extension dependencies, 
    /// not intializing the extensions.
    /// </remarks>
    protected bool IsTestMode { get; init; }

    /// <summary>
    /// Get the extension path.
    /// </summary>
    internal DirectoryInfo ExtensionPath => new DirectoryInfo(ext_path);

    /// <summary>
    /// Initializes the GfxCore bootstrap.
    /// </summary>
    /// <param name="testMode">If TRUE, the bootstrap will only resolve dependencies and not initialize the extensions.</param>
    /// <remarks>
    /// In test mode the bootstrap will only resolve extension dependencies, 
    /// not intializing the extensions.
    /// </remarks>
    protected GfxBootstrap(bool testMode)
    {
        IsTestMode = testMode;
        OnLoad(_registries = new());
    }

    /// <summary>
    /// When overriden in a derived class, execute any load logic.
    /// </summary>
    /// <param name="registries">The registry manager</param>
    protected virtual void OnLoad(RegistryManager registries) { }
    /// <summary>
    /// Log a message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    protected abstract void Log(string message);

    //  load the extensions - if testMode, only resolve dependencies,
    //  do not initialize the extensions
    internal void Initialize()
    {
        Log($"[GfxCore :: Bootstrap] Begin Extension Discovery");

        var curr_dir = string.Empty;
        Log($"{"",15}Current Directory: {curr_dir = Directory.GetCurrentDirectory()}");
        Log($"{"",15}Extension Path: {ext_path = Path.Combine(curr_dir, EXTENSION_PATH)}");

        if (!Directory.Exists(ext_path))
        {
            Log($"{"",15}Creating Extension Path: {ext_path}");
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
    internal static GfxBootstrap Load(bool testMode, Action<BootstrapInitializedEvent> onInitialized)
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
        var bootstrap = ctor != null ? (GfxBootstrap)ctor.Invoke(args) : throw new InvalidOperationException("A valid bootstrap constuctor was not found.");
        onInitialized(new BootstrapInitializedEvent(bootstrap._registries)
        {
            Sender = bootstrap,
        });
        bootstrap.Log("[GfxCore :: Bootstrap] Bootstrap Initialized");

        return bootstrap;
    }

    //  get the executing directory
    private static string GetExecutingDirectory()
    {
        var location = new Uri(Assembly.GetExecutingAssembly().Location);
        return new FileInfo(location.AbsolutePath).Directory.FullName;
    }
}