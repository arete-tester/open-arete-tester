using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Net;
using System.Xml.Linq;
using System.Configuration;
using Microsoft.Win32;

namespace AreteTester.Core
{
    public class ChromeDriversDownloader
    {
        private string chromeDriversConfigPath = ConfigurationManager.AppSettings["web_url"] + "/Chromedrivers.xml";
        private string chromeDriversConfigLocalPath = Globals.LocalDir + @"ChromeDrivers\Chromedrivers.xml";
        private string chromeDriversRemotePath = ConfigurationManager.AppSettings["web_url"] + "/download/ChromeDrivers/";
        private string chromeDriversLocalPath = Globals.LocalDir + @"ChromeDrivers\";

        public string ChromeDriversLocalPath
        {
            get { return this.chromeDriversLocalPath; }
        }

        public void Download()
        {
            Download(false);
        }

        public void Download(bool isShell)
        {
            try
            {
                if (isShell)
                {
                    string localDir = Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(Globals)).Location) + @"\";
                    chromeDriversConfigLocalPath = chromeDriversConfigLocalPath.Replace(Globals.LocalDir, localDir);
                    chromeDriversLocalPath = chromeDriversLocalPath.Replace(AreteTester.Core.Globals.LocalDir, localDir);
                }

                if (Directory.Exists(chromeDriversLocalPath) == false)
                {
                    Directory.CreateDirectory(chromeDriversLocalPath);
                }

                string chromeVersion = GetChromeVersion();

                if (String.IsNullOrEmpty(chromeVersion)) return;

                WebClient client = new WebClient();
                client.DownloadFile(chromeDriversConfigPath, chromeDriversConfigLocalPath);

                XDocument xdoc = XDocument.Load(chromeDriversConfigLocalPath);
                foreach (XElement element in xdoc.Descendants("Driver"))
                {
                    string version = element.Attribute("version").Value;

                    if (version == chromeVersion)
                    {
                        string driverRemotePath = chromeDriversRemotePath + version + "/chromedriver.exe";
                        string driverLocalPath = chromeDriversLocalPath + version + @"\chromedriver.exe";

                        if (File.Exists(driverLocalPath) == false)
                        {
                            string localdir = chromeDriversLocalPath + version;
                            if (Directory.Exists(localdir) == false)
                            {
                                Directory.CreateDirectory(localdir);
                            }
                            client.DownloadFile(driverRemotePath, driverLocalPath);
                        }

                        break;
                    }
                }
            }
            catch
            {
                // TODO: 
            }
        }

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
