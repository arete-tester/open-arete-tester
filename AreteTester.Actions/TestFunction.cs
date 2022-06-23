using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Threading;

namespace AreteTester.Actions
{
    [Serializable]
    public class TestFunction : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public string Name { get; set; }

        [Browsable(false)]
        [Category("Properties")]
        public List<ActionBase> Actions { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public TestFunctionType FunctionType { get; set; }

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

        public TestFunction()
        {
            this.ActionType = "TestFunction";
            this.Name = "Function_" + Guid.NewGuid().ToString().ToLower().Replace("-", "").Substring(0, 5);
            this.Enabled = true;
            this.IsExpanded = false;
            this.FunctionType = TestFunctionType.Regular;
            this.Actions = new List<ActionBase>();
        }

        public override void Process()
        {
            if (AreteTester.Actions.Globals.RunnerStatus != RunnerStatusType.Running) return;

            base.Process();

            NotifyOutput(new Output()
            {
                ActionType = this.ActionType,
                Description = this.Description,
                IsAssertion = false,
                DoNotLog = true
            });

            foreach (ActionBase action in this.Actions)
            {
                try
                {
                    if (action.IsBreakpointSet)
                    {
                        if (Globals.PausedAction != action && Globals.BreakpointActionsToIgnore.Contains(action) == false)
                        {
                            AreteTester.Actions.Globals.RunnerStatus = RunnerStatusType.Paused;
                            Globals.BreakpointActionsToIgnore.Clear();
                        }
                    }

                    if (AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Stopped) break;

                    if (AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Paused)
                    {
                        Globals.PausedAction = action;
                        NotifyProcessPaused(action);
                        break;
                    }

                    if (Globals.PausedAction != null && Globals.PausedAction != action) continue;

                    if (Globals.PausedAction != null && Globals.PausedAction == action) Globals.PausedAction = null;

                    if (action.Enabled && IfConditionEvaler.Eval(action.If))
                    {
                        action.Process();

                        Thread.Sleep(Preferences.Instance.WaitDuration);
                    }
                }
                catch (Exception exc)
                {
                    base.HandleException(action, exc);
                    break;
                }
            }
        }
    }
}
