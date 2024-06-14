using log4net;
using OpenQA.Selenium;

namespace PracticalTaskSelenium.Core.Browser;

public sealed class DriverFactory
{
    private static readonly DriverFactory _instance = new DriverFactory();
    private IWebDriver _driver;
    private DriverFactory() { }

    public static DriverFactory Instance
    {
        get
        {
            return _instance;
        }
    }

    public IWebDriver GetDriver(IBrowserDriver browserDriver, bool headless, string downloadDirectory, ILog log)
    {
        if (_driver == null)
        {
            _driver = browserDriver.CreateDriver(headless, downloadDirectory);
        }
        return _driver;
    }

    public void QuitDriver()
    {
        if (_driver != null)
        {
            _driver.Quit();
            _driver = null;
        }
    }
}
