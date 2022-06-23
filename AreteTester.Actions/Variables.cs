using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Linq;

namespace AreteTester.Actions
{
    public class Variables
    {
        private static Variables instance;
        private Dictionary<string, object> variables = new Dictionary<string, object>();

        private Variables() { }

        public static Variables Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Variables();
                }
                return instance;
            }
        }

        internal SmtpSettings SmtpSettings { get; set; }

        internal DatabaseAction Database { get; set; }

        public void SetValue(string variableName, object value)
        {
            if (variables.ContainsKey(variableName) == false)
            {
                variables.Add(variableName, null);
            }

            variables[variableName] = value;
        }

        public object GetValue(string variableName)
        {
            if (String.IsNullOrEmpty(variableName) == false && variables.ContainsKey(variableName))
            {
                return variables[variableName];
            }
            return null;
        }

        public void Remove(string variableName)
        {
            if (variables.ContainsKey(variableName) == false)
            {
                variables.Remove(variableName);
            }
        }

        public string Apply(string text)
        {
            Regex regex = new Regex(@"@[\w]+", RegexOptions.Compiled);
            foreach (Match match in regex.Matches(text))
            {
                object o = Variables.Instance.GetValue(match.Value);
                if (o == null) continue; // which means variable doesnt exists

                string value = Convert.ToString(o);

                if (o is Boolean)
                {
                    value = value.ToLower();
                }

                text = text.Replace(match.Value, value);
                text = text.Replace("`", "");
            }

            return text;
        }

        public void LoadVariables()
        {
            string projectPath = (string)Variables.Instance.GetValue("$$$ProjectPath");

            string variablesFilePath = projectPath + @"\Variables.xml";

            if (File.Exists(variablesFilePath) == false) return;

            foreach (XElement variableElement in XElement.Load(variablesFilePath).Elements("Variable"))
            {
                string name = variableElement.Attribute("name").Value;
                string value = variableElement.Attribute("value").Value;

                Variables.Instance.SetValue(name, value);
            }
        }

        public void SaveVariables()
        {
            string projectPath = (string)Variables.Instance.GetValue("$$$ProjectPath");
            string variablesFile = projectPath + @"\Variables.xml";

            if (File.Exists(variablesFile) == false)
            {
                XElement variablesElement = new XElement("Variables",
                                                new XElement("Variable", new XAttribute("name", "Sample_name1"), new XAttribute("value", "sample_value1")));
                variablesElement.Save(variablesFile);
            }

            XDocument doc = XDocument.Load(variablesFile);
            foreach (string variable in this.variables.Keys)
            {
                string value = Convert.ToString(Variables.Instance.GetValue(variable));

                doc.Element("Variables").Add(new XElement("Variable", new XAttribute("name", variable), new XAttribute("value", value)));

                doc.Save(variablesFile);
            }
        }

        public List<string> GetVariables()
        {
            List<string> vars = new List<string>();
            foreach (string variable in this.variables.Keys)
            {
                vars.Add(variable);
            }

            return vars;
        }
    }
}
