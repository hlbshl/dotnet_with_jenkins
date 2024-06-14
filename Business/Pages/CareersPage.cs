using log4net;
using OpenQA.Selenium;
using PracticalTaskSelenium.Core.Utilities;

namespace PracticalTaskSelenium.Business.Pages;

public class CareersPage : BasePage
{
    private const string DefaultLocation = "All Locations";
    private readonly By _keywordFieldLocator = By.Id("new_form_job_search-keyword");
    private readonly By _locationFieldLocator = By.XPath("//span[@title='All Locations']");
    private readonly By _allLocationsSelectionLocator = By.CssSelector("div.os-padding li[title='All Locations']");
    private readonly By _remoteCheckBoxLocator = By.XPath("//p/input[@name='remote']/following-sibling::label");
    private readonly By _findCarrerButtonLocator = By.XPath("//button[contains(text(),'Find')]");
    private readonly By _lastViewAndApplyButtonLocator = By.XPath("//li[@class='search-result__item'][last()]//a[.='View and apply']");

    public CareersPage(IWebDriver driver, ILog log) : base(driver, log)
    {
    }

    public CareersPage SearchPositionByKeywordAndLocation(string keyword, string country)
    {
        _log.Info($"Search will be executed by '{keyword}' keyword in {country} location");
        var keywordField = WaitForElementAndReturnIt(_keywordFieldLocator);
        var locationField = _driver.FindElement(_locationFieldLocator);

        ActionsHelper.ClickAndSendKeys(_driver, keywordField, 2, keyword, _log);

        if (country != DefaultLocation)
        {
            ActionsHelper.ClickAndSendKeys(_driver, locationField, 2, country, _log);
            string locationWithTestParameter = String.Format("div.os-padding li[title='All Cities in {0}']", country);
            _driver.FindElement(By.CssSelector(locationWithTestParameter)).Click();
        }
        else
        {
            locationField.Click();
            _driver.FindElement(_allLocationsSelectionLocator).Click();
        }

        _driver.FindElement(_remoteCheckBoxLocator).Click();
        _driver.FindElement(_findCarrerButtonLocator).Click();
        _log.Info("The search was executed");

        return this;
    }

    public PositionPage OpenLastPositionInResults()
    {
        _log.Info("The last found position will be opened");
        IWebElement lastViewAndApplyButton = WaitForElementAndReturnIt(_lastViewAndApplyButtonLocator);
        lastViewAndApplyButton.Click();
        return new PositionPage(_driver, _log);
    }
}
