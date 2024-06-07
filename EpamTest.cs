using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace PracticalTaskSelenium;

[TestFixture]
public class EpamTest
{
    private const string EpamHomePage = "https://www.epam.com";
    private const string DefaultLocation = "All Locations";

    private static IWebDriver driver;
    private static WebDriverWait wait;

    private readonly By careersLinkLocator = By.LinkText("Careers");
    private readonly By keywordFieldLocator = By.Id("new_form_job_search-keyword");
    private readonly By locationFieldLocator = By.XPath("//span[@title='All Locations']");
    private readonly By remoteCheckBoxLocator = By.XPath("//p/input[@name='remote']/following-sibling::label");
    private readonly By findCarrerButtonLocator = By.XPath("//button[contains(text(),'Find')]");
    private readonly By allLocationsSelectionLocator = By.CssSelector("div.os-padding li[title='All Locations']");
    private readonly By lastViewAndApplyButtonLocator = By.XPath("//li[@class='search-result__item'][last()]//a[.='View and apply']");
    private readonly By positionArticleLocator = By.TagName("article");
    private readonly By searchIconLocator = By.ClassName("dark-iconheader-search__search-icon");
    private readonly By searchPanelLocator = By.ClassName("header-search__panel");
    private readonly By searchInputLocator = By.Name("q");
    private readonly By searchFindButtonLocator = By.XPath("//button/descendant::span[contains(text(),'Find')]");
    private readonly By articlesLocator = By.TagName("article");

    [OneTimeSetUp]
    public static void SetUpDriver()
    {      
        driver = new ChromeDriver();
        driver.Manage().Window.Maximize();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5))
        {
            PollingInterval = TimeSpan.FromSeconds(0.25),
            Message = "Element was not found"
        };
    }

    [TestCase("C#", "United States")]
    [TestCase("Python", "Spain")]
    [TestCase("Java", "All Locations")]
    public void ValidatePositionSearchByCriteria(string keyWord, string country)
    {
        driver.Navigate().GoToUrl(EpamHomePage);
        driver.FindElement(careersLinkLocator).Click();

        var keywordField = wait.Until(driver => driver.FindElement(keywordFieldLocator));
        var locationField = driver.FindElement(locationFieldLocator);

        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", keywordField);

        ClickAndSendKeysActions(keywordField, 2, keyWord);

        if (country != DefaultLocation)
        {
            ClickAndSendKeysActions(locationField, 2, country);
            string locationWithTestParameter = String.Format("div.os-padding li[title='All Cities in {0}']", country);
            driver.FindElement(By.CssSelector(locationWithTestParameter)).Click();
        }
        else
        {
            locationField.Click();
            driver.FindElement(allLocationsSelectionLocator).Click();
        }

        driver.FindElement(remoteCheckBoxLocator).Click();
        driver.FindElement(findCarrerButtonLocator).Click();

        wait.Until(driver => driver.FindElement(lastViewAndApplyButtonLocator)).Click();

        wait.Until(driver => driver.FindElements(positionArticleLocator));

        string queryResultWithParameter = String.Format("//*[contains(text(), '{0}')]", keyWord);

        var articlesWithKeyword = driver.FindElements(By.XPath(queryResultWithParameter));

        Assert.That(articlesWithKeyword.Count > 0,
            "Searched keyword wasn't found in the search results");
    }

    [TestCase("BLOCKCHAIN")]
    [TestCase("Cloud")]
    [TestCase("Automation")]
    public void ValidateGlobalSearch(string searchQuery)
    {
        driver.Navigate().GoToUrl(EpamHomePage);
        driver.FindElement(searchIconLocator).Click();

        var searchPanel = wait.Until(driver => driver.FindElement(searchPanelLocator));
        var searchInput = searchPanel.FindElement(searchInputLocator);

        ClickAndSendKeysActions(searchInput, 2, searchQuery);

        searchPanel.FindElement(searchFindButtonLocator).Click();

        IList<IWebElement> links = wait.Until(driver => driver.FindElements(articlesLocator));
        bool allLinksContainSearchedWord = links.All(link => link.Text.ToLower().Contains(searchQuery.ToLower()));

        Assert.That(links.Count > 0 && allLinksContainSearchedWord,
            "Either there are no links in the search results or not all contain searched programming language");
    }

    [OneTimeTearDown]
    public static void TearDownDriver()
    {
        driver.Quit();
    }

    public void ClickAndSendKeysActions(IWebElement element, int pauseDurationSeconds, string testParamater)
    {
        var clickAndSendKeysActions = new Actions(driver);

        clickAndSendKeysActions
            .MoveToElement(element)
            .Click(element)
            .Pause(TimeSpan.FromSeconds(pauseDurationSeconds))
            .SendKeys(testParamater)
            .Pause(TimeSpan.FromSeconds(pauseDurationSeconds))
            .Perform();
    }
}