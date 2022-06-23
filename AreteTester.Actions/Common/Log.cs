
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
    public class Log : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public string Text { get; set; }

        public Log()
        {
            this.ActionType = "Log";
        }

        public override void Process()
        {
            base.Process();

            NotifyOutput(new Output()
            {
                ActionType = this.ActionType,
                Description = this.Description,
                Message = Variables.Instance.Apply(Text),
                IsAssertion = false,
                DoNotLog = false
            });
        }

        public override ValidationResult Validate()
        {
            ValidationResult result = base.Validate();
            
            if (String.IsNullOrEmpty(this.Text))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Text is empty." });
            }

            return result;
        }
    }
}