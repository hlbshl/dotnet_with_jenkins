using log4net;
using OpenQA.Selenium;

namespace PracticalTaskSelenium.Business.Pages;

public class ArticlePage : BasePage
{
    private readonly By _articleTitleLocator = By.XPath("//span[@class='font-size-80-33']");
    public string ArticleName { get; private set; }
    public ArticlePage(IWebDriver driver, ILog log, string articleName) : base(driver, log)
    {
        ArticleName = articleName;
    }
    public string GetArticleTitle()
    {
        string articleTitle = WaitForElementAndReturnIt(_articleTitleLocator).Text;
        _log.Info("Article title is following: " + articleTitle);
        return articleTitle;
    }
}