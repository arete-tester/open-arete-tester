using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Data;

namespace AreteTester.Actions
{
    [Serializable]
    public class RunVariablesExpression : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        [TypeConverter(typeof(VariablePropertyConverter))]
        public string Variable { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public string Expression { get; set; }

        public RunVariablesExpression()
        {
            this.ActionType = "RunVariablesExpression";
            this.Variable = "@var";
        }

        public override void Process()
        {
            base.Process();

            string expression = Variables.Instance.Apply(this.Expression);

            DataTable dt = new DataTable();
            var value = dt.Compute(expression, "");

            Variables.Instance.SetValue(this.Variable, value);

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
            
            if (String.IsNullOrEmpty(this.Expression))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Expression is empty." });
            }

            return result;
        }
    }
}
