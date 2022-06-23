namespace AreteTester.UI
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lnkUrl = new System.Windows.Forms.LinkLabel();
            this.lblSupportEmail = new System.Windows.Forms.Label();
            this.lblApplicationVersion = new System.Windows.Forms.Label();
            this.lblLatestBuild = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Version:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Email: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Website: ";
            // 
            // lnkUrl
            // 
            this.lnkUrl.AutoSize = true;
            this.lnkUrl.Location = new System.Drawing.Point(79, 130);
            this.lnkUrl.Name = "lnkUrl";
            this.lnkUrl.Size = new System.Drawing.Size(105, 17);
            this.lnkUrl.TabIndex = 4;
            this.lnkUrl.TabStop = true;
            this.lnkUrl.Text = "www.weburl.com";
            this.lnkUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkUrl_LinkClicked);
            // 
            // lblSupportEmail
            // 
            this.lblSupportEmail.AutoSize = true;
            this.lblSupportEmail.Location = new System.Drawing.Point(79, 104);
            this.lblSupportEmail.Name = "lblSupportEmail";
            this.lblSupportEmail.Size = new System.Drawing.Size(138, 17);
            this.lblSupportEmail.TabIndex = 5;
            this.lblSupportEmail.Text = "support@domain.com";
            // 
            // lblApplicationVersion
            // 
            this.lblApplicationVersion.AutoSize = true;
            this.lblApplicationVersion.Location = new System.Drawing.Point(79, 78);
            this.lblApplicationVersion.Name = "lblApplicationVersion";
            this.lblApplicationVersion.Size = new System.Drawing.Size(25, 17);
            this.lblApplicationVersion.TabIndex = 6;
            this.lblApplicationVersion.Text = "1.0";
            // 
            // lblLatestBuild
            // 
            this.lblLatestBuild.AutoSize = true;
            this.lblLatestBuild.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLatestBuild.Location = new System.Drawing.Point(171, 78);
            this.lblLatestBuild.Name = "lblLatestBuild";
            this.lblLatestBuild.Size = new System.Drawing.Size(15, 17);
            this.lblLatestBuild.TabIndex = 8;
            this.lblLatestBuild.Text = "1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(130, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 17);
            this.label6.TabIndex = 7;
            this.label6.Text = "Build:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AreteTester.UI.Properties.Resources.logo_text;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(281, 52);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(367, 166);
            this.Controls.Add(this.lblLatestBuild);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblApplicationVersion);
            this.Controls.Add(this.lblSupportEmail);
            this.Controls.Add(this.lnkUrl);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About Arete Tester";
            this.Load += new System.EventHandler(this.AboutForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel lnkUrl;
        private System.Windows.Forms.Label lblSupportEmail;
        private System.Windows.Forms.Label lblApplicationVersion;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblLatestBuild;
        private System.Windows.Forms.Label label6;
    }
}