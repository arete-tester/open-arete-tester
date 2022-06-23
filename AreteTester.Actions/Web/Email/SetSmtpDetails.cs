using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace AreteTester.Actions
{
    [Serializable]
    public class SetSmtpDetails: ActionBase
    {
        [DisplayName("SMTP Server")]
        public string SmtpServer { get; set; }

        [DisplayName("Email Account")]
        public string Account { get; set; }

        public string Password { get; set; }

        public int Port { get; set; }

        public bool SSL { get; set; }

        public SetSmtpDetails()
        {
            this.SSL = true;
            this.Port = 587;
            this.ActionType = "SetSmtpDetails";
        }

        public override void Process()
        {
            base.Process();

            Variables.Instance.SmtpSettings = new SmtpSettings()
            {
                SmtpServer = this.SmtpServer,
                Account = this.Account,
                Password = this.Password,
                Port = this.Port,
                SSL = this.SSL
            };
        }

        public override ValidationResult Validate()
        {
            ValidationResult result = base.Validate();

            if (String.IsNullOrEmpty(this.Account))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Account (From email address) is empty." });
            }

            if (String.IsNullOrEmpty(this.Account) == false)
            {
                string to = Variables.Instance.Apply(this.Account);
                bool isValidEmail = Regex.IsMatch(to, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                if (isValidEmail == false)
                {
                    result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Accout (From email address) is not in the valid format." });
                }
            }

            if (String.IsNullOrEmpty(this.SmtpServer))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "SMTP server is empty." });
            }

            if (String.IsNullOrEmpty(this.Password))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Password is empty." });
            }

            return result;
        }
    }
}
