using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace AreteTester.UI
{
    public partial class ResultFilesSelectionForm : Form
    {
        public string ResultFilesPath { get; set; }

        public List<string> SelectedFiles { get; set; }

        public ResultFilesSelectionForm()
        {
            InitializeComponent();

            this.SelectedFiles = new List<string>();
        }

        private void ResultFilesSelectionForm_Load(object sender, EventArgs e)
        {
            if (Directory.Exists(this.ResultFilesPath))
            {
                string[] files = Directory.GetFiles(this.ResultFilesPath, "*.xml", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    dgrFiles.Rows.Add(file);
                }
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dgrFiles.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgrFiles.SelectedRows)
                {
                    this.SelectedFiles.Add((string)row.Cells[0].Value);
                }
            }

            this.DialogResult = DialogResult.OK;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
