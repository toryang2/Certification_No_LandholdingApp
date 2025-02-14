using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace NoLandholdingApp
{
    public partial class ReportForm : Form
    {
        private bool isSchoolRequirements;
        private DataTable reportData;

        public ReportForm(bool isSchoolRequirements, DataTable data)
        {
            InitializeComponent();
            this.reportData = data;
            this.isSchoolRequirements = isSchoolRequirements;
            this.Icon = Properties.Resources.logo;
        }

        private void ReportForm_Load(object sender, EventArgs e)
        {
            reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local;

            // Set the ReportViewer to PrintLayout view
            reportViewer1.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.Percent;
            reportViewer1.ZoomPercent = 100;  // Optional: Set the initial zoom level
            reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);

            if (reportData == null || reportData.Rows.Count == 0)
            {
                MessageBox.Show("No data available for the report.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Choose the correct RDLC file
            string reportPath = isSchoolRequirements
                ? "NoLandholdingApp.CertificationReport-SchoolRequirements.rdlc"
                : "NoLandholdingApp.CertificationReport.rdlc";

            reportViewer1.LocalReport.ReportEmbeddedResource = reportPath;

            // Clear previous data sources
            reportViewer1.LocalReport.DataSources.Clear();

            // Ensure dataset name matches RDLC
            ReportDataSource rds = new ReportDataSource("nolandholding_dataset", reportData);
            reportViewer1.LocalReport.DataSources.Add(rds);

            // Check if the report is being rendered correctly
            reportViewer1.LocalReport.Refresh();

            // Refresh the report
            reportViewer1.RefreshReport();

            // Debugging: Log the number of records
            Console.WriteLine($"Records in report: {reportData.Rows.Count}");
        }
    }
}
