using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoLandholdingApp
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            SetFormProperties();
        }

        private void databaseSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Pass the reference of formInput if opened from preferences, else null
            DatabaseSetup dbSetup = new DatabaseSetup(); // Pass 'this' (formInput) reference
            dbSetup.ShowDialog();
        }

        private void SetFormProperties()
        {
            this.Icon = Properties.Resources.setting;
        }
    }
}
