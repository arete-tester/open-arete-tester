using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace AreteTester
{
    public partial class SplashForm : Form
    {
        public event EventHandler UpdateCompleted;
        public event EventHandler DownloadChromeDriversCompleted;

        public SplashForm()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
        }

        public Form MainForm { get; set; }

        private void SplashForm_Load(object sender, EventArgs e)
        {
            string imageFile = "Splash.jpg";
            string binImageFile = Program.LocalBinDir + "Splash.jpg";

            pbSplash.Image = Image.FromFile(File.Exists(binImageFile) ? binImageFile : imageFile);

            UpdateBinaries();
        }

        public void UpdateBinaries()
        {
            Thread updateThread = new Thread(new ThreadStart(UpdateThread));
            updateThread.IsBackground = true;
            updateThread.Start();
        }

        private void UpdateThread()
        {
            lblStatus.Text = "Checking for latest software updates...";

            Updater updater = new Updater();
            updater.CheckForUpdateCompleted += new EventHandler(updater_CheckForUpdateCompleted);
            updater.Update();

            Thread.Sleep(1000);

            if (UpdateCompleted != null) UpdateCompleted(this, null);
        }

        private void updater_CheckForUpdateCompleted(object sender, EventArgs e)
        {
            Thread.Sleep(1000);
            lblStatus.Text = "Updating the software...";
        }

        public void DownloadChromeDrivers()
        {
            Thread downloadChromeDriversThread = new Thread(new ThreadStart(DownloadChromeDriversThread));
            downloadChromeDriversThread.IsBackground = true;
            downloadChromeDriversThread.Start();
        }

        private void DownloadChromeDriversThread()
        {
            lblStatus.Text = "Downloading Chrome drivers...";
            
            this.MainForm.GetType().GetMethod("DownloadChromeDrivers").Invoke(this.MainForm, null);

            Thread.Sleep(1000);

            this.DialogResult = DialogResult.OK;

            if (DownloadChromeDriversCompleted != null) DownloadChromeDriversCompleted(this, null);
        }
    }
}