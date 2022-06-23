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
    public class FileAppendText : ActionBase
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
        public bool EndWithNewline { get; set; }

        public FileAppendText()
        {
            this.EndWithNewline = true;
            this.ActionType = "FileAppendText";
        }

        public override void Process()
        {
            base.Process();

            string text = Variables.Instance.Apply(this.Text);
            string filepath = Variables.Instance.Apply(this.FilePath);

            File.AppendAllText(filepath, text);

            if (this.EndWithNewline)
            {
                File.AppendAllText(filepath, Environment.NewLine);
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
