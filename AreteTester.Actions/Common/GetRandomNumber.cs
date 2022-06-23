using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using OpenQA.Selenium;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace AreteTester.Actions
{
    [Serializable]
    public class GetRandomNumber : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        [TypeConverter(typeof(VariablePropertyConverter))]
        public string Variable { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public int RangeMinimum { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public int RangeMaximum { get; set; }

        public GetRandomNumber()
        {
            this.ActionType = "GetRandomNumber";
            this.Variable = "@var";
            this.RangeMinimum = 0;
            this.RangeMaximum = Int32.MaxValue;
        }

        public override void Process()
        {
            base.Process();

            Random random = new Random();
            Variables.Instance.SetValue(this.Variable, random.Next(this.RangeMinimum, this.RangeMaximum));

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

            if (String.IsNullOrEmpty(this.Variable))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Variable is empty." });
            }
            if (String.IsNullOrEmpty(this.Variable) == false)
            {
                if (this.Variable.Length > 30)
                {
                    result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Variable name length cannot be more than 30 characters." });
                }

                Regex variableRegex = new Regex("^@[a-zA-Z0-9_]+$", RegexOptions.Compiled);
                if (variableRegex.IsMatch(this.Variable) == false)
                {
                    result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Variable name: " + this.Variable + " is not the valid format. Use only alphabets, numbers, underscore (_) and start with @." });
                }

                if (this.Variable.Contains("@") && this.Variable.IndexOf('@') > 0)
                {
                    result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Variable name: " + this.Variable + " contains character @ in it. Variable name can only start with @." });
                }
            }

            return result;
        }
    }
}