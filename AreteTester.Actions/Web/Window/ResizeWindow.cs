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
    public class ResizeWindow : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public int Height { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public int Width { get; set; }

        public ResizeWindow()
        {
            this.ActionType = "ResizeWindow";
        }

        public override void  Process()
        {
            base.Process();

            Globals.Driver.Manage().Window.Size = new Size(this.Width, this.Height);

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
