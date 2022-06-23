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
    public class CloseTab : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public int Index { get; set; }

        public CloseTab()
        {
            this.ActionType = "CloseTab";
        }

        public override void  Process()
        {
            base.Process();

            if (Globals.Driver.WindowHandles.Count > this.Index)
            {
                Globals.Driver.SwitchTo().Window(Globals.Driver.WindowHandles[this.Index]);
                Globals.Driver.Close();
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
