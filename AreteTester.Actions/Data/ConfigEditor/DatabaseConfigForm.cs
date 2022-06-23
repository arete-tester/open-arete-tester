using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AreteTester.Actions
{
    public partial class DatabaseConfigForm : Form
    {
        public DatabaseConfigForm()
        {
            InitializeComponent();
        }

        public DatabaseAction DatabaseAction { get; set; }

        private void DatabaseActionForm_Load(object sender, EventArgs e)
        {
            rbSqlServer.Checked = DatabaseAction.DatabaseType == DatabaseType.SqlServer;
            rbOther.Checked = DatabaseAction.DatabaseType == DatabaseType.Other;
            txtConnectionString.Text = DatabaseAction.ConnectionString;
            txtDllPath.Text = DatabaseAction.DllPath;
            txtConnectionClassName.Text = DatabaseAction.ConnectionClassName;
            txtCommandClassName.Text = DatabaseAction.CommandClassName;
            txtAdapterClassName.Text = DatabaseAction.AdapterClassName;

            txtDllPath.Enabled = rbOther.Checked;
            txtConnectionClassName.Enabled = rbOther.Checked;
            txtCommandClassName.Enabled = rbOther.Checked;
            txtAdapterClassName.Enabled = rbOther.Checked;
        }

        private void rbSqlServer_CheckedChanged(object sender, EventArgs e)
        {
            txtDllPath.Enabled = rbSqlServer.Checked == false;
            txtConnectionClassName.Enabled = rbSqlServer.Checked == false;
            txtCommandClassName.Enabled = rbSqlServer.Checked == false;
            txtAdapterClassName.Enabled = rbSqlServer.Checked == false;
        }

        private void rbOther_CheckedChanged(object sender, EventArgs e)
        {
            txtDllPath.Enabled = rbOther.Checked;
            txtConnectionClassName.Enabled = rbOther.Checked;
            txtCommandClassName.Enabled = rbOther.Checked;
            txtAdapterClassName.Enabled = rbOther.Checked;

            if (rbOther.Checked)
            {
                txtConnectionClassName.Text = "MySql.Data.MySqlClient.MySqlConnection";
                txtCommandClassName.Text = "MySql.Data.MySqlClient.MySqlCommand";
                txtAdapterClassName.Text = "MySql.Data.MySqlClient.MySqlDataAdapter";
            }
            else
            {
                txtConnectionClassName.Text = txtCommandClassName.Text = txtAdapterClassName.Text = string.Empty;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DatabaseAction.DatabaseType = rbSqlServer.Checked ? DatabaseType.SqlServer : DatabaseType.Other;
            DatabaseAction.ConnectionString = txtConnectionString.Text;
            DatabaseAction.DllPath = txtDllPath.Text;
            DatabaseAction.ConnectionClassName = txtConnectionClassName.Text;
            DatabaseAction.CommandClassName = txtCommandClassName.Text;
            DatabaseAction.AdapterClassName = txtAdapterClassName.Text;

            this.DialogResult = DialogResult.OK;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
