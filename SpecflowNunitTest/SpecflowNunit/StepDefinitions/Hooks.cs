﻿using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using TechTalk.SpecFlow;

[assembly:Parallelizable(ParallelScope.Fixtures)]
namespace SpecflowNunit.StepDefinitions
{
    [Binding]
    public class Hooks : Steps
    {
        private static ExtentTest _featureName;
        private static ExtentTest _scnarioName;
        private static ExtentReports extent;
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
            var htmlReporter = new ExtentHtmlReporter(@$"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent}\Reports\ExtentReports.html");
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _driver = new ChromeDriver();
            _objectContainer.RegisterInstanceAs<IWebDriver>(_driver);
            _scnarioName = _featureName.CreateNode<Scenario>(_scenarioContext.ScenarioInfo.Title);
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

                if (stepType == "Given")
                    _scnarioName.CreateNode<Given>(scenarioContext.StepContext.StepInfo.Text).Fail(scenarioContext.TestError.Message, mediaEntity);
                else if (stepType == "When")
                    _scnarioName.CreateNode<When>(scenarioContext.StepContext.StepInfo.Text).Fail(scenarioContext.TestError.Message, mediaEntity);
                else if (stepType == "Then")
                    _scnarioName.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text).Fail(scenarioContext.TestError.Message, mediaEntity);
                else if (stepType == "And")
                    _scnarioName.CreateNode<And>(scenarioContext.StepContext.StepInfo.Text).Fail(scenarioContext.TestError.Message, mediaEntity);
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