using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Windows.Forms;

namespace NoLandholdingApp
{
    public partial class SearchResultReportForm : Form
    {
        private DataTable reportData;
        private string reportType;

        public SearchResultReportForm(DataTable data, string type)
        {
            InitializeComponent();
            this.reportData = data;
            this.reportType = type;
            this.Icon = Properties.Resources.logo;
        }

        private void SearchResultReportForm_Load(object sender, EventArgs e)
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

            // Conditionally set the RDLC report path based on reportType (typeset)
            string reportPath = "NoLandholdingApp.CertificationReport.rdlc";  // Default report

            if (reportType == "Scholarship") // Example: If type is "School", change report
            {
                reportPath = "NoLandholdingApp.CertificationReport-Scholarship.rdlc"; // Update to your school report
            }
            else if (reportType == "Hospitalization") // Example: If type is "Hospital", change report
            {
                reportPath = "NoLandholdingApp.CertificationReport.rdlc"; // Update to your hospital report
            }
            // Add more conditions for other report types if needed

            // Set the appropriate RDLC report path
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
