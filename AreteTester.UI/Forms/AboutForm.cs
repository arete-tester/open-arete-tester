using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using System.Configuration;

namespace AreteTester.UI
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();

            lblApplicationVersion.Text = ConfigurationManager.AppSettings["version"];
            lblSupportEmail.Text = "support@" + AreteTester.Core.Globals.WebDomain;
            lnkUrl.Text = AreteTester.Core.Globals.WebUrl;
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            string currentVersionsFile = AreteTester.Core.Globals.LocalBinDir + @"CurrentVersions.xml";
            if (File.Exists(currentVersionsFile))
            {
                XDocument xdoc = XDocument.Load(currentVersionsFile);

                lblLatestBuild.Text = xdoc.Root.Attribute("latest_build").Value;
            }
        }

        private void lnkUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(AreteTester.Core.Globals.WebUrl);
        }
    }
}
