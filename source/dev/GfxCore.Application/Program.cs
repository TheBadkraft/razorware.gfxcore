// See https://aka.ms/new-console-template for more information

using RazorWare.GfxCore.Extensibility;
using RazorWare.GfxCore.Extensibility.Logging;
using RazorWare.GfxCore.Facade;
using RazorWare.GfxCore.Registries;
using RazorWare.GfxCore.Runtime;


using (var app = new Application())
{
    app.Launch();
}


/*
    This is the application class that will initialize the GfxCore runtime 
    from the base GfxApplication.
*/
#nullable disable
public class Application : GfxApplication
{
    public IServiceRegistry GfxServices => Registries.Resolve<IServiceRegistry>();

    private ILogger Logger { get; set; }

    protected override void OnLoad()
    {
        //  get the default logger
        Logger = GfxServices.Resolve<ILogger>();
    }

    protected override void OnLaunch()
    {
        //  run the application
        Run(() => new Applet(Logger));
    }
}

/*
    This class is a simple example of an application facade. There is
    no window ... just the console. Launching the framework, we 
    can create a simple application facade that will load the GfxCore 
    runtime, intialize its registries, and then run the application.
*/
public class Applet : IFacade
{
    private ILogger Logger { get; init; }

    /// <summary>
    /// Get or set the flag indicating if the application should stop.
    /// </summary>
    public bool IsStopRequested { get; set; } = false;

    //  this applet could be anything, but for now, it's just a logger
    public Applet(ILogger logger)
    {
        Logger = logger;

        //  gfxcore application will test the extension loading and dependency resolution
        Logger.Log("[GfxCore :: Application] Loaded");
    }

}