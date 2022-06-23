
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using OpenQA.Selenium;
using System.ComponentModel;
using System.Drawing.Design;
using System.ComponentModel.Design;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;

namespace AreteTester.Actions
{
    [Serializable]
    public class SendEmail : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public string To { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public string Subject { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string Body { get; set; }

        public SendEmail()
        {
            this.ActionType = "SendEmail";
        }

        public override void Process()
        {
            base.Process();

            if (Variables.Instance.SmtpSettings == null)
            {
                NotifyOutput(new Output()
                {
                    ActionType = this.ActionType,
                    Description = this.Description,
                    Message = "SMTP settings not found. Use 'Set SMTP Details' action before this action to set all required SMTP details.",
                    IsAssertion = false,
                    DoNotLog = true
                });

                return;
            }

            string subject = Variables.Instance.Apply(this.Subject);
            string body = Variables.Instance.Apply(this.Body);

            var smtp = new SmtpClient
            {
                Host = Variables.Instance.SmtpSettings.SmtpServer,
                Port = Variables.Instance.SmtpSettings.Port,
                EnableSsl = Variables.Instance.SmtpSettings.SSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Variables.Instance.SmtpSettings.Account, Variables.Instance.SmtpSettings.Password)
            };

            var message = new MailMessage(Variables.Instance.SmtpSettings.Account, this.To, subject, body);

            smtp.Send(message);

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
            if (String.IsNullOrEmpty(this.To))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "To email address is empty." });
            }

            if (String.IsNullOrEmpty(this.To) == false)
            {
                string to = Variables.Instance.Apply(this.To);
                bool isValidEmail = Regex.IsMatch(to, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                if (isValidEmail == false)
                {
                    result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Email address is not in the valid format." });
                }
            }

            if (String.IsNullOrEmpty(this.Subject))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Subject is empty." });
            }
            if (String.IsNullOrEmpty(this.Body))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Body is empty." });
            }

            return result;
        }
    }
}