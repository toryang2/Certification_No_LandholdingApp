using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonLibrary;
using Microsoft.Reporting.WinForms;

namespace NoLandholdingApp
{
    public partial class ReportForm : Form
    {
        private DataTable reportData;
        private string selectedType; // Store selected type

        public ReportForm(DataTable data, string selectedType)
        {
            InitializeComponent();
            this.reportData = data;
            this.selectedType = selectedType.Trim().ToLower(); // Store and normalize it
            this.Icon = Properties.Resources.logo;


            this.KeyPreview = true;
            // Subscribe to KeyDown event
            this.KeyDown += new KeyEventHandler(reportViewer1_KeyDown);
        }

        private void ReportForm_Load(object sender, EventArgs e)
        {
            reportViewer1.ProcessingMode = ProcessingMode.Local;

            // Set the ReportViewer to PrintLayout view
            reportViewer1.ZoomMode = ZoomMode.Percent;
            reportViewer1.ZoomPercent = 100;
            reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);

            if (reportData == null || reportData.Rows.Count == 0)
            {
                MessageBox.Show("No data available for the report.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Determine the RDLC file based on the selectedType from comboBoxTypeList
            string reportPath;
            if (selectedType == "hospitalization")
            {
                reportPath = "NoLandholdingApp.rdlc.CertificationReport.rdlc";
            }
            else if (selectedType == "scholarship")
            {
                reportPath = "NoLandholdingApp.rdlc.CertificationReport-Scholarship.rdlc";
            }
            else
            {
                reportPath = "NoLandholdingApp.rdlc.CertificationReport.rdlc"; // Default fallback
            }

            // Set the selected RDLC file
            reportViewer1.LocalReport.ReportEmbeddedResource = reportPath;

            // Clear previous data sources
            reportViewer1.LocalReport.DataSources.Clear();

            // Ensure dataset name matches RDLC
            ReportDataSource rds = new ReportDataSource("nolandholding_dataset", reportData);
            reportViewer1.LocalReport.DataSources.Add(rds);

            // === Add this part ===
            //var parameters = new ReportParameter[]
            //{
            //    new ReportParameter("LoggedInUserInitials", UserSession.Initials ?? string.Empty)
            //};
            //reportViewer1.LocalReport.SetParameters(parameters);
            // =====================

            // Refresh the report
            reportViewer1.LocalReport.Refresh();
            reportViewer1.RefreshReport();

            reportViewer1.Focus();

            // Debugging: Log the selected report and number of records
            Console.WriteLine($"Selected Report: {reportPath}");
            Console.WriteLine($"Records in report: {reportData.Rows.Count}");
            Console.WriteLine($"User Initials Passed: {UserSession.Initials}");  // For debugging

        }

        private void reportViewer1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.P)
            {
                e.Handled = true;
                PrintReport();
            }
        }

        private void PrintReport()
        {
            reportViewer1.PrintDialog();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
