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
    public class GetWindowSize : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public string HeightVariable { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public string WidthVariable { get; set; }

        public GetWindowSize()
        {
            this.ActionType = "GetWindowSize";
        }

        public override void  Process()
        {
            base.Process();

            Variables.Instance.SetValue(this.HeightVariable, Globals.Driver.Manage().Window.Size.Height);
            Variables.Instance.SetValue(this.WidthVariable, Globals.Driver.Manage().Window.Size.Width);

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

            if (String.IsNullOrEmpty(this.HeightVariable))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "HeightVariable is empty." });
            }

            if (String.IsNullOrEmpty(this.WidthVariable))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "WidthVariable is empty." });
            }

            return result;
        }
    }
}
