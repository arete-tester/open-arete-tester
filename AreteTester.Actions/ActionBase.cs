using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace AreteTester.Actions
{
    // Alerts
    [XmlInclude(typeof(AcceptAlert))]
    [XmlInclude(typeof(CancelAlert))]
    [XmlInclude(typeof(GetAlertText))]
    [XmlInclude(typeof(IsAlertDisplayed))]

    // Hyperlink
    [XmlInclude(typeof(ClickHyperlink))]
    [XmlInclude(typeof(GetHyperlinkCaption))]
    [XmlInclude(typeof(GetHyperlinkUrl))]

    // Button
    [XmlInclude(typeof(ClickButton))]
    [XmlInclude(typeof(GetButtonCaption))]

    // Check box
    [XmlInclude(typeof(GetCheckBoxCaption))]
    [XmlInclude(typeof(IsCheckBoxSet))]
    [XmlInclude(typeof(SetCheckBox))]
    [XmlInclude(typeof(UnsetCheckBox))]

    // Click Div
    [XmlInclude(typeof(ClickDiv))]
    [XmlInclude(typeof(SetDivInnerHtml))]

    // Radio button
    [XmlInclude(typeof(IsRadioButtonSet))]
    [XmlInclude(typeof(SetRadioButton))]

    // Random
    [XmlInclude(typeof(GetGUID))]
    [XmlInclude(typeof(GetRandomNumber))]  

    // Dropdown
    [XmlInclude(typeof(GetDropdownItemsCount))]
    [XmlInclude(typeof(IsDropdownItemExists))]
    [XmlInclude(typeof(IsDropdownItemSelected))]
    [XmlInclude(typeof(SelectDropdownItemByIndex))]
    [XmlInclude(typeof(SelectDropdownItemByText))]
    [XmlInclude(typeof(SelectDropdownItemByValue))]

    // Listbox
    [XmlInclude(typeof(DeselectAllListboxItems))]
    [XmlInclude(typeof(DeselectListboxItemByIndex))]
    [XmlInclude(typeof(DeselectListboxItemByText))]
    [XmlInclude(typeof(DeselectListboxItemByValue))]
    [XmlInclude(typeof(GetListboxItemsCount))]
    [XmlInclude(typeof(IsListboxItemExists))]
    [XmlInclude(typeof(IsListboxItemSelected))]
    [XmlInclude(typeof(IsListboxMultipleSelectionAllowed))]
    [XmlInclude(typeof(SelectAllListboxItems))]
    [XmlInclude(typeof(SelectListboxItemByIndex))]
    [XmlInclude(typeof(SelectListboxItemByText))]
    [XmlInclude(typeof(SelectListboxItemByValue))]

    // Email Control
    [XmlInclude(typeof(ClearEmail))]
    [XmlInclude(typeof(GetEmailValue))]
    [XmlInclude(typeof(SetEmailValue))]

    // File
    [XmlInclude(typeof(FileAppendText))]
    [XmlInclude(typeof(FileReadAllText))]
    [XmlInclude(typeof(TextExistsInFile))]

    // Headers
    [XmlInclude(typeof(GetH1Text))]
    [XmlInclude(typeof(GetH2Text))]
    [XmlInclude(typeof(GetH3Text))]
    [XmlInclude(typeof(GetH4Text))]
    [XmlInclude(typeof(GetH5Text))]
    [XmlInclude(typeof(GetH6Text))]

    // Password
    [XmlInclude(typeof(ClearPassword))]
    [XmlInclude(typeof(GetPasswordValue))]
    [XmlInclude(typeof(SetPasswordValue))]

    // Table
    [XmlInclude(typeof(GetCellValue))]
    [XmlInclude(typeof(GetRowsCount))]
    [XmlInclude(typeof(GetColumnsCount))]

    // Text Area
    [XmlInclude(typeof(ClearTextArea))]
    [XmlInclude(typeof(GetTextAreaValue))]
    [XmlInclude(typeof(SetTextAreaValue))]

    // Textbox
    [XmlInclude(typeof(ClearTextBox))]
    [XmlInclude(typeof(GetTextBoxValue))]
    [XmlInclude(typeof(SetTextBoxValue))]

    // Span
    [XmlInclude(typeof(GetSpanText))]    

    // String
    [XmlInclude(typeof(GetTextLength))]
    [XmlInclude(typeof(InsertInText))]
    [XmlInclude(typeof(ReplaceInText))]
    [XmlInclude(typeof(TextContains))]
    [XmlInclude(typeof(TextEndsWith))]
    [XmlInclude(typeof(TextIndexOf))]
    [XmlInclude(typeof(TextPadLeft))]
    [XmlInclude(typeof(TextPadRight))]
    [XmlInclude(typeof(TextStartsWith))]
    [XmlInclude(typeof(TextSubstring))]
    [XmlInclude(typeof(TextTrimEnd))]
    [XmlInclude(typeof(TextTrimStart))]
    [XmlInclude(typeof(TrimText))]
    [XmlInclude(typeof(TextToUpper))]
    [XmlInclude(typeof(TextToLower))]
    [XmlInclude(typeof(TextToLines))]
    [XmlInclude(typeof(GetRegexMatchesInText))]
    [XmlInclude(typeof(GetRegexMatchInText))]
    
    // Paragraph
    [XmlInclude(typeof(GetParagraphText))]

    // Variables
    [XmlInclude(typeof(RemoveVariable))]
    [XmlInclude(typeof(SetVariable))]
    [XmlInclude(typeof(ConvertVariable))]
    [XmlInclude(typeof(RunVariablesExpression))]
    [XmlInclude(typeof(RunComparisonExpression))]

    // Navigate
    [XmlInclude(typeof(NavigateUrl))]
    [XmlInclude(typeof(GetCurrentUrl))]

    // Common
    [XmlInclude(typeof(GetAttributeValue))]
    [XmlInclude(typeof(GetElementInnerText))]
    [XmlInclude(typeof(PressEnterKey))]
    [XmlInclude(typeof(PressKeys))]
    [XmlInclude(typeof(QuitWebDriver))]
    [XmlInclude(typeof(Refresh))]
    [XmlInclude(typeof(Wait))]
    [XmlInclude(typeof(WaitForElementDisplayed))]
    [XmlInclude(typeof(Log))]
    [XmlInclude(typeof(CaptureScreenshot))]
    [XmlInclude(typeof(IsDisplayed))]
    [XmlInclude(typeof(MouseClickElement))]
    [XmlInclude(typeof(DoubleClickElement))]
    [XmlInclude(typeof(TriggerElementEvent))]

    // Email
    [XmlInclude(typeof(SetSmtpDetails))]
    [XmlInclude(typeof(SendEmail))]

    // Repeater
    [XmlInclude(typeof(Repeater))]
    [XmlInclude(typeof(Continue))]
    [XmlInclude(typeof(Break))]

    // iFrame
    [XmlInclude(typeof(SwichToFrame))]
    [XmlInclude(typeof(SwichToDefaultContent))]

    [XmlInclude(typeof(ExecuteFunction))]
    [XmlInclude(typeof(ExecuteExternalMethod))]

    // Cookies
    [XmlInclude(typeof(ClearAllCookies))]

    // Page
    [XmlInclude(typeof(GetPageSource))]
    [XmlInclude(typeof(GetPageText))]
    [XmlInclude(typeof(ScrollToElement))]
    [XmlInclude(typeof(ScrollDownByPixcels))]
    [XmlInclude(typeof(ScrollToBottom))]
    [XmlInclude(typeof(ScrollToTop))]

    // Window
    [XmlInclude(typeof(GetWindowPosition))]
    [XmlInclude(typeof(GetWindowSize))]
    [XmlInclude(typeof(GetWindowTitle))]
    [XmlInclude(typeof(MaximizeWindow))]
    [XmlInclude(typeof(MinimizeWindow))]
    [XmlInclude(typeof(ResizeWindow))]
    [XmlInclude(typeof(CloseWindow))]
    [XmlInclude(typeof(OpenNewTab))]
    [XmlInclude(typeof(CloseTab))]
    [XmlInclude(typeof(SwitchToTab))]
    [XmlInclude(typeof(GoBack))]
    [XmlInclude(typeof(GoForward))]

    // XPath
    [XmlInclude(typeof(DiscoverXPath))]

    // Data
    [XmlInclude(typeof(SetDatabaseConfig))]
    [XmlInclude(typeof(SqlExecuteQuery))]
    [XmlInclude(typeof(SelectFieldMapping))]
    [XmlInclude(typeof(SqlExecuteNonQuery))]

    // Validations
    [XmlInclude(typeof(AssertEqual))]
    [XmlInclude(typeof(AssertIsFalse))]
    [XmlInclude(typeof(AssertIsTrue))]
    [XmlInclude(typeof(AssertRegex))]
    [XmlInclude(typeof(AssertAttribute))]
    [XmlInclude(typeof(AssertInnerText))]
    [XmlInclude(typeof(AssertStyle))]

    [Serializable]
    public class ActionBase
    {
        public static event EventHandler ProcessStarted;
        public static event EventHandler ProcessPaused;
        public static event EventHandler<NotificationEventArgs> NotificationReceived;
        public static event EventHandler<ActionExceptionEventArgs> ExceptionReceived;
        public static event EventHandler<ExecuteFunctionEventArgs> ExecuteFunctionCalled;

        [Browsable(false)]
        [XmlAttribute]
        [Category("Properties")]
        public string ActionType { get; set; }

        [XmlAttribute]
        [Category("Display")]
        public string Description { get; set; }

        [XmlAttribute]
        [Category("Condition")]
        public string If { get; set; }

        [XmlAttribute]
        [Category("Condition")]
        public bool Enabled { get; set; }

        [XmlAttribute]
        [Browsable(false)]
        [Category("Properties")]
        public bool IsBreakpointSet { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        [Category("Properties")]
        public virtual string NodeText
        {
            get
            {
                string nodeText = ActionType;

                if (String.IsNullOrEmpty(Description) == false)
                {
                    nodeText += " : " + Description;
                }

                return nodeText;
            }
        }

        [XmlAttribute]
        [Category("Display")]
        public FontStyle FontStyle { get; set; }

        public ActionBase()
        {
            this.Enabled = true;
            this.FontStyle = FontStyle.Regular;
        }

        public virtual void Process()
        {
            if (ProcessStarted != null)
            {
                ProcessStarted(this, null);
            }
        }

        public virtual ValidationResult Validate()
        {
            ValidationResult result = new ValidationResult();
            if (Preferences.Instance.WarnEmptyDescription && String.IsNullOrEmpty(Description))
            {
                result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Warning, Message = "Description is empty." });
            }

            return result;
        }

        protected void HandleException(ActionBase action, Exception exc)
        {
            string projectPath = (string)Variables.Instance.GetValue("$$$ProjectPath");
            string exceptionLogFile = projectPath + @"\exception.log";
            File.AppendAllText(exceptionLogFile, (exc.Message + Environment.NewLine + exc.StackTrace + Environment.NewLine));

            NotifyException(action, exc);
        }

        protected void NotifyOutput(Output output)
        {
            if (NotificationReceived != null)
            {
                NotificationReceived(this, new NotificationEventArgs() { Output = output });
            }
        }

        protected void NotifyException(ActionBase action, Exception exc)
        {
            if (ExceptionReceived != null)
            {
                ExceptionReceived(this, new ActionExceptionEventArgs() { Action = action, Exception = exc });
            }
        }

        protected void NotifyProcessPaused(ActionBase action)
        {
            if (ProcessPaused != null)
            {
                ProcessPaused(action, null);
            }
        }

        protected void NotifyExecuteMethodCalled(string functionFullName)
        {
            if (ExecuteFunctionCalled != null)
            {
                ExecuteFunctionCalled(this, new ExecuteFunctionEventArgs() { FunctionFullName = functionFullName });
            }
        }
    }

    public enum FontStyle
    {
        Regular,
        Bold,
        Italic
    }

    public class NotificationEventArgs : EventArgs
    {
        public Output Output { get; set; }
    }

    public class ActionExceptionEventArgs : EventArgs
    {
        public ActionBase Action { get; set; }

        public Exception Exception { get; set; }
    }

    public class ExecuteFunctionEventArgs : EventArgs
    {
        public string FunctionFullName { get; set; }
    }
}
