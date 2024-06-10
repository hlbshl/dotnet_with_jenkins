using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PracticalTaskSelenium.Utilities;


namespace PracticalTaskSelenium.Pages;

public class AboutPage : BasePage
{
    private readonly By epamAtGlanceLocator = By.XPath("//span[contains(text(), 'EPAM at')]");
    private readonly By downloadButtonLocator = By.XPath("//span[contains(text(), 'DOWNLOAD')]");
    public AboutPage(IWebDriver driver, WebDriverWait wait) : base(driver, wait)
    {
    }
    public AboutPage ScrollToEpamAtGlanceSectionAndDownloadOverviewPDF()
    {
        var atGlanceLocation = wait.Until(driver => driver.FindElement(epamAtGlanceLocator));
        ActionsHelper.ScrollToElement(driver, atGlanceLocation);
        driver.FindElement(downloadButtonLocator).Click();

        return this;
    }
}