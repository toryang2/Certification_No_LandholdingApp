namespace NoLandholdingApp
{
    partial class DatabaseSetup
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
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.checkBoxEnableDatabaseEdit = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(112, 22);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(159, 20);
            this.txtServer.TabIndex = 0;
            this.txtServer.TextChanged += new System.EventHandler(this.txtServer_TextChanged);
            // 
            // txtDatabase
            // 
            this.txtDatabase.Location = new System.Drawing.Point(112, 48);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(159, 20);
            this.txtDatabase.TabIndex = 1;
            this.txtDatabase.TextChanged += new System.EventHandler(this.txtDatabase_TextChanged);
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(112, 74);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(159, 20);
            this.txtUser.TabIndex = 3;
            this.txtUser.TextChanged += new System.EventHandler(this.txtUser_TextChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(112, 100);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(159, 20);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // checkBoxEnableDatabaseEdit
            // 
            this.checkBoxEnableDatabaseEdit.AutoSize = true;
            this.checkBoxEnableDatabaseEdit.Location = new System.Drawing.Point(278, 50);
            this.checkBoxEnableDatabaseEdit.Name = "checkBoxEnableDatabaseEdit";
            this.checkBoxEnableDatabaseEdit.Size = new System.Drawing.Size(15, 14);
            this.checkBoxEnableDatabaseEdit.TabIndex = 2;
            this.checkBoxEnableDatabaseEdit.UseVisualStyleBackColor = true;
            this.checkBoxEnableDatabaseEdit.CheckedChanged += new System.EventHandler(this.checkBoxEnableDatabaseEdit_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Server / IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Database Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Username";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Password";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(212, 126);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(59, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Location = new System.Drawing.Point(112, 126);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(94, 23);
            this.btnTestConnection.TabIndex = 5;
            this.btnTestConnection.Text = "Test Connection";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // DatabaseSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 167);
            this.Controls.Add(this.btnTestConnection);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBoxEnableDatabaseEdit);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.txtDatabase);
            this.Controls.Add(this.txtServer);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DatabaseSetup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Database Setup";
            this.Load += new System.EventHandler(this.DatabaseSetup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.CheckBox checkBoxEnableDatabaseEdit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnTestConnection;
    }
}