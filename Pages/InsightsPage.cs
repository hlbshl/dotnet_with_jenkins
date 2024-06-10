using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PracticalTaskSelenium.Utilities;

namespace PracticalTaskSelenium.Pages;

public class InsightsPage : BasePage
{
    private readonly By rightArrowLocator = By.CssSelector("div.slider.section:nth-child(1) button:nth-child(3)");
    private readonly By currentPageInSliderLocator = By.CssSelector("span.slider__pagination--current-page");
    private readonly By carouselTitleLocator = By.XPath("//div[@class='owl-item active']//p/span");
    private readonly By readMoreLinkLocator = By.XPath("//div[@class='owl-item active']//a[contains(@class,'custom-link')]");


    public InsightsPage(IWebDriver driver, WebDriverWait wait) : base(driver, wait)
    {
    }

    public InsightsPage NavigateToCarouselPage(int slide)
    {
        while (true)
        {
            string currentPageInSliderText = wait
                .Until(driver => driver.FindElement(currentPageInSliderLocator))
                .Text
                .Trim();
            int currentPageInSlider = Int32.Parse(currentPageInSliderText);

            if (currentPageInSlider == slide)
            {
                break;
            }

            driver.FindElement(rightArrowLocator).Click();
        }
        return this;
    }

    public ArticlePage SaveTitleAndNavigateToArticle()
    {
        var articleTitle = wait.Until(CustomWaiters.ElementIsVisible(carouselTitleLocator));
        string articleName = articleTitle.Text;
        wait.Until(CustomWaiters.ElementToBeClickable(readMoreLinkLocator)).Click();

        return new ArticlePage(driver, wait, articleName);
    }
}