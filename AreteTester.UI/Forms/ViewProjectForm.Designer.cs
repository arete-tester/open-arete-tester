namespace AreteTester.UI
{
    partial class ViewProjectForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewProjectForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label2 = new System.Windows.Forms.Label();
            this.tvActions = new System.Windows.Forms.TreeView();
            this.actionTreeImages = new System.Windows.Forms.ImageList(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.pgActionValues = new System.Windows.Forms.PropertyGrid();
            this.btnOpenProject = new System.Windows.Forms.Button();
            this.lblProjectPath = new System.Windows.Forms.Label();
            this.btnExportSelection = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Location = new System.Drawing.Point(12, 51);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.tvActions);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.pgActionValues);
            this.splitContainer1.Size = new System.Drawing.Size(803, 482);
            this.splitContainer1.SplitterDistance = 558;
            this.splitContainer1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Project Actions Tree";
            // 
            // tvActions
            // 
            this.tvActions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvActions.ImageIndex = 0;
            this.tvActions.ImageList = this.actionTreeImages;
            this.tvActions.Location = new System.Drawing.Point(3, 29);
            this.tvActions.Name = "tvActions";
            this.tvActions.SelectedImageIndex = 0;
            this.tvActions.Size = new System.Drawing.Size(548, 446);
            this.tvActions.TabIndex = 0;
            this.tvActions.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvAction_AfterSelect);
            // 
            // actionTreeImages
            // 
            this.actionTreeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("actionTreeImages.ImageStream")));
            this.actionTreeImages.TransparentColor = System.Drawing.Color.Transparent;
            this.actionTreeImages.Images.SetKeyName(0, "config.png");
            this.actionTreeImages.Images.SetKeyName(1, "progress.png");
            this.actionTreeImages.Images.SetKeyName(2, "break_point.png");
            this.actionTreeImages.Images.SetKeyName(3, "break_point_progress.png");
            this.actionTreeImages.Images.SetKeyName(4, "project.png");
            this.actionTreeImages.Images.SetKeyName(5, "module.png");
            this.actionTreeImages.Images.SetKeyName(6, "class.png");
            this.actionTreeImages.Images.SetKeyName(7, "method.png");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Action Properties";
            // 
            // pgActionValues
            // 
            this.pgActionValues.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pgActionValues.HelpVisible = false;
            this.pgActionValues.Location = new System.Drawing.Point(6, 29);
            this.pgActionValues.Name = "pgActionValues";
            this.pgActionValues.Size = new System.Drawing.Size(228, 397);
            this.pgActionValues.TabIndex = 9;
            // 
            // btnOpenProject
            // 
            this.btnOpenProject.Location = new System.Drawing.Point(12, 12);
            this.btnOpenProject.Name = "btnOpenProject";
            this.btnOpenProject.Size = new System.Drawing.Size(112, 33);
            this.btnOpenProject.TabIndex = 1;
            this.btnOpenProject.Text = "Open Project";
            this.btnOpenProject.UseVisualStyleBackColor = true;
            this.btnOpenProject.Click += new System.EventHandler(this.btnOpenProject_Click);
            // 
            // lblProjectPath
            // 
            this.lblProjectPath.AutoSize = true;
            this.lblProjectPath.Font = new System.Drawing.Font("Segoe UI", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProjectPath.Location = new System.Drawing.Point(130, 20);
            this.lblProjectPath.Name = "lblProjectPath";
            this.lblProjectPath.Size = new System.Drawing.Size(0, 17);
            this.lblProjectPath.TabIndex = 9;
            // 
            // btnExportSelection
            // 
            this.btnExportSelection.Location = new System.Drawing.Point(130, 12);
            this.btnExportSelection.Name = "btnExportSelection";
            this.btnExportSelection.Size = new System.Drawing.Size(112, 33);
            this.btnExportSelection.TabIndex = 10;
            this.btnExportSelection.Text = "Export Selection";
            this.btnExportSelection.UseVisualStyleBackColor = true;
            this.btnExportSelection.Click += new System.EventHandler(this.btnExportSelection_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(3, 429);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(239, 46);
            this.label1.TabIndex = 15;
            this.label1.Text = "Property changes can be exported, but changes made are not saved in the actual pr" +
                "oject. ";
            // 
            // ViewProjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 545);
            this.Controls.Add(this.btnExportSelection);
            this.Controls.Add(this.lblProjectPath);
            this.Controls.Add(this.btnOpenProject);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ViewProjectForm";
            this.Text = "View Project";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView tvActions;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PropertyGrid pgActionValues;
        private System.Windows.Forms.Button btnOpenProject;
        private System.Windows.Forms.Label lblProjectPath;
        private System.Windows.Forms.ImageList actionTreeImages;
        private System.Windows.Forms.Button btnExportSelection;
        private System.Windows.Forms.Label label1;
    }
}