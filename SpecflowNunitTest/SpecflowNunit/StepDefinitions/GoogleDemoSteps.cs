using FluentAssertions;
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

        public GoogleDemoSteps(ScenarioContext scenarioContext, IWebDriver driver)
        {
            _scenarioContext = scenarioContext;
            _driver = driver;
        }

        [Given(@"I am in ""(.*)""")]
        public void GivenIAmIn(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }
        
        [When(@"I enter a keyword ""(.*)""")]
        public void WhenIEnterAKeyword(string keyword)
        {
            // _driver.SwitchTo().ActiveElement();
            // _driver.FindElement(By.Id("L2AGLb")).Click();
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
