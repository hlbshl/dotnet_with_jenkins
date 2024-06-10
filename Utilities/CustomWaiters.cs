using OpenQA.Selenium;

namespace PracticalTaskSelenium.Utilities;

public static class CustomWaiters
{
    public static Func<IWebDriver, IWebElement> ElementToBeClickable(By locator)
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

    public static Func<IWebDriver, IWebElement> ElementIsVisible(By locator)
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