using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace AreteTester.UI
{
    static class Program
    {
        private static Mutex mutex = null;

        [STAThread]
        static void Main(string[] args)
        {
            const string appName = "AreteTester";
            bool createdNew;

            mutex = new Mutex(true, appName, out createdNew);
            if (!createdNew)
            {
                MessageBox.Show("Only one instance of AreteTester is allowed." + Environment.NewLine + Environment.NewLine + "Use Project -> View Project option if you want to refer any other project.", "Single Instance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }  

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new MainForm(args));
        }
    }
}
