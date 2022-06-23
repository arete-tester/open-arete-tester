using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AreteTester.Actions;
using System.Threading;

namespace AreteTester.UI
{
    internal class ActionValidator
    {
        private static ActionValidator instance;

        public TreeView ActionTree { get; set; }
        public Dictionary<TreeNode, ValidationResult> ValidationResults { get; set; }

        public static ActionValidator Instance
        {
            get
            {
                if (instance == null) instance = new ActionValidator();

                return instance;
            }
        }

        private ActionValidator()
        {
            this.ValidationResults = new Dictionary<TreeNode, ValidationResult>();
        }

        public void RunnerValidation()
        {
            ValidationResults.Clear();

            Validate(ActionTree.Nodes);
        }

        public bool IsErrorFound()
        {
            bool errorFound = false;
            foreach (ValidationResult result in ValidationResults.Values)
            {
                foreach (ValidationMessage message in result.Messages)
                {
                    if (message.MessageType == ValidationMessageType.Error)
                    {
                        errorFound = true;
                        break;
                    }
                }

                if (errorFound) break;
            }

            return errorFound;
        }

        private void Validate(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                node.ForeColor = System.Drawing.Color.Black;

                if (node.Tag != null && node.Tag is ActionBase)
                {
                    ValidationResult result = ((ActionBase)node.Tag).Validate();

                    bool errorFound = false;
                    foreach (ValidationMessage message in result.Messages)
                    {
                        if (message.MessageType == ValidationMessageType.Error)
                        {
                            errorFound = true;
                            break;
                        }
                    }

                    if (result.Messages.Count > 0)
                    {
                        this.ValidationResults.Add(node, result);

                        if (errorFound)
                        {
                            node.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            node.ForeColor = System.Drawing.Color.Orange;
                        }
                    }
                }

                if (node.Nodes.Count > 0)
                {
                    Validate(node.Nodes);
                }
            }
        }
    }
}
