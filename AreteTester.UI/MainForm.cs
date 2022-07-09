using AreteTester.Actions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Diagnostics;
using System.Xml.Linq;
using System.Net;
using Microsoft.Win32;
using System.Xml;
using AreteTester.Core;

namespace AreteTester.UI
{
    public partial class MainForm : Form
    {
        private string profilePath = AreteTester.Core.Globals.LocalDir + @"\Profile.xml";

        private string videosPath = AreteTester.Core.Globals.WebUrl + "/Videos.xml";
        private string videosLocalPath = AreteTester.Core.Globals.LocalDir + @"\Videos.xml";

        private string chromeDriversLocalPath = AreteTester.Core.Globals.LocalDir + @"\ChromeDrivers\";

        private string preferencesFile = AreteTester.Core.Globals.LocalDir + "Preferences.xml";
        private Profile profile = new Profile();
        private TreeNode selectedActionNode;
        private Project project;
        private string projectPath;
        private object copiedObject;
        private TreeNode nodeCut;

        private Thread variablesThread;
        private bool exitvariablesThread = false;

        private SearchForm searchForm;
        private DocumentUpdateProgressForm documentUpdateProgressForm;
        private bool isStartedFromCommand = false;

        public string ProjectPath
        {
            get
            {
                return this.projectPath;
            }
        }

        public MainForm()
        {
            System.AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            InitializeComponent();

            if (File.Exists(preferencesFile))
            {
                Preferences.Instance = (Preferences)LoadSave.Load(typeof(Preferences), preferencesFile);
            }

            Runner.Instance.ExecuteButton = tbtnExecute;
            Runner.Instance.PauseButton = tbtnPause;
            Runner.Instance.ResumeButton = tbtnResume;
            Runner.Instance.AbortButton = tbtnAbort;

            Runner.Instance.StartVariablesThread += new EventHandler(Runner_StartVariablesThread);
            Runner.Instance.EndVariablesThread += new EventHandler(Runner_EndVariablesThread);

            splitContainerMain.Panel2Collapsed = true;

            ProjectOpenCloseEnableDisableMenus(false);

            var propertyPageButton = typeof(PropertyGrid).GetField("btnViewPropertyPages", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(pgActionValues);
            propertyPageButton.GetType().GetProperty("Visible").SetValue(propertyPageButton, false, null);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                string exceptionLog = AreteTester.Core.Globals.LocalDir + @"\exception.log";
                Exception exc = (Exception)e.ExceptionObject;
                File.AppendAllText(exceptionLog, (exc.Message + Environment.NewLine + exc.StackTrace + Environment.NewLine));

                MessageBox.Show("Log Path: " + exceptionLog + Environment.NewLine + Environment.NewLine + exc.Message + Environment.NewLine + exc.StackTrace, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Environment.Exit(0);
            }
            catch
            {
                // NOTE: Exception ignored
            }
        }

        public MainForm(string[] args) : this()
        {
            CheckForIllegalCrossThreadCalls = false;

            if (args.Length > 0)
            {
                this.isStartedFromCommand = true;
                this.projectPath = args[0];
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadWindow(string.Empty);

            if (this.isStartedFromCommand)
            {
                Execute(true, true, null);

                Application.Exit();
            }
        }

        public void DownloadChromeDrivers()
        {
            ChromeDriversDownloader chromeDriverDownloader = new ChromeDriversDownloader();
            chromeDriverDownloader.Download();
            this.chromeDriversLocalPath = chromeDriverDownloader.ChromeDriversLocalPath;
        }

        private void LoadWindow(string path)
        {
            DownloadVideosConfiguration();
            HelpVideosManager.Instance.Load(videosLocalPath);
            if (HelpVideosManager.Instance.Videos.Count > 0)
            {
                LoadHelpVideoMenus();
            }

            this.Activate();

            if (this.isStartedFromCommand)
            {
                LoadWindowThread(path);
            }
            else
            {
                Thread loadWindowThread = new Thread(new ParameterizedThreadStart(LoadWindowThread));
                loadWindowThread.IsBackground = true;
                loadWindowThread.Start(path);
            }
        }

        private void HelpWizardMenuMenu_Click(object sender, EventArgs e)
        {
            Wizard wizard = (Wizard)((ToolStripMenuItem)sender).Tag;

            WizardForm wizardForm = new WizardForm();
            wizardForm.LoadWizard(wizard);
            wizardForm.ShowDialog();
        }

        private void LoadHelpVideoMenus()
        {
            foreach (HelpVideo video in HelpVideosManager.Instance.Videos)
            {
                ToolStripMenuItem videoMenu = new ToolStripMenuItem(video.Description.TrimStart("#".ToCharArray()));
                videoMenu.Font = new Font(videoMenu.Font, System.Drawing.FontStyle.Regular);
                videoMenu.Click += new EventHandler(HelpVideoMenu_Click);
                videoMenu.Tag = video;

                mnuVideos.DropDownItems.Add(videoMenu);
            }
        }

        private void HelpVideoMenu_Click(object sender, EventArgs e)
        {
            HelpVideo video = (HelpVideo)((ToolStripMenuItem)sender).Tag;

            Process.Start(video.URL);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (project != null && MessageBox.Show("Do you want to save project before exiting application ?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SaveProject();
            }

            if (AreteTester.Actions.Globals.Driver != null)
            {
                AreteTester.Actions.Globals.Driver.Quit();
            }
        }

        private void DownloadVideosConfiguration()
        {
            try
            {
                WebClient client = new WebClient();
                client.DownloadFile(videosPath, videosLocalPath);
            }
            catch
            {
                // TODO: 
            }
        }

        private void LoadWindowThread(object o)
        {
            string path = (string)o;
            LoadActions();

            if (String.IsNullOrEmpty(path) == false)
            {
                projectPath = path;

                OpenProject();

                Execute(true, true, null);
            }
            else if (File.Exists(profilePath))
            {
                profile = (Profile)LoadSave.Load(typeof(Profile), profilePath);

                if (profile != null && String.IsNullOrEmpty(profile.ProjectPath) == false && Directory.Exists(profile.ProjectPath))
                {
                    projectPath = profile.ProjectPath;

                    OpenProject();
                }
            }
        }

        private void LoadActions()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(LoadActions));
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(AreteTester.Core.Globals.LocalBinDir + "Actions.xml");

                foreach (XmlNode categoryNode in doc.DocumentElement.ChildNodes)
                {
                    string categoryName = categoryNode.Attributes["name"].Value;
                    TreeNode categoryTreeNode = tvActionOptions.Nodes.Add(categoryName);

                    foreach (XmlNode groupOrActionNode in categoryNode.ChildNodes)
                    {
                        if (groupOrActionNode.Name == "ActionGroup")
                        {
                            string groupName = groupOrActionNode.Attributes["name"].Value;
                            TreeNode groupTreeNode = categoryTreeNode.Nodes.Add(groupName);

                            foreach (XmlNode actionNode in groupOrActionNode.ChildNodes)
                            {
                                AddActionNode(groupTreeNode, actionNode);
                            }
                        }
                        else if (groupOrActionNode.Name == "Action")
                        {
                            AddActionNode(categoryTreeNode, groupOrActionNode);
                        }
                    }
                }
            }
        }

        private void AddActionNode(TreeNode groupTreeNode, XmlNode actionNode)
        {
            ActionOption action = new ActionOption()
            {
                Name = actionNode.Attributes["name"].Value,
                TypeName = actionNode.Attributes["type"].Value,
                Description = actionNode.Attributes["description"].Value,
                HelpUrl = actionNode.Attributes["help_url"].Value
            };

            TreeNode actionTreeNode = groupTreeNode.Nodes.Add(action.Name);
            actionTreeNode.ImageIndex = actionTreeNode.SelectedImageIndex = 1;
            actionTreeNode.Tag = action;
        }

        protected override bool ProcessCmdKey(ref Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N))
            {
                NewProject_Click(null, null);
                return true;
            }
            else if (keyData == (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O))
            {
                OpenProject_Click(null, null);
                return true;
            }
            else if (keyData == (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S))
            {
                SaveProject_Click(null, null);
                return true;
            }
            else if (keyData == (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F))
            {
                Search_Click(null, null);
                return true;
            }
            else if (keyData == (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5))
            {
                Validate_Click(null, null);
                return true;
            }
            else if (keyData == (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F6))
            {
                mnuExecute_Click(null, null);
                return true;
            }
            else if (keyData == (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F7))
            {
                PauseExecution_Clicked(null, null);
                return true;
            }
            else if (keyData == (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F8))
            {
                ResumeExecution_Clicked(null, null);
                return true;
            }
            else if (keyData == (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F9))
            {
                AbortExecution_Clicked(null, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void tvAction_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tvActions.SelectedNode = e.Node;
                selectedActionNode = e.Node;
                contextMenuStrip1.Show(tvActions, e.Location);
            }
        }

