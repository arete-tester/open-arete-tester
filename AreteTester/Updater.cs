using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;

namespace AreteTester
{
    internal class Updater
    {
        private string currentVersionsFile = Program.LocalBinDir + @"CurrentVersions.xml";
        private string serverVersionsFile = Program.LocalBinDir + @"ServerVersions.xml";

        private WebClient webClient = new WebClient();
        private string webUrl = string.Empty;

        private Dictionary<string, FileConfig> buildFiles = new Dictionary<string, FileConfig>();
        private Dictionary<string, int> currentFiles = new Dictionary<string, int>();

        public event EventHandler CheckForUpdateCompleted;

        internal Updater()
        {
            if (Directory.Exists(Program.LocalBinDir) == false)
            {
                Directory.CreateDirectory(Program.LocalBinDir);
            }

            webUrl = ConfigurationManager.AppSettings["web_url"];
        }

        public void Update()
        {
            try
            {
                DownloadServerVersionsXml();

                LoadCurrentFileVersions();

                IdentifyFilesToDownload();

                if (buildFiles.Values.Count(x => x.Download) > 0 && CheckForUpdateCompleted != null)
                {
                    CheckForUpdateCompleted(this, null);
                }

                DownloadFiles();

                SaveCurrentVersionXml();
            }
            catch
            {
                // TODO: 
            }
        }

        private void DownloadServerVersionsXml()
        {
            string serverPath = webUrl + "/ServerVersions.xml";

            webClient.DownloadFile(serverPath, serverVersionsFile);
        }

        private void LoadCurrentFileVersions()
        {
            if (File.Exists(currentVersionsFile) == false) return;

            XmlDocument doc = new XmlDocument();
            doc.Load(currentVersionsFile);

            XmlNodeList fileNodes = doc.DocumentElement.SelectNodes("/files/file");
            foreach (XmlNode fileNode in fileNodes)
            {
                string filename = fileNode.Attributes["name"].Value;
                int buildNumber = Convert.ToInt32(fileNode.Attributes["build"].Value);

                currentFiles.Add(filename, buildNumber);
            }
        }

        private void IdentifyFilesToDownload()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(serverVersionsFile);

            XmlNodeList buildNodes = doc.DocumentElement.SelectNodes("/builds/build");
            foreach (XmlNode buildNode in buildNodes)
            {
                int buildNumber = Convert.ToInt32(buildNode.Attributes["value"].Value);

                foreach (XmlNode fileNode in buildNode.ChildNodes)
                {
                    string filename = fileNode.Attributes["name"].Value;
                    string binPath = fileNode.Attributes["bin_path"].Value;
                    string serverPath = fileNode.Attributes["server_path"].Value;

                    if (buildFiles.ContainsKey(filename))
                    {
                        if (buildNumber > buildFiles[filename].BuildNumber)
                        {
                            buildFiles[filename].BuildNumber = buildNumber;
                            buildFiles[filename].BinPath = binPath;
                            buildFiles[filename].ServerPath = serverPath;
                        }
                    }
                    else
                    {
                        buildFiles.Add(filename, new FileConfig() { Filename = filename, BuildNumber = buildNumber, BinPath = binPath, ServerPath = serverPath });
                    }
                }
            }

            foreach (string file in currentFiles.Keys)
            {
                string destinationPath = GetBinFilePath(buildFiles[file]);
                if (buildFiles.ContainsKey(file) && buildFiles[file].BuildNumber == currentFiles[file] && File.Exists(destinationPath))
                {
                    buildFiles[file].Download = false;
                }
            }
        }

        private void DownloadFiles()
        {
            foreach (FileConfig fileConfig in buildFiles.Values)
            {
                if (fileConfig.Download == false) continue;

                string destinationPath = GetBinFilePath(fileConfig);
                
                string destinationDir = Path.GetDirectoryName(destinationPath);
                if (Directory.Exists(destinationDir) == false)
                {
                    Directory.CreateDirectory(destinationDir);
                }

                string serverPath = webUrl + "/bin/" + fileConfig.BuildNumber + "/" + fileConfig.ServerPath + fileConfig.Filename;

                webClient.DownloadFile(serverPath, destinationPath);   
            }
        }

        private void SaveCurrentVersionXml()
        {
            int latestBuild = 0;
            foreach (FileConfig fileConfig in buildFiles.Values)
            {
                if (fileConfig.BuildNumber > latestBuild)
                {
                    latestBuild = fileConfig.BuildNumber;
                }
            }

            XElement filesXml = new XElement("files", new XAttribute("latest_build", latestBuild));
            foreach (FileConfig fileConfig in buildFiles.Values)
            {
                filesXml.Add(new XElement("file", new XAttribute("name", fileConfig.Filename), new XAttribute("build", "" + fileConfig.BuildNumber)));
            }

            filesXml.Save(currentVersionsFile);
        }

        private string GetBinFilePath(FileConfig fileConfig)
        {
            return Program.LocalBinDir + (String.IsNullOrEmpty(fileConfig.BinPath) ? "" : (@"\" + fileConfig.BinPath)) + @"\" + fileConfig.Filename;
        }
    }


    internal class FileConfig
    {
        public string Filename { get; set; }

        public int BuildNumber { get; set; }

        public string BinPath { get; set; }

        public string ServerPath { get; set; }

        public bool Download { get; set; }

        public FileConfig()
        {
            this.Download = true;
        }
    }
}
