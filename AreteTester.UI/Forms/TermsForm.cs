using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace AreteTester.UI
{
    public partial class TermsForm : Form
    {
        public TermsForm()
        {
            InitializeComponent();
        }

        private void TermsForm_Load(object sender, EventArgs e)
        {
            string licenseFile = AreteTester.Core.Globals.LocalBinDir + @"\" + "LICENSE";

            txtTerms.Text = File.ReadAllText(licenseFile);
            txtTerms.SelectionStart = 0;
        }
    }
}
