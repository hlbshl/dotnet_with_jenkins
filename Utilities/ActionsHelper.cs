using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace PracticalTaskSelenium.Utilities;

public static class ActionsHelper
{
    public static void ClickAndSendKeysActions(IWebDriver driver, IWebElement element, int pauseDurationSeconds, string testParamater)
    {
        var clickAndSendKeysActions = new Actions(driver);

        clickAndSendKeysActions
            .ScrollToElement(element)
            .Click(element)
            .Pause(TimeSpan.FromSeconds(pauseDurationSeconds))
            .SendKeys(testParamater)
            .Pause(TimeSpan.FromSeconds(pauseDurationSeconds))
            .Perform();
    }

    public static void ScrollToElement(IWebDriver driver, IWebElement element)
    {
        var scrollToElement = new Actions(driver);

        scrollToElement
            .ScrollToElement(element)
            .Perform();
    }
}