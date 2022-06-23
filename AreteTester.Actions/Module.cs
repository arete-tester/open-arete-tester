using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AreteTester.Actions
{
    [Serializable]
    public class Module : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public string Name { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public string Document { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        [Category("Properties")]
        public List<TestClass> TestClasses { get; set; }

        [Browsable(false)]
        public List<Module> Modules { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        public new string If { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        public new bool IsBreakpointSet { get; set; }

        [Browsable(false)]
        [XmlIgnore]
        [Category("Properties")]
        public override string NodeText
        {
            get
            {
                string nodeText = ActionType;

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
        public bool IsExpanded { get; set; }

        public Module()
        {
            this.ActionType = "Module";
            this.Name = "module_" + Guid.NewGuid().ToString().ToLower().Replace("-", "").Substring(0, 5);
            this.Enabled = true;
            this.IsExpanded = false;
            this.TestClasses = new List<TestClass>();
            this.Modules = new List<Module>();
        }

        public override void Process()
        {
            base.Process();

            foreach (TestClass testClass in this.TestClasses)
            {
                if (AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Stopped) break;
                if (AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Paused && AreteTester.Actions.Globals.PausedAction != null) break;

                if (testClass.Enabled && IfConditionEvaler.Eval(testClass.If))
                {
                    testClass.Process();
                }
            }

            foreach (Module module in this.Modules)
            {
                module.Process();
            }
        }

        public override ValidationResult Validate()
        {
            ValidationResult result = base.Validate();

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

            // validate for multiple classes with same name
            List<string> classNames = new List<string>();
            List<string> duplicateClassNames = new List<string>();
            foreach(TestClass cls in this.TestClasses)
            {
                if (classNames.Contains(cls.Name) && duplicateClassNames.Contains(cls.Name) == false)
                {
                    duplicateClassNames.Add(cls.Name);
                    result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Module: " + this.Name + " contains multiple classes with the name: " + cls.Name });
                }
                else
                {
                    classNames.Add(cls.Name);
                }
            }

            return result;
        }
    }
}
