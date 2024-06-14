using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace PracticalTaskSelenium.Core.Browser;

public class BrowserSettings
{
    public static ChromeOptions ChromeOptions(bool headless, string downloadDirectory)
    {
        var options = new ChromeOptions();
        if (headless)
        {
            options.AddArgument("--headless");
            options.AddArgument("--window-size=1920,1080");
        }
        options.AddUserProfilePreference("download.default_directory", downloadDirectory);
        options.AddUserProfilePreference("download.prompt_for_download", false);
        options.AddUserProfilePreference("download.directory_upgrade", true);
        options.AddUserProfilePreference("safebrowsing.enabled", true);

        return options;
    }

    public static FirefoxOptions FirefoxOptions(bool headless, string downloadDirectory)
    {
        var options = new FirefoxOptions();
        if (headless)
        {
            options.AddArgument("--headless");
            options.AddArgument("--window-size=1920,1080");
        }
        options.SetPreference("browser.download.dir", downloadDirectory);
        options.SetPreference("browser.download.folderList", 2);
        options.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/octet-stream");

        return options;
    }
}