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
    public class GetRegexMatchesInText : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        [TypeConverter(typeof(VariablePropertyConverter))]
        public string ListVariable { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public string Text { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public string Regex { get; set; }

        public GetRegexMatchesInText()
        {
            this.ActionType = "GetRegexMatchesInText";
            this.ListVariable = "@listVar";
        }

        public override void Process()
        {
            base.Process();

            string text = Variables.Instance.Apply(this.Text);

            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(this.Regex, System.Text.RegularExpressions.RegexOptions.Compiled);

            List<string> matches = new List<string>();
            
            foreach (Match match in regex.Matches(text))
            {
                matches.Add(match.Value);
            }

            Variables.Instance.SetValue(this.ListVariable, matches);

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

            if (String.IsNullOrEmpty(this.ListVariable))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Variable is empty." });
            }

            if (String.IsNullOrEmpty(this.ListVariable) == false)
            {
                if (this.ListVariable.Length > 30)
                {
                    result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Variable name length cannot be more than 30 characters." });
                }

                Regex variableRegex = new Regex("^@[a-zA-Z0-9_]+$", RegexOptions.Compiled);
                if (variableRegex.IsMatch(this.ListVariable) == false)
                {
                    result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Variable name: " + this.ListVariable + " is not the valid format. Use only alphabets, numbers, underscore (_) and start with @." });
                }

                if (this.ListVariable.Contains("@") && this.ListVariable.IndexOf('@') > 0)
                {
                    result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Variable name: " + this.ListVariable + " contains character @ in it. Variable name can only start with @." });
                }
            }

            if (String.IsNullOrEmpty(this.Text))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Text is empty." });
            }

            return result;
        }
    }
}