        private void tvAction_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (Preferences.Instance.ClipboardToXPath == false) return;

            object action = e.Node.Tag;
            if (action != null && action is ActionBase)
            {
                System.Reflection.PropertyInfo xpathProperty = e.Node.Tag.GetType().GetProperty("XPath");
                if (xpathProperty != null)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        string text = Clipboard.GetText();
                        xpathProperty.SetValue(action, text, null);

                        object o = pgActionValues.SelectedObject;
                        pgActionValues.SelectedObject = null;
                        pgActionValues.SelectedObject = o;
                    }
                }
            }
        }

        private void tvAction_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Up)
                {
                    MoveNodeUp();
                }
                else if (e.KeyCode == System.Windows.Forms.Keys.Down)
                {
                    MoveNodeDown();
                }
                else if (e.KeyCode == System.Windows.Forms.Keys.X)
                {
                    Cut();
                }
                else if (e.KeyCode == System.Windows.Forms.Keys.C)
                {
                    Copy(false);
                }
                else if (e.KeyCode == System.Windows.Forms.Keys.V)
                {
                    Paste();
                }
            }

            if (e.KeyCode == System.Windows.Forms.Keys.Delete)
            {
                Delete_Click(sender, e);
            }
        }

        private void tvActionOptions_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Level == 0) return;

            if (e.Node.Level == 1 && e.Node.Parent.Text == "Actions") return;

            if (selectedActionNode == null) return;

            if (selectedActionNode.Tag is Project || selectedActionNode.Tag is Module || selectedActionNode.Tag is TestClass) return;

            string type = string.Empty;
            string description = string.Empty;
            if (e.Node.Tag is ActionOption)
            {
                ActionOption actionOption = (ActionOption)e.Node.Tag;
                type = actionOption.TypeName;
                description = actionOption.Description;
            }

            ActionBase action = (ActionBase)Activator.CreateInstance("AreteTester.Actions", type).Unwrap();
            if (Preferences.Instance.SetDefaultDescription)
            {
                action.Description = description;
            }
            bool addNode = false;
            bool addParentNode = false;

            if (selectedActionNode.Tag is TestFunction && (action is SelectFieldMapping) == false)
            {
                TestFunction test = (TestFunction)selectedActionNode.Tag;
                test.Actions.Add(action);
                addNode = true;
            }
            if (selectedActionNode.Tag is Repeater && (action is SelectFieldMapping) == false)
            {
                Repeater repeater = (Repeater)selectedActionNode.Tag;
                repeater.Actions.Add(action);
                addNode = true;
            }
            else if (selectedActionNode.Parent != null && selectedActionNode.Parent.Tag is TestFunction && (action is SelectFieldMapping) == false)
            {
                TestFunction test = (TestFunction)selectedActionNode.Parent.Tag;
                test.Actions.Add(action);
                addParentNode = true;
            }
            else if (selectedActionNode.Parent != null && selectedActionNode.Parent.Tag is Repeater && (action is SelectFieldMapping) == false)
            {
                Repeater repeater = (Repeater)selectedActionNode.Parent.Tag;
                repeater.Actions.Add(action);
                addParentNode = true;
            }
            else if (selectedActionNode.Tag is SqlExecuteQuery && action is SelectFieldMapping)
            {
                SqlExecuteQuery executeQuery = (SqlExecuteQuery)selectedActionNode.Tag;
                executeQuery.MappingActions.Add((SelectFieldMapping)action);
                addNode = true;
            }
            else if (selectedActionNode.Parent != null && selectedActionNode.Parent.Tag is SqlExecuteQuery && action is SelectFieldMapping)
            {
                SqlExecuteQuery executeQuery = (SqlExecuteQuery)selectedActionNode.Parent.Tag;
                executeQuery.MappingActions.Add((SelectFieldMapping)action);
                addParentNode = true;
            }

            if (addNode)
            {
                TreeNode node = selectedActionNode.Nodes.Add(action.NodeText);
                node.Tag = action;
                selectedActionNode.Expand();
            }
            else if (addParentNode)
            {
                TreeNode node = selectedActionNode.Parent.Nodes.Add(action.NodeText);
                node.Tag = action;
                selectedActionNode.Expand();
            }
        }

        private void tvActions_AfterSelect(object sender, TreeViewEventArgs e)
        {
            selectedActionNode = e.Node;

            pgActionValues.SelectedObject = selectedActionNode.Tag;

            EnableDisableContextMenusAndToolbarButtons();
        }

        private void tvActions_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is Module)
            {
                ((Module)e.Node.Tag).IsExpanded = false;
            }
            else if (e.Node.Tag is TestClass)
            {
                ((TestClass)e.Node.Tag).IsExpanded = false;
            }
            else if (e.Node.Tag is TestFunction)
            {
                ((TestFunction)e.Node.Tag).IsExpanded = false;
            }
            else if (e.Node.Tag is Repeater)
            {
                ((Repeater)e.Node.Tag).IsExpanded = false;
            }
        }

        private void tvActions_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is Module)
            {
                ((Module)e.Node.Tag).IsExpanded = true;
            }
            else if (e.Node.Tag is TestClass)
            {
                ((TestClass)e.Node.Tag).IsExpanded = true;
            }
            else if (e.Node.Tag is TestFunction)
            {
                ((TestFunction)e.Node.Tag).IsExpanded = true;
            }
            else if (e.Node.Tag is Repeater)
            {
                ((Repeater)e.Node.Tag).IsExpanded = true;
            }
        }

        private void pgActionValues_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (selectedActionNode != null)
            {
                if (selectedActionNode.Tag is Project) ValidateForEmptyName<Project>((Project)selectedActionNode.Tag, "Project", e);
                else if (selectedActionNode.Tag is Module) ValidateForEmptyName<Module>((Module)selectedActionNode.Tag, "Module", e);
                else if (selectedActionNode.Tag is TestClass) ValidateForEmptyName<TestClass>((TestClass)selectedActionNode.Tag, "Test class", e);
                else if (selectedActionNode.Tag is TestFunction) ValidateForEmptyName<TestFunction>((TestFunction)selectedActionNode.Tag, "Test function", e);

                if (selectedActionNode.Tag is ActionBase)
                {
                    ActionBase action = (ActionBase)selectedActionNode.Tag;
                    selectedActionNode.Text = action.NodeText;
                    ProjectActionTreeLoader.SetTreeNodeFontStyle(tvActions, selectedActionNode);

                    ReorderNode();
                }
            }
        }

        private void ValidateForEmptyName<T>(T o, string actionName, PropertyValueChangedEventArgs e)
        {
            T action = (T)selectedActionNode.Tag;

            string name = (string)action.GetType().GetProperty("Name").GetValue(action, null);
            if (String.IsNullOrEmpty(name))
            {
                MessageBox.Show(actionName +  " name is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.ChangedItem.PropertyDescriptor.SetValue(pgActionValues.SelectedObject, e.OldValue);
            }
        }

        private void ReorderNode()
        {
            if (selectedActionNode != null)
            {
                if (selectedActionNode.Tag is ActionBase)
                {
                    ActionBase action = (ActionBase)selectedActionNode.Tag;

                    if (action is TestFunction)
                    {
                        ReorderTestFunction((TestFunction)action);
                    }
                    else if (action is TestClass)
                    {
                        ReorderTestClass((TestClass)action);
                    }
                    else if (action is Module)
                    {
                        ReorderModule((Module)action);
                    }
                }
            }
        }

        private void ReorderTestFunction(TestFunction function)
        {
            if (selectedActionNode.Parent.Tag != null && selectedActionNode.Parent.Tag is TestClass)
            {
                TestClass cls = (TestClass)selectedActionNode.Parent.Tag;

                List<string> names = cls.Functions.Select(m => m.Name).ToList();

                int index = GetSortedIndex(names, function.Name);

                TreeNode functionNode = selectedActionNode;
                TreeNode clsNode = selectedActionNode.Parent;
                clsNode.Nodes.Remove(selectedActionNode);
                clsNode.Nodes.Insert(index, functionNode);

                tvActions.SelectedNode = functionNode;

                cls.Functions.Remove(function);
                cls.Functions.Insert(index, function);
            }
        }

        private void ReorderTestClass(TestClass cls)
        {
            if (selectedActionNode.Parent.Tag != null && selectedActionNode.Parent.Tag is Module)
            {
                Module module = (Module)selectedActionNode.Parent.Tag;

                List<string> names = module.TestClasses.Select(t => t.Name).ToList();

                int index = GetSortedIndex(names, cls.Name);

                TreeNode clsNode = selectedActionNode;
                TreeNode moduleNode = selectedActionNode.Parent;
                moduleNode.Nodes.Remove(selectedActionNode);
                moduleNode.Nodes.Insert(index, clsNode);

                tvActions.SelectedNode = clsNode;
                
                module.TestClasses.Remove(cls);
                module.TestClasses.Insert(index, cls);
            }
        }

        private void ReorderModule(Module module)
        {
            if (selectedActionNode.Parent.Tag != null && (selectedActionNode.Parent.Tag is Module || selectedActionNode.Parent.Tag is Project))
            {
                List<string> names = null;
                Module parentModule = null;
                Project project = null;

                if (selectedActionNode.Parent.Tag is Module)
                {
                    parentModule = (Module)selectedActionNode.Parent.Tag;
                }
                else if (selectedActionNode.Parent.Tag is Project)
                {
                    project = (Project)selectedActionNode.Parent.Tag;
                }

                if (parentModule != null)
                {
                    names = parentModule.Modules.Select(t => t.Name).ToList();
                }

                if (project != null)
                {
                    names = project.Modules.Select(t => t.Name).ToList();
                }

                int index = GetSortedIndex(names, module.Name);

                TreeNode moduleNode = selectedActionNode;
                TreeNode parentNode = selectedActionNode.Parent;
                parentNode.Nodes.Remove(selectedActionNode);
                parentNode.Nodes.Insert(index, moduleNode);

                tvActions.SelectedNode = moduleNode;

                if (parentModule != null)
                {
                    parentModule.Modules.Remove(module);
                    parentModule.Modules.Insert(index, module);
                }

                if (project != null)
                {
                    project.Modules.Remove(module);
                    project.Modules.Insert(index, module);
                }
            }
        }

        private int GetSortedIndex(List<string> names, string name)
        {
            List<string> sortedNames = names.OrderBy(n => n).ToList();
            int index = sortedNames.IndexOf(name);

            while (index < sortedNames.Count - 1 && sortedNames[index + 1].Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                index++;
            }

            return index;
        }

        private void NewProject_Click(object sender, EventArgs e)
        {
            // Save and close current project
            if (project != null)
            {
                DialogResult result = MessageBox.Show("Do you want to save current project before creating new project ?", "Save Project", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    SaveProject();
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            CloseProject();

            NewProjectForm newProjectForm = new NewProjectForm ();
            if (newProjectForm.ShowDialog() == DialogResult.OK)
            {
                // create new project
                project = new Project();
                project.Name = newProjectForm.ProjectName;

                projectPath = newProjectForm.ProjectPath;

                project.Modules.Add(new Module());
                project.Modules[0].IsExpanded = true;
                project.Modules[0].TestClasses.Add(new TestClass());
                project.Modules[0].TestClasses[0].IsExpanded = true;
                project.Modules[0].TestClasses[0].Functions.Add(new TestFunction());
                project.Modules[0].TestClasses[0].Functions[0].IsExpanded = true;

                if (String.IsNullOrEmpty(newProjectForm.URL) == false)
                {
                    project.Modules[0].TestClasses[0].Functions[0].Actions.Add(new NavigateUrl() { URL = newProjectForm.URL });
                }

                project.OutputPath = projectPath + @"\__output";

                SaveProject();

                LoadProjectTree();

                XElement variablesElement = new XElement("Variables",
                                                new XElement("Variable", new XAttribute("name", "sample_var1"), new XAttribute("value", "value1")));
                variablesElement.Save(projectPath + @"\Variables.xml");

                profile.AddRecentProject(this.projectPath);
                profile.ProjectPath = projectPath;
                LoadSave.Save(typeof(Profile), profile, profilePath);

                ProjectOpenCloseEnableDisableMenus(true);

                this.Text = "AreteTester" + " (" + projectPath + ")";
            }
        }

        private void OpenProject_Click(object sender, EventArgs e)
        {
            if (project != null)
            {
                if (MessageBox.Show("Do you want to save current project before opening an other project ?", "Save Project", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    SaveProject();
                }
            }

            // Open project
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                projectPath = dlg.SelectedPath;

                OpenProject();                
            }
        }

        private void OpenProject()
        {
            try
            {
                pgActionValues.SelectedObject = null;

                ProjectOpenCloseEnableDisableMenus(true);

                dgrVariableValues.Rows.Clear();
                if (AreteTester.Actions.Globals.RunnerStatus != RunnerStatusType.Stopped)
                {
                    Runner.Instance.Abort();
                }

                this.project = ProjectLoader.LoadProject(this.projectPath);

                tvActions.Nodes.Clear();

                LoadProjectTree();

                AreteTester.Actions.Globals.RunnerStatus = RunnerStatusType.Stopped;

                Variables.Instance.SetValue("$$$ProjectPath", this.projectPath);
                Variables.Instance.LoadVariables();

                if (tvActions.Nodes.Count > 0) tvActions.Nodes[0].Expand();

                this.profile.ProjectPath = projectPath;
                this.profile.AddRecentProject(this.projectPath);
                LoadSave.Save(typeof(Profile), profile, profilePath);

                this.Text = "AreteTester" + " (" + projectPath + ")";
            }
            catch 
            {
                // TODO: 
            }
        }

        private void LoadProjectTree()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(LoadProjectTree));
            }
            else
            {
                ProjectActionTreeLoader.LoadProjectTree(tvActions, this.project);

                ProjectActionTreeLoader.ExpandCollapseNodes(tvActions);
            }
        }

        private void SaveProject_Click(object sender, EventArgs e)
        {
            SaveProject();
        }

        private void CloseProject_Click(object sender, EventArgs e)
        {
            CloseProject();
        }

        private void mnuViewProject_Click(object sender, EventArgs e)
        {
            ViewProjectForm viewProjectForm = new ViewProjectForm();
            viewProjectForm.Show();
        }

        private void mnuExecute_Click(object sender, EventArgs e)
        {
            if (this.project == null) return;

            Variables.Instance.SetValue("$$$ProjectPath", this.projectPath);
            Variables.Instance.SetValue("$$$ProjectOutputPath", this.project.OutputPath);

            Execute(false, true, null);
        }

        private void Execute(bool wait, bool start, object selectedAction)
        {
            if (project != null)
            {
                if (start)
                {
                    ShowOutputWindowByPreference();
                }

                StartChromeDriver();

                Runner.Instance.ActionsTree = tvActions;
                Runner.Instance.VariablesGrid = dgrVariableValues;
                Runner.Instance.OutputTextBox = txtOutput;
                Runner.Instance.Run(this.project, selectedAction, wait, start);
            }
        }

        private void PauseExecution_Clicked(object sender, EventArgs e)
        {
            AreteTester.Actions.Globals.RunnerStatus = RunnerStatusType.Paused;
        }

        private void ResumeExecution_Clicked(object sender, EventArgs e)
        {
            Execute(false, false, null);
        }

        private void AbortExecution_Clicked(object sender, EventArgs e)
        {
            Runner.Instance.Abort();
        }

        private void mnuRemoveAllBreakpoints_Click(object sender, EventArgs e)
        {
            if (project == null) return;

            RemoveAllBreakpoints(tvActions.Nodes[0]);
        }

        private void RemoveAllBreakpoints(TreeNode node)
        {
            if (node.Tag is ActionBase)
            {
                ActionBase action = (ActionBase)node.Tag;
                if (action.IsBreakpointSet)
                {
                    action.IsBreakpointSet = false;
                    node.ImageIndex = node.SelectedImageIndex = (action.IsBreakpointSet ? 2 : 0);
                }
            }

            foreach (TreeNode childNode in node.Nodes)
            {
                RemoveAllBreakpoints(childNode);
            }
        }

        private void Validate_Click(object sender, EventArgs e)
        {
            if (project == null) return;

            splitContainerMain.Panel2Collapsed = false;
            tabOutput.SelectedIndex = 1;
            mnuViewOutputWindow.Text = "Hide Output Window";

            Thread validationThread = new Thread(new ThreadStart(ValidationThread));
            validationThread.IsBackground = true;
            validationThread.Start();
        }

        private void ValidationThread()
        {
            ActionValidator.Instance.ActionTree = this.tvActions;
            ActionValidator.Instance.RunnerValidation();

            if (ActionValidator.Instance.IsErrorFound())
            {
                MessageBox.Show("Actions validation is complete. Found action(s) with errors!", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                AutoClosingMessageBox.Show("Actions validation is complete.", "Validation", 2000);
            }

            txtValidationMessages.Text = string.Empty;
            foreach (TreeNode node in ActionValidator.Instance.ValidationResults.Keys)
            {
                txtValidationMessages.Text += (GetNodePath(node, "").Trim("->".ToCharArray()).Trim() + Environment.NewLine);
                foreach (ValidationMessage validationMessage in ActionValidator.Instance.ValidationResults[node].Messages)
                {
                    string message = validationMessage.MessageType == ValidationMessageType.Error ? "ERROR" : "WARNING";
                    message += " : ";
                    message += validationMessage.Message;
                    message += Environment.NewLine;

                    txtValidationMessages.Text += message;
                }

                txtValidationMessages.Text += Environment.NewLine;
            }
        }

        private string GetNodePath(TreeNode node, string path)
        {
            string[] parts = node.Text.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length > 1)
            {
                path = "(" + parts[0] + ":" + parts[1] + ")->" + path;
            }
            else if (parts.Length == 1)
            {
                path = "(" + parts[0] + ")->" + path;
            }

            if (node.Parent != null)
            {
                path = GetNodePath(node.Parent, path);
            }

            return path;
        }

        private bool SaveProject()
        {
            if (project == null) return false;

            if (String.IsNullOrEmpty(projectPath)) return false;

            List<string> messages = new List<string>();
            ValidateProjectForSave(this.project, messages);
            if (messages.Count > 0)
            {
                string displayMessage = "Project cannot be saved due to following errors";
                displayMessage += Environment.NewLine + String.Join(Environment.NewLine, messages.ToArray());

                MessageBox.Show(displayMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return true; // do not set null to project object
            }

            if (String.IsNullOrEmpty(projectPath) == false && project != null)
            {
                //BackupProject();

                string projectFile = projectPath + @"\project.atr";
                LoadSave.Save(typeof(Project), project, projectFile);

                foreach (Module module in project.Modules)
                {
                    SaveModule(module, projectPath);
                }

                AutoClosingMessageBox.Show("Project successfully saved.", "Save Project", 2000);
            }

            return true;
        }

        private void ValidateProjectForSave(Project project, List<string> messages)
        {
            List<string> names = new List<string>();
            foreach (Module module in project.Modules)
            {
                if (names.Contains(module.Name))
                {
                    messages.Add("Project contains multiple modules of name: " + module.Name + " under it.");
                }
                else
                {
                    names.Add(module.Name);
                }

                ValidateModuleForSave(module, messages);
            }
        }

        private void ValidateModuleForSave(Module module, List<string> messages)
        {
            List<string> clsNames = new List<string>();
            foreach (TestClass cls in module.TestClasses)
            {
                if (clsNames.Contains(cls.Name))
                {
                    messages.Add("Module: " + module.Name + " contains multiple test classes of name: " + cls.Name + " under it.");
                }
                else
                {
                    clsNames.Add(cls.Name);
                }
            }

            List<string> moduleNames = new List<string>();
            foreach (Module childModule in module.Modules)
            {
                if (moduleNames.Contains(childModule.Name))
                {
                    messages.Add("Module: " + module.Name + " contains multiple modules of name: " + childModule.Name + " under it.");
                }
                else
                {
                    moduleNames.Add(childModule.Name);
                }

                ValidateModuleForSave(childModule, messages);
            }
        }

        private void BackupProject()
        {
            string backupPath = projectPath + @"\__backup";
            if (Directory.Exists(backupPath)) Directory.Delete(backupPath, true);

            Directory.CreateDirectory(backupPath);

            foreach (string dirPath in Directory.GetDirectories(projectPath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(projectPath, backupPath));
            }

            foreach (string newPath in Directory.GetFiles(projectPath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(projectPath, backupPath), true);
            }

            foreach (string dirPath in Directory.GetDirectories(projectPath, "*", SearchOption.TopDirectoryOnly))
            {
                if ((new DirectoryInfo(dirPath)).Name != "__backup")
                {
                    Directory.Delete(dirPath, true);
                }
            }
        }

        private void SaveModule(Module module, string parentPath)
        {
            string modulePath = parentPath + @"\" + module.Name;
            if (Directory.Exists(modulePath) == false)
            {
                Directory.CreateDirectory(modulePath);
            }

            List<string> existingFiles = new List<string>();
            string[] files = Directory.GetFiles(modulePath);
            foreach (string file in files)
            {
                existingFiles.Add(Path.GetFileName(file).ToLower());
            }

            List<string> clsFiles = new List<string>();
            foreach (TestClass cls in module.TestClasses)
            {
                string classFile = modulePath + @"\" + cls.Name + ".atr";
                LoadSave.Save(typeof(TestClass), cls, classFile);

                clsFiles.Add(Path.GetFileName(classFile).ToLower());
            }

            foreach (string existingFile in existingFiles)
            {
                if (clsFiles.Contains(existingFile) == false)
                {
                    File.Delete(modulePath + @"\" + existingFile);
                }
            }

            foreach (Module childModule in module.Modules)
            {
                SaveModule(childModule, modulePath);
            }
        }

        private void CloseProject()
        {
            project = null;
            projectPath = string.Empty;
            tvActions.Nodes.Clear();

            if (String.IsNullOrEmpty(projectPath) == false)
            {
                profile.ProjectPath = projectPath;
                LoadSave.Save(typeof(Profile), profile, profilePath);
            }

            pgActionValues.SelectedObject = null;
            ProjectOpenCloseEnableDisableMenus(false);

            dgrVariableValues.Rows.Clear();
            if (AreteTester.Actions.Globals.RunnerStatus != RunnerStatusType.Stopped)
            {
                Runner.Instance.Abort();
            }

            this.Text = "AreteTester";
        }

        private void ProjectOpenCloseEnableDisableMenus(bool isOpened)
        {
            mnuCloseProject.Enabled = tbtnCloseProject.Enabled = isOpened;
            mnuSaveProject.Enabled = tbtnSaveProject.Enabled = isOpened;

            mnuExecute.Enabled = tbtnExecute.Enabled = isOpened;
            mnuPause.Enabled = tbtnPause.Enabled = false;
            mnuResume.Enabled = tbtnResume.Enabled = false;
            mnuAbort.Enabled = tbtnAbort.Enabled = false;

            mnuRemoveAllBreakpoints.Enabled = isOpened;

            tbtnCopy.Enabled = tbtnCut.Enabled = tbtnPaste.Enabled = tbtnDelete.Enabled = isOpened;
            tbtnExport.Enabled = tbtnImport.Enabled = isOpened;

            tbtnSearch.Enabled = tbtnValidate.Enabled = isOpened;

            tbtnValidationMessages.Enabled = tbtnExceptionMessages.Enabled = isOpened;

            tbtnAddModule.Enabled = tbtnAddTestClass.Enabled = tbtnAddTestFunction.Enabled = isOpened;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StartChromeDriver()
        {
            string chromeDriverExePath = string.Empty;

            string chromeVersion = ChromeDriversDownloader.GetChromeVersion();
            if (String.IsNullOrEmpty(chromeVersion) == false)
            {
                chromeDriverExePath = chromeDriversLocalPath + chromeVersion;
            }

            if (Directory.Exists(chromeDriverExePath) == false)
            {
                MessageBox.Show("Chrome driver not found. Restart the application to download Chrome driver automatically. Contact vendor if problem persists", "Chrome not found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<string> extensionPaths = new List<string>();
            if (Preferences.Instance.LaunchXPathFinder)
            {
                string pathToExtension = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\Extensions\ihnknokegkbpmofmafnkoadfjkhlogph\";
                if (Directory.Exists(pathToExtension))
                {
                    string[] extensionDirs = Directory.GetDirectories(pathToExtension);
                    if (extensionDirs.Length > 0)
                    {
                        extensionPaths.Add(extensionDirs[0]);
                    }
                }
            }

            DriverLoader.StartChromeDriver(chromeDriverExePath, extensionPaths, this.project.UserDataDir);
        }

        private void AddModule_Click(object sender, EventArgs e)
        {
            if (selectedActionNode == null || selectedActionNode.Tag == null) return;

            if (selectedActionNode.Tag == null || (selectedActionNode.Tag is Project == false && selectedActionNode.Tag is Module == false)) return;

            Module module = new Module();
            TreeNode moduleNode = selectedActionNode.Nodes.Add(module.NodeText);
            moduleNode.ImageIndex = moduleNode.SelectedImageIndex = Constants.MODULE_IMAGE_INDEX;
            moduleNode.Tag = module;

            if (selectedActionNode.Tag is Project)
            {
                ((Project)selectedActionNode.Tag).Modules.Add(module);
            }
            else if (selectedActionNode.Tag is Module)
            {
                ((Module)selectedActionNode.Tag).Modules.Add(module);
            }

            tvActions.SelectedNode = moduleNode;

            ReorderModule(module);

            selectedActionNode.Expand();
        }

        private void AddClass_Click(object sender, EventArgs e)
        {
            if (selectedActionNode == null || selectedActionNode.Tag == null) return;

            if (selectedActionNode.Tag == null || selectedActionNode.Tag is Module == false) return;

            TestClass cls = new TestClass();
            // add setup and tear down methods by default
            TestFunction setupFunction = new TestFunction() { FunctionType = TestFunctionType.SetUp, Name = "Setup" };
            cls.Functions.Add(setupFunction);

            TestFunction teardownFunction = new TestFunction() { FunctionType = TestFunctionType.Teardown, Name = "Teardown" };
            cls.Functions.Add(teardownFunction);

            TreeNode testClassNode = selectedActionNode.Nodes.Add(cls.NodeText);
            testClassNode.ImageIndex = testClassNode.SelectedImageIndex = Constants.CLASS_IMAGE_INDEX;
            testClassNode.Tag = cls;

            foreach (TestFunction function in cls.Functions)
            {
                ProjectActionTreeLoader.LoadTestFunctionNode(function, tvActions, testClassNode);
            }

            if (selectedActionNode.Tag is Module)
            {
                ((Module)selectedActionNode.Tag).TestClasses.Add(cls);
            }

            tvActions.SelectedNode = testClassNode;

            ReorderTestClass(cls);

            selectedActionNode.Expand();
        }

        private void AddTestFunction_Click(object sender, EventArgs e)
        {
            if (selectedActionNode == null) return;

            if (selectedActionNode.Tag == null || selectedActionNode.Tag is TestClass == false) return;

            TestFunction function = new TestFunction();
            TreeNode functionNode = selectedActionNode.Nodes.Add(function.NodeText);
            functionNode.ImageIndex = functionNode.SelectedImageIndex = Constants.FUNCTION_IMAGE_INDEX;
            functionNode.Tag = function;

            if (selectedActionNode.Tag is TestClass)
            {
                ((TestClass)selectedActionNode.Tag).Functions.Add(function);
            }

            tvActions.SelectedNode = functionNode;

            ReorderTestFunction(function);

            selectedActionNode.Expand();
        }

        private void Cut_Click(object sender, EventArgs e)
        {
            Cut();   
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            Copy(false);
        }

        private void Paste_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            Delete(tvActions.SelectedNode);
        }

        private void Cut()
        {
            Copy(true);
        }

        private void Copy(bool isCut)
        {
            if (tvActions.SelectedNode == null) return;

            this.copiedObject = DeepClone(tvActions.SelectedNode.Tag);
            this.nodeCut = (isCut ? tvActions.SelectedNode : null);
        }

        private void Paste()
        {
            if (tvActions.SelectedNode == null) return;

            if (this.copiedObject == null) return;

            object targetParentNode = tvActions.SelectedNode.Tag;

            object objectToPaste = DeepClone(this.copiedObject);

            bool pasted = false;
            if (objectToPaste is TestFunction && targetParentNode is TestClass)
            {
                ((TestClass)targetParentNode).Functions.Add((TestFunction)objectToPaste);
                ProjectActionTreeLoader.LoadTestFunctionNode((TestFunction)objectToPaste, tvActions, tvActions.SelectedNode);
                pasted = true;
            }
            else if (objectToPaste is TestClass && targetParentNode is Module)
            {
                ((Module)targetParentNode).TestClasses.Add((TestClass)objectToPaste);
                ProjectActionTreeLoader.LoadTestClassesInTree(new List<TestClass>() { (TestClass)objectToPaste }, tvActions, tvActions.SelectedNode);
                pasted = true;
            }
            else if (objectToPaste is Module && targetParentNode is Module)
            {
                ((Module)targetParentNode).Modules.Add((Module)objectToPaste);
                ProjectActionTreeLoader.LoadModulesInTree(new List<Module>() { (Module)objectToPaste }, tvActions, tvActions.SelectedNode);
                pasted = true;
            }
            else if (objectToPaste is Module && targetParentNode is Project)
            {
                ((Project)targetParentNode).Modules.Add((Module)objectToPaste);
                ProjectActionTreeLoader.LoadModulesInTree(new List<Module>() { (Module)objectToPaste }, tvActions, tvActions.SelectedNode);
                pasted = true;
            }
            else if (objectToPaste is ActionBase && targetParentNode is TestFunction
                && (objectToPaste is Project) == false && (objectToPaste is Module) == false && (objectToPaste is TestClass) == false && (objectToPaste is TestFunction) == false)
            {
                ((TestFunction)targetParentNode).Actions.Add((ActionBase)objectToPaste);
                TreeNode childNode = tvActions.SelectedNode.Nodes.Add(((ActionBase)objectToPaste).NodeText);
                childNode.Tag = objectToPaste;
                ProjectActionTreeLoader.SetTreeNodeFontStyle(tvActions, childNode);

                if (objectToPaste is Repeater)
                {
                    ProjectActionTreeLoader.LoadRepeater((Repeater)objectToPaste, tvActions, childNode);
                }
                pasted = true;
            }
            else if(objectToPaste is Repeater && targetParentNode is Repeater)
            {
                ((Repeater)targetParentNode).Actions.Add((ActionBase)objectToPaste);
                TreeNode childNode = tvActions.SelectedNode.Nodes.Add(((ActionBase)objectToPaste).NodeText);
                childNode.Tag = objectToPaste;
                ProjectActionTreeLoader.SetTreeNodeFontStyle(tvActions, childNode);

                if (objectToPaste is Repeater)
                {
                    ProjectActionTreeLoader.LoadRepeater((Repeater)objectToPaste, tvActions, childNode);
                }
                pasted = true;
            }
            else if (objectToPaste is ActionBase && targetParentNode is ActionBase 
                && (tvActions.SelectedNode.Parent != null && tvActions.SelectedNode.Parent.Tag is TestFunction))
            {
                ((TestFunction)tvActions.SelectedNode.Parent.Tag).Actions.Add((ActionBase)objectToPaste);
                TreeNode childNode = tvActions.SelectedNode.Parent.Nodes.Add(((ActionBase)objectToPaste).NodeText);
                childNode.Tag = objectToPaste;
                ProjectActionTreeLoader.SetTreeNodeFontStyle(tvActions, childNode);
                pasted = true;
            }

            if (pasted && this.nodeCut != null)
            {
                Delete(this.nodeCut);
                this.nodeCut = null;
            }
        }

        private void Delete(TreeNode nodeToDelete)
        {
            if (nodeToDelete == null) return;

            object o = nodeToDelete.Tag;
            if (o is Project) return;

            object parent = nodeToDelete.Parent.Tag;

            if (o is TestFunction)
            {
                TestClass cls = (TestClass)nodeToDelete.Parent.Tag;
                if (cls.Functions.Contains((TestFunction)o))
                {
                    cls.Functions.Remove((TestFunction)o);
                    nodeToDelete.Remove();
                }
            }
            else if (o is TestClass)
            {
                bool confirmDelete = true;
                if (this.nodeCut == null)
                {
                    confirmDelete = (MessageBox.Show("Test class deletion cannot be reverted. Are you sure you want to delete it.", "Delete Class", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK);
                }

                if (confirmDelete)
                {
                    Module module = (Module)nodeToDelete.Parent.Tag;
                    if (module.TestClasses.Contains((TestClass)o))
                    {
                        module.TestClasses.Remove((TestClass)o);

                        DeleteClassFile(nodeToDelete);

                        nodeToDelete.Remove();

                        SaveProject();
                    }
                }
            }
            else if ( o is Module && parent is Project)
            {
                bool confirmDelete = true;
                if (this.nodeCut == null)
                {
                    confirmDelete = (MessageBox.Show("Module deletion cannot be reverted. Are you sure you want to delete it.", "Delete Class", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK);
                }

                if (confirmDelete)
                {
                    Project project = (Project)nodeToDelete.Parent.Tag;
                    if (project.Modules.Contains((Module)o))
                    {
                        project.Modules.Remove((Module)o);

                        DeleteModuleFolder(nodeToDelete);

                        nodeToDelete.Remove();

                        SaveProject();
                    }
                }
            }
            else if (o is Module && parent is Module)
            {
                if (MessageBox.Show("Module deletion cannot be reverted. Are you sure you want to delete it.", "Delete Class", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    Module module = (Module)nodeToDelete.Parent.Tag;
                    if (module.Modules.Contains((Module)o))
                    {
                        module.Modules.Remove((Module)o);

                        DeleteModuleFolder(nodeToDelete);
                        
                        nodeToDelete.Remove();

                        SaveProject();
                    }
                }
            }
            else if (o is SelectFieldMapping && parent is SqlExecuteQuery)
            {
                SqlExecuteQuery executeQuery = (SqlExecuteQuery)nodeToDelete.Parent.Tag;
                if (executeQuery.MappingActions.Contains((SelectFieldMapping)o))
                {
                    executeQuery.MappingActions.Remove((SelectFieldMapping)o);
                    nodeToDelete.Remove();
                }
            }
            else if (parent is Repeater)
            {
                Repeater repeater = (Repeater)nodeToDelete.Parent.Tag;
                if (repeater.Actions.Contains((ActionBase)o))
                {
                    repeater.Actions.Remove((ActionBase)o);
                    nodeToDelete.Remove();
                }
            }
            else if (o is ActionBase && parent is TestFunction)
            {
                TestFunction function = (TestFunction)nodeToDelete.Parent.Tag;
                if (function.Actions.Contains((ActionBase)o))
                {
                    function.Actions.Remove((ActionBase)o);
                    nodeToDelete.Remove();
                }
            }
        }

        private void DeleteModuleFolder(TreeNode node)
        {
            string path = string.Empty;
            
            while (node.Parent != null)
            {
                if (node.Tag is Module)
                {
                    Module module = (Module)node.Tag;
                    path = module.Name + @"\" + path;
                }

                node = node.Parent;
            }

            path = this.projectPath + @"\" + path;

            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
            catch
            {
                MessageBox.Show("Delete module directory failed. Make sure that respective directory or its sub-directory is not open in windows explorer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteClassFile(TreeNode node)
        {
            string path = string.Empty;

            while (node.Parent != null)
            {
                if (node.Tag is TestClass)
                {
                    TestClass cls = (TestClass)node.Tag;
                    path = cls.Name + @"\" + path;
                }
                else if (node.Tag is Module)
                {
                    Module module = (Module)node.Tag;
                    path = module.Name + @"\" + path;
                }

                node = node.Parent;
            }

            path = this.projectPath + @"\" + path;
            path = path.Trim(@"\".ToCharArray());
            path += ".atr";

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private static object DeepClone(object obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return formatter.Deserialize(ms);
            }
        }

        private void mnuExpand_Click(object sender, EventArgs e)
        {
            if (tvActions.SelectedNode == null) return;

            tvActions.SelectedNode.Expand();
        }

        private void mnuExpandAll_Click(object sender, EventArgs e)
        {
            if (tvActions.SelectedNode == null) return;

            tvActions.SelectedNode.ExpandAll();
        }

        private void mnuCollapse_Click(object sender, EventArgs e)
        {
            if (tvActions.SelectedNode == null) return;

            tvActions.SelectedNode.Collapse();
        }

        private void mnuUp_Click(object sender, EventArgs e)
        {
            MoveNodeUp();
        }

        private void mnuDown_Click(object sender, EventArgs e)
        {
            MoveNodeDown();
        }

        private void MoveNodeUp()
        {
            if (tvActions.SelectedNode == null || tvActions.SelectedNode.Parent == null) return;

            if (tvActions.SelectedNode.Parent.Tag is TestFunction || tvActions.SelectedNode.Parent.Tag is Repeater)
            {
                List<ActionBase> actions = null;

                if (tvActions.SelectedNode.Parent.Tag is TestFunction)
                {
                    TestFunction testFunction = (TestFunction)tvActions.SelectedNode.Parent.Tag;
                    actions = testFunction.Actions;
                }
                else if (tvActions.SelectedNode.Parent.Tag is Repeater)
                {
                    Repeater repeater = (Repeater)tvActions.SelectedNode.Parent.Tag;
                    actions = repeater.Actions;
                }

                ActionBase action = (ActionBase)tvActions.SelectedNode.Tag;
                if (actions.Contains(action))
                {
                    int index = actions.IndexOf(action);

                    if (index > 0)
                    {
                        ActionBase temp = actions[index];
                        actions[index] = actions[index - 1];
                        actions[index - 1] = temp;

                        TreeNode node = (TreeNode)tvActions.SelectedNode.Clone();
                        TreeNode parent = tvActions.SelectedNode.Parent;

                        parent.Nodes.RemoveAt(index);
                        parent.Nodes.Insert(index - 1, node);

                        tvActions.SelectedNode = parent.Nodes[index - 1]; // ALERT: looks like a bug in .Net framework!
                    }
                }
            }
        }

        private void MoveNodeDown()
        {
            if (tvActions.SelectedNode == null || tvActions.SelectedNode.Parent == null) return;

            if (tvActions.SelectedNode.Parent.Tag is TestFunction || tvActions.SelectedNode.Parent.Tag is Repeater)
            {
                List<ActionBase> actions = null;

                if (tvActions.SelectedNode.Parent.Tag is TestFunction)
                {
                    TestFunction testFunction = (TestFunction)tvActions.SelectedNode.Parent.Tag;
                    actions = testFunction.Actions;
                }
                else if (tvActions.SelectedNode.Parent.Tag is Repeater)
                {
                    Repeater repeater = (Repeater)tvActions.SelectedNode.Parent.Tag;
                    actions = repeater.Actions;
                }

                ActionBase action = (ActionBase)tvActions.SelectedNode.Tag;
                if (actions.Contains(action))
                {
                    int index = actions.IndexOf(action);

                    if (index < actions.Count - 1)
                    {
                        ActionBase temp = actions[index];
                        actions[index] = actions[index + 1];
                        actions[index + 1] = temp;

                        TreeNode node = (TreeNode)tvActions.SelectedNode.Clone();
                        TreeNode parent = tvActions.SelectedNode.Parent;

                        parent.Nodes.RemoveAt(index);
                        parent.Nodes.Insert(index + 1, node);

                        tvActions.SelectedNode = parent.Nodes[index + 1];
                    }
                }
            }
        }


        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (selectedActionNode.Tag == null) return;

            EnableDisableContextMenusAndToolbarButtons();
        }

        private void EnableDisableContextMenusAndToolbarButtons()
        {
            mnuModule.Enabled = mnuClass.Enabled = mnuTestFunction.Enabled  = false;
            mnuImport.Enabled = mnuExport.Enabled = false;
            mnuCopyFullName.Enabled = false;
            tbtnAddModule.Enabled = tbtnAddTestClass.Enabled = tbtnAddTestFunction.Enabled = false;
            tbtnExecute.Enabled = false;
            tbtnImport.Enabled = tbtnExport.Enabled = false;

            mnuUp.Enabled = mnuDown.Enabled = true;
            if (selectedActionNode.Tag.GetType() == typeof(Project))
            {
                mnuModule.Enabled = true;
                mnuClass.Enabled = mnuTestFunction.Enabled = false;
                mnuUp.Enabled = mnuDown.Enabled = false;
                mnuSetBreakpoint.Enabled = false;
                mnuImport.Enabled = true;

                tbtnAddModule.Enabled = true;
                tbtnAddTestClass.Enabled = tbtnAddTestFunction.Enabled = tbtnExecute.Enabled = false;
                tbtnImport.Enabled = true;
            }
            else if (selectedActionNode.Tag.GetType() == typeof(Module))
            {
                mnuModule.Enabled = mnuClass.Enabled = true;
                mnuTestFunction.Enabled = false;
                mnuUp.Enabled = mnuDown.Enabled = false;
                mnuSetBreakpoint.Enabled = false;
                mnuImport.Enabled = mnuExport.Enabled = true;
                mnuCopyFullName.Enabled = true;

                tbtnAddModule.Enabled = tbtnAddTestClass.Enabled = true;
                tbtnAddTestFunction.Enabled = tbtnExecute.Enabled = false;
                tbtnImport.Enabled = tbtnExport.Enabled = true;
            }
            else if (selectedActionNode.Tag.GetType() == typeof(TestClass))
            {
                mnuTestFunction.Enabled = true;
                mnuModule.Enabled = mnuClass.Enabled = false;
                mnuUp.Enabled = mnuDown.Enabled = false;
                mnuSetBreakpoint.Enabled = false;
                mnuImport.Enabled = mnuExport.Enabled = true;
                mnuCopyFullName.Enabled = true;

                tbtnAddTestFunction.Enabled = true;
                tbtnExecute.Enabled = true;
                tbtnAddModule.Enabled = tbtnAddTestClass.Enabled = false;
                tbtnImport.Enabled = tbtnExport.Enabled = true;
            }
            else if (selectedActionNode.Tag.GetType() == typeof(TestFunction))
            {
                mnuModule.Enabled = mnuClass.Enabled = mnuTestFunction.Enabled = false;
                mnuUp.Enabled = mnuDown.Enabled = false;
                mnuSetBreakpoint.Enabled = false;
                mnuExport.Enabled = true;
                mnuCopyFullName.Enabled = true;

                tbtnAddModule.Enabled = tbtnAddTestClass.Enabled = tbtnAddTestFunction.Enabled = false;
                tbtnExecute.Enabled = true;
                tbtnExport.Enabled = true;
            }
            else if (selectedActionNode.Tag is ActionBase)
            {
                mnuSetBreakpoint.Enabled = true;
                mnuSetBreakpoint.Text = (((ActionBase)selectedActionNode.Tag).IsBreakpointSet ? "Clear Breakpoint" : "Set Breakpoint");
            }

            mnuExecuteFunction.Enabled = AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Stopped;
            mnuPauseFunction.Enabled = AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Running;
            mnuResumeFunction.Enabled = AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Paused;
            mnuAbortFunction.Enabled = AreteTester.Actions.Globals.RunnerStatus != RunnerStatusType.Stopped;

            tbtnExecute.Enabled = AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Stopped;
            tbtnPause.Enabled = AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Running;
            tbtnResume.Enabled = AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Paused;
            tbtnAbort.Enabled = AreteTester.Actions.Globals.RunnerStatus != RunnerStatusType.Stopped;
        }

        private void mnuLicense_Click(object sender, EventArgs e)
        {
            Process.Start("notepad", Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(MainForm)).Location) + @"\LICENSE");
        }

        private void mnuExecuteFunction_Click(object sender, EventArgs e)
        {
            Execute(false, true, selectedActionNode.Tag);
        }

        private void mnuPauseFunction_Click(object sender, EventArgs e)
        {
            AreteTester.Actions.Globals.RunnerStatus = RunnerStatusType.Paused;
        }

        private void mnuResumeFunction_Click(object sender, EventArgs e)
        {
            Execute(false, false, selectedActionNode.Tag);
        }

        private void mnuAbortFunction_Click(object sender, EventArgs e)
        {
            Runner.Instance.Abort();
        }

        private void mnuValidationMessages_Click(object sender, EventArgs e)
        {
            if (selectedActionNode == null || selectedActionNode.Tag == null) return;

            if (ActionValidator.Instance.ValidationResults.ContainsKey(selectedActionNode))
            {
                List<string> messages = new List<string>();
                foreach (ValidationMessage validationMessage in ActionValidator.Instance.ValidationResults[selectedActionNode].Messages)
                {
                    string message = validationMessage.MessageType == ValidationMessageType.Error ? "ERROR" : "WARNING";
                    message += " : ";
                    message += validationMessage.Message;

                    messages.Add(message);
                }

                MessageBox.Show(String.Join(Environment.NewLine, messages.ToArray()), "Validation", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("No validation message(s) available. You can view validation messages if any of its property value(s) found invalid before execution.", "Exception(s)", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void mnuExceptionMessage_Click(object sender, EventArgs e)
        {
            if (selectedActionNode == null || selectedActionNode.Tag == null) return;

            if (Runner.ActionExceptions != null && Runner.ActionExceptions.ContainsKey(selectedActionNode))
            {
                List<Exception> excs = Runner.ActionExceptions[selectedActionNode];
                string excMessages = string.Empty;

                foreach (Exception exc in excs)
                {
                    excMessages += exc.Message + Environment.NewLine + Environment.NewLine;
                }

                MessageBox.Show(excMessages, "Exception(s)", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("No exception message(s) available. You can view exception messages if execution unexpectedly at this point. Action with exceptions are highlighted in yellow color.", "Exception(s)", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void mnuRecentProjects_DropDownOpening(object sender, EventArgs e)
        {
            mnuRecentProjects.DropDownItems.Clear();
            List<RecentProject> recentProjects = this.profile.RecentProjects.OrderBy(x => x.DateTimeOpened).ToList();
            for (int i = recentProjects.Count - 1; i >= 0; i--)
            {
                RecentProject project = recentProjects[i];
                if (Directory.Exists(project.Path))
                {
                    ToolStripMenuItem menuItem = new ToolStripMenuItem(project.Path);
                    menuItem.Tag = project.Path;
                    menuItem.Click += new EventHandler(RecentProjectMenuItem_Click);
                    mnuRecentProjects.DropDownItems.Add(menuItem);
                }
                else
                {
                    recentProjects.Remove(project);
                }
            }

            LoadSave.Save(typeof(Profile), profile, profilePath);
        }

        private void RecentProjectMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;

            this.projectPath = (string)menuItem.Tag;

            OpenProject();            
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            (new AboutForm()).Show();
        }

        private void mnuPreferences_Click(object sender, EventArgs e)
        {
            PreferencesForm preferencesForm = new PreferencesForm();
            preferencesForm.ShowDialog();
        }

        private void mnuRun_DropDownOpening(object sender, EventArgs e)
        {
            if (this.project == null) return;

            mnuExecute.Enabled = AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Stopped;
            mnuPause.Enabled = tbtnPause.Enabled = AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Running;
            mnuResume.Enabled = tbtnResume.Enabled = AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Paused;
            mnuAbort.Enabled = tbtnAbort.Enabled = AreteTester.Actions.Globals.RunnerStatus != RunnerStatusType.Stopped;

            mnuViewOutputWindow.Text = (splitContainerMain.Panel2Collapsed) ? "View Output Window" : "Hide Output Window";
        }

        private void mnuCopyFullName_Click(object sender, EventArgs e)
        {
            if (selectedActionNode == null) return;

            ActionBase action = (ActionBase)selectedActionNode.Tag;
            List<string> names = new List<string>();

            if (action is Module) names.Add(((Module)action).Name);
            else if (action is TestClass) names.Add(((TestClass)action).Name);
            else if (action is TestFunction) names.Add(((TestFunction)action).Name);

            TreeNode parentNode = selectedActionNode.Parent;
            while ((parentNode.Tag is Project) == false)
            {
                if (parentNode.Tag is TestClass)
                {
                    names.Add(((TestClass)parentNode.Tag).Name);
                }
                else if (parentNode.Tag is Module)
                {
                    names.Add(((Module)parentNode.Tag).Name);
                }
                else if (parentNode.Tag is TestFunction)
                {
                    names.Add(((TestFunction)parentNode.Tag).Name);
                }

                parentNode = parentNode.Parent;
            }

            names.Reverse();
            string fullName = String.Join(".", names.ToArray());

            Clipboard.SetText(fullName);
        }

        private void mnuSetBreakpoint_Click(object sender, EventArgs e)
        {
            if (selectedActionNode.Tag is ActionBase)
            {
                ActionBase action = (ActionBase)selectedActionNode.Tag;
                action.IsBreakpointSet = (mnuSetBreakpoint.Text == "Set Breakpoint");
                selectedActionNode.ImageIndex = selectedActionNode.SelectedImageIndex = (action.IsBreakpointSet ? 2 : 0);
            }
        }

        private void Search_Click(object sender, EventArgs e)
        {
            if (searchForm == null || searchForm.IsDisposed)
            {
                searchForm = new SearchForm();
                searchForm.CloseClicked += new EventHandler(searchForm_CloseClicked);

                searchForm.ActionTree = tvActions;
            }

            searchForm.Show();
            searchForm.BringToFront();
        }

        private void searchForm_CloseClicked(object sender, EventArgs e)
        {
            searchForm.Close();
            searchForm = null;
        }

        private void mnuReports_DropDownOpening(object sender, EventArgs e)
        {
            if (project == null) return;

            mnuReports.DropDownItems.Clear();
            ToolStripMenuItem showDirectoryMenuItem = new ToolStripMenuItem("Open Output Directory");
            showDirectoryMenuItem.Tag = "Open Output Directory";
            showDirectoryMenuItem.Click += new EventHandler(ReportsMenuItem_Click);
            mnuReports.DropDownItems.Add(showDirectoryMenuItem);

            mnuReports.DropDownItems.Add(new ToolStripSeparator());

            if (Directory.Exists(project.OutputPath))
            {
                DirectoryInfo info = new DirectoryInfo(project.OutputPath);
                FileInfo[] files = info.GetFiles().OrderByDescending(p => p.CreationTime).ToArray();
                for (int i = 0; i < files.Length && i < 5; i++)
                {
                    FileInfo file = files[i];
                    string filename = Path.GetFileNameWithoutExtension(file.FullName);
                    ToolStripMenuItem menuItem = new ToolStripMenuItem(filename);
                    menuItem.Tag = file.FullName;
                    menuItem.Click += new EventHandler(ReportsMenuItem_Click);
                    mnuReports.DropDownItems.Add(menuItem);
                }
            }
        }

        private void ReportsMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            string file = (string)menuItem.Tag;

            if (file == "Open Output Directory")
            {
                Process.Start("explorer.exe", project.OutputPath);
            }
            else if (File.Exists(file))
            {
                Process.Start(file);
            }
        }

        private void mnuImport_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Multiselect = false;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    object o = null;

                    string xml = File.ReadAllText(dlg.FileName);
                    if (xml.Trim().EndsWith("</Module>"))
                    {
                        o = LoadSave.Load(typeof(Module), dlg.FileName);
                    }
                    else if (xml.Trim().EndsWith("</TestClass>"))
                    {
                        o = LoadSave.Load(typeof(TestClass), dlg.FileName);
                    }
                    else if (xml.Trim().EndsWith("</TestFunctoin>"))
                    {
                        o = LoadSave.Load(typeof(TestFunction), dlg.FileName);
                    }

                    if (o != null)
                    {
                        if (o is Module)
                        {
                            if ((selectedActionNode.Tag is Project || selectedActionNode.Tag is Module) == false)
                            {
                                MessageBox.Show("Module can be imported only on project or another module.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            if (selectedActionNode.Tag is Project)
                            {
                                ((Project)selectedActionNode.Tag).Modules.Add((Module)o);
                                ProjectActionTreeLoader.LoadModulesInTree(new List<Module>() { (Module)o }, tvActions, selectedActionNode);
                                MessageBox.Show("Import is completed. Imported content is persisted only when project is saved.", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else if (selectedActionNode.Tag is Module)
                            {
                                ((Module)selectedActionNode.Tag).Modules.Add((Module)o);
                                ProjectActionTreeLoader.LoadModulesInTree(new List<Module>() { (Module)o }, tvActions, selectedActionNode);
                                MessageBox.Show("Import is completed. Imported content is persisted only when project is saved.", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }

                        if (o is TestClass)
                        {
                            if ((selectedActionNode.Tag is Module) == false)
                            {
                                MessageBox.Show("Test Class can be imported only on a module.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            ((Module)selectedActionNode.Tag).TestClasses.Add((TestClass)o);
                            ProjectActionTreeLoader.LoadTestClassesInTree(new List<TestClass>() { (TestClass)o }, tvActions, selectedActionNode);
                            MessageBox.Show("Import is completed. Imported content is persisted only when project is saved.", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        if (o is TestFunction)
                        {
                            if ((selectedActionNode.Tag is TestClass) == false)
                            {
                                MessageBox.Show("Test Function can be imported only on a test class.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            ((TestClass)selectedActionNode.Tag).Functions.Add((TestFunction)o);
                            ProjectActionTreeLoader.LoadTestFunctionNode((TestFunction)o, tvActions, selectedActionNode);
                            MessageBox.Show("Import is completed. Imported content is persisted only when project is saved.", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("File import failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuExport_Click(object sender, EventArgs e)
        {
            if (selectedActionNode == null) return;

            if (selectedActionNode.Tag != null && selectedActionNode.Tag is ActionBase)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    LoadSave.Save(selectedActionNode.Tag.GetType(), selectedActionNode.Tag, dlg.FileName);
                    MessageBox.Show("Export is completed.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void mnuTermsAndConditions_Click(object sender, EventArgs e)
        {
            (new TermsForm()).ShowDialog();
        }

        private void mnuViewOutputWindow_Click(object sender, EventArgs e)
        {
            if (splitContainerMain.Panel2Collapsed)
            {
                splitContainerMain.Panel2Collapsed = false;
                txtOutput.Text = Runner.Instance.OutputText;
                mnuViewOutputWindow.Text = "Hide Output Window";
            }
            else
            {
                splitContainerMain.Panel2Collapsed = true;
                mnuViewOutputWindow.Text = "View Output Window";
            }
        }

        private void ShowOutputWindowByPreference()
        {
            if (splitContainerMain.Panel2Collapsed && Preferences.Instance.ShowOutputWindowOnExecution)
            {
                splitContainerMain.Panel2Collapsed = false;
                mnuViewOutputWindow.Text = "Hide Output Window";
            }
        }

        private void mnuDocumentation_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.aretetester.com/help/index.html");
        }

        private void mnuSamples_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.aretetester.com/help/aretetester-samples.html");
        }

        private void mnuCloseChromeDriverInstances_Click(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcesses();
            IEnumerable<Process> chromeDrivers = processes.Where(x => x.ProcessName == "chromedriver");
            foreach (Process process in chromeDrivers)
            {
                process.Kill();
            }
        }

        private void suggestedActionsControl_StartChromeDriverClicked(object sender, EventArgs e)
        {
            StartChromeDriver();
        }

        private void tvActionOptions_KeyDown(object sender, KeyEventArgs e)
        {
            if (tvActionOptions.SelectedNode == null || tvActionOptions.SelectedNode.Tag == null) return;

            if (e.KeyCode == System.Windows.Forms.Keys.F1)
            {
                string url = string.Empty;
                if (tvActionOptions.SelectedNode.Tag is ActionOption)
                {
                    url = ((ActionOption)tvActionOptions.SelectedNode.Tag).HelpUrl;
                } 

                if (String.IsNullOrEmpty(url) == false)
                {
                    Process.Start(AreteTester.Core.Globals.WebUrl + url);
                }
            }
        }

        private void mnuApplyResults_Click(object sender, EventArgs e)
        {
            ResultFilesSelectionForm resultFilesSelectionForm = new ResultFilesSelectionForm();
            resultFilesSelectionForm.ResultFilesPath = this.project.OutputPath;

            if (resultFilesSelectionForm.ShowDialog() == DialogResult.OK)
            {
                ExcelDocumentManager.ApplyingInDocument += new EventHandler<ExcelDocumentEventArgs>(ExcelDocumentManager_ApplyingInDocument);
                this.documentUpdateProgressForm = new DocumentUpdateProgressForm();
                this.documentUpdateProgressForm.Show(this);
                this.documentUpdateProgressForm.BringToFront();

                Thread thread = new Thread(new ParameterizedThreadStart(ApplyInDocumentThread));
                thread.IsBackground = true;
                thread.Start(resultFilesSelectionForm.SelectedFiles);
            }
        }

        private void mnuClearResults_Click(object sender, EventArgs e)
        {
            ExcelDocumentManager.ClearingInDocument += new EventHandler<ExcelDocumentEventArgs>(ExcelDocumentManager_ClearingInDocument);
            this.documentUpdateProgressForm = new DocumentUpdateProgressForm();
            this.documentUpdateProgressForm.Show(this);
            this.documentUpdateProgressForm.BringToFront();

            Thread thread = new Thread(new ThreadStart(ClearInDocumentThread));
            thread.IsBackground = true;
            thread.Start();
        }

        private void ApplyInDocumentThread(object o)
        {
            List<string> selctedFiles = (List<string>)o;

            ExcelDocumentManager.Apply(this.project, selctedFiles);
        }

        private void ClearInDocumentThread()
        {
            ExcelDocumentManager.Clear(this.project);
        }

        private void ExcelDocumentManager_ApplyingInDocument(object sender, ExcelDocumentEventArgs e)
        {
            if (this.documentUpdateProgressForm != null)
            {
                this.documentUpdateProgressForm.DocumentFile = e.Document;
                if (String.IsNullOrEmpty(e.Document))
                {
                    this.documentUpdateProgressForm.Close();
                }
            }
        }

        private void ExcelDocumentManager_ClearingInDocument(object sender, ExcelDocumentEventArgs e)
        {
            if (this.documentUpdateProgressForm != null)
            {
                this.documentUpdateProgressForm.DocumentFile = e.Document;
                if (String.IsNullOrEmpty(e.Document))
                {
                    this.documentUpdateProgressForm.Close();
                }
            }
        }

        private void Runner_StartVariablesThread(object sender, EventArgs e)
        {
            if (this.variablesThread == null || this.variablesThread.ThreadState != System.Threading.ThreadState.Running)
            {
                exitvariablesThread = false;

                Thread variablesThread = new Thread(new ThreadStart(VariablesThread));
                variablesThread.IsBackground = true;
                variablesThread.Start();
            }
        }

        private void Runner_EndVariablesThread(object sender, EventArgs e)
        {
            this.exitvariablesThread = true;
        }

        private void VariablesThread()
        {
            while (true)
            {
                UpdateVariablesInGrid();

                Thread.Sleep(1000);
            }
        }

        private void UpdateVariablesInGrid()
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(UpdateVariablesInGrid));
                }
                else
                {
                    List<string> variables = Variables.Instance.GetVariables();
                    variables.RemoveAll(x => x.StartsWith("@") == false);

                    Dictionary<string, int> existingVariables = new Dictionary<string, int>();

                    try
                    {
                        // remove non-existant variables from the grid
                        for (int i = dgrVariableValues.Rows.Count - 1; i >= 0; i--)
                        {
                            DataGridViewRow row = dgrVariableValues.Rows[i];
                            string name = (string)row.Cells[0].Value;
                            if (existingVariables.ContainsKey(name) == false)
                            {
                                existingVariables.Add(name, i);
                            }

                            if (variables.Contains(name) == false)
                            {
                                dgrVariableValues.Rows.Remove(row);
                            }
                        }

                        foreach (string variable in variables)
                        {
                            string value = Convert.ToString(Variables.Instance.GetValue(variable));
                            if (existingVariables.ContainsKey(variable))
                            {
                                dgrVariableValues[1, existingVariables[variable]].Value = value;
                            }
                            else
                            {
                                dgrVariableValues.Rows.Add(variable, value);
                            }
                        }

                        if (exitvariablesThread && this.variablesThread != null)
                        {
                            dgrVariableValues.Rows.Clear();
                            return;
                        }
                    }
                    catch
                    {
                        // TODO: 
                    }
                }
            }
            catch
            {
                // TODO: 
            }
        }
    }

    internal class ActionOption
    {
        public string Name { get; set; }

        public string TypeName { get; set; }

        public string Description { get; set; }

        public string HelpUrl { get; set; }
    }
}
