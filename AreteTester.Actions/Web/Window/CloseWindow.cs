﻿using System;
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
    public class CloseWindow : ActionBase
    {
        public CloseWindow()
        {
            this.ActionType = "CloseWindow";
        }

        public override void  Process()
        {
            base.Process();

            Globals.Driver.Close();

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
