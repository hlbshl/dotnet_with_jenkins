using OpenQA.Selenium;
using PracticalTaskSelenium.Business.Pages;

namespace PracticalTaskSelenium.Tests;

[TestFixture]
public class EpamTest : BaseTest
{
    private BasePage _basePage;
    private SearchPage _searchPage;
    private PositionPage _positionPage;
    private InsightsPage _insightsPage;
    private ArticlePage _articlePage;

    protected override void InitializePages()
    {
        _basePage = new BasePage(_driver, Log);
        _insightsPage = new InsightsPage(_driver, Log);
        _searchPage = new SearchPage(_driver, Log);
        _positionPage = new PositionPage(_driver, Log);
        Log.Info("The pages were initialized");
    }

    [TestCase("C#", "United States")]
    [TestCase("Python", "Spain")]
    [TestCase("Java", "All Locations")]
    public void ValidatePositionSearchByCriteria(string keyword, string country)
    {

        _basePage
            .OpenBasePageAndAcceptCookies()
            .NavigateToPage<CareersPage>("Careers")
            .SearchPositionByKeywordAndLocation(keyword, country)
            .OpenLastPositionInResults();

        Assert.That(_positionPage.FindKeywordsInPositionDescription(keyword).Count > 0,
            "Searched keyword wasn't found in the search results");
    }

    [TestCase("BLOCKCHAIN")]
    [TestCase("Automation")]
    [TestCase("Cloud")]
    public void ValidateGlobalSearch(string searchQuery)
    {
        _basePage
            .OpenBasePageAndAcceptCookies()
            .PerformSearchByKeyword(searchQuery);

        IList<IWebElement> links = _searchPage.GetAllArticles();
        bool allLinksContainSearchedWord = links.All(link => link.Text.ToLower().Contains(searchQuery.ToLower()));

        Assert.That(_searchPage.GetAllArticles().Count > 0 && allLinksContainSearchedWord,
            "Either there are no links in the search results or not all contain searched programming language");
    }

    [TestCase(3)]
    [TestCase(4)]
    [TestCase(5)]
    public void CarouselTitleEqualToArticleTitleAtSlide(int slide)
    {
        _basePage
            .OpenBasePageAndAcceptCookies()
            .NavigateToPage<InsightsPage>("Insights")
            .NavigateToCarouselPage(slide);

        _articlePage = _insightsPage.SaveTitleAndNavigateToArticle();

        Assert.That(_articlePage.GetArticleTitle().Trim(), Is.EqualTo(_articlePage.ArticleName.Trim()));
    }

    [TestCase("EPAM_Corporate_Overview_Q4_EOY.pdf")]
    public void DownloadedFileNameIsExpected(string fileName)
    {
        _basePage
            .OpenBasePageAndAcceptCookies()
            .NavigateToPage<AboutPage>("About")
            .ScrollToEpamAtGlanceSectionAndDownloadOverviewPDF();

        bool isDownloaded = _basePage.WaitForFileDownloadAndValidate(fileName, 60);

        Assert.That(isDownloaded);
    }
}