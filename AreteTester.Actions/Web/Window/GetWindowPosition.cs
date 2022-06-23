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
    public class GetWindowPosition : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public string XVariable { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public string YVariable { get; set; }

        public GetWindowPosition()
        {
            this.ActionType = "GetWindowPosition";
        }

        public override void  Process()
        {
            base.Process();

            Variables.Instance.SetValue(this.XVariable, Globals.Driver.Manage().Window.Position.X);
            Variables.Instance.SetValue(this.YVariable, Globals.Driver.Manage().Window.Position.Y);

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

            if (String.IsNullOrEmpty(this.XVariable))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "XVariable is empty." });
            }

            if (String.IsNullOrEmpty(this.YVariable))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "YVariable is empty." });
            }

            return result;
        }
    }
}
