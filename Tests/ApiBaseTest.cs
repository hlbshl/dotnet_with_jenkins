using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Configuration;
using NUnit.Framework.Interfaces;
using PracticalTaskSelenium.Core.API;
using PracticalTaskSelenium.Core.Configuration;
using PracticalTaskSelenium.Core.Enums;

namespace PracticalTaskSelenium.Tests.Api;

public abstract class ApiBaseTest
{
    private LoggerLevel _loggerLevel;
    protected BaseClient _client;

    public ApiBaseTest()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("EnvironmentSettings.json")
            .Build();

        var settings = configuration.GetSection("TestEnvironment").Get<TestEnvironmentSettings>();

        _loggerLevel = Enum.Parse<LoggerLevel>(settings.LoggerLevel);
    }

    protected ILog Log
    {
        get { return LogManager.GetLogger(this.GetType()); }
    }

    [OneTimeSetUp]
    public void SetUp()
    {
        Log.Info("Creating base client");
        _client = new BaseClient("https://jsonplaceholder.typicode.com");
        SetUpTestEnvironment();
    }

    protected void SetUpTestEnvironment()
    {
        XmlConfigurator.Configure(new FileInfo("Log.config"));

        var hierarchy = (Hierarchy)LogManager.GetRepository();
        hierarchy.Root.Level = hierarchy.LevelMap[_loggerLevel.ToString()];
        hierarchy.Configured = true;
        Log.Info($"Logger level is set to {_loggerLevel.ToString()}");
    }

    [TearDown]
    public void AfterTest()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
        {
            var failedTestName = TestContext.CurrentContext.Test.Name;
            Log.Error($"[FAILED] {failedTestName}");
        }
    }
}