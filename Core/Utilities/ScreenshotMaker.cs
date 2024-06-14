using log4net;
using OpenQA.Selenium;

namespace PracticalTaskSelenium.Core.Utilities;
public class ScreenshotMaker
{
    private static string NewScreenshotName
    {
        get { return "_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss-fff") + ".png"; }
    }
    public static Screenshot CaptureBrowserScreenshot(IWebDriver driver, ILog log)
    {
        log.Info("Screenshot is taken");
        return ((ITakesScreenshot)driver).GetScreenshot();
    }

    public static string SaveScreenshot(Screenshot screenshot, ILog log)
    {
        var screenshotPath = Path.Combine(Directory.GetCurrentDirectory(), "Display" + NewScreenshotName);
        log.Info($"Screenshot is being saved with the following name: Display{NewScreenshotName}");
        screenshot.SaveAsFile(screenshotPath);
        return screenshotPath;
    }
}
