using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using OpenQA.Selenium;

namespace AreteTester.Actions
{
    [Serializable]
    public class WaitForElementDisplayed : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public int MaxWaitDuration { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public int AttemptWaitDuration { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public string ID { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public string XPath { get; set; }

        public WaitForElementDisplayed()
        {
            this.ActionType = "WaitForElementDisplayed";

            this.AttemptWaitDuration = 1;
            this.MaxWaitDuration = 120;
        }

        public override void  Process()
        {
            base.Process();

            DateTime startTime = DateTime.Now;

            while (true)
            {
                if (this.IsDisplayed()) return;

                System.Threading.Thread.Sleep(this.AttemptWaitDuration * 1000);

                if ((DateTime.Now - startTime).Seconds > this.MaxWaitDuration) return;
            }
        }

        private bool IsDisplayed()
        {
            string xpath = Variables.Instance.Apply(this.XPath);
            IWebElement element = (String.IsNullOrEmpty(this.ID) == false ? Globals.Driver.FindElement(By.Id(this.ID)) : Globals.Driver.FindElement(By.XPath(xpath)));

            return element.Displayed;
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
