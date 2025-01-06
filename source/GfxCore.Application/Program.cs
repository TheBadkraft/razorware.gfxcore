// See https://aka.ms/new-console-template for more information

using (var app = new Application())
{
    app.Run(() => new AppWindow(new(0, 0), new(800, 600)) {
        //Title = "GfxCore.Application"
    });
}
