
using System.Runtime.CompilerServices;
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
    /// Initializes a new instance of the <see cref="GfxApplication"/> class.
    /// </summary>
    protected GfxApplication(bool testMode = false)
    {
        _bootstrap = GfxBootstrap.Load(testMode);

        if (!testMode)
        {
            //  initialize the application
        }
    }

    /// <summary>
    /// Resolves a registry type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IRegistry<T> ResolveRegistry<T>()
    {
        return _bootstrap.ResolveRegistry<T>();
    }
    /// <summary>
    /// Starts the application runtime
    /// </summary>
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
    }

    /// <summary>
    /// Disposes of the application
    /// </summary>
    public void Dispose()
    {
        //  clean up any resources
    }

}
