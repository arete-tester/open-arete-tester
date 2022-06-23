using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace AreteTester.Actions
{
    [Serializable]
    public class ExecuteExternalMethod : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public string DLL { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public string Class { get; set; }

        [XmlAttribute]
        [Category("Properties")]
        public string Method { get; set; }

        public ExecuteExternalMethod()
        {
            this.ActionType = "ExecuteExternalMethod";
        }

        public override void Process()
        {
            base.Process();

            string binPath = (string)Variables.Instance.GetValue("$$$BinPath");
            string binfile = binPath.Trim(@"\".ToCharArray()) + @"\" + this.DLL;

            var asm = Assembly.LoadFile(binfile);
            var type = asm.GetType(this.Class);
            var instance = Activator.CreateInstance(type);

            // set properties
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            // get web driver property
            PropertyInfo webDriverProperty = properties.Where(x => x.Name == "WebDriver").FirstOrDefault();
            if (webDriverProperty != null)
            {
                webDriverProperty.SetValue(instance, Globals.Driver, null);
            }

            // get variables property
            Dictionary<string, object> variables = null;
            PropertyInfo variablesProperty = properties.Where(x => x.Name == "Variables").FirstOrDefault();
            if (variablesProperty != null)
            {
                object propertyValue = variablesProperty.GetValue(instance, null);
                if (propertyValue != null)
                {
                    variables = (Dictionary<string, object>)propertyValue;
                    foreach (string key in variables.Keys.ToArray())
                    {
                        variables[key] = Variables.Instance.GetValue(key);
                    }
                }
            }

            // execute method
            type.InvokeMember(this.Method, BindingFlags.Default | BindingFlags.InvokeMethod, null, instance, null);
            
            // update variables
            if (variables != null)
            {
                foreach (string key in variables.Keys.ToArray())
                {
                    Variables.Instance.SetValue(key, variables[key]);
                }
            }
        }
    }
}
