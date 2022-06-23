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
    public class ClearAllCookies : ActionBase
    {
        public ClearAllCookies()
        {
            this.ActionType = "ClearAllCookies";
        }

        public override void Process()
        {
            base.Process();

            Globals.Driver.Manage().Cookies.DeleteAllCookies();

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
