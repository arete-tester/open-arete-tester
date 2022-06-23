using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace AreteTester.Actions
{
    [Serializable]
    public class ExecuteFunction : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public string FunctionFullName { get; set; }

        public ExecuteFunction()
        {
            this.ActionType = "ExecuteFunction";
        }

        public override void Process()
        {
            base.Process();

            NotifyExecuteMethodCalled(this.FunctionFullName);
        }

        public override ValidationResult Validate()
        {
            ValidationResult result = base.Validate();

            if (String.IsNullOrEmpty(this.FunctionFullName) )
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Function full name is empty." });
            }

            return result;
        }
    }
}
