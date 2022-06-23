using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AreteTester.Actions
{
    [Serializable]
    public class DatabaseAction
    {
        public DatabaseType DatabaseType { get; set; }

        public string ConnectionString { get; set; }

        public string DllPath { get; set; }

        public string ConnectionClassName { get; set; }

        public string CommandClassName { get; set; }

        public string AdapterClassName { get; set; }

        public override string ToString()
        {
            return "Database configuration...";
        }
    }

    public enum DatabaseType
    {
        SqlServer,
        Other
    }
}
