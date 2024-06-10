using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PracticalTaskSelenium.Pages;
public class SearchPage : BasePage
{
    private readonly By articlesLocator = By.TagName("article");

    public SearchPage(IWebDriver driver, WebDriverWait wait) : base(driver, wait)
    {
    }
    public IList<IWebElement> GetAllArticles()
    {
        return wait.Until(driver => driver.FindElements(articlesLocator));
    }
}