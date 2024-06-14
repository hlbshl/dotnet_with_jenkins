using log4net;
using OpenQA.Selenium;
using PracticalTaskSelenium.Core.Utilities;

namespace PracticalTaskSelenium.Business.Pages;

public class AboutPage : BasePage
{
    private readonly By _epamAtGlanceLocator = By.XPath("//span[contains(text(), 'EPAM at')]");
    private readonly By _downloadButtonLocator = By.XPath("//span[contains(text(), 'DOWNLOAD')]");
    public AboutPage(IWebDriver driver, ILog log) : base(driver, log)
    {
    }
    public AboutPage ScrollToEpamAtGlanceSectionAndDownloadOverviewPDF()
    {
        _log.Info("File is about to be downloaded");
        var atGlanceLocation = WaitForElementAndReturnIt(_epamAtGlanceLocator);
        ActionsHelper.ScrollTo(_driver, atGlanceLocation, _log);
        _driver.FindElement(_downloadButtonLocator).Click();

        return this;
    }
}