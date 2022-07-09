using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using System.IO;
using AreteTester;
using AreteTester.Actions;

namespace AreteTester.UI
{
    internal class Runner
    {
        public event EventHandler StartVariablesThread;
        public event EventHandler EndVariablesThread;

        private Dictionary<ActionBase, TreeNode> actionNodeMapping = new Dictionary<ActionBase, TreeNode>();
        private Dictionary<string, TreeNode> nameFunctionNodeMapping = new Dictionary<string, TreeNode>();
        private TreeNode currentNode;
        
        private static bool actionFound = false;
        private static List<string> outputs = new List<string>();

        public TreeView ActionsTree { get; set; }

        public DataGridView VariablesGrid { get; set; }

        public string OutputText { get; set; }
        public TextBox OutputTextBox { get; set; }

        public ToolStripButton ExecuteButton { get; set; }
        public ToolStripButton PauseButton { get; set; }
        public ToolStripButton ResumeButton { get; set; }
        public ToolStripButton AbortButton { get; set; }

        public static Dictionary<TreeNode, List<Exception>> ActionExceptions { get; set; }

        private static Runner instance;

        private Runner()
        {
            AreteTester.Core.Runner.Instance.RunThreadStarted += new EventHandler<RunThreadStartedEventArgs>(RunThread_Started);
            AreteTester.Core.Runner.Instance.RunThreadCompleted += new EventHandler(Instance_RunThreadCompleted);

            ActionBase.ProcessStarted += new EventHandler(ActionBase_ProcessStarted);
            ActionBase.ProcessPaused += new EventHandler(ActionBase_ProcessPaused);
            ActionBase.NotificationReceived += new EventHandler<NotificationEventArgs>(ActionBase_NotificationReceived);
            ActionBase.ExceptionReceived += new EventHandler<ActionExceptionEventArgs>(ActionBase_ExceptionReceived);
        }

        public static Runner Instance
        {
            get
            {
                if (instance == null) instance = new Runner();

                return instance;
            }
        }

        public void Run(Project project, object selectedAction, bool wait, bool start)
        {
            outputs.Clear();
            ActionExceptions = new Dictionary<TreeNode, List<Exception>>();

            try { this.VariablesGrid.Rows.Clear(); }
            catch { }

            if (start)
            {
                this.OutputTextBox.Clear();
                this.OutputText = string.Empty;
            }
            else
            {
                this.OutputTextBox.Text = this.OutputText;
            }

            actionNodeMapping.Clear();
            LoadActionNodeMapping(this.ActionsTree.Nodes[0]);
            LoadNameFunctionNodeMapping(this.ActionsTree.Nodes[0]);

            if (ValidateActions() == false) return;

            AreteTester.Core.Runner.Instance.Run(project, selectedAction, wait, start);

            if (StartVariablesThread != null) StartVariablesThread(this, null);
        }

        public void Abort()
        {
            if (currentNode == null) return;

            AreteTester.Actions.Globals.RunnerStatus = RunnerStatusType.Stopped;

            SetToolbarButtonStatus();

            if (currentNode.Tag != null && currentNode.Tag is ActionBase)
            {
                ActionBase action = (ActionBase)currentNode.Tag;
                ResetCurrentNodeImageIndex();
            }

            AppendOutputText("Project execution aborted either manually or after reaching Freeware limits (10 actions)" + Environment.NewLine);
        }

        private void LoadActionNodeMapping(TreeNode node)
        {
            node.BackColor = System.Drawing.Color.Empty;
            foreach (TreeNode childNode in node.Nodes)
            {
                if (childNode.Tag != null && childNode.Tag is ActionBase)
                {
                    actionNodeMapping.Add((ActionBase)childNode.Tag, childNode);
                }

                LoadActionNodeMapping(childNode);
            }
        }

        private void LoadNameFunctionNodeMapping(TreeNode node)
        {
            if (node.Tag is TestFunction)
            {
                string fullname = GetFunctionFullName(node);
                if (nameFunctionNodeMapping.ContainsKey(fullname) == false)
                {
                    nameFunctionNodeMapping.Add(fullname, node);
                }
            }

            foreach (TreeNode childNode in node.Nodes)
            {
                LoadNameFunctionNodeMapping(childNode);
            }
        }

        private string GetFunctionFullName(TreeNode node)
        {
            TestFunction function = (TestFunction)node.Tag;
            string fullname = function.Name;

            TreeNode parentNode = node.Parent;
            while (parentNode != null)
            {
                if (parentNode.Tag != null)
                {
                    if (parentNode.Tag is TestClass)
                    {
                        fullname = ((TestClass)parentNode.Tag).Name + "." + fullname;
                    }
                    else if (parentNode.Tag is Module)
                    {
                        fullname = ((Module)parentNode.Tag).Name + "." + fullname;
                    }
                }

                parentNode = parentNode.Parent;
            }

            return fullname;
        }

