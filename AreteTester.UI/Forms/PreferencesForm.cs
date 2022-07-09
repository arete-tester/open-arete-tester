using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using AreteTester.Actions;
using AreteTester.Core;

namespace AreteTester.UI
{
    public partial class PreferencesForm : Form
    {
        private string preferencesFile = AreteTester.Core.Globals.LocalDir + "Preferences.xml";

        public PreferencesForm()
        {
            InitializeComponent();
        }

        private void PreferencesForm_Load(object sender, EventArgs e)
        {
            chkCopyClipboardToXPath.Checked = Preferences.Instance.ClipboardToXPath;
            chkLaunchXPathFinder.Checked = Preferences.Instance.LaunchXPathFinder;
            txtXPathFinderVersion.Text = Preferences.Instance.XPathFinderVersion;
            txtDefaultWorkspace.Text = Preferences.Instance.DefaultWorkspace;
            numWaitDuration.Value = Preferences.Instance.WaitDuration;
            chkShowOutputWindow.Checked = Preferences.Instance.ShowOutputWindowOnExecution;
            chkEmptyDescriptionWarning.Checked = Preferences.Instance.WarnEmptyDescription;
            chkSetDefaultDescription.Checked = Preferences.Instance.SetDefaultDescription;
            chkIgnoreEmptyDescription.Checked = Preferences.Instance.IgnoreEmptyDescription;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Preferences.Instance.ClipboardToXPath = chkCopyClipboardToXPath.Checked;
            Preferences.Instance.LaunchXPathFinder = chkLaunchXPathFinder.Checked;
            Preferences.Instance.XPathFinderVersion = txtXPathFinderVersion.Text;
            Preferences.Instance.DefaultWorkspace = txtDefaultWorkspace.Text;
            Preferences.Instance.WaitDuration = (int)numWaitDuration.Value;
            Preferences.Instance.ShowOutputWindowOnExecution = chkShowOutputWindow.Checked;
            Preferences.Instance.WarnEmptyDescription = chkEmptyDescriptionWarning.Checked;
            Preferences.Instance.SetDefaultDescription = chkSetDefaultDescription.Checked;
            Preferences.Instance.IgnoreEmptyDescription = chkIgnoreEmptyDescription.Checked;

            LoadSave.Save(typeof(Preferences), Preferences.Instance, preferencesFile);

            AutoClosingMessageBox.Show("Preferences saved successfully", "Save", 2000);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void lnkViewXPathFinder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://chrome.google.com/webstore/detail/xpath-finder/ihnknokegkbpmofmafnkoadfjkhlogph");
        }

        private void btnBrowseDefaultWorkspace_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtDefaultWorkspace.Text = dlg.SelectedPath;
            }
        }
    }
}
