using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace AreteTester.Actions
{
    [Serializable]
    public class SqlExecuteNonQuery : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public string SQL { get; set; }

        public SqlExecuteNonQuery()
        {
            this.ActionType = "SqlExecuteNonQuery";
            this.Description = "SQL Execute Non Query";
        }

        public override void Process()
        {
            base.Process();

            if (Variables.Instance.Database == null)
            {
                NotifyOutput(new Output()
                {
                    ActionType = this.ActionType,
                    Description = this.Description,
                    Message = "Database configuration not found. Use 'Set Database Configuration' action before this action to set required database information like connection string etc.",
                    IsAssertion = false,
                    DoNotLog = true
                });

                return;
            }

            DatabaseAdapter adapter = new DatabaseAdapter(Variables.Instance.Database);
            string sql = Variables.Instance.Apply(this.SQL);
            adapter.ExecuteNonQuery(sql);
        }

        public override ValidationResult Validate()
        {
            ValidationResult result = base.Validate();

            if (String.IsNullOrEmpty(this.SQL))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "SQL input is empty." });
            }

            return result;
        }
    }
}
