
using RazorWare.GfxCore.Windowing;

namespace RazorWare.GfxCore;

/// <summary>
/// The application instance
/// </summary>
public abstract class App : IDisposable
{

    /// <summary>
    /// Starts the application runtime
    /// </summary>
    public virtual void Run<T>(Func<T> action) where T : Window
    {
        action().Launch();
    }
    /// <summary>
    /// Disposes of the application
    /// </summary>
    public void Dispose()
    {
        //  clean up any resources
    }

}
