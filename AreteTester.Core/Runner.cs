using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;
using System.IO;
using AreteTester;
using AreteTester.Actions;

namespace AreteTester.Core
{
    public class Runner
    {
        public event EventHandler<RunThreadStartedEventArgs> RunThreadStarted;
        public event EventHandler RunThreadCompleted;

        private string htmlReportPath = string.Empty;
        private string xmlResultPath = string.Empty;
        private int sucessCount = 0, failCount = 0;

        private Report report;        
        
        private static List<string> outputs = new List<string>();
        private Dictionary<string, TestFunction> nameTestFunctionMapping = new Dictionary<string, TestFunction>();

        private static Runner instance;

        private Runner()
        {
            AreteTester.Actions.Globals.RunnerStatus = RunnerStatusType.Stopped;

            ActionBase.NotificationReceived += new EventHandler<NotificationEventArgs>(ActionBase_NotificationReceived);
            ActionBase.ExceptionReceived += new EventHandler<ActionExceptionEventArgs>(ActionBase_ExceptionReceived);
            ActionBase.ExecuteFunctionCalled += new EventHandler<ExecuteFunctionEventArgs>(ActionBase_ExecuteFunctionCalled);
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
            if (start) AreteTester.Actions.Globals.PausedAction = null;

            AreteTester.Actions.Globals.RunnerDateTime = DateTime.Now;
            sucessCount = failCount = 0;
            outputs.Clear();

            report = new Report();
            report.ProjectName = project.Name;

            LoadNameToFunctionMapping(project);

            Thread runThread = new Thread(new ParameterizedThreadStart(RunThread));
            runThread.IsBackground = true;
            runThread.Start(new List<object>() { project, selectedAction });

            if (wait)
            {
                runThread.Join();
            }
        }

        private void RunThread(object objects)
        {
            try
            {
                Project project = (Project)((List<object>)objects)[0];
                object classOrFunctionOrAction = ((List<object>)objects)[1];

                // TODO: add validations

                if (Directory.Exists(project.OutputPath) == false)
                {
                    Directory.CreateDirectory(project.OutputPath);
                }

                this.htmlReportPath = project.OutputPath + @"\" + project.Name + " " + AreteTester.Actions.Globals.RunnerDateTime.ToString("yyyy-MMM-dd HH-mm-ss", CultureInfo.InvariantCulture) + ".html";
                this.xmlResultPath = project.OutputPath + @"\" + project.Name + "_result_" + AreteTester.Actions.Globals.RunnerDateTime.ToString("yyyy_MMM_dd_HH_mm_ss", CultureInfo.InvariantCulture) + ".xml";

                try
                {

                    if (AreteTester.Actions.Globals.RunnerStatus != RunnerStatusType.Paused)
                    {
                        report.ExecutionStart = DateTime.Now;
                    }

                    AreteTester.Actions.Globals.RunnerStatus = RunnerStatusType.Running;
                    if (RunThreadStarted != null) RunThreadStarted(this, new RunThreadStartedEventArgs() { Project = project, ClassOrFunctionOrAction = classOrFunctionOrAction });

                    if (classOrFunctionOrAction == null)
                    {
                        project.Process();
                    }
                    else
                    {
                        ((ActionBase)classOrFunctionOrAction).Process();
                    }

                    if (AreteTester.Actions.Globals.RunnerStatus != RunnerStatusType.Paused)
                    {
                        AreteTester.Actions.Globals.RunnerStatus = RunnerStatusType.Stopped;
                        report.ExecutionEnd = DateTime.Now;
                    }

                    if (RunThreadCompleted != null) RunThreadCompleted(this, null);
                }
                catch (Exception exc)
                {
                    string projectPath = (string)Variables.Instance.GetValue("$$$ProjectPath");
                    string exceptionLog = projectPath + @"\exception.log";
                    File.WriteAllText(exceptionLog, (exc.Message + Environment.NewLine + exc.StackTrace + Environment.NewLine));
                }

                report.SuccessCount = sucessCount;
                report.FailureCount = failCount;
                report.WriteHtmlReport(htmlReportPath);
                report.WriteXmlResult(xmlResultPath);
            }
            catch
            {
                // TODO:
            }
        }

