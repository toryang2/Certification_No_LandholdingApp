namespace NoLandholdingApp
{
    partial class formInput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formInput));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownSystem = new System.Windows.Forms.ToolStripDropDownButton();
            this.changePasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shutdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButtonAbout = new System.Windows.Forms.ToolStripButton();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.linkScholarship = new System.Windows.Forms.LinkLabel();
            this.linkPhilHealth = new System.Windows.Forms.LinkLabel();
            this.linkNoLandHolding = new System.Windows.Forms.LinkLabel();
            this.linkBailBond = new System.Windows.Forms.LinkLabel();
            this.linkHospitalization = new System.Windows.Forms.LinkLabel();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.linkLabelTotalAggregate = new System.Windows.Forms.LinkLabel();
            this.labelTotalAggregate = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.linkLabelCertificateThatNoTitle = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.linkLabelNoImprovement = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownSystem,
            this.toolStripButtonAbout});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1784, 25);
            this.toolStrip.TabIndex = 30;
            this.toolStrip.Text = "toolStripMenu";
            // 
            // toolStripDropDownSystem
            // 
            this.toolStripDropDownSystem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownSystem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changePasswordToolStripMenuItem,
            this.preferenceToolStripMenuItem,
            this.logoutToolStripMenuItem,
            this.shutdownToolStripMenuItem});
            this.toolStripDropDownSystem.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownSystem.Image")));
            this.toolStripDropDownSystem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownSystem.Name = "toolStripDropDownSystem";
            this.toolStripDropDownSystem.ShowDropDownArrow = false;
            this.toolStripDropDownSystem.Size = new System.Drawing.Size(49, 22);
            this.toolStripDropDownSystem.Text = "System";
            this.toolStripDropDownSystem.ToolTipText = "System";
            // 
            // changePasswordToolStripMenuItem
            // 
            this.changePasswordToolStripMenuItem.Image = global::NoLandholdingApp.Properties.Resources.change_password;
            this.changePasswordToolStripMenuItem.Name = "changePasswordToolStripMenuItem";
            this.changePasswordToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.changePasswordToolStripMenuItem.Text = "ChangePassword";
            // 
            // preferenceToolStripMenuItem
            // 
            this.preferenceToolStripMenuItem.Image = global::NoLandholdingApp.Properties.Resources.preference;
            this.preferenceToolStripMenuItem.Name = "preferenceToolStripMenuItem";
            this.preferenceToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.preferenceToolStripMenuItem.Text = "Preference";
            this.preferenceToolStripMenuItem.Click += new System.EventHandler(this.preferenceToolStripMenuItem_Click);
            // 
            // shutdownToolStripMenuItem
            // 
            this.shutdownToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("shutdownToolStripMenuItem.Image")));
            this.shutdownToolStripMenuItem.Name = "shutdownToolStripMenuItem";
            this.shutdownToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.shutdownToolStripMenuItem.Text = "Shutdown";
            // 
            // toolStripButtonAbout
            // 
            this.toolStripButtonAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonAbout.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAbout.Image")));
            this.toolStripButtonAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAbout.Name = "toolStripButtonAbout";
            this.toolStripButtonAbout.Size = new System.Drawing.Size(44, 22);
            this.toolStripButtonAbout.Text = "About";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Location = new System.Drawing.Point(263, 290);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(56, 26);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.linkScholarship);
            this.groupBox2.Controls.Add(this.linkPhilHealth);
            this.groupBox2.Controls.Add(this.linkNoLandHolding);
            this.groupBox2.Controls.Add(this.linkBailBond);
            this.groupBox2.Controls.Add(this.linkHospitalization);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(40, 28);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(279, 245);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // linkScholarship
            // 
            this.linkScholarship.AutoSize = true;
            this.linkScholarship.Cursor = System.Windows.Forms.Cursors.Default;
            this.linkScholarship.Font = new System.Drawing.Font("Tahoma", 12F);
            this.linkScholarship.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkScholarship.LinkColor = System.Drawing.Color.RoyalBlue;
            this.linkScholarship.Location = new System.Drawing.Point(16, 160);
            this.linkScholarship.Name = "linkScholarship";
            this.linkScholarship.Size = new System.Drawing.Size(90, 19);
            this.linkScholarship.TabIndex = 5;
            this.linkScholarship.TabStop = true;
            this.linkScholarship.Text = "Scholarship";
            this.linkScholarship.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.linkScholarship.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkScholarship_LinkClicked);
            // 
            // linkPhilHealth
            // 
            this.linkPhilHealth.AutoSize = true;
            this.linkPhilHealth.Cursor = System.Windows.Forms.Cursors.Default;
            this.linkPhilHealth.Font = new System.Drawing.Font("Tahoma", 12F);
            this.linkPhilHealth.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkPhilHealth.LinkColor = System.Drawing.Color.RoyalBlue;
            this.linkPhilHealth.Location = new System.Drawing.Point(16, 136);
            this.linkPhilHealth.Name = "linkPhilHealth";
            this.linkPhilHealth.Size = new System.Drawing.Size(80, 19);
            this.linkPhilHealth.TabIndex = 4;
            this.linkPhilHealth.TabStop = true;
            this.linkPhilHealth.Text = "PhilHealth";
            this.linkPhilHealth.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.linkPhilHealth.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkPhilHealth_LinkClicked);
            // 
            // linkNoLandHolding
            // 
            this.linkNoLandHolding.AutoSize = true;
            this.linkNoLandHolding.Cursor = System.Windows.Forms.Cursors.Default;
            this.linkNoLandHolding.Font = new System.Drawing.Font("Tahoma", 12F);
            this.linkNoLandHolding.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkNoLandHolding.LinkColor = System.Drawing.Color.RoyalBlue;
            this.linkNoLandHolding.Location = new System.Drawing.Point(16, 112);
            this.linkNoLandHolding.Name = "linkNoLandHolding";
            this.linkNoLandHolding.Size = new System.Drawing.Size(128, 19);
            this.linkNoLandHolding.TabIndex = 3;
            this.linkNoLandHolding.TabStop = true;
            this.linkNoLandHolding.Text = "No Land Holding";
            this.linkNoLandHolding.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            // 
            // linkBailBond
            // 
            this.linkBailBond.AutoSize = true;
            this.linkBailBond.Cursor = System.Windows.Forms.Cursors.Default;
            this.linkBailBond.Font = new System.Drawing.Font("Tahoma", 12F);
            this.linkBailBond.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkBailBond.LinkColor = System.Drawing.Color.RoyalBlue;
            this.linkBailBond.Location = new System.Drawing.Point(16, 88);
            this.linkBailBond.Name = "linkBailBond";
            this.linkBailBond.Size = new System.Drawing.Size(75, 19);
            this.linkBailBond.TabIndex = 2;
            this.linkBailBond.TabStop = true;
            this.linkBailBond.Text = "Bail Bond";
            this.linkBailBond.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.linkBailBond.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkBailBond_LinkClicked);
            // 
            // linkHospitalization
            // 
            this.linkHospitalization.AutoSize = true;
            this.linkHospitalization.Cursor = System.Windows.Forms.Cursors.Default;
            this.linkHospitalization.Font = new System.Drawing.Font("Tahoma", 12F);
            this.linkHospitalization.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkHospitalization.LinkColor = System.Drawing.Color.RoyalBlue;
            this.linkHospitalization.Location = new System.Drawing.Point(16, 64);
            this.linkHospitalization.Name = "linkHospitalization";
            this.linkHospitalization.Size = new System.Drawing.Size(112, 19);
            this.linkHospitalization.TabIndex = 1;
            this.linkHospitalization.TabStop = true;
            this.linkHospitalization.Text = "Hospitalization";
            this.linkHospitalization.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.linkHospitalization.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkHospitalization_LinkClicked);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(15, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(248, 29);
            this.label7.TabIndex = 0;
            this.label7.Text = "NO LAND HOLDING";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.linkLabelTotalAggregate);
            this.groupBox1.Controls.Add(this.labelTotalAggregate);
            this.groupBox1.Location = new System.Drawing.Point(325, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(279, 245);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // linkLabelTotalAggregate
            // 
            this.linkLabelTotalAggregate.AutoSize = true;
            this.linkLabelTotalAggregate.Cursor = System.Windows.Forms.Cursors.Default;
            this.linkLabelTotalAggregate.Font = new System.Drawing.Font("Tahoma", 12F);
            this.linkLabelTotalAggregate.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelTotalAggregate.LinkColor = System.Drawing.Color.RoyalBlue;
            this.linkLabelTotalAggregate.Location = new System.Drawing.Point(16, 64);
            this.linkLabelTotalAggregate.Name = "linkLabelTotalAggregate";
            this.linkLabelTotalAggregate.Size = new System.Drawing.Size(123, 19);
            this.linkLabelTotalAggregate.TabIndex = 1;
            this.linkLabelTotalAggregate.TabStop = true;
            this.linkLabelTotalAggregate.Text = "Total Aggregate";
            this.linkLabelTotalAggregate.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            // 
            // labelTotalAggregate
            // 
            this.labelTotalAggregate.AutoSize = true;
            this.labelTotalAggregate.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.labelTotalAggregate.Location = new System.Drawing.Point(15, 16);
            this.labelTotalAggregate.Name = "labelTotalAggregate";
            this.labelTotalAggregate.Size = new System.Drawing.Size(246, 29);
            this.labelTotalAggregate.TabIndex = 0;
            this.labelTotalAggregate.Text = "TOTAL AGGREGATE";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.linkLabelCertificateThatNoTitle);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(610, 28);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(403, 245);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            // 
            // linkLabelCertificateThatNoTitle
            // 
            this.linkLabelCertificateThatNoTitle.AutoSize = true;
            this.linkLabelCertificateThatNoTitle.Cursor = System.Windows.Forms.Cursors.Default;
            this.linkLabelCertificateThatNoTitle.Font = new System.Drawing.Font("Tahoma", 12F);
            this.linkLabelCertificateThatNoTitle.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelCertificateThatNoTitle.LinkColor = System.Drawing.Color.RoyalBlue;
            this.linkLabelCertificateThatNoTitle.Location = new System.Drawing.Point(16, 64);
            this.linkLabelCertificateThatNoTitle.Name = "linkLabelCertificateThatNoTitle";
            this.linkLabelCertificateThatNoTitle.Size = new System.Drawing.Size(172, 19);
            this.linkLabelCertificateThatNoTitle.TabIndex = 1;
            this.linkLabelCertificateThatNoTitle.TabStop = true;
            this.linkLabelCertificateThatNoTitle.Text = "Certificate that No Title";
            this.linkLabelCertificateThatNoTitle.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(15, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(369, 29);
            this.label2.TabIndex = 0;
            this.label2.Text = "CERTIFICATE THAT NO TITLE";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.linkLabelNoImprovement);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Location = new System.Drawing.Point(1019, 28);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(279, 245);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            // 
            // linkLabelNoImprovement
            // 
            this.linkLabelNoImprovement.AutoSize = true;
            this.linkLabelNoImprovement.Cursor = System.Windows.Forms.Cursors.Default;
            this.linkLabelNoImprovement.Font = new System.Drawing.Font("Tahoma", 12F);
            this.linkLabelNoImprovement.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelNoImprovement.LinkColor = System.Drawing.Color.RoyalBlue;
            this.linkLabelNoImprovement.Location = new System.Drawing.Point(16, 64);
            this.linkLabelNoImprovement.Name = "linkLabelNoImprovement";
            this.linkLabelNoImprovement.Size = new System.Drawing.Size(130, 19);
            this.linkLabelNoImprovement.TabIndex = 1;
            this.linkLabelNoImprovement.TabStop = true;
            this.linkLabelNoImprovement.Text = "No Improvement";
            this.linkLabelNoImprovement.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(15, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(242, 29);
            this.label3.TabIndex = 0;
            this.label3.Text = "NO IMPROVEMENT";
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.logoutToolStripMenuItem.Text = "Logout";
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.logoutToolStripMenuItem_Click);
            // 
            // formInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1784, 961);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.groupBox3);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "formInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Municipal Assessor\'s Office: Certification";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownSystem;
        private System.Windows.Forms.ToolStripMenuItem changePasswordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preferenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shutdownToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonAbout;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.LinkLabel linkHospitalization;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.LinkLabel linkScholarship;
        private System.Windows.Forms.LinkLabel linkPhilHealth;
        private System.Windows.Forms.LinkLabel linkNoLandHolding;
        private System.Windows.Forms.LinkLabel linkBailBond;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.LinkLabel linkLabelTotalAggregate;
        private System.Windows.Forms.Label labelTotalAggregate;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.LinkLabel linkLabelCertificateThatNoTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.LinkLabel linkLabelNoImprovement;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem;
    }
}

