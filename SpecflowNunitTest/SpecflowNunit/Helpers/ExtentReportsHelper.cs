using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace SpecflowNunit.Helpers
{
    public class ExtentReportsHelper
    {
        public ExtentReports _extentReports { get; set; }
        public ExtentV3HtmlReporter _reporter { get; set; }
        public ExtentTest _extentTest { get; set; }
        public ExtentReportsHelper()
        {
            _extentReports = new ExtentReports();
            _reporter = new ExtentV3HtmlReporter(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ExtentReports.html"));
            _reporter.Config.DocumentTitle = "Automation Testing Report";
            _reporter.Config.ReportName = "Regression Testing";
            _reporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;
            _extentReports.AttachReporter(_reporter);
            _extentReports.AddSystemInfo("Application Under Test", "Replace with your Application Name");
            _extentReports.AddSystemInfo("Environment", "QA");
            _extentReports.AddSystemInfo("Machine", Environment.MachineName);
            _extentReports.AddSystemInfo("OS", Environment.OSVersion.VersionString);
        }
        public void CreateTest(string testName)
        {
            _extentTest = _extentReports.CreateTest(testName);
        }
        public void SetStepStatusPass(string stepDescription)
        {
            _extentTest.Log(Status.Pass, stepDescription);
        }
        public void SetStepStatusWarning(string stepDescription)
        {
            _extentTest.Log(Status.Warning, stepDescription);
        }
        public void SetTestStatusPass()
        {
            _extentTest.Pass("Test Executed Sucessfully!");
        }
        public void SetTestStatusFail(string message = null)
        {
            var printMessage = "<p><b>Test FAILED!</b></p>";
            if (!string.IsNullOrEmpty(message))
            {
                printMessage += $"Message: <br>{message}<br>";
            }
            _extentTest.Fail(printMessage);
        }
        public void AddTestFailureScreenshot(string base64ScreenCapture)
        {
            _extentTest.AddScreenCaptureFromBase64String(base64ScreenCapture, "Screenshot on Error:");
        }
        public void SetTestStatusSkipped()
        {
            _extentTest.Skip("Test skipped!");
        }
        public void Close()
        {
            _extentReports.Flush();
        }
    }
}