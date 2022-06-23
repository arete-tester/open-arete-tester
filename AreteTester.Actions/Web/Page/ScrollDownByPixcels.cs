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
    public class ScrollDownByPixcels : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public string Value { get; set; }

        public ScrollDownByPixcels()
        {
            this.ActionType = "ScrollDownByPixcels";
        }

        public override void Process()
        {
            base.Process();

            IJavaScriptExecutor javascriptDriver = (IJavaScriptExecutor)AreteTester.Actions.Globals.Driver;

            string value = Variables.Instance.Apply(this.Value);
            javascriptDriver.ExecuteScript("window.scrollBy(0,arguments[0]);", value);

            NotifyOutput(new Output()
            {
                ActionType = this.ActionType,
                Description = this.Description,
                IsAssertion = false,
                DoNotLog = false
            });
        }
    }
}
