using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using PracticalTaskSelenium.Pages;

namespace PracticalTaskSelenium.Tests;

[TestFixture(false)]
public class EpamTest
{
    private IWebDriver driver;
    private WebDriverWait wait;
    private BasePage basePage;
    private SearchPage searchPage;
    private PositionPage positionPage;
    private InsightsPage insightsPage;
    private ArticlePage articlePage;
    private bool headless;
    private string downloadDirectory;

    public EpamTest(bool headless)
    {
        this.headless = headless;
    }

    [OneTimeSetUp]
    public void SetUpDriver()
    {
        downloadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Downloads");

        if (!Directory.Exists(downloadDirectory))
        {
            Directory.CreateDirectory(downloadDirectory);
        }

        var options = new ChromeOptions();

        if (headless)
        {
            options.AddArgument("--headless");
            options.AddArgument("--window-size=1920,1080");
        }

        options.AddUserProfilePreference("download.default_directory", downloadDirectory);
        options.AddUserProfilePreference("download.prompt_for_download", false);
        options.AddUserProfilePreference("download.directory_upgrade", true);
        options.AddUserProfilePreference("safebrowsing.enabled", true);

        driver = new ChromeDriver(options);
        driver.Manage().Window.Maximize();

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5))
        {
            PollingInterval = TimeSpan.FromSeconds(0.25),
            Message = "Element was not found"
        };

        basePage = new BasePage(driver, wait);
        insightsPage = new InsightsPage(driver, wait);
        searchPage = new SearchPage(driver, wait);
        positionPage = new PositionPage(driver, wait);
    }

    [TestCase("C#", "United States")]
    [TestCase("Python", "Spain")]
    [TestCase("Java", "All Locations")]
    public void ValidatePositionSearchByCriteria(string keyword, string country)
    {

        basePage
            .Open()
            .AcceptCookies()
            .NavigateToPage<CareersPage>("Careers")
            .SearchPositionByKeywordAndLocation(keyword, country)
            .OpenLastPositionInResults();

        Assert.That(positionPage.FindKeywordsInPositionDescription(keyword).Count > 0,
            "Searched keyword wasn't found in the search results");
    }

    [TestCase("BLOCKCHAIN")]
    [TestCase("Automation")]
    [TestCase("Cloud")]
    public void ValidateGlobalSearch(string searchQuery)
    {
        basePage
            .Open()
            .PerformSearchByKeyword(searchQuery);

        IList<IWebElement> links = searchPage.GetAllArticles();
        bool allLinksContainSearchedWord = links.All(link => link.Text.ToLower().Contains(searchQuery.ToLower()));

        Assert.That(searchPage.GetAllArticles().Count > 0 && allLinksContainSearchedWord,
            "Either there are no links in the search results or not all contain searched programming language");
    }

    [TestCase(3)]
    [TestCase(4)]
    [TestCase(5)]
    public void CarouselTitleEqualToArticleTitleAtSlide(int slide)
    {
        basePage
            .Open()
            .AcceptCookies()
            .NavigateToPage<InsightsPage>("Insights")
            .NavigateToCarouselPage(slide);

        articlePage = insightsPage.SaveTitleAndNavigateToArticle();

        Assert.That(articlePage.GetArticleTitle().Trim(), Is.EqualTo(articlePage.ArticleName.Trim()));
    }

    [TestCase("EPAM_Corporate_Overview_Q4_EOY.pdf")]
    public void DownloadedFileNameIsExpected(string fileName)
    {
        basePage
            .Open()
            .AcceptCookies()
            .NavigateToPage<AboutPage>("About")
            .ScrollToEpamAtGlanceSectionAndDownloadOverviewPDF();

        bool isDownloaded = basePage.WaitForFileDownloadAndValidate(fileName, 60);
        Assert.That(isDownloaded);
    }

    [OneTimeTearDown]
    public void TearDownDriver()
    {
        driver.Quit();

        if (Directory.Exists(downloadDirectory))
        {
            Directory.Delete(downloadDirectory, true);
        }
    }
}