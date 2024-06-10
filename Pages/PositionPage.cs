using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PracticalTaskSelenium.Pages;

public class PositionPage : BasePage
{
    private readonly By positionArticleLocator = By.TagName("article");

    public PositionPage(IWebDriver driver, WebDriverWait wait) : base(driver, wait)
    {
    }

    public IList<IWebElement> FindKeywordsInPositionDescription(string keyWord)
    {
        wait.Until(driver => driver.FindElements(positionArticleLocator));

        string queryResultWithParameter = String.Format("//*[contains(text(), '{0}')]", keyWord);

        IList<IWebElement> articlesWithKeyword = driver.FindElements(By.XPath(queryResultWithParameter));
        return articlesWithKeyword;
    }
}