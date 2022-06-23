using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace AreteTester.UI
{
    public class WizardManager
    {
        private static WizardManager instance;

        public List<Wizard> Wizards { get; set; }

        private WizardManager()
        {
            this.Wizards = new List<Wizard>();
        }

        public static WizardManager Instance
        {
            get
            {
                if (instance == null) instance = new WizardManager();

                return instance;
            }
        }

        public void Load(string path)
        {
            if (File.Exists(path) == false) return;

            try
            {
                XDocument xdoc = XDocument.Load(path);

                foreach (XElement element in xdoc.Descendants("Wizard"))
                {
                    Wizard wizard = new Wizard();
                    wizard.Id = element.Attribute("id").Value;
                    wizard.Category = element.Attribute("category").Value;
                    wizard.Color = element.Attribute("color").Value;
                    wizard.Title = element.Attribute("text").Value;

                    foreach (XElement contentElement in element.Descendants("Content"))
                    {
                        WizardContent wizardContent = new WizardContent();
                        wizardContent.Type = (WizardContentType)Enum.Parse(typeof(WizardContentType), contentElement.Attribute("type").Value);
                        wizardContent.Path = contentElement.Attribute("path").Value;

                        wizard.Content.Add(wizardContent);

                        foreach (XElement referenceElement in contentElement.Descendants("ReferenceLink"))
                        {
                            WizardReferenceLink referenceLink = new WizardReferenceLink();
                            referenceLink.Text = referenceElement.Attribute("text").Value;
                            referenceLink.Path = referenceElement.Attribute("path").Value;
                            referenceLink.Width = Int32.Parse(referenceElement.Attribute("width").Value);

                            wizardContent.ReferenceLinks.Add(referenceLink);
                        }
                    }

                    this.Wizards.Add(wizard);
                }
            }
            catch
            {
                //NOTE: catching all exception, because HTML saying content not available is downloaded if .xml was not found on server
            }
        }
    }

    public class Wizard
    {
        public string Id { get; set; }

        public string Category { get; set; }

        public string Color { get; set; }

        public string Title { get; set; }

        public List<WizardContent> Content { get; set; }

        public Wizard()
        {
            this.Content = new List<WizardContent>();
        }
    }

    public class WizardContent
    {
        public WizardContentType Type { get; set; }

        public string Path { get; set; }

        public List<WizardReferenceLink> ReferenceLinks { get; set; }

        public WizardContent()
        {
            this.ReferenceLinks = new List<WizardReferenceLink>();
        }
    }

    public class WizardReferenceLink
    {
        public string Text { get; set; }

        public string Path { get; set; }

        public int Width { get; set; }
    }

    public enum WizardContentType
    {
        HTML, 
        Image, 
        Gif
    }
}
