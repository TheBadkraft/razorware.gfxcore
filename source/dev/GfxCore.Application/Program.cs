// See https://aka.ms/new-console-template for more information

using RazorWare.GfxCore.Facade;
using RazorWare.GfxCore.Runtime;


using (var app = new Application())
{
    app.Run(() => new Applet()
    {
    });
}


public class Application : GfxApplication
{

}

public class Applet : IExecutable
{
    public Applet()
    {
        //  gfxcore application will test the extension loading and dependency resolution
        Console.WriteLine("GfxCore Application Loading ...");
    }
}