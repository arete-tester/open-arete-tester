using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AreteTester.Actions;
using AreteTester.Core;

namespace AreteTester.UI
{
    public partial class ViewProjectForm : Form
    {
        private Project project;

        public ViewProjectForm()
        {
            InitializeComponent();
        }

        private void btnOpenProject_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                lblProjectPath.Text = dlg.SelectedPath;

                this.project = ProjectLoader.LoadProject(dlg.SelectedPath);

                LoadProjectTree();
            }
        }

        private void LoadProjectTree()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(LoadProjectTree));
            }
            else
            {
                ProjectActionTreeLoader.LoadProjectTree(tvActions, this.project);

                ProjectActionTreeLoader.ExpandCollapseNodes(tvActions);
            }
        }

        private void tvAction_AfterSelect(object sender, TreeViewEventArgs e)
        {
            pgActionValues.SelectedObject = e.Node.Tag;
        }

        private void lnkExport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void btnExportSelection_Click(object sender, EventArgs e)
        {
            if (tvActions.SelectedNode == null) return;

            if (tvActions.SelectedNode.Tag is Project)
            {
                MessageBox.Show("Project cannot be exported. Select Class, Function, or any action added under a function.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (tvActions.SelectedNode.Tag is Module)
            {
                MessageBox.Show("Module cannot be exported. Select Class, Function, or any action added under a function.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (tvActions.SelectedNode.Tag != null && tvActions.SelectedNode.Tag is ActionBase)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "XML Files | *.xml";
                dlg.DefaultExt = "xml";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    LoadSave.Save(tvActions.SelectedNode.Tag.GetType(), tvActions.SelectedNode.Tag, dlg.FileName);
                }
            }
        }
    }
}
