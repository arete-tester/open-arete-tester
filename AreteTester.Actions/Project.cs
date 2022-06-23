using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Drawing.Design;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace AreteTester.Actions
{
    [Serializable]
    public class Project : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public string Name { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        [Editor(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string OutputPath { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public string BinPath { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public string UserDataDir { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public WebDriverType WebDriverType { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        public new string If { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        public new bool IsBreakpointSet { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        [Category("Properties")]
        public override string NodeText
        {
            get
            {
                string nodeText = "Project";
                if (String.IsNullOrEmpty(Name) == false)
                {
                    nodeText += " : " + Name;
                }

                if (String.IsNullOrEmpty(Description) == false)
                {
                    nodeText += " : " + Description;
                }

                return nodeText;
            }
        }

        [Browsable(false)]
        public List<Module> Modules { get; set; }

        public Project()
        {
            this.IsBreakpointSet = false;
            this.ActionType = "Project";
            this.Name = "Project";
            this.WebDriverType = AreteTester.Actions.WebDriverType.Chrome;
            this.Modules = new List<Module>();
        }

        public override void Process()
        {
            base.Process();

            Globals.CurrentProject = this;
            Variables.Instance.SetValue("$$$BinPath", this.BinPath);

            foreach (Module module in this.Modules)
            {
                if (AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Stopped) break;
                if (AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Paused && AreteTester.Actions.Globals.PausedAction != null) break;

                if (module.Enabled)
                {
                    module.Process();
                }
            }
            Variables.Instance.SetValue("$$$BinPath", this.BinPath);
        }

        public override ValidationResult Validate()
        {
            ValidationResult result = new ValidationResult();
            if (String.IsNullOrEmpty(OutputPath))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "OutputPath cannot be empty" });
            }

            // validate for multiple modules with same name
            List<string> moduleNames = new List<string>();
            List<string> duplicateModuleNames = new List<string>();
            foreach (Module module in this.Modules)
            {
                if (moduleNames.Contains(module.Name) && duplicateModuleNames.Contains(module.Name) == false)
                {
                    duplicateModuleNames.Add(module.Name);
                    result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Module: " + this.Name + " contains multiple modules with the name: " + module.Name });
                }
                else
                {
                    moduleNames.Add(module.Name);
                }
            }

            return result;
        }
    }
}
