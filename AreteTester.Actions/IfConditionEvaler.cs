using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AreteTester.Actions
{
    public class IfConditionEvaler
    {
        public static bool Eval(string code)
        {
            if (String.IsNullOrEmpty(code)) return true;

            CompilerResults results = Compile(code);

            if (results.Errors.Count == 0)
            {
                System.Reflection.Assembly a = results.CompiledAssembly;
                object o = a.CreateInstance("CSCodeEvaler.CSCodeEvaler");

                Type t = o.GetType();
                MethodInfo mi = t.GetMethod("EvalCode");

                return (bool)mi.Invoke(o, null);
            }

            return false;
        }

        public static bool HasError(string code)
        {
            CompilerResults results = Compile(code);

            return results.Errors.Count > 0;
        }

        private static CompilerResults Compile(string code)
        {
            code = Variables.Instance.Apply(code);

            CSharpCodeProvider c = new CSharpCodeProvider();
            ICodeCompiler icc = c.CreateCompiler();
            CompilerParameters cp = new CompilerParameters();

            cp.ReferencedAssemblies.Add("system.dll");
            cp.ReferencedAssemblies.Add("system.xml.dll");
            cp.ReferencedAssemblies.Add("system.data.dll");
            cp.ReferencedAssemblies.Add("system.windows.forms.dll");
            cp.ReferencedAssemblies.Add("system.drawing.dll");

            cp.CompilerOptions = "/t:library";
            cp.GenerateInMemory = true;

            StringBuilder sb = new StringBuilder("");
            sb.Append("using System;\n");
            sb.Append("using System.Xml;\n");
            sb.Append("using System.Data;\n");
            sb.Append("using System.Data.SqlClient;\n");
            sb.Append("using System.Windows.Forms;\n");
            sb.Append("using System.Drawing;\n");

            sb.Append("namespace CSCodeEvaler{ \n");
            sb.Append("public class CSCodeEvaler{ \n");
            sb.Append("public object EvalCode(){\n");
            sb.Append("return " + code + "; \n");
            sb.Append("} \n");
            sb.Append("} \n");
            sb.Append("}\n");

            return icc.CompileAssemblyFromSource(cp, sb.ToString());
        }
    }
}
