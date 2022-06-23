namespace AreteTester.UI
{
    partial class TermsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TermsForm));
            this.txtTerms = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtTerms
            // 
            this.txtTerms.Location = new System.Drawing.Point(12, 13);
            this.txtTerms.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTerms.Multiline = true;
            this.txtTerms.Name = "txtTerms";
            this.txtTerms.ReadOnly = true;
            this.txtTerms.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtTerms.Size = new System.Drawing.Size(600, 411);
            this.txtTerms.TabIndex = 0;
            // 
            // TermsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(624, 437);
            this.Controls.Add(this.txtTerms);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TermsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Terms and Conditions";
            this.Load += new System.EventHandler(this.TermsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTerms;
    }
}