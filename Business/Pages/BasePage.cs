using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PracticalTaskSelenium.Core.Utilities;

namespace PracticalTaskSelenium.Business.Pages;

public class BasePage
{
    private static string Url { get; } = "https://www.epam.com";
    private string _downloadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Downloads");
    private readonly By _searchIconLocator = By.ClassName("dark-iconheader-search__search-icon");
    private readonly By _searchPanelLocator = By.ClassName("header-search__panel");
    private readonly By _searchInputLocator = By.Name("q");
    private readonly By _searchFindButtonLocator = By.XPath("//button/descendant::span[contains(text(),'Find')]");
    private readonly By _cookiesPanelLocator = By.CssSelector("#onetrust-banner-sdk");
    private readonly By _acceptCookiesButtonLocator = By.CssSelector("#onetrust-accept-btn-handler");

    protected IWebDriver _driver;
    protected ILog _log;
    protected WebDriverWait Wait { get; set; }
    private static bool _cookiesAccepted = false;

    public BasePage(IWebDriver driver, ILog log)
    {
        _driver = driver ?? throw new ArgumentException(nameof(driver));
        Wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5))
        {
            PollingInterval = TimeSpan.FromSeconds(0.25),
            Message = "Element was not found"
        };
        _log = log ?? throw new ArgumentException(nameof(log));
    }

    public BasePage Open()
    {
        _driver.Url = Url;
        _log.Info(Url + " is opened");
        return this;
    }

    public BasePage AcceptCookies()
    {
        if (!_cookiesAccepted)
        {
            Wait.Until(CustomWaiters.IsElementVisible(_cookiesPanelLocator));
            Wait.Until(CustomWaiters.IsElementClickable(_acceptCookiesButtonLocator)).Click();
            _cookiesAccepted = true;
            _log.Info("Cookies were accepted");
        }
        return this;
    }

    public BasePage OpenBasePageAndAcceptCookies()
    {
        return this.Open().AcceptCookies();
    }

    public T NavigateToPage<T>(string pageLink) where T : BasePage
    {
        _log.Info($"Opening '{pageLink}' page");
        By linkLocator = By.LinkText($"{pageLink}");
        WaitForElementAndReturnIt(linkLocator).Click();

        return (T)Activator.CreateInstance(typeof(T), _driver, _log);
    }

    public IWebElement WaitForElementAndReturnIt(By locator)
    {
        _log.Info($"Waiting for {locator.ToString()} to appear");
        return Wait.Until(_driver => _driver.FindElement(locator));
    }

    public SearchPage PerformSearchByKeyword(string searchQuery)
    {
        _log.Info($"Search by '{searchQuery}' will be done");
        _driver.FindElement(_searchIconLocator).Click();

        var searchPanel = WaitForElementAndReturnIt(_searchPanelLocator);
        var searchInput = searchPanel.FindElement(_searchInputLocator);

        ActionsHelper.ClickAndSendKeys(_driver, searchInput, 2, searchQuery, _log);

        searchPanel.FindElement(_searchFindButtonLocator).Click();

        return new SearchPage(_driver, _log);
    }

    public bool WaitForFileDownloadAndValidate(string fileName, int timeoutInSeconds)
    {
        _log.Info("Waiting for a file to be downloaded");
        var endTime = DateTime.Now.AddSeconds(timeoutInSeconds);
        while (DateTime.Now < endTime)
        {
            if (File.Exists(Path.Combine(_downloadDirectory, fileName)))
            {
                _log.Info("The file is downloaded");
                return true;
            }
            Thread.Sleep(1000);
        }
        return false;
    }
}