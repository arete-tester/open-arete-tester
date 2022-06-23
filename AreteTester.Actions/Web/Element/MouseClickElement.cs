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
    public class MouseClickElement : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public string ID { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public string XPath { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public bool XPathFixed { get; set; }

        public MouseClickElement()
        {
            this.ActionType = "MouseClickElement";
            this.XPathFixed = false;
        }

        public override void  Process()
        {
            base.Process();

            string xpath = Variables.Instance.Apply(this.XPath);
            IWebElement element = (String.IsNullOrEmpty(this.ID) == false ? Globals.Driver.FindElement(By.Id(this.ID)) : Globals.Driver.FindElement(By.XPath(xpath)));

            OpenQA.Selenium.Interactions.Actions builder = new OpenQA.Selenium.Interactions.Actions(Globals.Driver);
            builder.MoveToElement(element, element.Location.X, element.Location.Y).Click().Build().Perform();

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
