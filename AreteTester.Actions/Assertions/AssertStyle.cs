using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using OpenQA.Selenium;
using System.Text.RegularExpressions;

namespace AreteTester.Actions
{
    [Serializable]
    public class AssertStyle : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public string Name { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public string ID { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public string XPath { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public string Style { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public string Expected { get; set; }

        [Browsable(false)]
        [XmlIgnore]
        [Category("Properties")]
        public override string NodeText
        {
            get
            {
                return this.ActionType
                    + " : " + (String.IsNullOrEmpty(this.Style) ? "<style name>" : this.Style)
                    + " : " + (String.IsNullOrEmpty(this.Expected) ? "<expected value>" : this.Expected);
            }
        }

        public AssertStyle()
        {
            this.ActionType = "AssertStyle";
        }

        public override void Process()
        {
            base.Process();

            string xpath = Variables.Instance.Apply(this.XPath);
            IWebElement element = (String.IsNullOrEmpty(this.ID) == false ? Globals.Driver.FindElement(By.Id(this.ID)) : Globals.Driver.FindElement(By.XPath(xpath)));

            bool result = Assert.AreEqual(Expected, element.GetCssValue(this.Style), AssertEqualType.String);

            NotifyOutput(new Output()
            {
                Name = this.Name,
                ActionType = this.ActionType,
                Description = this.Description,
                IsAssertion = true,
                Expected = Expected,
                Actual = element.GetCssValue(this.Style),
                Success = result
            });
        }

        public override ValidationResult Validate()
        {
            ValidationResult result = base.Validate();
            
            if (String.IsNullOrEmpty(this.ID) && String.IsNullOrEmpty(this.XPath))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Both ID and XPath is empty." });
            }

            if (String.IsNullOrEmpty(this.ID) == false && String.IsNullOrEmpty(this.XPath) == false)
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Warning, Message = "Both ID and XPath assigned. ID takes precedence over XPath." });
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

            if (String.IsNullOrEmpty(this.Expected))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Expected is empty." });
            }
            
            if (String.IsNullOrEmpty(this.Style))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Style is empty." });
            }

            return result;
        }
    }
}
