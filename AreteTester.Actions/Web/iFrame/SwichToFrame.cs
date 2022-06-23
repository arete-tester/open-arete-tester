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
    public class SwichToFrame : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public string Id { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public int Index { get; set; }

        public SwichToFrame()
        {
            this.ActionType = "SwichToFrame";
            this.Index = -1;
        }

        public override void Process()
        {
            base.Process();

            if (this.Index >= 0)
            {
                Globals.Driver.SwitchTo().Frame(this.Index);
            }
            else
            {
                IWebElement frame = Globals.Driver.FindElement(By.XPath(String.Format(@"//*[@id=""{0}""]", this.Id)));

                Globals.Driver.SwitchTo().Frame(frame);
            }

            NotifyOutput(new Output()
            {
                ActionType = this.ActionType,
                Description = this.Description,
                IsAssertion = false,
                DoNotLog = false
            });
        }

        public override ValidationResult Validate()
        {
            ValidationResult result = base.Validate();
            if (String.IsNullOrEmpty(this.Id) && this.Index == -1)
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Both ID and Index not set. Set value for either of it." });
            }

            return result;
        }
    }
}
