using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PracticalTaskSelenium.Utilities;

namespace PracticalTaskSelenium.Pages;
public class BasePage
{
    private static string Url { get; } = "https://www.epam.com";
    private string downloadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Downloads");
    private readonly By searchIconLocator = By.ClassName("dark-iconheader-search__search-icon");
    private readonly By searchPanelLocator = By.ClassName("header-search__panel");
    private readonly By searchInputLocator = By.Name("q");
    private readonly By searchFindButtonLocator = By.XPath("//button/descendant::span[contains(text(),'Find')]");
    private readonly By cookiesPanelLocator = By.CssSelector("#onetrust-banner-sdk");
    private readonly By acceptCookiesButtonLocator = By.CssSelector("#onetrust-accept-btn-handler");

    protected IWebDriver driver;
    protected WebDriverWait wait;
    private static bool cookiesAccepted = false;

    public BasePage(IWebDriver driver, WebDriverWait wait)
    {
        this.driver = driver ?? throw new ArgumentException(nameof(driver));
        this.wait = wait ?? throw new ArgumentException(nameof(wait));
    }

    public BasePage Open()
    {
        driver.Url = Url;
        return this;
    }

    public BasePage AcceptCookies()
    {
        if (!cookiesAccepted)
        {
            wait.Until(CustomWaiters.ElementIsVisible(cookiesPanelLocator));
            wait.Until(CustomWaiters.ElementToBeClickable(acceptCookiesButtonLocator)).Click();
            cookiesAccepted = true;
        }
        return this;
    }

    public T NavigateToPage<T>(string pageLink) where T : BasePage
    {
        By linkLocator = By.LinkText($"{pageLink}");
        wait.Until(driver => driver.FindElement(linkLocator)).Click();

        return (T)Activator.CreateInstance(typeof(T), driver, wait);
    }

    public SearchPage PerformSearchByKeyword(string searchQuery)
    {
        driver.FindElement(searchIconLocator).Click();

        var searchPanel = wait.Until(driver => driver.FindElement(searchPanelLocator));
        var searchInput = searchPanel.FindElement(searchInputLocator);

        ActionsHelper.ClickAndSendKeysActions(driver, searchInput, 2, searchQuery);

        searchPanel.FindElement(searchFindButtonLocator).Click();

        return new SearchPage(driver, wait);
    }

    public bool WaitForFileDownloadAndValidate(string fileName, int timeoutInSeconds)
    {
        var endTime = DateTime.Now.AddSeconds(timeoutInSeconds);
        while (DateTime.Now < endTime)
        {
            if (File.Exists(Path.Combine(downloadDirectory, fileName)))
            {
                return true;
            }
            Thread.Sleep(1000);
        }
        return false;
    }
}