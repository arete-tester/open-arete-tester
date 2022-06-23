using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AreteTester.Actions
{
    public class Preferences
    {
        private static Preferences instance;

        public static Preferences Instance
        {
            get
            {
                if (instance == null) instance = new Preferences();
                return instance;
            }
            set
            {
                instance = value;
            }
        }

        public bool ClipboardToXPath { get; set; }
        public bool LaunchXPathFinder { get; set; }
        public string XPathFinderVersion { get; set; }
        public string DefaultWorkspace { get; set; }
        public int WaitDuration { get; set; }
        public bool ShowOutputWindowOnExecution { get; set; }
        public bool WarnEmptyDescription { get; set; }
        public bool SetDefaultDescription { get; set; }
        public bool IgnoreEmptyDescription { get; set; }

        public Preferences()
        {
            this.ClipboardToXPath = true;
            this.LaunchXPathFinder = true;
            this.XPathFinderVersion = "0.9.7";
            this.DefaultWorkspace = string.Empty;
            this.WaitDuration = 200;
            this.ShowOutputWindowOnExecution = true;
            this.WarnEmptyDescription = false;
            this.SetDefaultDescription = false;
            this.IgnoreEmptyDescription = true;
        }

    }
}
