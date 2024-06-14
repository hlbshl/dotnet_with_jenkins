using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace PracticalTaskSelenium.Core.Utilities;

public static class ActionsHelper
{
    public static void ClickAndSendKeys(IWebDriver driver, IWebElement element, int pauseDurationSeconds, string testParameter, ILog log)
    {
        var clickAndSendKeysActions = new Actions(driver);

        log.Info($"Page is scrolled to {element.Text} to send {testParameter} keys");

        clickAndSendKeysActions
            .ScrollToElement(element)
            .Click(element)
            .Pause(TimeSpan.FromSeconds(pauseDurationSeconds))
            .SendKeys(testParameter)
            .Pause(TimeSpan.FromSeconds(pauseDurationSeconds))
            .Perform();
    }

    public static void ScrollTo(IWebDriver driver, IWebElement element, ILog log)
    {
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        log.Info($"The page is scrolled to {element.Location}");

        var scrollToElement = new Actions(driver);

        scrollToElement
            .ScrollToElement(element)
            .Perform();
    }

    public static void ClickWithPause(IWebDriver driver, IWebElement element, int pauseDurationSeconds, ILog log)
    {
        var clickWithPause = new Actions(driver);

        log.Info($"{element.Location} is clicked with a delay");

        clickWithPause
            .Click(element)
            .Pause(TimeSpan.FromSeconds(pauseDurationSeconds))
            .Perform();
    }
}