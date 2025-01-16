namespace RazorWare.GfxCore.Runtime;

/// <summary>
/// Represents a runtime interface.
/// </summary>
public interface IRuntime : IDisposable
{
    /// <summary>
    /// Runs the specified action and launches the application loop.
    /// </summary>
    /// <typeparam name="T">The type of the executable action.</typeparam>
    /// <param name="action">The action to run.</param>
    void Run<T>(Func<T> action) where T : IFacade;
}