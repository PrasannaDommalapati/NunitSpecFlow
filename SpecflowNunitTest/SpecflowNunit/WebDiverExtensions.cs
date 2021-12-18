using AventStack.ExtentReports;
using OpenQA.Selenium;

namespace SpecflowNunit
{
    public static class WebDiverExtensions
    {
        public static MediaEntityModelProvider CaptureScreenshotAndReturnModel(this IWebDriver driver, string name)
        {
            ITakesScreenshot ts = (ITakesScreenshot)driver;
            Screenshot screenshot = ts.GetScreenshot();
            return MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot.AsBase64EncodedString, name).Build();
        }
    }
}