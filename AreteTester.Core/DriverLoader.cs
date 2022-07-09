using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium.Chrome;
using System.IO;

namespace AreteTester.Core
{
    public class DriverLoader
    {
        public static void StartChromeDriver(string chromeDriverPath, List<string> extensionPaths, string userDataDir)
        {
            bool startDriver = false;

            try
            {
                if (AreteTester.Actions.Globals.Driver == null || AreteTester.Actions.Globals.Driver.WindowHandles.Count <= 0)
                {
                    startDriver = true;
                }
            }
            catch
            {
                startDriver = true;
            }

            if (startDriver)
            {
                try
                {
                    ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService(chromeDriverPath);
                    chromeDriverService.HideCommandPromptWindow = true;
                    ChromeOptions options = new ChromeOptions();
                    options.AddArguments("no-sandbox");

                    if (String.IsNullOrEmpty(userDataDir) == false)
                    {
                        options.AddArguments("user-data-dir=" + userDataDir.Replace(@"\", "/"));
                    }

                    if (extensionPaths != null)
                    {
                        foreach (string extensionPath in extensionPaths)
                        {
                            options.AddArguments("load-extension=" + extensionPath);
                        }
                    }

                    AreteTester.Actions.Globals.Driver = new ChromeDriver(chromeDriverService, options);
                    AreteTester.Actions.Globals.Driver.Manage().Timeouts().ImplicitWait = new TimeSpan(0, 0, 0);
                }
                catch
                {
                    // TODO: not a good idea to hide exceptions here
                }
            }
        }
    }
}
