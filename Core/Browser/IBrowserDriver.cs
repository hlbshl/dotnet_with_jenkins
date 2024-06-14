using OpenQA.Selenium;

public interface IBrowserDriver
{
    IWebDriver CreateDriver(bool headless, string downloadDirectory);
}