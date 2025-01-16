
using MindForge.TestRunner.Logging;
// using RazorWare.GfxCore.Fonts;

namespace RazorWare.GfxCore.Testing;

[TestContainer]
public class FontManagerTests
{
    // private FontManager FontManager { get; set; }

    public static TestContext TestContext { get; set; }

    #region Tests
    [Test]
    public void TestTest()
    {
        string[] expFonts = { "DejaVuSans", "LiberationMono-Bold", "Quicksand-Medium" };
        // Assert.ContainsAll(FontManager.FontNames, expFonts);
    }

    #endregion Tests

    #region Setup
    [SetUp]
    public void Setup()
    {
        //  Switched to FreeTypeSharp ...
        try
        {
            // FontManager = new FontManager(false);
        }
        catch (Exception ex)
        {
            TestContext.Logger.Log(DebugLevel.Error, ex.Message);
        }
    }
    [TearDown]
    public void TearDown()
    {
        // FontManager?.Dispose();
        // FontManager = null;
    }
    #endregion Setup
    #region Initialize Container
    [ContainerInitialize]
    public static void Initialize()
    {

    }
    [ContainerCleanUp]
    public static void CleanUp()
    {

    }
    #endregion Initialize Container
}
