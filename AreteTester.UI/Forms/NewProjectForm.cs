using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AreteTester.Actions;
using System.IO;
using System.Text.RegularExpressions;

namespace AreteTester.UI
{
    public partial class NewProjectForm : Form
    {
        public NewProjectForm()
        {
            InitializeComponent();
        }

        public string ProjectName
        {
            get { return txtName.Text; }
        }

        public string ProjectPath
        {
            get { return txtPath.Text; }
        }

        public string URL
        {
            get { return txtUrl.Text; }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Project name is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Regex regex = new Regex(@"^[a-zA-Z0-9\s]*$");
            if (regex.IsMatch(txtName.Text) == false)
            {
                MessageBox.Show("Only alpha numeric and whitespace characters are allowed for project name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (String.IsNullOrEmpty(txtPath.Text))
            {
                MessageBox.Show("Project path is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Directory.Exists(txtPath.Text) == false)
            {
                MessageBox.Show("Directory: " + txtPath.Text + " not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnBrowseProjectPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (String.IsNullOrEmpty(Preferences.Instance.DefaultWorkspace) == false && Directory.Exists(Preferences.Instance.DefaultWorkspace))
            {
                dlg.SelectedPath = Preferences.Instance.DefaultWorkspace;
            }
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtPath.Text = dlg.SelectedPath;
            }
        }
    }
}
