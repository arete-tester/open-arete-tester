using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace AreteTester.Actions
{
    public class VariablePropertyConverter : StringConverter
    {
        public override Boolean GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }

        public override Boolean GetStandardValuesExclusive(ITypeDescriptorContext context) { return false; }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<String> variables = Variables.Instance.GetVariables();
            for (int i = variables.Count - 1; i >= 0; i--)
            {
                if (variables[i].StartsWith("@") == false)
                {
                    variables.RemoveAt(i);
                }
            }

            // TODO: Low performance
            FillVariables(Globals.CurrentProject, variables);

            return new System.ComponentModel.TypeConverter.StandardValuesCollection(variables);
        }

        private void FillVariables(ActionBase action, List<string> variables)
        {
            if (action is Project)
            {
                foreach (var module in ((Project)action).Modules)
                {
                    FillVariables(module, variables);
                }
            }
            else if (action is Module)
            {
                foreach (Module module in ((Module)action).Modules)
                {
                    FillVariables(module, variables);
                }

                foreach (TestClass cls in ((Module)action).TestClasses)
                {
                    FillVariables(cls, variables);
                }
            }
            else if (action is TestClass)
            {
                foreach (TestFunction function in ((TestClass)action).Functions)
                {
                    FillVariables(function, variables);
                }
            }
            else if (action is TestFunction)
            {
                foreach (ActionBase actionInFunction in ((TestFunction)action).Actions)
                {
                    PropertyInfo property = actionInFunction.GetType().GetProperty("Variable");
                    if (property != null)
                    {
                        object varName = property.GetValue(actionInFunction, null);
                        if (varName != null && variables.Contains((string)varName) == false)
                        {
                            variables.Add((string)varName);
                        }
                    }
                }
            }
        }
    }
}
