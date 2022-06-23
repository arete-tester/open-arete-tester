﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AreteTester.Actions
{
    [Serializable]
    public class ConvertVariable : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public VariableType ConvertToType { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        [TypeConverter(typeof(VariablePropertyConverter))]
        public string Variable { get; set; }

        public ConvertVariable()
        {
            this.ActionType = "ConvertVariable";
            this.Variable = "@var";
            this.ConvertToType = VariableType.String;
        }

        public override void Process()
        {
            base.Process();

            object value = Variables.Instance.GetValue(this.Variable);
            object result = null;
            switch (this.ConvertToType)
            {
                case VariableType.String: result = Convert.ToString(value); break;
                case VariableType.Numeric: result = Convert.ToDecimal(value); break;
                case VariableType.DateTime: result = Convert.ToDateTime(value); break;
                case VariableType.Boolean: result = Convert.ToBoolean(value); break;
            }

            Variables.Instance.SetValue(this.Variable, result);

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

    public enum VariableType
    {
        String,
        Numeric,
        Boolean,
        DateTime
    }
}