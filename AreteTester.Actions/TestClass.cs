using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AreteTester.Actions
{
    [Serializable]
    public class TestClass : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public string Name { get; set; }

        [Browsable(false)]
        [Category("Properties")]
        public List<TestFunction> Functions { get; set; }

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

        public TestClass()
        {
            this.ActionType = "TestClass";
            this.Name = "Class_" + Guid.NewGuid().ToString().ToLower().Replace("-", "").Substring(0, 5);
            this.Enabled = true;
            this.IsExpanded = false;
            this.Functions = new List<TestFunction>();
        }

        public override void Process()
        {
            base.Process();

            // setup function
            foreach (TestFunction function in this.Functions)
            {
                if (AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Stopped) break;
                if (AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Paused && AreteTester.Actions.Globals.PausedAction != null) break;

                if (function.Enabled && function.FunctionType == TestFunctionType.SetUp && IfConditionEvaler.Eval(function.If))
                {
                    function.Process();
                    break;
                }
            }

            // function
            foreach (TestFunction function in this.Functions)
            {
                if (AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Stopped) break;
                if (AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Paused && AreteTester.Actions.Globals.PausedAction != null) break;

                if (function.Enabled && function.FunctionType == TestFunctionType.Regular && IfConditionEvaler.Eval(function.If))
                {
                    function.Process();
                }
            }

            // tear down function
            foreach (TestFunction function in this.Functions)
            {
                if (AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Stopped) break;
                if (AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Paused && AreteTester.Actions.Globals.PausedAction != null) break;

                if (function.Enabled && function.FunctionType == TestFunctionType.Teardown && IfConditionEvaler.Eval(function.If))
                {
                    function.Process();
                    break;
                }
            }
        }

        public override ValidationResult Validate()
        {
            ValidationResult result = base.Validate();

            // check for more than one setup functions
            int count = 0;
            foreach (TestFunction function in this.Functions)
            {
                if (function.FunctionType == TestFunctionType.SetUp) count++;
            }

            if (count > 1)
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Test class: " + this.Name + " contains more than once Setup functions." });
            }

            // check for more than one teardown functions
            count = 0;
            foreach (TestFunction function in this.Functions)
            {
                if (function.FunctionType == TestFunctionType.Teardown) count++;
            }

            if (count > 1)
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Test class: " + this.Name + " contains more than once Teardown functions." });
            }

            // validate functions with same names
            List<string> functionNames = new List<string>();
            List<string> duplicateFunctionNames = new List<string>();
            foreach (TestFunction function in this.Functions)
            {
                if (functionNames.Contains(function.Name) && duplicateFunctionNames.Contains(function.Name) == false)
                {
                    duplicateFunctionNames.Add(function.Name);
                    result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Test class: " + this.Name + " contains multiple functions with the name: " + function.Name });
                }
                else
                {
                    functionNames.Add(function.Name);
                }
            }

            return result;
        }
    }
}