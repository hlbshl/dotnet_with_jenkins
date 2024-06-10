using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PracticalTaskSelenium.Pages;

public class ArticlePage : BasePage
{
    private readonly By articleTitleLocator = By.XPath("//span[@class='font-size-80-33']");
    public string ArticleName { get; private set; }
    public ArticlePage(IWebDriver driver, WebDriverWait wait, string articleName) : base(driver, wait)
    {
        ArticleName = articleName;
    }
    public string GetArticleTitle()
    {
        Thread.Sleep(10000);
        string articleName = wait.Until(driver => driver.FindElement(articleTitleLocator)).Text;
        return articleName;
    }
}