// See https://aka.ms/new-console-template for more information

using RazorWare.GfxCore;
using RazorWare.GfxCore.Graphics;

public class Application : App
{

}

public class AppWindow : Window
{
    public AppWindow(Vector position, Vector size) : base(position, size)
    {
        Title = "AppWindow";
    }
}
