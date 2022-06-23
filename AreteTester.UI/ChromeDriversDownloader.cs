using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Net;
using System.Xml.Linq;
using System.Configuration;

namespace AreteTester.UI
{
    internal class ChromeDriversDownloader
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
            try
            {
                if (Directory.Exists(chromeDriversLocalPath) == false)
                {
                    Directory.CreateDirectory(chromeDriversLocalPath);
                }

                string chromeVersion = Globals.GetChromeVersion();

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
    }
}
