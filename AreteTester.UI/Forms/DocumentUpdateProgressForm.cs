using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AreteTester.UI
{
    public partial class DocumentUpdateProgressForm : Form
    {
        public DocumentUpdateProgressForm()
        {
            InitializeComponent();
        }

        public string DocumentFile
        {
            set { this.lblDocumentPath.Text = value; }
        }
    }
}