        private bool ValidateActions()
        {
            ActionValidator.Instance.ActionTree = ActionsTree;
            ActionValidator.Instance.RunnerValidation();

            if (ActionValidator.Instance.ValidationResults.Count > 0)
            {
                if (ActionValidator.Instance.IsErrorFound())
                {
                    MessageBox.Show("Issues found with test action(s).", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AreteTester.Actions.Globals.RunnerStatus = RunnerStatusType.Stopped;
                    return false;
                }
            }

            return ActionValidator.Instance.IsErrorFound() == false;
        }

        private void RunThread_Started(object sender, RunThreadStartedEventArgs args)
        {
            try
            {
                object classOrFunctionOrAction = args.ClassOrFunctionOrAction;

                try
                {

                    AreteTester.Actions.Globals.BreakpointActionsToIgnore = new List<ActionBase>();
                    if (AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Paused)
                    {
                        actionFound = false;
                        FillActionsWithBreakpointToIgnore(this.ActionsTree.Nodes[0]);
                    }

                    if (AreteTester.Actions.Globals.RunnerStatus != RunnerStatusType.Paused)
                    {
                        if (classOrFunctionOrAction == null)
                        {
                            AppendOutputText("Starting project execution..." + Environment.NewLine);
                        }
                    }

                    AreteTester.Actions.Globals.RunnerStatus = RunnerStatusType.Running;
                    
                    SetToolbarButtonStatus();                   
                }
                catch (Exception exc)
                {
                    string projectPath = (string)Variables.Instance.GetValue("$$$ProjectPath");
                    string exceptionLog = projectPath + @"\exception.log";
                    File.WriteAllText(exceptionLog, (exc.Message + Environment.NewLine + exc.StackTrace + Environment.NewLine));
                }

                if (currentNode != null && currentNode.ForeColor == System.Drawing.Color.Blue) currentNode.ForeColor = System.Drawing.Color.Black;
            }
            catch
            {
                // TODO:
            }
        }

        private void Instance_RunThreadCompleted(object sender, EventArgs e)
        {
            if (EndVariablesThread != null) EndVariablesThread(this, null);

            if (currentNode != null && currentNode.Tag != null && currentNode.Tag is ActionBase && AreteTester.Actions.Globals.RunnerStatus != RunnerStatusType.Paused)
            {
                ResetCurrentNodeImageIndex();
            }

            SetToolbarButtonStatus();
        }

        private void ActionBase_ExceptionReceived(object sender, ActionExceptionEventArgs e)
        {
            if (actionNodeMapping.ContainsKey(e.Action))
            {
                TreeNode node = actionNodeMapping[e.Action];

                node.BackColor = System.Drawing.Color.Yellow;

                if (ActionExceptions.ContainsKey(node) == false)
                {
                    ActionExceptions.Add(node, new List<Exception>());
                }

                ActionExceptions[node].Add(e.Exception);
            }

            string formattedMessage = AreteTester.Core.Runner.Instance.FormatExceptionMessage(e.Action, "ERROR : " + e.Exception.Message);
            //report.Logs.Add(new LogReportItem() { Description = formattedMessage });

            AppendOutputText(formattedMessage.Replace("<br/>", Environment.NewLine) + Environment.NewLine);
        }

        private void ActionBase_ProcessStarted(object sender, EventArgs e)
        {
            ResetCurrentNodeImageIndex();

            if (AreteTester.Actions.Globals.RunnerStatus != RunnerStatusType.Running) return;

            if (AreteTester.Actions.Globals.PausedAction != null) return;

            if (sender is ActionBase)
            {
                ActionBase action = (ActionBase)sender;
                if (actionNodeMapping.ContainsKey(action))
                {
                    TreeNode node = actionNodeMapping[action];
                    node.Parent.Expand();
                    System.Threading.Thread.Sleep(Preferences.Instance.WaitDuration);
                    currentNode = node;
                    SetCurrentNodeImageIndex();
                }
            }

            SetToolbarButtonStatus();
        }

        private void ActionBase_ProcessPaused(object sender, EventArgs e)
        {
            ResetCurrentNodeImageIndex();

            if (sender is ActionBase)
            {
                ActionBase action = (ActionBase)sender;
                if (actionNodeMapping.ContainsKey(action))
                {
                    TreeNode node = actionNodeMapping[action];
                    node.Parent.Expand();
                    currentNode = node;
                    SetCurrentNodeImageIndex();
                    AppendOutputText("Project execution paused..." + Environment.NewLine);
                }
            }

            SetToolbarButtonStatus();
        }

        private void ActionBase_NotificationReceived(object sender, NotificationEventArgs e)
        {
            if (e.Output.DoNotLog == false)
            {
                string formattedMessage = AreteTester.Core.Runner.Instance.FormatLogMessage(e.Output);
                AppendOutputText(formattedMessage.Replace("<br/>", Environment.NewLine) + Environment.NewLine);
            }

            if (e.Output.IsAssertion && sender is ActionBase)
            {
                ActionBase action = (ActionBase)sender;
                if (actionNodeMapping.ContainsKey(action))
                {
                    TreeNode node = actionNodeMapping[action];
                    node.ForeColor = (e.Output.Success) ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                }
            }
        }

        private void FillActionsWithBreakpointToIgnore(TreeNode node)
        {
            if (actionFound) return;

            foreach (TreeNode childNode in node.Nodes)
            {
                if (childNode.Tag != null && childNode.Tag is ActionBase)
                {
                    ActionBase action = (ActionBase)childNode.Tag;

                    if (actionFound == false && action == AreteTester.Actions.Globals.PausedAction)
                    {
                        actionFound = true;
                    }

                    if (action.IsBreakpointSet)
                    {
                        if (action != AreteTester.Actions.Globals.PausedAction && actionFound == false)
                        {
                            AreteTester.Actions.Globals.BreakpointActionsToIgnore.Add(action);
                        }
                    }
                }

                FillActionsWithBreakpointToIgnore(childNode);
            }
        }

        private void SetToolbarButtonStatus()
        {
            ExecuteButton.Enabled = AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Stopped;
            PauseButton.Enabled = AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Running;
            ResumeButton.Enabled = AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Paused;
            AbortButton.Enabled = AreteTester.Actions.Globals.RunnerStatus != RunnerStatusType.Stopped;
        }

        private void AppendOutputText(string text)
        {
            this.OutputText += text;
            this.OutputTextBox.AppendText(text);
        }

        private void SetCurrentNodeImageIndex()
        {
            if (currentNode == null || currentNode.Tag == null) return;

            ActionBase action = (ActionBase)currentNode.Tag;

            if (action.IsBreakpointSet)
            {
                currentNode.ImageIndex = currentNode.SelectedImageIndex = Constants.RUNNING_BREAKPOINT_ACTION_INDEX;
            }
            else
            {
                if (AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Paused)
                {
                    currentNode.ImageIndex = currentNode.SelectedImageIndex = Constants.RUNNING_ACTION_INDEX;
                }
                else
                {
                    if (action is Project) currentNode.ImageIndex = currentNode.SelectedImageIndex = Constants.PROJECT_IMAGE_INDEX;
                    else if (action is Module) currentNode.ImageIndex = currentNode.SelectedImageIndex = Constants.MODULE_IMAGE_INDEX;
                    else if (action is TestClass) currentNode.ImageIndex = currentNode.SelectedImageIndex = Constants.CLASS_IMAGE_INDEX;
                    else if (action is TestFunction) currentNode.ImageIndex = currentNode.SelectedImageIndex = Constants.FUNCTION_IMAGE_INDEX;
                    else currentNode.ImageIndex = currentNode.SelectedImageIndex = Constants.RUNNING_ACTION_INDEX;
                }
            }
        }

        private void ResetCurrentNodeImageIndex()
        {
            if (currentNode == null || currentNode.Tag == null) return;

            ActionBase action = (ActionBase)currentNode.Tag;

            if (action.IsBreakpointSet)
            {
                currentNode.ImageIndex = currentNode.SelectedImageIndex = Constants.BREAKPOINT_ACTION_INDEX;
            }
            else
            {
                if (action is Project) currentNode.ImageIndex = currentNode.SelectedImageIndex = Constants.PROJECT_IMAGE_INDEX;
                else if (action is Module) currentNode.ImageIndex = currentNode.SelectedImageIndex = Constants.MODULE_IMAGE_INDEX;
                else if (action is TestClass) currentNode.ImageIndex = currentNode.SelectedImageIndex = Constants.CLASS_IMAGE_INDEX;
                else if (action is TestFunction) currentNode.ImageIndex = currentNode.SelectedImageIndex = Constants.FUNCTION_IMAGE_INDEX;
                else currentNode.ImageIndex = currentNode.SelectedImageIndex = Constants.DEFAULT_ACTION_INDEX;
            }
        }
    }
}
