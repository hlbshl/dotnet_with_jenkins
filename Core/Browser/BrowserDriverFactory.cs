using PracticalTaskSelenium.Core.Enums;

namespace PracticalTaskSelenium.Core.Browser;

public class BrowserDriverFactory
{
    public static IBrowserDriver GetBrowserDriver(BrowserType browserType)
    {
        switch (browserType)
        {
            case BrowserType.Chrome:
                return new ChromeBrowserDriver();
            case BrowserType.Firefox:
                return new FirefoxBrowserDriver();
            default:
                throw new ArgumentException("Unsupported browser");
        }
    }
}