using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.IO;
using System.Net;

namespace AreteTester
{
    static class Program
    {
        private static Mutex mutex = null;

        public static string LocalBinDir
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\AreteTester\bin\"; }
        }

        private static SplashForm splashForm;
        private static Form mainForm;
        private static string[] cmdArgs;

        [STAThread]
        static void Main(string[] args)
        {
            cmdArgs = args;

            const string appName = "AreteTester";
            bool createdNew;

            ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            mutex = new Mutex(true, appName, out createdNew);
            if (!createdNew)
            {
                MessageBox.Show("Only one instance of AreteTester is allowed." + Environment.NewLine + Environment.NewLine + "Use Project -> View Project option if you want to refer any other project.", "Single Instance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            mainForm = CreateInstance("AreteTester.UI.MainForm", cmdArgs) as Form;

            splashForm = new SplashForm();
            splashForm.UpdateCompleted += new EventHandler(splashForm_UpdateCompleted);
            splashForm.DownloadChromeDriversCompleted += new EventHandler(splashForm_DownloadChromeDriversCompleted);

            if (splashForm.ShowDialog() == DialogResult.OK)
            {
                if (mainForm == null) return;

                Application.Run(mainForm);
            }
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string assemblyName = new AssemblyName(args.Name).Name;
            string assemblyPath = Path.Combine(LocalBinDir, assemblyName + ".dll");
            
            if (!File.Exists(assemblyPath)) return null;

            if (assemblyName == "AreteTester.UI") return null;
            
            return Assembly.LoadFrom(assemblyPath);
        }

        private static void splashForm_UpdateCompleted(object sender, EventArgs e)
        {
            mainForm = CreateInstance("AreteTester.UI.MainForm", cmdArgs) as Form;

            splashForm.MainForm = mainForm;

            splashForm.DownloadChromeDrivers();
        }

        private static void splashForm_DownloadChromeDriversCompleted(object sender, EventArgs e)
        {
        }

        public static object CreateInstance(string typename, params object[] args)
        {
            string dllPath = LocalBinDir + @"AreteTester.UI.dll";
            if (File.Exists(dllPath))
            {
                var dll = Assembly.LoadFrom(dllPath);

                return Activator.CreateInstance(dll.GetType(typename), args);
            }

            return null;
        }
    }
}
