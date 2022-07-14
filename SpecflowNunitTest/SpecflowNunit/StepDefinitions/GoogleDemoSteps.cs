using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using TechTalk.SpecFlow;

namespace SpecflowNunit.StepDefinitions
{
    [Binding]
    public class GoogleDemoSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IWebDriver _driver;
        public TestContext TestContext { get; set; }

        public GoogleDemoSteps(ScenarioContext scenarioContext, TestContext testContext,IWebDriver driver)
        {
            _scenarioContext = scenarioContext;
            TestContext = testContext;
            _driver = driver;
        }

        [Given(@"I am in ""(.*)""")]
        public void GivenIAmIn(string url)
        {
            //url = TestContext.Parameters["url"].ToString() ?? "https://www.facebook.com";
            _driver.Navigate().GoToUrl(url);
        }
        
        [When(@"I enter a keyword ""(.*)""")]
        public void WhenIEnterAKeyword(string keyword)
        {
            _driver.SwitchTo().ActiveElement();
            _driver.FindElement(By.Id("L2AGLb")).Click();
            _driver.FindElement(By.Name("q")).SendKeys(keyword);
            _driver.FindElement(By.Name("q")).SendKeys(Keys.Enter);
        }
        
        [Then(@"I should see the page title contains ""(.*)""")]
        public void ThenIShouldSeeThePageTitleContains(string titleText)
        {
            _driver.Title.Should().StartWith(titleText);
        }
    }
}
