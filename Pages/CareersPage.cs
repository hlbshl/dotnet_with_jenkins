using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PracticalTaskSelenium.Utilities;

namespace PracticalTaskSelenium.Pages;

public class CareersPage : BasePage
{
    private const string DefaultLocation = "All Locations";
    private readonly By keywordFieldLocator = By.Id("new_form_job_search-keyword");
    private readonly By locationFieldLocator = By.XPath("//span[@title='All Locations']");
    private readonly By allLocationsSelectionLocator = By.CssSelector("div.os-padding li[title='All Locations']");
    private readonly By remoteCheckBoxLocator = By.XPath("//p/input[@name='remote']/following-sibling::label");
    private readonly By findCarrerButtonLocator = By.XPath("//button[contains(text(),'Find')]");
    private readonly By lastViewAndApplyButtonLocator = By.XPath("//li[@class='search-result__item'][last()]//a[.='View and apply']");
    
    public CareersPage(IWebDriver driver, WebDriverWait wait) : base(driver, wait)
    {
    }

    public CareersPage SearchPositionByKeywordAndLocation(string keyword, string country)
    {
        var keywordField = wait.Until(driver => driver.FindElement(keywordFieldLocator));
        var locationField = driver.FindElement(locationFieldLocator);

        ActionsHelper.ClickAndSendKeysActions(driver, keywordField, 2, keyword);

        if (country != DefaultLocation)
        {
            ActionsHelper.ClickAndSendKeysActions(driver, locationField, 2, country);
            string locationWithTestParameter = String.Format("div.os-padding li[title='All Cities in {0}']", country);
            driver.FindElement(By.CssSelector(locationWithTestParameter)).Click();
        }
        else
        {
            locationField.Click();
            driver.FindElement(allLocationsSelectionLocator).Click();
        }

        driver.FindElement(remoteCheckBoxLocator).Click();
        driver.FindElement(findCarrerButtonLocator).Click();

        return this;
    }

    public PositionPage OpenLastPositionInResults()
    {
        IWebElement lastViewAndApplyButton = wait.Until(driver => driver.FindElement(lastViewAndApplyButtonLocator));
        lastViewAndApplyButton.Click();
        return new PositionPage(driver, wait);
    }
}
