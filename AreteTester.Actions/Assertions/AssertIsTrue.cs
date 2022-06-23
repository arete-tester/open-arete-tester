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
    public class AssertIsTrue : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public string Name { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        [TypeConverter(typeof(VariablePropertyConverter))]
        public string Variable { get; set; }

        public AssertIsTrue()
        {
            this.ActionType = "AssertIsTrue";
            this.Variable = "@var";
        }

        public override void Process()
        {
            base.Process();

            bool result = Assert.IsTrue(Variables.Instance.GetValue(Variable));

            NotifyOutput(new Output()
            {
                Name = this.Name,
                ActionType = this.ActionType,
                Description = this.Description,
                IsAssertion = true,
                Expected = true.ToString(),
                Actual = result.ToString(),
                Success = result
            });
        }

        public override ValidationResult Validate()
        {
            ValidationResult result = new ValidationResult();

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
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Warning, Message = "Variable is empty." });
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
