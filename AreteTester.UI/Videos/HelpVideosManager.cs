using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace AreteTester.UI
{
    internal class HelpVideosManager
    {
        private static HelpVideosManager instance;

        public List<HelpVideo> Videos { get; set; }

        private HelpVideosManager()
        {
            this.Videos = new List<HelpVideo>();
        }

        public static HelpVideosManager Instance
        {
            get
            {
                if (instance == null) instance = new HelpVideosManager();

                return instance;
            }
        }

        public void Load(string path)
        {
            if (File.Exists(path) == false) return;

            try
            {
                XDocument xdoc = XDocument.Load(path);

                foreach (XElement element in xdoc.Descendants("Video"))
                {
                    HelpVideo wizard = new HelpVideo();
                    wizard.Description = element.Attribute("description").Value;
                    wizard.URL = element.Attribute("url").Value;

                    this.Videos.Add(wizard);
                }
            }
            catch
            {
                //NOTE: catching all exception, because HTML saying content not available is downloaded if .xml was not found on server
            }
        }
    }

    public class HelpVideo
    {
        public string Description { get; set; }

        public string URL { get; set; }
    }
}
