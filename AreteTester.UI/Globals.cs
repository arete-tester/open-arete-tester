using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Win32;

namespace AreteTester.UI
{
    internal class Globals
    {
        public static string LocalDir
        {
            get
            {
                string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\AreteTester\";
                if (Directory.Exists(local) == false)
                {
                    Directory.CreateDirectory(local);
                }

                return local;
            }
        }

        public static string LocalBinDir
        {
            get { return LocalDir + @"bin\"; }
        }

        public static string WebUrl
        {
            get { return "https://www.aretetester.com"; }
        }

        public static string WebDomain
        {
            get { return "aretetester.com"; }
        }

        static Globals()
        {
        }

        public static bool Exit { get; set; }

        public static string GetChromeVersion()
        {
            object regVersion = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Google\Chrome\BLBeacon", "version", null);
            if (regVersion != null)
            {
                string version = (string)regVersion;
                if (version.Contains("."))
                {
                    return version.Split('.')[0];
                }
            }

            return string.Empty;
        }
    }
}
