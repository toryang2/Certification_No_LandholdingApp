namespace NoLandholdingApp
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButtonMore = new System.Windows.Forms.ToolStripDropDownButton();
            this.databaseSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButtonMore});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Size = new System.Drawing.Size(800, 25);
            this.toolStripMenu.TabIndex = 0;
            this.toolStripMenu.Text = "toolStripMenu";
            // 
            // toolStripDropDownButtonMore
            // 
            this.toolStripDropDownButtonMore.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButtonMore.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.databaseSettingsToolStripMenuItem});
            this.toolStripDropDownButtonMore.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonMore.Image")));
            this.toolStripDropDownButtonMore.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonMore.Name = "toolStripDropDownButtonMore";
            this.toolStripDropDownButtonMore.ShowDropDownArrow = false;
            this.toolStripDropDownButtonMore.Size = new System.Drawing.Size(39, 22);
            this.toolStripDropDownButtonMore.Text = "More";
            // 
            // databaseSettingsToolStripMenuItem
            // 
            this.databaseSettingsToolStripMenuItem.Name = "databaseSettingsToolStripMenuItem";
            this.databaseSettingsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.databaseSettingsToolStripMenuItem.Text = "Database Settings";
            this.databaseSettingsToolStripMenuItem.Click += new System.EventHandler(this.databaseSettingsToolStripMenuItem_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.toolStripMenu);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonMore;
        private System.Windows.Forms.ToolStripMenuItem databaseSettingsToolStripMenuItem;
    }
}