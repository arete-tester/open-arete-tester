using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AreteTester.Actions;

namespace AreteTester.UI
{
    public partial class SearchForm : Form
    {
        public event EventHandler CloseClicked;

        private string searchText;
        private TreeNode currentTreeNode;

        public SearchForm()
        {
            InitializeComponent();
        }

        public TreeView ActionTree { get; set; }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchNodes(this.ActionTree.Nodes);
        }

        private void SearchNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (currentTreeNode == null)
                {
                    ActionBase action = (ActionBase)node.Tag;

                    if (action.ActionType.ToLower().Contains(txtSearchFor.Text.ToLower()))
                    {
                        this.ActionTree.SelectedNode = currentTreeNode = node;
                        return;
                    }
                    else if (String.IsNullOrEmpty(action.Description) == false && action.Description.ToLower().Contains(txtSearchFor.Text.ToLower()))
                    {
                        this.ActionTree.SelectedNode = currentTreeNode = node;
                        return;
                    }
                    else if (String.IsNullOrEmpty(action.NodeText) == false && action.NodeText.ToLower().Contains(txtSearchFor.Text.ToLower()))
                    {
                        this.ActionTree.SelectedNode = currentTreeNode = node;
                        return;
                    }
                }

                if (node == currentTreeNode)
                {
                    currentTreeNode = null;
                }

                SearchNodes(node.Nodes);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (CloseClicked != null)
            {
                CloseClicked(this, null);
            }
        }
    }
}
