using OpenQA.Selenium;

namespace PracticalTaskSelenium.Core.Utilities;

public static class CustomWaiters
{
    public static Func<IWebDriver, IWebElement> IsElementClickable(By locator)
    {
        return driver =>
        {
            try
            {
                var element = driver.FindElement(locator);
                return (element != null && element.Displayed && element.Enabled) ? element : null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        };
    }

    public static Func<IWebDriver, IWebElement> IsElementVisible(By locator)
    {
        return driver =>
        {
            try
            {
                var element = driver.FindElement(locator);
                return (element != null && element.Displayed) ? element : null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        };
    }
}