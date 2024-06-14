using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PracticalTaskSelenium.Core.Browser;

public class ChromeBrowserDriver : IBrowserDriver
{
    public IWebDriver CreateDriver(bool headless, string downloadDirectory)
    {
        return new ChromeDriver(BrowserSettings.ChromeOptions(headless, downloadDirectory));
    }
}