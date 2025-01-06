using System.ComponentModel;

using RazorWare.GfxCore.Graphics;

namespace RazorWare.GfxCore.Windowing;

/// <summary>
/// The application window
/// </summary>
public abstract class Window
{
    private GameWindow glWindow;

    public string Title
    {
        get => glWindow.Title;
        set => glWindow.Title = value;
    }
    public Vector Size { get; private set; }
    public Vector Position
    {
        get => new(glWindow.Location.X, glWindow.Location.Y);
        set => glWindow.Location = new((int)value.X, (int)value.Y);
    }
    public string Name => Title;

    protected Window(Vector position, Vector size)
    {
        glWindow = new GameWindow(GameWindowSettings.Default, new NativeWindowSettings()
        {
            Size = new((int)size.X, (int)size.Y),
            Title = "GfxCore.Window"
        });
        glWindow.Load += OnLoad;
        glWindow.RenderFrame += OnRenderFrame;
        glWindow.Resize += OnResize;
        glWindow.Closing += OnClosing;
    }

    /// <summary>
    /// Launches the window
    /// </summary>
    internal void Launch()
    {
        glWindow.Run();
    }

    /// <summary>
    /// Called when the window is loaded
    /// </summary>
    protected virtual void OnLoad()
    {
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);      //  set clear color
    }

    protected virtual void OnResize(ResizeEventArgs args)
    {
        GL.Viewport(0, 0, args.Width, args.Height);  //  set viewport
        Size = new(args.Width, args.Height);         //  set size
    }

    private void OnClosing(CancelEventArgs args)
    {
        //  nothing to do here ...
    }

    /// <summary>
    /// Called when the window is rendered
    /// </summary>
    /// <param name="args">The frame arguments</param>
    private void OnRenderFrame(FrameEventArgs args)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);    //  clear color buffer
        Render();
        glWindow.SwapBuffers();                      //  swap buffers
    }

    /// <summary>
    /// Renders the window
    /// </summary>
    protected virtual void Render()
    {

    }
}
