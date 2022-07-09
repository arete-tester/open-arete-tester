using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AreteTester.Actions;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using AreteTester.Core;

namespace AreteTester.UI
{
    internal class ProjectActionTreeLoader
    {


        public static void LoadProjectTree(TreeView tvActions, Project project)
        {
            TreeNode projectNode = tvActions.Nodes.Add(project.NodeText);
            projectNode.ImageIndex = projectNode.SelectedImageIndex = Constants.PROJECT_IMAGE_INDEX;
            projectNode.Tag = project;
            SetTreeNodeFontStyle(tvActions, projectNode);

            LoadModulesInTree(project.Modules, tvActions, projectNode);
        }

        public static void LoadModulesInTree(List<Module> modules, TreeView tvActions, TreeNode parentNode)
        {
            foreach (Module module in modules)
            {
                TreeNode moduleNode = LoadModuleNode(module, tvActions, parentNode);

                LoadTestClassesInTree(module.TestClasses, tvActions, moduleNode);

                if (module.Modules.Count > 0)
                {
                    LoadModulesInTree(module.Modules, tvActions, moduleNode);
                }
            }
        }

        public static void LoadTestClassesInTree(List<TestClass> testClasses, TreeView tvActions, TreeNode moduleNode)
        {
            foreach (TestClass cls in testClasses)
            {
                TreeNode clsNode = LoadTestClassNode(cls, tvActions, moduleNode);

                foreach (TestFunction sequence in cls.Functions)
                {
                    LoadTestFunctionNode(sequence, tvActions, clsNode);
                }
            }
        }

        public static TreeNode LoadModuleNode(Module module, TreeView tvActions, TreeNode node)
        {
            TreeNode moduleNode = node.Nodes.Add(module.NodeText);
            moduleNode.ImageIndex = moduleNode.SelectedImageIndex = Constants.MODULE_IMAGE_INDEX;
            moduleNode.Tag = module;
            SetTreeNodeFontStyle(tvActions, moduleNode);

            return moduleNode;
        }

        public static TreeNode LoadTestClassNode(TestClass cls, TreeView tvActions, TreeNode node)
        {
            TreeNode clsNode = node.Nodes.Add(cls.NodeText);
            clsNode.ImageIndex = clsNode.SelectedImageIndex = Constants.CLASS_IMAGE_INDEX;
            clsNode.Tag = cls;
            SetTreeNodeFontStyle(tvActions, clsNode);

            return clsNode;
        }

        public static void LoadTestFunctionNode(TestFunction test, TreeView tvActions, TreeNode node)
        {
            TreeNode functionNode = node.Nodes.Add(test.NodeText);
            functionNode.ImageIndex = functionNode.SelectedImageIndex = Constants.FUNCTION_IMAGE_INDEX;
            functionNode.Tag = test;
            SetTreeNodeFontStyle(tvActions, functionNode);

            foreach (ActionBase action in test.Actions)
            {
                TreeNode actionNode = functionNode.Nodes.Add(action.NodeText);
                actionNode.ImageIndex = actionNode.SelectedImageIndex = (action.IsBreakpointSet ? 2 : 0);
                actionNode.Tag = action;
                SetTreeNodeFontStyle(tvActions, actionNode);

                if (action is SqlExecuteQuery)
                {
                    LoadSelectDataAction((SqlExecuteQuery)action, tvActions, actionNode);
                }
                else if (action is Repeater)
                {
                    LoadRepeater((Repeater)action, tvActions, actionNode);
                }
            }
        }

        public static void LoadSelectDataAction(SqlExecuteQuery sqlExecuteQuery, TreeView tvActions, TreeNode node)
        {
            foreach (SelectFieldMapping mapping in sqlExecuteQuery.MappingActions)
            {
                TreeNode mappingNode = node.Nodes.Add(mapping.NodeText);
                mappingNode.Tag = mapping;
                SetTreeNodeFontStyle(tvActions, mappingNode);
            }
        }

        public static void LoadRepeater(Repeater repeater, TreeView tvActions, TreeNode node)
        {
            foreach (ActionBase action in repeater.Actions)
            {
                TreeNode actionNode = node.Nodes.Add(action.NodeText);
                actionNode.Tag = action;
                SetTreeNodeFontStyle(tvActions, actionNode);

                if (action is Repeater)
                {
                    LoadRepeater((Repeater)action, tvActions, actionNode);
                }
            }
        }

        public static void SetTreeNodeFontStyle(TreeView tvActions, TreeNode node)
        {
            switch (((ActionBase)node.Tag).FontStyle)
            {
                case AreteTester.Actions.FontStyle.Regular:
                    node.NodeFont = new Font(tvActions.Font, System.Drawing.FontStyle.Regular);
                    break;
                case AreteTester.Actions.FontStyle.Italic:
                    node.NodeFont = new Font(tvActions.Font, System.Drawing.FontStyle.Italic);
                    break;
                case AreteTester.Actions.FontStyle.Bold:
                    node.NodeFont = new Font(tvActions.Font, System.Drawing.FontStyle.Bold);
                    break;
            }
        }

        public static void ExpandCollapseNodes(TreeView tvActions)
        {
            if (tvActions.Nodes.Count > 0)
            {
                tvActions.Nodes[0].Expand();

                foreach (TreeNode moduleNode in tvActions.Nodes[0].Nodes)
                {
                    if (moduleNode.Tag is Module)
                    {
                        ExpandCollapseModuleNode(moduleNode);
                    }
                }
            }
        }

        public static void ExpandCollapseModuleNode(TreeNode moduleNode)
        {
            if (((Module)moduleNode.Tag).IsExpanded) moduleNode.Expand();

            foreach (TreeNode node in moduleNode.Nodes)
            {
                if (node.Tag is Module)
                {
                    ExpandCollapseModuleNode(node);
                }
                else
                {
                    if (((TestClass)node.Tag).IsExpanded) node.Expand();

                    foreach (TreeNode functionNode in node.Nodes)
                    {
                        TestFunction testFunction = (TestFunction)functionNode.Tag;
                        if (testFunction.IsExpanded) functionNode.Expand();

                        foreach (TreeNode actionNode in functionNode.Nodes)
                        {
                            if (actionNode.Tag is Repeater)
                            {
                                ExpandCollapseRepeaterNode(actionNode);
                            }
                        }
                    }
                }
            }
        }

        private static void ExpandCollapseRepeaterNode(TreeNode repeaterNode)
        {
            if (((Repeater)repeaterNode.Tag).IsExpanded) repeaterNode.Expand();
            foreach (TreeNode actionNode in repeaterNode.Nodes)
            {
                if (actionNode.Tag is Repeater)
                {
                    ExpandCollapseRepeaterNode(actionNode);
                }
            }
        }
    }
}
