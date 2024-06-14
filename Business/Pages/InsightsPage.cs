using log4net;
using OpenQA.Selenium;
using PracticalTaskSelenium.Core.Utilities;

namespace PracticalTaskSelenium.Business.Pages;

public class InsightsPage : BasePage
{
    private readonly By _rightArrowLocator = By.CssSelector("div.slider.section:nth-child(1) button:nth-child(3)");
    private readonly By _currentPageInSliderLocator = By.CssSelector("span.slider__pagination--current-page");
    private readonly By _carouselTitleLocator = By.XPath("//div[@class='owl-item active']//p/span");
    private readonly By _readMoreLinkLocator = By.XPath("//div[@class='owl-item active']//a[contains(@class,'custom-link')]");


    public InsightsPage(IWebDriver driver, ILog log) : base(driver, log)
    {
    }

    public InsightsPage NavigateToCarouselPage(int slide)
    {
        while (true)
        {
            string currentPageInSliderText = WaitForElementAndReturnIt(_currentPageInSliderLocator)
                .Text
                .Trim();
            int currentPageInSlider = Int32.Parse(currentPageInSliderText);

            if (currentPageInSlider == slide)
            {
                _log.Debug($"The slider is on {slide} slide");
                break;
            }

            var rightArrow = _driver.FindElement(_rightArrowLocator);
            ActionsHelper.ClickWithPause(_driver, rightArrow, 1, _log);
        }
        return this;
    }

    public ArticlePage SaveTitleAndNavigateToArticle()
    {
        _log.Info("Navigating to article");
        var articleTitle = Wait.Until(CustomWaiters.IsElementVisible(_carouselTitleLocator));
        string articleName = articleTitle.Text;
        Wait.Until(CustomWaiters.IsElementClickable(_readMoreLinkLocator)).Click();

        return new ArticlePage(_driver, _log, articleName);
    }
}