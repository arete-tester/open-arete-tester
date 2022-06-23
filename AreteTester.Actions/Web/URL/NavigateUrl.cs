using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace AreteTester.Actions
{
    [Serializable]
    public class NavigateUrl : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public string URL { get; set; }

        public NavigateUrl()
        {
            this.ActionType = "NavigateUrl";
        }

        public override void Process()
        {
            base.Process();

            string url = Variables.Instance.Apply(this.URL);
            Globals.Driver.Navigate().GoToUrl(url);

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
            if (String.IsNullOrEmpty(this.URL))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "URL is empty." });
            }

            return result;
        }
    }
}
