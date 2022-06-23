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
    public class SwitchToTab : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public int Index { get; set; }

        public SwitchToTab()
        {
            this.ActionType = "SwitchToTab";
        }

        public override void  Process()
        {
            base.Process();

            Globals.Driver.SwitchTo().Window(Globals.Driver.WindowHandles[this.Index]);

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
