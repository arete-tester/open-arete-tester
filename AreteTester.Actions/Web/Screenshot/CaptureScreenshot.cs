
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using OpenQA.Selenium;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;

namespace AreteTester.Actions
{
    [Serializable]
    public class CaptureScreenshot : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public string FileName { get; set; }

        public CaptureScreenshot()
        {
            this.ActionType = "CaptureScreenshot";
        }

        public override void Process()
        {
            base.Process();

            Screenshot screenshot = ((ITakesScreenshot)Globals.Driver).GetScreenshot();
            string screenshot1 = screenshot.AsBase64EncodedString;
            byte[] screenshotAsByteArray = screenshot.AsByteArray;

            string projectPath = (string)Variables.Instance.GetValue("$$$ProjectOutputPath");

            string screenshotsDir = projectPath + @"\Screenshots_" + Globals.RunnerDateTime.ToString("yyyy-MMM-dd HH_mm_ss");
            if(Directory.Exists(screenshotsDir) == false) Directory.CreateDirectory(screenshotsDir);

            string screenshotFile = string.Empty;
            if (String.IsNullOrEmpty(this.FileName))
            {
                screenshotFile = screenshotsDir + @"\screenshot_" + this.Description + "_" + DateTime.Now.ToString("yyyy-MMM-dd HH_mm_ss_fff") + ".png";
            }
            else
            {
                screenshotFile = screenshotsDir + @"\" + this.FileName;
            }

            screenshot.SaveAsFile(screenshotFile, ScreenshotImageFormat.Png);
            
            NotifyOutput(new Output()
            {
                ActionType = this.ActionType,
                Description = this.Description,
                IsAssertion = false,
                DoNotLog = false
            });
        }

        public override ValidationResult Validate()
        {
            ValidationResult result = base.Validate();

            if (String.IsNullOrEmpty(this.FileName))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "File name is empty." });
            }

            Regex variableRegex = new Regex(@"^[a-zA-Z0-9_]+\.png$", RegexOptions.Compiled);
            if (variableRegex.IsMatch(this.FileName) == false)
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Filename name: " + this.FileName + " is not the valid format. Use only alphabets, numbers, underscore (_) and extension as .png" });
            }

            return result;
        }
    }
}
