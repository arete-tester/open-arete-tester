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
    public class GoForward : ActionBase
    {
        public GoForward()
        {
            this.ActionType = "GoForward";
        }

        public override void  Process()
        {
            base.Process();

            Globals.Driver.Navigate().Forward();

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
