using log4net;
using OpenQA.Selenium;

namespace PracticalTaskSelenium.Business.Pages;

public class PositionPage : BasePage
{
    private readonly By _positionArticleLocator = By.TagName("article");

    public PositionPage(IWebDriver driver, ILog log) : base(driver, log)
    {
    }

    public IList<IWebElement> FindKeywordsInPositionDescription(string keyWord)
    {
        WaitForElementAndReturnIt(_positionArticleLocator);

        string queryResultWithParameter = String.Format("//*[contains(text(), '{0}')]", keyWord);
        _log.Info("The search for position in description has started");
        return _driver.FindElements(By.XPath(queryResultWithParameter));
    }
}