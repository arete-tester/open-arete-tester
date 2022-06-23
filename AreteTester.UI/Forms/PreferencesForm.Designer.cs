namespace AreteTester.UI
{
    partial class PreferencesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreferencesForm));
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.chkCopyClipboardToXPath = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtXPathFinderVersion = new System.Windows.Forms.TextBox();
            this.lnkViewXPathFinder = new System.Windows.Forms.LinkLabel();
            this.chkLaunchXPathFinder = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDefaultWorkspace = new System.Windows.Forms.TextBox();
            this.btnBrowseDefaultWorkspace = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numWaitDuration = new System.Windows.Forms.NumericUpDown();
            this.chkShowOutputWindow = new System.Windows.Forms.CheckBox();
            this.chkEmptyDescriptionWarning = new System.Windows.Forms.CheckBox();
            this.chkSetDefaultDescription = new System.Windows.Forms.CheckBox();
            this.chkIgnoreEmptyDescription = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numWaitDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(312, 298);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 30);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(405, 298);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 30);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chkCopyClipboardToXPath
            // 
            this.chkCopyClipboardToXPath.AutoSize = true;
            this.chkCopyClipboardToXPath.Location = new System.Drawing.Point(12, 24);
            this.chkCopyClipboardToXPath.Name = "chkCopyClipboardToXPath";
            this.chkCopyClipboardToXPath.Size = new System.Drawing.Size(407, 21);
            this.chkCopyClipboardToXPath.TabIndex = 10;
            this.chkCopyClipboardToXPath.Text = "Copy clipboard text to XPath property on mouse left double click";
            this.chkCopyClipboardToXPath.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 17);
            this.label1.TabIndex = 11;
            this.label1.Text = "XPath Finder version:";
            // 
            // txtXPathFinderVersion
            // 
            this.txtXPathFinderVersion.Location = new System.Drawing.Point(163, 78);
            this.txtXPathFinderVersion.Name = "txtXPathFinderVersion";
            this.txtXPathFinderVersion.Size = new System.Drawing.Size(271, 25);
            this.txtXPathFinderVersion.TabIndex = 12;
            // 
            // lnkViewXPathFinder
            // 
            this.lnkViewXPathFinder.AutoSize = true;
            this.lnkViewXPathFinder.Location = new System.Drawing.Point(198, 106);
            this.lnkViewXPathFinder.Name = "lnkViewXPathFinder";
            this.lnkViewXPathFinder.Size = new System.Drawing.Size(236, 17);
            this.lnkViewXPathFinder.TabIndex = 13;
            this.lnkViewXPathFinder.TabStop = true;
            this.lnkViewXPathFinder.Text = "View XPath Finder in chrome web store";
            this.lnkViewXPathFinder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkViewXPathFinder_LinkClicked);
            // 
            // chkLaunchXPathFinder
            // 
            this.chkLaunchXPathFinder.AutoSize = true;
            this.chkLaunchXPathFinder.Location = new System.Drawing.Point(12, 51);
            this.chkLaunchXPathFinder.Name = "chkLaunchXPathFinder";
            this.chkLaunchXPathFinder.Size = new System.Drawing.Size(268, 21);
            this.chkLaunchXPathFinder.TabIndex = 14;
            this.chkLaunchXPathFinder.Text = "Launch XPath Finder with Selenium driver.";
            this.chkLaunchXPathFinder.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 17);
            this.label2.TabIndex = 15;
            this.label2.Text = "Default Workspace";
            // 
            // txtDefaultWorkspace
            // 
            this.txtDefaultWorkspace.Location = new System.Drawing.Point(163, 126);
            this.txtDefaultWorkspace.Name = "txtDefaultWorkspace";
            this.txtDefaultWorkspace.Size = new System.Drawing.Size(271, 25);
            this.txtDefaultWorkspace.TabIndex = 16;
            // 
            // btnBrowseDefaultWorkspace
            // 
            this.btnBrowseDefaultWorkspace.Location = new System.Drawing.Point(440, 122);
            this.btnBrowseDefaultWorkspace.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBrowseDefaultWorkspace.Name = "btnBrowseDefaultWorkspace";
            this.btnBrowseDefaultWorkspace.Size = new System.Drawing.Size(44, 30);
            this.btnBrowseDefaultWorkspace.TabIndex = 17;
            this.btnBrowseDefaultWorkspace.Text = ". . .";
            this.btnBrowseDefaultWorkspace.UseVisualStyleBackColor = true;
            this.btnBrowseDefaultWorkspace.Click += new System.EventHandler(this.btnBrowseDefaultWorkspace_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 160);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(146, 17);
            this.label3.TabIndex = 18;
            this.label3.Text = "Execution Wait Duration";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(301, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 17);
            this.label4.TabIndex = 20;
            this.label4.Text = "(milliseconds)";
            // 
            // numWaitDuration
            // 
            this.numWaitDuration.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numWaitDuration.Location = new System.Drawing.Point(163, 158);
            this.numWaitDuration.Maximum = new decimal(new int[] {
            900000,
            0,
            0,
            0});
            this.numWaitDuration.Name = "numWaitDuration";
            this.numWaitDuration.Size = new System.Drawing.Size(132, 25);
            this.numWaitDuration.TabIndex = 21;
            // 
            // chkShowOutputWindow
            // 
            this.chkShowOutputWindow.AutoSize = true;
            this.chkShowOutputWindow.Location = new System.Drawing.Point(15, 189);
            this.chkShowOutputWindow.Name = "chkShowOutputWindow";
            this.chkShowOutputWindow.Size = new System.Drawing.Size(280, 21);
            this.chkShowOutputWindow.TabIndex = 22;
            this.chkShowOutputWindow.Text = "Show output window when execution starts.";
            this.chkShowOutputWindow.UseVisualStyleBackColor = true;
            // 
            // chkEmptyDescriptionWarning
            // 
            this.chkEmptyDescriptionWarning.AutoSize = true;
            this.chkEmptyDescriptionWarning.Location = new System.Drawing.Point(15, 216);
            this.chkEmptyDescriptionWarning.Name = "chkEmptyDescriptionWarning";
            this.chkEmptyDescriptionWarning.Size = new System.Drawing.Size(244, 21);
            this.chkEmptyDescriptionWarning.TabIndex = 23;
            this.chkEmptyDescriptionWarning.Text = "Show warning if description is empty.";
            this.chkEmptyDescriptionWarning.UseVisualStyleBackColor = true;
            // 
            // chkSetDefaultDescription
            // 
            this.chkSetDefaultDescription.AutoSize = true;
            this.chkSetDefaultDescription.Location = new System.Drawing.Point(15, 243);
            this.chkSetDefaultDescription.Name = "chkSetDefaultDescription";
            this.chkSetDefaultDescription.Size = new System.Drawing.Size(390, 21);
            this.chkSetDefaultDescription.TabIndex = 24;
            this.chkSetDefaultDescription.Text = "Set default description when an action is added to the project.";
            this.chkSetDefaultDescription.UseVisualStyleBackColor = true;
            // 
            // chkIgnoreEmptyDescription
            // 
            this.chkIgnoreEmptyDescription.AutoSize = true;
            this.chkIgnoreEmptyDescription.Location = new System.Drawing.Point(15, 270);
            this.chkIgnoreEmptyDescription.Name = "chkIgnoreEmptyDescription";
            this.chkIgnoreEmptyDescription.Size = new System.Drawing.Size(233, 21);
            this.chkIgnoreEmptyDescription.TabIndex = 25;
            this.chkIgnoreEmptyDescription.Text = "Ignore empty description in output.";
            this.chkIgnoreEmptyDescription.UseVisualStyleBackColor = true;
            // 
            // PreferencesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(504, 341);
            this.Controls.Add(this.chkIgnoreEmptyDescription);
            this.Controls.Add(this.chkSetDefaultDescription);
            this.Controls.Add(this.chkEmptyDescriptionWarning);
            this.Controls.Add(this.chkShowOutputWindow);
            this.Controls.Add(this.numWaitDuration);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnBrowseDefaultWorkspace);
            this.Controls.Add(this.txtDefaultWorkspace);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkLaunchXPathFinder);
            this.Controls.Add(this.lnkViewXPathFinder);
            this.Controls.Add(this.txtXPathFinderVersion);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkCopyClipboardToXPath);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PreferencesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Preferences";
            this.Load += new System.EventHandler(this.PreferencesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numWaitDuration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox chkCopyClipboardToXPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtXPathFinderVersion;
        private System.Windows.Forms.LinkLabel lnkViewXPathFinder;
        private System.Windows.Forms.CheckBox chkLaunchXPathFinder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDefaultWorkspace;
        private System.Windows.Forms.Button btnBrowseDefaultWorkspace;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numWaitDuration;
        private System.Windows.Forms.CheckBox chkShowOutputWindow;
        private System.Windows.Forms.CheckBox chkEmptyDescriptionWarning;
        private System.Windows.Forms.CheckBox chkSetDefaultDescription;
        private System.Windows.Forms.CheckBox chkIgnoreEmptyDescription;
    }
}