using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Configuration;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using PracticalTaskSelenium.Core.Browser;
using PracticalTaskSelenium.Core.Enums;
using PracticalTaskSelenium.Core.Utilities;
using PracticalTaskSelenium.Core.Configuration;

namespace PracticalTaskSelenium.Tests
{
    public abstract class BaseTest
    {
        protected IWebDriver _driver;
        private string _downloadDirectory = "";
        private readonly BrowserType _browserType;
        private bool _headless;
        protected LoggerLevel _loggerLevel;
        public BaseTest()
        {
            var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("EnvironmentSettings.json")
           .Build();

            var settings = configuration.GetSection("TestEnvironment").Get<TestEnvironmentSettings>();

            _browserType = Enum.Parse<BrowserType>(settings.BrowserType);
            _headless = settings.Headless;
            _loggerLevel = Enum.Parse<LoggerLevel>(settings.LoggerLevel);
        }

        protected ILog Log
        {
            get { return LogManager.GetLogger(this.GetType()); }
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            Log.Info("Setting up environment");
            SetUpTestEnvironment();
            Log.Info("Initializing pages");
            InitializePages();
        }

        protected void SetUpTestEnvironment()
        {
            XmlConfigurator.Configure(new FileInfo("Log.config"));

            var hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Root.Level = hierarchy.LevelMap[_loggerLevel.ToString()];
            hierarchy.Configured = true;
            Log.Info($"Logger level is set to {_loggerLevel.ToString()}");

            _downloadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Downloads");

            if (!Directory.Exists(_downloadDirectory))
            {
                Log.Info($"Creating {_downloadDirectory} directory for downloaded files");
                Directory.CreateDirectory(_downloadDirectory);
            }
            Log.Info("Openning web application");
            var browserDriver = BrowserDriverFactory.GetBrowserDriver(_browserType);
            _driver = DriverFactory.Instance.GetDriver(browserDriver, _headless, _downloadDirectory, Log);
            _driver.Manage().Window.Maximize();
        }

        protected abstract void InitializePages();

        [TearDown]
        public void AfterTest()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var failedTestName = TestContext.CurrentContext.Test.Name;
                Log.Error($"[FAILED] {failedTestName}: Screenshot was taken");
                var screenshot = ScreenshotMaker.CaptureBrowserScreenshot(_driver, Log);
                ScreenshotMaker.SaveScreenshot(screenshot, Log);
            }
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Log.Info("Closing web application");

            DriverFactory.Instance.QuitDriver();

            if (Directory.Exists(_downloadDirectory))
            {
                Log.Info("Deleting directory for downloaded files");
                Directory.Delete(_downloadDirectory, true);
            }
        }
    }
}