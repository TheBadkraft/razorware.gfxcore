
using RazorWare.GfxCore.Runtime;

namespace RazorWare.GfxCore.Facade;

/// <summary>
/// The base application facade.
/// </summary>
public abstract partial class GfxApplication : IRuntime
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GfxApplication"/> class.
    /// </summary>
    protected GfxApplication()
    {
        //  initialize the application
    }

    /// <summary>
    /// Starts the application runtime
    /// </summary>
    public virtual void Run<T>(Func<T> action) where T : IExecutable
    {
        while (true)
        {
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
