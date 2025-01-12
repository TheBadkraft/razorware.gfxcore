// See https://aka.ms/new-console-template for more information

using RazorWare.GfxCore.Facade;
using RazorWare.GfxCore.Graphics;
using RazorWare.GfxCore.Windowing;

public class Application : RazorWare.GfxCore.Facade.Application
{

}

public class AppWindow : Window
{
    public AppWindow(Vector position, Vector size) : base(position, size)
    {

    }
}
