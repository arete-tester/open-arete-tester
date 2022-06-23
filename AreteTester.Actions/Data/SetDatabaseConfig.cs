using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;

namespace AreteTester.Actions
{
    [Serializable]
    public class SetDatabaseConfig : ActionBase
    {
        [Category("Properties")]
        [Editor(typeof(DatabaseActionEditor), typeof(UITypeEditor))]
        public DatabaseAction Database { get; set; }

        public SetDatabaseConfig()
        {
            this.ActionType = "SetDatabaseConfig";
            this.Database = new DatabaseAction();
        }

        public override void Process()
        {
            base.Process();

            Variables.Instance.Database = new DatabaseAction()
            {
                 AdapterClassName = this.Database.AdapterClassName,
                 CommandClassName = this.Database.CommandClassName,
                 ConnectionClassName = this.Database.ConnectionClassName,
                 ConnectionString = this.Database.ConnectionString,
                 DatabaseType = this.Database.DatabaseType,
                 DllPath = this.Database.DllPath
            };
        }

        public override ValidationResult Validate()
        {
            ValidationResult result = base.Validate();

            if (String.IsNullOrEmpty(this.Database.ConnectionString))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Connection string is empty." });
            }

            return result;
        }
    }
}
