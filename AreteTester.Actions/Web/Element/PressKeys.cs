using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using OpenQA.Selenium;
using System.ComponentModel;
using System.Threading;

namespace AreteTester.Actions
{
    [Serializable]
    public class PressKeys : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public string ID { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public string XPath { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public bool XPathFixed { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public bool Ctrl { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public bool Alt { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public bool Shift { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public bool Home { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public bool End { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public bool PageUp { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public bool PageDown { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public string Keys { get; set; }

        public PressKeys()
        {
            this.ActionType = "PressKeys";
            this.XPathFixed = false;
        }

        public override void Process()
        {
            base.Process();

            string xpath = Variables.Instance.Apply(this.XPath);
            IWebElement element = (String.IsNullOrEmpty(this.ID) == false ? Globals.Driver.FindElement(By.Id(this.ID)) : Globals.Driver.FindElement(By.XPath(xpath)));

            OpenQA.Selenium.Interactions.Actions action = new OpenQA.Selenium.Interactions.Actions(Globals.Driver);

            if (this.Ctrl)
            {
                action.KeyDown(OpenQA.Selenium.Keys.Control);
                Thread.Sleep(1000);
            }

            if (this.Alt)
            {
                action.KeyDown(OpenQA.Selenium.Keys.Alt);
                Thread.Sleep(1000);
            }

            if (this.Shift)
            {
                action.KeyDown(OpenQA.Selenium.Keys.Shift);
                Thread.Sleep(1000);
            }
            
            if (this.Home)
            {
                action.SendKeys(OpenQA.Selenium.Keys.Home);
                Thread.Sleep(1000);
            }

            if (this.End)
            {
                action.SendKeys(OpenQA.Selenium.Keys.End);
                Thread.Sleep(1000);
            }

            if (this.PageUp)
            {
                action.SendKeys(OpenQA.Selenium.Keys.PageUp);
                Thread.Sleep(1000);
            }

            if (this.PageDown)
            {
                action.SendKeys(OpenQA.Selenium.Keys.PageDown);
                Thread.Sleep(1000);
            }

            if (String.IsNullOrEmpty(this.Keys) == false)
            {
                action.SendKeys(this.Keys);
                Thread.Sleep(1000);
            }
            
            action.Build().Perform();

            /*string keys = (Ctrl ? OpenQA.Selenium.Keys.Control : "")
                          + (Home ? OpenQA.Selenium.Keys.Home : "")
                          + (End ? OpenQA.Selenium.Keys.End : "") 
                          + ( ? "" : this.Keys.ToLower());

            element.SendKeys(keys);*/

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
