using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using OpenQA.Selenium;
using System.Reflection;
using System.Collections;
using System.Text.RegularExpressions;

namespace AreteTester.Actions
{
    [Serializable]
    public class Repeater : ActionBase
    {
        [XmlAttribute]
        [Category("HTML Properties")]
        public string XPath { get; set; }

        [XmlAttribute]
        [Category("HTML Properties")]
        public string RepeatElementName { get; set; }

        [XmlAttribute]
        [Category("HTML Properties")]
        public bool RepeatElementIndexed { get; set; }

        [XmlAttribute]
        [Category("HTML Properties")]
        public bool IgnoreUnmatched { get; set; }

        [XmlAttribute]
        [Category("HTML Page")]
        public string PageIndexVariable { get; set; }

        [XmlAttribute]
        [Category("HTML Page")]
        public string PageCount { get; set; }

        [XmlAttribute]
        [Category("HTML Loop")]
        public string LoopIndexVariable { get; set; }

        [XmlAttribute]
        [Category("List")]
        public string ListVariable { get; set; }

        [XmlAttribute]
        [Category("List")]
        public string ListItemVariable { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public RepeaterType RepeaterType { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public bool RepeatBackwards { get; set; }

        [Browsable(false)]
        public List<ActionBase> Actions { get; set; }

        [Browsable(false)]
        public bool IsExpanded { get; set; }

        public Repeater()
        {
            this.ActionType = "Repeater";
            this.Description = "Repeat set of actions for similar group of items.";
            this.IgnoreUnmatched = false;
            this.LoopIndexVariable = "@var";
            this.PageIndexVariable = "@var";
            this.PageCount = "1";
            this.RepeaterType = RepeaterType.HTML;
            this.RepeatElementIndexed = true;
            this.RepeatBackwards = false;

            this.Actions = new List<ActionBase>();
        }

        public override void Process()
        {
            base.Process();

            if (this.Actions.Count == 0) return;

            switch (this.RepeaterType)
            {
                case RepeaterType.HTML: LoopHtml(); break;
                case RepeaterType.List: LoopList(); break;
            }
        }

        private void LoopList()
        {
            var list = Variables.Instance.GetValue(this.ListVariable);

            if (list == null) return;

            if (list is List<string>)
            {
                LoopListTemplate<List<string>>((List<string>)list);
            }
            else if (list is List<int>)
            {
                LoopListTemplate<List<int>>((List<int>)list);
            }
        }

        private void LoopListTemplate<T>(T list) where T : IList
        {
            foreach (var item in list)
            {
                if (AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Stopped) break;

                Variables.Instance.SetValue(this.ListItemVariable, item);

                foreach (ActionBase action in this.Actions)
                {
                    if (AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Stopped) break;

                    if (action.Enabled && IfConditionEvaler.Eval(action.If))
                    {
                        if (action is Repeater)
                        {
                            ((Repeater)action).Process();
                            continue;
                        }

                        try
                        {
                            action.Process();
                        }
                        catch
                        {
                            throw;
                        }
                    }
                }
            }
        }

        private void LoopHtml()
        {
            int pageCount = Convert.ToInt32(Variables.Instance.Apply(this.PageCount));

            for (int pageIndex = 0; pageIndex < pageCount; pageIndex++)
            {
                Variables.Instance.SetValue(this.PageIndexVariable, pageIndex);

                string repeaterXPath = Variables.Instance.Apply(this.XPath);
                IWebElement element = Globals.Driver.FindElement(By.XPath(repeaterXPath));
                IList<IWebElement> childElements = element.FindElements(By.XPath("./*"));
                List<string> xpaths = new List<string>();

                int loopIndex = Convert.ToInt32(Variables.Instance.GetValue(this.LoopIndexVariable));
                if (this.RepeatBackwards) loopIndex = childElements.Count - 1;

                for (; (this.RepeatBackwards ? (loopIndex >= 0) : (loopIndex < childElements.Count));)
                {
                    IWebElement childElement = childElements[loopIndex];

                    if (childElement.TagName == this.RepeatElementName)
                    {
                        xpaths.Add(repeaterXPath + @"/" + this.RepeatElementName + (this.RepeatElementIndexed ? ("[" + (loopIndex + 1) + "]") : ""));
                    }

                    loopIndex = this.RepeatBackwards ? loopIndex - 1 : loopIndex + 1;
                }

                loopIndex = 0;
                if (this.RepeatBackwards) loopIndex = childElements.Count - 1;

                foreach (string childXpath in xpaths)
                {
                    Variables.Instance.SetValue(this.LoopIndexVariable, loopIndex);

                    if (AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Stopped) break;

                    foreach (ActionBase action in this.Actions)
                    {
                        if (AreteTester.Actions.Globals.RunnerStatus == RunnerStatusType.Stopped) break;

                        if (action.Enabled && IfConditionEvaler.Eval(action.If))
                        {
                            if (action is Repeater)
                            {
                                ((Repeater)action).Process();
                                continue;
                            }

                            string xpath = GetXPath(action);
                            string appliedXpath = Variables.Instance.Apply(xpath);
                            bool xpathFixed = GetIsXPathFixed(action);
                            if (xpathFixed == false && String.IsNullOrEmpty(xpath) == false)
                            {
                                string fullXpath = childXpath + appliedXpath;

                                SetXPath(action, fullXpath);
                            }

                            try
                            {
                                action.Process();

                                SetXPath(action, xpath);
                            }
                            catch (Exception exc)
                            {
                                if (exc.Message.ToLower().Contains("unable to locate element"))
                                {
                                    if (this.IgnoreUnmatched == false) throw;
                                }
                                else
                                {
                                    throw;
                                }
                            }
                            finally
                            {
                                if (String.IsNullOrEmpty(xpath) == false)
                                {
                                    SetXPath(action, xpath);
                                }
                            }
                        }
                    }

                    loopIndex = this.RepeatBackwards ? loopIndex - 1 : loopIndex + 1;
                }
            }
        }

        private string GetXPath(object src)
        {
            object value = null;

            PropertyInfo property = src.GetType().GetProperty("XPath");
            if (property != null)
            {
                value = property.GetValue(src, null);
            }

            return (value == null ? "" : (string)value);
        }

        private bool GetIsXPathFixed(object src)
        {
            object value = null;

            PropertyInfo property = src.GetType().GetProperty("XPathFixed");
            if (property != null)
            {
                value = property.GetValue(src, null);
            }

            return (value == null ? false : (bool)value);
        }

        private void SetXPath(object src, string xpath)
        {
            PropertyInfo propertyInfo = src.GetType().GetProperty("XPath");
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(src, Convert.ChangeType(xpath, propertyInfo.PropertyType), null);
            }
        }

        public override ValidationResult Validate()
        {
            ValidationResult result = base.Validate();

            if (this.RepeaterType == RepeaterType.HTML)
            {
                if (String.IsNullOrEmpty(this.XPath))
                {
                    result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "XPath is empty." });
                }

                if (String.IsNullOrEmpty(this.PageCount))
                {
                    result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Page count is empty." });
                }

                if (String.IsNullOrEmpty(this.RepeatElementName))
                {
                    result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "RepeatElementName property is empty." });
                }
            }
            else if (this.RepeaterType == RepeaterType.List)
            {
                if (String.IsNullOrEmpty(this.ListVariable))
                {
                    result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Variable is empty." });
                }

                if (String.IsNullOrEmpty(this.ListVariable) == false)
                {
                    if (this.ListVariable.Length > 30)
                    {
                        result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Variable name length cannot be more than 30 characters." });
                    }

                    Regex variableRegex = new Regex("^@[a-zA-Z0-9_]+$", RegexOptions.Compiled);
                    if (variableRegex.IsMatch(this.ListVariable) == false)
                    {
                        result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Variable name: " + this.ListVariable + " is not the valid format. Use only alphabets, numbers, underscore (_) and start with @." });
                    }

                    if (this.ListVariable.Contains("@") && this.ListVariable.IndexOf('@') > 0)
                    {
                        result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Variable name: " + this.ListVariable + " contains character @ in it. Variable name can only start with @." });
                    }
                }

                if (String.IsNullOrEmpty(this.ListItemVariable))
                {
                    result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Variable is empty." });
                }

                if (String.IsNullOrEmpty(this.ListItemVariable) == false)
                {
                    if (this.ListItemVariable.Length > 30)
                    {
                        result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Variable name length cannot be more than 30 characters." });
                    }

                    Regex variableRegex = new Regex("^@[a-zA-Z0-9_]+$", RegexOptions.Compiled);
                    if (variableRegex.IsMatch(this.ListItemVariable) == false)
                    {
                        result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Variable name: " + this.ListItemVariable + " is not the valid format. Use only alphabets, numbers, underscore (_) and start with @." });
                    }

                    if (this.ListItemVariable.Contains("@") && this.ListItemVariable.IndexOf('@') > 0)
                    {
                        result.Messages.Add(new ValidationMessage() { MessageType = ValidationMessageType.Error, Message = "Variable name: " + this.ListItemVariable + " contains character @ in it. Variable name can only start with @." });
                    }
                }
            }

            return result;
        }
    }
}
