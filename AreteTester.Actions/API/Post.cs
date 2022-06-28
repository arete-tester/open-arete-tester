using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using OpenQA.Selenium;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Net;
using System.Drawing.Design;
using System.ComponentModel.Design;

namespace AreteTester.Actions
{
    [Serializable]
    public class Post : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public string URL { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string Headers { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public string Variable { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string Data { get; set; }

        public Post()
        {
            this.ActionType = "Post";
        }

        public override void Process()
        {
            base.Process();

            WebClient client = new WebClient();

            // set headers
            string[] headers = this.Headers.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string header in headers)
            {
                string[] parts = header.Trim().Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                client.Headers[parts[0]] = parts[1];
            }

            var response = client.UploadString(this.URL, "POST", this.Data);

            Variables.Instance.SetValue(this.Variable, response);

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

            if (String.IsNullOrEmpty(this.Data))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Data is empty." });
            }

            return result;
        }
    }
}