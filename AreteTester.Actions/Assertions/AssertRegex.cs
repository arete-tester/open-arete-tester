using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using RegularExpressions = System.Text.RegularExpressions;
using System.Text.RegularExpressions;

namespace AreteTester.Actions
{
    [Serializable]
    public class AssertRegex : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public string Name { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        [TypeConverter(typeof(VariablePropertyConverter))]
        public string Variable { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public string Regex { get; set; }

        public AssertRegex()
        {
            this.ActionType = "AssertRegex";
            this.Variable = "@var";
        }

        public override void  Process()
        {
            base.Process();

            bool result = false;
            if (String.IsNullOrEmpty(this.Regex) == false)
            {
                RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(this.Regex, System.Text.RegularExpressions.RegexOptions.Compiled);
                result = regex.IsMatch((string)Variables.Instance.GetValue(this.Variable));
            }

            NotifyOutput(new Output()
            {
                Name = this.Name,
                ActionType = this.ActionType,
                Description = this.Description,
                IsAssertion = true,
                Expected = this.Regex.ToString(),
                Actual = Variables.Instance.GetValue(this.Variable) as string,
                Success = result
            });
        }

        public override ValidationResult Validate()
        {
            ValidationResult result = base.Validate();
            
            if (String.IsNullOrEmpty(this.Regex))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Regular expression pattern is empty." });
            }

            if (String.IsNullOrEmpty(this.Name))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Name field is empty." });
            }

            Regex nameRegex = new Regex("^[a-zA-Z0-9_]+$", RegexOptions.Compiled);
            if (nameRegex.IsMatch(this.Name) == false)
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Name field: " + this.Name + " is not the valid format. Use only alphabets, numbers, underscore (_)." });
            }

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
