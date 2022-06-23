using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.IO;

namespace AreteTester.Actions
{
    [Serializable]
    public class TextExistsInFile : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        [TypeConverter(typeof(VariablePropertyConverter))]
        public string FilePath { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        [TypeConverter(typeof(VariablePropertyConverter))]
        public string Text { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        [TypeConverter(typeof(VariablePropertyConverter))]
        public string Variable { get; set; }

        public TextExistsInFile()
        {
            this.ActionType = "TextExistsInFile";
        }

        public override void Process()
        {
            base.Process();

            string text = Variables.Instance.Apply(this.Text);
            string filepath = Variables.Instance.Apply(this.FilePath);

            if (File.Exists(filepath))
            {
                string fileText = File.ReadAllText(filepath);

                Variables.Instance.SetValue(this.Variable, fileText.Contains(text));
            }
            else
            {
                Variables.Instance.SetValue(this.Variable, false);
            }

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
            
            if (String.IsNullOrEmpty(this.FilePath))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "File Path is empty." });
            }

            if (String.IsNullOrEmpty(this.Text))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Text is empty." });
            }

            return result;
        }
    }
}
