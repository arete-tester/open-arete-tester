using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace AreteTester.Actions
{
    [Serializable]
    public class TextContains : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        [TypeConverter(typeof(VariablePropertyConverter))]
        public string Variable { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        [TypeConverter(typeof(VariablePropertyConverter))]
        public string Text { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        [TypeConverter(typeof(VariablePropertyConverter))]
        public string TextToCompare { get; set; }

        public TextContains()
        {
            this.ActionType = "TextContains";
            this.Variable = "@var";
        }

        public override void Process()
        {
            base.Process();

            string text = Variables.Instance.Apply(this.Text);
            string textToCompare = Variables.Instance.Apply(this.TextToCompare);

            bool contains = text.Contains(textToCompare);

            Variables.Instance.SetValue(this.Variable, contains);

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

            if (String.IsNullOrEmpty(this.Text))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Text is empty." });
            }

            if (String.IsNullOrEmpty(this.TextToCompare))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Text to compare is empty." });
            }

            return result;
        }
    }
}
