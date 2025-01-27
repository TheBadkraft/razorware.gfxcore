
using System.Reflection;
using RazorWare.GfxCore.Events;
using RazorWare.GfxCore.Extensibility;
using RazorWare.GfxCore.Registries;
using RazorWare.GfxCore.Utilities;

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

    private readonly RegistryManager _registries;
    private readonly ExtensionLoader _loader;

    /// <summary>
    /// Determine if bootstrap in test mode
    /// </summary>
    /// <remarks>
    /// In test mode the bootstrap will only resolve extension dependencies, 
    /// not intializing the extensions.
    /// </remarks>
    protected bool IsTestMode { get; init; }

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
        _registries = new();

        //  now we notify that the bootstrap is initialized
        OnBootstrapInitialize(this);
        _loader = new(_registries);
    }

    /// <summary>
    /// When overriden in a derived class, execute any load logic.
    /// </summary>
    /// <param name="registries">The registry manager</param>
    protected virtual void OnInitialize(RegistryManager registries) { }
    /// <summary>
    /// Log a message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    protected abstract void Log(string message);

    //  make sure we have the basic registries
    private static void OnBootstrapInitialize(GfxBootstrap bootstrap)
    {
        //  these registries are required for basic functionality
        if (!bootstrap._registries.CanResolve<IAssemblyRegistry>())
        {
            throw new InvalidOperationException("Assembly registry not found");
        }
        if (!bootstrap._registries.CanResolve<IEventSourceRegistry>())
        {
            throw new InvalidOperationException("EventSource registry not found");
        }
        if (!bootstrap._registries.CanResolve<ICommandTargetRegistry>())
        {
            throw new InvalidOperationException("CommandTarget registry not found");
        }
        if (!bootstrap._registries.CanResolve<IExtensionRegistry>())
        {
            throw new InvalidOperationException("Extension registry not found");
        }

        //  register assembly dependencies
        var assemblies = bootstrap._registries.Resolve<IAssemblyRegistry>();
        assemblies.Register(Assembly.GetExecutingAssembly());

        bootstrap.OnInitialize(bootstrap._registries);
    }

    /// <summary>
    /// Begin loading the extensions.
    /// </summary>
    internal void LoadExtensions()
    {
        _loader.DiscoverExtensions(out var mods);
    }

    /// <summary>
    /// Loads the bootstrap concrete implementation in GfxCore
    /// </summary>
    /// <param name="testMode">If TRUE, the bootstrap will only resolve dependencies and not initialize the extensions.</param>
    /// <param name="onLoaded">The action to execute when the bootstrap is initialized.</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    internal static GfxBootstrap Load(bool testMode, Action<BootstrapInitializedEvent> onInitialized)
    {
        //  load the assembly
        var loadPath = Path.Combine(Utils.GetExecutingDirectory(), GFX_CORE_DLL);
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
}