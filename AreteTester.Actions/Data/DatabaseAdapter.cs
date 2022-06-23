using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace AreteTester.Actions
{
    internal class DatabaseAdapter
    {
        private DatabaseAction databaseAction;

        private Assembly dbAssembly;

        internal DatabaseAdapter(DatabaseAction databaseAction)
        {
            this.databaseAction = databaseAction;
        }

        internal DataTable ExecuteQuery(string sql)
        {
            if (String.IsNullOrEmpty(sql)) return null;

            IDbConnection connection = CreateConnection();

            IDbCommand command = CreateCommand(sql, connection);

            try
            {
                connection.Open();

                IDbDataAdapter dataAdapter = CreateDataAdapter(command);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            finally
            {
                connection.Close();
            }

            return null;
        }

        internal void ExecuteNonQuery(string sql)
        {
            IDbConnection connection = CreateConnection();

            IDbCommand command = CreateCommand(sql, connection);

            try
            {
                connection.Open();

                command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
        }

        private IDbConnection CreateConnection()
        {
            IDbConnection connection = null;

            if (databaseAction.DatabaseType == DatabaseType.SqlServer)
            {
                connection = new SqlConnection(databaseAction.ConnectionString);
            }
            else
            {
                if (dbAssembly == null)
                {
                    dbAssembly = Assembly.LoadFile(databaseAction.DllPath);
                }

                connection = (IDbConnection)Activator.CreateInstance(dbAssembly.GetType(databaseAction.ConnectionClassName));
            }

            return connection;
        }

        private IDbCommand CreateCommand(string sql, IDbConnection connection)
        {           
            IDbCommand command = null;

            if (databaseAction.DatabaseType == DatabaseType.SqlServer)
            {
                command = new SqlCommand();
            }
            else
            {
                if (dbAssembly == null)
                {
                    var asm = Assembly.LoadFile(databaseAction.DllPath);
                }

                command = (IDbCommand)Activator.CreateInstance(dbAssembly.GetType(databaseAction.CommandClassName));
            }

            if (command != null)
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                command.Connection = connection;
            }

            return command;
        }

        private IDbDataAdapter CreateDataAdapter(IDbCommand command)
        {
            IDbDataAdapter dataAdapter = null;

            if (databaseAction.DatabaseType == DatabaseType.SqlServer)
            {
                dataAdapter = new SqlDataAdapter((SqlCommand)command);
            }
            else
            {
                if (dbAssembly == null)
                {
                    var asm = Assembly.LoadFile(databaseAction.DllPath);
                }

                dataAdapter = (IDbDataAdapter)Activator.CreateInstance(dbAssembly.GetType(databaseAction.AdapterClassName), command);
            }

            return dataAdapter;
        }
    }
}
