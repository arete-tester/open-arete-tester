using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using OpenQA.Selenium;
using System.ComponentModel;

namespace AreteTester.Actions
{
    [Serializable]
    public class ClickButton : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public string ID { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public string Class { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public string XPath { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public bool XPathFixed { get; set; }

        public ClickButton()
        {
            this.ActionType = "ClickButton";
            this.XPathFixed = false;
        }

        public override void  Process()
        {
            base.Process();

            string xpath = Variables.Instance.Apply(this.XPath);
            IWebElement element = null;
            if (String.IsNullOrEmpty(this.ID) == false) element = Globals.Driver.FindElement(By.Id(this.ID));
            else if (String.IsNullOrEmpty(this.Class) == false)
            {
                string cls = "." + this.Class.Replace(" ", ".");
                element = Globals.Driver.FindElement(By.CssSelector(cls));
                System.Threading.Thread.Sleep(3000);
            }
            else element = Globals.Driver.FindElement(By.XPath(xpath));

            //IJavaScriptExecutor javascriptDriver = (IJavaScriptExecutor)AreteTester.Actions.Globals.Driver;
            //javascriptDriver.ExecuteScript("arguments[0].click();", element);

            OpenQA.Selenium.Interactions.Actions builder = new OpenQA.Selenium.Interactions.Actions(Globals.Driver);
            builder.Click(element).Build().Perform();

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

            if (String.IsNullOrEmpty(this.ID) && String.IsNullOrEmpty(this.XPath))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Both ID and XPath is empty." });
            }

            if (String.IsNullOrEmpty(this.ID) == false && String.IsNullOrEmpty(this.XPath) == false)
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Warning, Message = "Both ID and XPath assigned. ID takes precedence over XPath." });
            }

            return result;
        }
    }
}
