using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using PracticalTaskSelenium.Core.Browser;

public class FirefoxBrowserDriver : IBrowserDriver
{
    public IWebDriver CreateDriver(bool headless, string downloadDirectory)
    {
        return new FirefoxDriver(BrowserSettings.FirefoxOptions(headless, downloadDirectory));
    }
}