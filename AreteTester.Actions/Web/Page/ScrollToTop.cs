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
    public class ScrollToTop : ActionBase
    {
        public ScrollToTop()
        {
            this.ActionType = "ScrollToTop";
        }

        public override void Process()
        {
            base.Process();

            IJavaScriptExecutor javascriptDriver = (IJavaScriptExecutor)AreteTester.Actions.Globals.Driver;

            javascriptDriver.ExecuteScript("window.scrollBy(0,0);");

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
