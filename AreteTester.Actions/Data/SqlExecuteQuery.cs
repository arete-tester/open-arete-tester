using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Data;

namespace AreteTester.Actions
{
    [Serializable]
    public class SqlExecuteQuery : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public string SQL { get; set; }

        [Browsable(false)]
        public List<SelectFieldMapping> MappingActions { get; set; }

        public SqlExecuteQuery()
        {
            this.ActionType = "SqlExecuteQuery";
            this.Description = "SQL Execute Query";

            this.MappingActions = new List<SelectFieldMapping>();
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
            
            DatabaseAdapter adapter = new DatabaseAdapter (Variables.Instance.Database);

            string sql = Variables.Instance.Apply(this.SQL);

            DataTable dt = adapter.ExecuteQuery(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                foreach (DataColumn column in dt.Columns)
                {
                    SelectFieldMapping mapping = this.MappingActions.Where(x => x.Field == column.ColumnName).FirstOrDefault();
                    if (mapping != null)
                    {
                        Variables.Instance.SetValue(mapping.Variable, row[mapping.Field]);
                    }
                }
            }
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
