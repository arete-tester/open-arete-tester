using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AreteTester.Actions
{
    public class ValidationResult
    {
        public List<ValidationMessage> Messages { get; set; }

        public ValidationResult()
        {
            this.Messages = new List<ValidationMessage>();
        }
    }

    public class ValidationMessage
    {
        public ValidationMessageType MessageType { get; set; }

        public string Message { get; set; }

        public ValidationMessage()
        {
            this.MessageType = ValidationMessageType.Error;
        }
    }

    public enum ValidationMessageType
    {
        Error, 
        Warning
    }
}
