
using RazorWare.GfxCore.Rendering;
using RazorWare.GfxCore.Windowing;

namespace RazorWare.GfxCore.Facade;

/// <summary>
/// The base application facade.
/// </summary>
public abstract partial class Application
{
    private Window CurrentWindow { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Application"/> class.
    /// </summary>
    protected Application()
    {
        //  initialize the application
    }

    /// <summary>
    /// Starts the application runtime
    /// </summary>
    public virtual void Run(Func<Window> action)
    {
        while (true)
        {
            if (CurrentWindow == null) break;

            CurrentWindow.Update();
            CurrentWindow.Render();

            UpdateCurrentView();
            RenderCurrentView();

            // Check for exit conditions or break loop
            if (CheckExitCondition()) break;
        }
    }
    protected abstract bool CheckExitCondition();

    protected virtual void OnWindowChanged(WindowEventArgs e)
    {
        WindowChanged?.Invoke(this, e);
    }

    public event EventHandler<WindowEventArgs> WindowChanged;

    protected override void RenderCurrentView()
    {
        if (CurrentView != null)
        {
            CurrentView.Render();
        }
    }

    protected override void UpdateCurrentView()
    {
        if (CurrentView != null)
        {
            CurrentView.Update();
        }
    }
    protected void SetCurrentWindow(Window window)
    {
        CurrentWindow = window;

    }
    /// <summary>
    /// Disposes of the application
    /// </summary>
    public void Dispose()
    {
        //  clean up any resources
    }

}
