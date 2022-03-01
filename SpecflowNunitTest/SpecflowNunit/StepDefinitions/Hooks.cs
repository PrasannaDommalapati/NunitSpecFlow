using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using TechTalk.SpecFlow;
using ExtentReporter = AventStack.ExtentReports.ExtentReports;

namespace SpecflowNunit.StepDefinitions
{
    [Binding]
    public class Hooks : Steps
    {
        private static ExtentTest _featureName;
        private static ExtentTest _scnarioName;
        private static ExtentReporter extent;
        private IObjectContainer _objectContainer { get; set; }
        private IWebDriver _driver { get; set; }
        private readonly ScenarioContext _scenarioContext;
        private readonly FeatureContext _featureContext;

        public Hooks(IObjectContainer objectContainer, ScenarioContext context, FeatureContext featureContext)
        {
            _objectContainer = objectContainer;
            _scenarioContext = context;
            _featureContext = featureContext;
        }

        [BeforeTestRun]
        public static void InitializeReport()
        {
            string testExecutionTime = DateTime.Now.ToString("HH-mm-dd-MM-yyyy");
            var htmlReporter = new ExtentV3HtmlReporter(@$"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent}\Reports\extent_{testExecutionTime}.html");
            htmlReporter.Config.Theme = Theme.Dark;
            extent = new ExtentReporter();
            extent.AddSystemInfo("Application Under Test", "Google Demo");
            extent.AddSystemInfo("Environment", "QA");
            extent.AddSystemInfo("Machine", Environment.MachineName);
            extent.AddSystemInfo("OS", Environment.OSVersion.VersionString);
            extent.AttachReporter(htmlReporter);
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            ChromeOptions option = new ChromeOptions();
            option.AddArguments("start-maximized");
            option.AddArguments("--disable-gpu");
            option.AddArguments("--headless");

            new DriverManager().SetUpDriver(new ChromeConfig());
            Console.WriteLine("Setup");

            _driver = new ChromeDriver(option);
            _objectContainer.RegisterInstanceAs<IWebDriver>(_driver);
            _scnarioName = _featureName.CreateNode<Scenario>(_scenarioContext.ScenarioInfo.Title);
            _scnarioName.AssignCategory(_scenarioContext.ScenarioInfo.Tags);
        }

        [AfterTestRun]
        public static void TearDownReport()
        {
            extent.Flush();
        }

        [AfterStep]
        public void InsertReportingSteps(ScenarioContext scenarioContext)
        {
            var stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();

            if (scenarioContext.TestError == null)
            {
                if (stepType == "Given")
                    _scnarioName.CreateNode<Given>(scenarioContext.StepContext.StepInfo.Text);
                else if (stepType == "When")
                    _scnarioName.CreateNode<When>(scenarioContext.StepContext.StepInfo.Text);
                else if (stepType == "Then")
                    _scnarioName.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text);
                else if (stepType == "And")
                    _scnarioName.CreateNode<And>(scenarioContext.StepContext.StepInfo.Text);
            }

            if (scenarioContext.TestError != null)
            {
                var mediaEntity = _driver.CaptureScreenshotAndReturnModel(scenarioContext.ScenarioInfo.Title.Trim());
                var errorMessage = $"Message: '{scenarioContext.TestError.Message}'>-----> StackTrace: '{scenarioContext.TestError.StackTrace}'";

                if (stepType == "Given")
                    _scnarioName.CreateNode<Given>(scenarioContext.StepContext.StepInfo.Text).Fail(errorMessage, mediaEntity);
                else if (stepType == "When")
                    _scnarioName.CreateNode<When>(scenarioContext.StepContext.StepInfo.Text).Fail(errorMessage, mediaEntity);
                else if (stepType == "Then")
                    _scnarioName.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text).Fail(errorMessage, mediaEntity);
                else if (stepType == "And")
                    _scnarioName.CreateNode<And>(scenarioContext.StepContext.StepInfo.Text).Fail(errorMessage, mediaEntity);
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _driver.Close();
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            _featureName = extent.CreateTest(featureContext.FeatureInfo.Title);
        }
    }
}
