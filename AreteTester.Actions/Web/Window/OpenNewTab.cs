using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using OpenQA.Selenium;
using System.ComponentModel;
using System.Drawing;

namespace AreteTester.Actions
{
    [Serializable]
    public class OpenNewTab : ActionBase
    {
        public OpenNewTab()
        {
            this.ActionType = "OpenNewTab";
        }

        public override void  Process()
        {
            base.Process();

            ((IJavaScriptExecutor)Globals.Driver).ExecuteScript("window.open();");
            Globals.Driver.SwitchTo().Window(Globals.Driver.WindowHandles.Last());

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
