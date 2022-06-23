using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Text.RegularExpressions;
using OpenQA.Selenium;

namespace AreteTester.Actions
{
    [Serializable]
    public class DiscoverXPath : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public string RepeatElementName { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public string XPath { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public DiscoverXPathType DiscoverXPathType { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        [TypeConverter(typeof(VariablePropertyConverter))]
        public string Variable { get; set; }

        public DiscoverXPath()
        {
            this.ActionType = "DiscoverXPath";
            this.XPath = string.Empty;
            this.DiscoverXPathType = DiscoverXPathType.Normal;
            this.Variable = "@var";
        }

        public override void  Process()
        {           
            base.Process();

            // check is displayed
            string xpath = Variables.Instance.Apply(this.XPath);

            // set variable
            Variables.Instance.SetValue(this.Variable, xpath);

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

            if (String.IsNullOrEmpty(this.XPath))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "XPath is empty." });
            }

            if (String.IsNullOrEmpty(this.Variable))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Variable is empty." });
            }

            return result;
        }
    }
}
