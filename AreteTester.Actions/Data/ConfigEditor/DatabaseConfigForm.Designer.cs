namespace AreteTester.Actions
{
    partial class DatabaseConfigForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDllPath = new System.Windows.Forms.TextBox();
            this.rbSqlServer = new System.Windows.Forms.RadioButton();
            this.rbOther = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.btnBrowseDLL = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtConnectionClassName = new System.Windows.Forms.TextBox();
            this.txtCommandClassName = new System.Windows.Forms.TextBox();
            this.txtAdapterClassName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Connection String";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Location = new System.Drawing.Point(15, 60);
            this.txtConnectionString.Multiline = true;
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtConnectionString.Size = new System.Drawing.Size(471, 63);
            this.txtConnectionString.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "ADO.Net DLL path";
            // 
            // txtDllPath
            // 
            this.txtDllPath.Location = new System.Drawing.Point(15, 146);
            this.txtDllPath.Name = "txtDllPath";
            this.txtDllPath.Size = new System.Drawing.Size(471, 25);
            this.txtDllPath.TabIndex = 3;
            // 
            // rbSqlServer
            // 
            this.rbSqlServer.AutoSize = true;
            this.rbSqlServer.Checked = true;
            this.rbSqlServer.Location = new System.Drawing.Point(141, 12);
            this.rbSqlServer.Name = "rbSqlServer";
            this.rbSqlServer.Size = new System.Drawing.Size(90, 21);
            this.rbSqlServer.TabIndex = 4;
            this.rbSqlServer.TabStop = true;
            this.rbSqlServer.Text = "SQL Server";
            this.rbSqlServer.UseVisualStyleBackColor = true;
            this.rbSqlServer.CheckedChanged += new System.EventHandler(this.rbSqlServer_CheckedChanged);
            // 
            // rbOther
            // 
            this.rbOther.AutoSize = true;
            this.rbOther.Location = new System.Drawing.Point(268, 12);
            this.rbOther.Name = "rbOther";
            this.rbOther.Size = new System.Drawing.Size(59, 21);
            this.rbOther.TabIndex = 5;
            this.rbOther.Text = "Other";
            this.rbOther.UseVisualStyleBackColor = true;
            this.rbOther.CheckedChanged += new System.EventHandler(this.rbOther_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Database";
            // 
            // btnBrowseDLL
            // 
            this.btnBrowseDLL.Location = new System.Drawing.Point(492, 146);
            this.btnBrowseDLL.Name = "btnBrowseDLL";
            this.btnBrowseDLL.Size = new System.Drawing.Size(43, 25);
            this.btnBrowseDLL.TabIndex = 7;
            this.btnBrowseDLL.Text = "...";
            this.btnBrowseDLL.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(399, 413);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 30);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(306, 413);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(87, 30);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 193);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(146, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "Connection Class Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 253);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(141, 17);
            this.label5.TabIndex = 12;
            this.label5.Text = "Command Class Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 313);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(128, 17);
            this.label6.TabIndex = 13;
            this.label6.Text = "Adapter Class Name";
            // 
            // txtConnectionClassName
            // 
            this.txtConnectionClassName.Location = new System.Drawing.Point(164, 190);
            this.txtConnectionClassName.Name = "txtConnectionClassName";
            this.txtConnectionClassName.Size = new System.Drawing.Size(322, 25);
            this.txtConnectionClassName.TabIndex = 14;
            // 
            // txtCommandClassName
            // 
            this.txtCommandClassName.Location = new System.Drawing.Point(164, 250);
            this.txtCommandClassName.Name = "txtCommandClassName";
            this.txtCommandClassName.Size = new System.Drawing.Size(322, 25);
            this.txtCommandClassName.TabIndex = 15;
            // 
            // txtAdapterClassName
            // 
            this.txtAdapterClassName.Location = new System.Drawing.Point(164, 310);
            this.txtAdapterClassName.Name = "txtAdapterClassName";
            this.txtAdapterClassName.Size = new System.Drawing.Size(322, 25);
            this.txtAdapterClassName.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label7.Location = new System.Drawing.Point(161, 371);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(343, 17);
            this.label7.TabIndex = 17;
            this.label7.Text = "Note: Database libraries not included due to licencing issues.";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(161, 218);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(226, 17);
            this.label8.TabIndex = 18;
            this.label8.Text = "Ex: System.Data.SqlClient.SqlConnection";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(161, 278);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(222, 17);
            this.label9.TabIndex = 19;
            this.label9.Text = "Ex: System.Data.SqlClient.SqlCommand";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(161, 338);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(235, 17);
            this.label10.TabIndex = 20;
            this.label10.Text = "Ex: System.Data.SqlClient.SqlDataAdapter";
            // 
            // DatabaseConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 458);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtAdapterClassName);
            this.Controls.Add(this.txtCommandClassName);
            this.Controls.Add(this.txtConnectionClassName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnBrowseDLL);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rbOther);
            this.Controls.Add(this.rbSqlServer);
            this.Controls.Add(this.txtDllPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtConnectionString);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DatabaseConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Database Configuration";
            this.Load += new System.EventHandler(this.DatabaseActionForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDllPath;
        private System.Windows.Forms.RadioButton rbSqlServer;
        private System.Windows.Forms.RadioButton rbOther;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnBrowseDLL;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtConnectionClassName;
        private System.Windows.Forms.TextBox txtCommandClassName;
        private System.Windows.Forms.TextBox txtAdapterClassName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
    }
}