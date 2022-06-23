using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace AreteTester.UI
{
    public partial class WizardForm : Form
    {
        private int index = 0;
        private Wizard wizard;

        public WizardForm()
        {
            InitializeComponent();
        }

        public void LoadWizard(string wizardId)
        {
            var wzrd = WizardManager.Instance.Wizards.Where(x => x.Id == wizardId).FirstOrDefault();
            if (wzrd != null)
            {
                LoadWizard(wzrd);
            }
        }

        public void LoadWizard(Wizard wizard)
        {
            this.wizard = wizard;
            this.Text = this.wizard.Title;
            btnPrev.Visible = btnNext.Visible = (this.wizard.Content.Count > 1);

            if (this.wizard.Content.Count > 0)
            {
                LoadContent(0);
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (index > 0 && this.wizard.Content.Count > 0) index--;

            LoadContent(index);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (index < this.wizard.Content.Count - 1) index++;

            LoadContent(index);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadContent(int index)
        {
            WizardContent content = this.wizard.Content[index];

            pnlContent.Controls.Clear();
            pnlReferenceLinks.Controls.Clear();

            switch (content.Type)
            {
                case WizardContentType.Image:
                case WizardContentType.Gif:
                    LoadImage(content.Path);
                    break;
                case WizardContentType.HTML:
                    LoadHtml(content.Path);
                    break;
            }

            LoadReferenceLink(content.ReferenceLinks);

            btnPrev.Enabled = (index > 0);
            btnNext.Enabled = (index < this.wizard.Content.Count - 1);
            btnClose.Enabled = (index == this.wizard.Content.Count - 1);
        }

        private void LoadImage(string path)
        {
            PictureBox pb = new PictureBox();
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            pb.Dock = DockStyle.Fill;
            pb.Load(path);

            pnlContent.Controls.Add(pb);
        }

        private void LoadHtml(string path)
        {
            WebBrowser browser = new WebBrowser() { ScriptErrorsSuppressed = true, Dock = DockStyle.Fill };
            browser.Navigate(path);

            pnlContent.Controls.Add(browser);
        }

        private void LoadReferenceLink(List<WizardReferenceLink> referenceLinks)
        {
            foreach (WizardReferenceLink referenceLink in referenceLinks)
            {
                Button btn = new Button() { Height = 29, Width = referenceLink.Width };
                btn.Text = referenceLink.Text;
                btn.Click += new EventHandler(ReferenceLink_Click);
                btn.Tag = referenceLink;

                pnlReferenceLinks.Controls.Add(btn);
            }
        }

        private void ReferenceLink_Click(object sender, EventArgs e)
        {
            WizardReferenceLink referenceLink = (WizardReferenceLink)((Button)sender).Tag;

            Process.Start(referenceLink.Path);
        }
    }
}
