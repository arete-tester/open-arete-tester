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
    public class MaximizeWindow : ActionBase
    {
        public MaximizeWindow()
        {
            this.ActionType = "MaximizeWindow";
        }

        public override void  Process()
        {
            base.Process();

            try
            {
                Globals.Driver.Manage().Window.Maximize();
            }
            catch
            {
                //TODO: Check whether window maximized instead of hiding error
            }

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