        private void LoadNameToFunctionMapping(Project project)
        {
            nameTestFunctionMapping.Clear();
            foreach (Module module in project.Modules)
            {
                LoadNameToFunctionMapping(module, new List<string>(){ module.Name });
            }
        }

        private void LoadNameToFunctionMapping(Module module, List<string> names)
        {
            foreach (Module childModule in module.Modules)
            {
                names.Add(childModule.Name);
                LoadNameToFunctionMapping(childModule, names);
            }

            foreach (TestClass cls in module.TestClasses)
            {
                names.Add(cls.Name);
                LoadNameToFunctionMapping(cls, names);
            }
        }

        private void LoadNameToFunctionMapping(TestClass cls, List<string> names)
        {
            foreach (TestFunction function in cls.Functions)
            {
                string fullName = String.Join(".", names.ToArray()) + "." + function.Name;
                nameTestFunctionMapping.Add(fullName, function);
            }
        }

        private void ActionBase_ExecuteFunctionCalled(object sender, ExecuteFunctionEventArgs e)
        {
            if (nameTestFunctionMapping.ContainsKey(e.FunctionFullName))
            {
                TestFunction function = nameTestFunctionMapping[e.FunctionFullName];
                if (function.Enabled && IfConditionEvaler.Eval(function.If))
                {
                    function.Process();
                }
            }
        }

        private void ActionBase_NotificationReceived(object sender, NotificationEventArgs e)
        {
            if(outputs.Contains(e.Output.Id)) return;

            outputs.Add(e.Output.Id);

            if (e.Output.DoNotLog == false)
            {
                string formattedMessage = FormatLogMessage(e.Output);
                report.Logs.Add(new LogReportItem() { Description = formattedMessage });
            }

            if (e.Output.IsAssertion && e.Output.Success) sucessCount++;
            if (e.Output.IsAssertion && e.Output.Success == false) failCount++;

            if (e.Output.IsAssertion)
            {
                AssertionReportItem assertionItem = new AssertionReportItem();
                if (String.IsNullOrEmpty(e.Output.Description) == false)
                {
                    assertionItem.Description = e.Output.Description;
                }
                else
                {
                    if(Preferences.Instance.IgnoreEmptyDescription == false)
                    {
                        assertionItem.Description = "[empty description] ";
                    }
                }
                assertionItem.Name = e.Output.Name;
                assertionItem.AssertionType = e.Output.ActionType;
                assertionItem.Expected = e.Output.Expected;
                assertionItem.Actual = e.Output.Actual;
                assertionItem.Success = e.Output.Success ? "1" : "0";

                report.Assertions.Add(assertionItem);
            }
        }

        private void ActionBase_ExceptionReceived(object sender, ActionExceptionEventArgs e)
        {
            string formattedMessage = FormatExceptionMessage(e.Action, "ERROR : " + e.Exception.Message);
            report.Logs.Add(new LogReportItem() { Description = formattedMessage });
        }

        public string FormatLogMessage(Output output)
        {
            string formattedMessage = string.Empty;

            formattedMessage += output.ActionType;
            if (String.IsNullOrEmpty(output.Description) == false)
            {
                formattedMessage += " : " + output.Description + ".";
            }
            else
            {
                if (Preferences.Instance.IgnoreEmptyDescription == false)
                {
                    formattedMessage += " : " + "[empty description] ";
                }
            }

            if (String.IsNullOrEmpty(output.Expected) == false && String.IsNullOrEmpty(output.Actual) == false)
            {
                formattedMessage += "Expected: " + output.Expected + ", Actual: " + output.Actual + ", Success: " + (output.Success ? "YES" : "NO") + ".";
            }
            formattedMessage += String.IsNullOrEmpty(output.Message) ? String.Empty : "<br/>" + output.Message;

            return formattedMessage;
        }

        public string FormatExceptionMessage(ActionBase action, string message)
        {
            string formattedMessage = string.Empty;

            formattedMessage += action.ActionType;
            if (String.IsNullOrEmpty(action.Description) == false)
            {
                formattedMessage += " : " + action.Description + ".";
            }
            else
            {
                if (Preferences.Instance.IgnoreEmptyDescription == false)
                {
                    formattedMessage += " : " + "[empty description] ";
                }
            }

            formattedMessage += String.IsNullOrEmpty(message) ? String.Empty : "<br/>" + message;

            return formattedMessage;
        }
    }
}
