using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AreteTester.Actions;
using System.IO;

namespace AreteTester.Core
{
    public class ProjectLoader
    {
        public static Project LoadProject(string projectPath)
        {
            Project project = (Project)LoadSave.Load(typeof(Project), projectPath + @"\project.atr");

            AreteTester.Actions.Globals.CurrentProject = project;

            foreach (Module module in project.Modules.OrderBy(m => m.Name))
            {
                LoadModule(module, projectPath);
            }

            return project;
        }

        private static void LoadModule(Module module, string parentPath)
        {
            string modulePath = parentPath + @"\" + module.Name;
            if (Directory.Exists(modulePath))
            {
                module.TestClasses.Clear();
                foreach (string file in Directory.GetFiles(modulePath).OrderBy(fl => fl))
                {
                    TestClass cls = (TestClass)LoadSave.Load(typeof(TestClass), file);

                    module.TestClasses.Add(cls);
                }
            }

            foreach (Module childModule in module.Modules)
            {
                LoadModule(childModule, modulePath);
            }
        }
    }
}
