using OpenQA.Selenium;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SpecflowNunit
{
    public static class WebDiverExtensions
    {
        public static string ScreenCaptureAsBase64String(this IWebDriver driver)
        {
            string path = @"C:/Users/Prasanna Dommalapati/Desktop/Reports/Screenshot/" + RandomString(4) + ".png";
            ITakesScreenshot ts = (ITakesScreenshot)driver;
            Screenshot screenshot = ts.GetScreenshot();
            screenshot.SaveAsFile(path, ScreenshotImageFormat.Png);
            return path;
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}