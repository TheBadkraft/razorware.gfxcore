
using System.Runtime.CompilerServices;
using RazorWare.GfxCore.Events;
using RazorWare.GfxCore.Registries;
using RazorWare.GfxCore.Runtime;

[assembly: InternalsVisibleTo("razorware.gfxcore")]

namespace RazorWare.GfxCore.Facade;

/// <summary>
/// The base application facade.
/// </summary>
public abstract partial class GfxApplication : IRuntime
{
    private readonly GfxBootstrap _bootstrap;

    private IFacade executable = null;

    /// <summary>
    /// Get the registry manager
    /// </summary>
    public RegistryManager Registries { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GfxApplication"/> class.
    /// </summary>
    protected GfxApplication(bool testMode = false)
    {
        //  load gfx bootstrap
        if ((_bootstrap = GfxBootstrap.Load(testMode, OnBootstrapInitialized)) == null)
        {
            throw new InvalidOperationException("The GfxCore bootstrap failed to load.");
        }
        //  load extensions
        _bootstrap.LoadExtensions();

        if (!testMode)
        {
            //  initialize the application
            OnLoad();
        }
    }

    /// <summary>
    /// Starts the application runtime
    /// </summary>
    /// <typeparam name="T">The facade type</typeparam>
    /// <param name="getExecutable">The function to get the executable</param>
    public virtual void Run<T>(Func<T> getExecutable) where T : IFacade
    {
        if (executable != null)
        {
            //  raise exception that the application is already running
            throw new InvalidOperationException("The application is already running.");
        }
        else  //  otherwise, start the application
        {
            executable = getExecutable();
        }

        //  this run loop will move to the rendering engine
        //  while we work on extension loading, this is our run loop
        while (!executable.IsStopRequested)
        {
            //  run for 3 seconds and then stop
            // Continue with another task after the delay
            _ = Task.Run(async () =>
            {
                await Task.Delay(2500);
                executable.IsStopRequested = true;
            });
        }

        //  close the application
        OnClose();
    }
    /// <summary>
    /// Launches the application
    /// </summary>
    public void Launch()
    {
        OnLaunch();
    }

    /// <summary>
    /// Called when the application is loaded
    /// </summary>
    protected virtual void OnLoad() { }
    /// <summary>
    /// Called when the application is launched
    /// </summary>
    protected virtual void OnLaunch() { }
    /// <summary>
    /// Called when the application is closed
    /// </summary>
    protected virtual void OnClose() { }
    /// <summary>
    /// Log a message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    protected abstract void Log(string message);

    /// <summary>
    /// Disposes of the application
    /// </summary>
    public void Dispose()
    {
        //  clean up any resources
    }

    //  when the bootstrap is initialized
    private void OnBootstrapInitialized(BootstrapInitializedEvent e)
    {
        Registries = e.Registries;
    }
}
