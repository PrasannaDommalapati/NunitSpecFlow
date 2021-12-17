using BoDi;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace SpecflowNunit.StepDefinitions
{
    [Binding]
    public sealed class Hooks
    {
        private IObjectContainer _objectContainer { get; set; }
        private IWebDriver _driver { get; set; }
        private readonly ScenarioContext _scenarioContext;
        private static Helpers.ExtentReportsHelper _extentHelper;

        public Hooks(IObjectContainer objectContainer, ScenarioContext context)
        {
            _scenarioContext = context;
            _objectContainer = objectContainer;
            _extentHelper = _extentHelper = new Helpers.ExtentReportsHelper();
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _extentHelper.CreateTest(_scenarioContext.ScenarioInfo.Title);
            _driver = new ChromeDriver();
            _objectContainer.RegisterInstanceAs<IWebDriver>(_driver);
        }

        [AfterScenario]
        public void WrapUpReport()
        {
            switch (_scenarioContext.ScenarioExecutionStatus)
            {
                case ScenarioExecutionStatus.OK:
                    _extentHelper.SetTestStatusPass();
                    break;
                case ScenarioExecutionStatus.TestError:
                    _extentHelper.SetTestStatusFail("One or more errors occurred during scenario execution");
                    break;
                case ScenarioExecutionStatus.UndefinedStep:
                    _extentHelper.SetStepStatusWarning(_scenarioContext.ScenarioInfo.Title);
                    break;
                case ScenarioExecutionStatus.Skipped:
                    _extentHelper.SetTestStatusSkipped();
                    break;
                default:
                    _extentHelper.SetTestStatusFail("Error occurred during determination of scenario status");
                    break;
            }

            _scenarioContext.Clear();
            _driver.Quit();
        }

        [AfterFeature]
        public static void AfterFeature()
        {
            _extentHelper.Close();
        }
    }
}
