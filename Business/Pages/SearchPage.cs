using log4net;
using OpenQA.Selenium;

namespace PracticalTaskSelenium.Business.Pages;
public class SearchPage : BasePage
{
    private readonly By _articlesLocator = By.TagName("article");

    public SearchPage(IWebDriver driver, ILog log) : base(driver, log)
    {
    }
    public IList<IWebElement> GetAllArticles()
    {
        _log.Info("The list of articles is returned");
        return Wait.Until(_driver => _driver.FindElements(_articlesLocator));
    }
}