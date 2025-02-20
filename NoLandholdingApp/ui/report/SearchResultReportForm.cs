using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Drawing.Printing;
using System.Drawing;
using System.IO;
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

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(reportViewer1_KeyDown);
        }

        private void SearchResultReportForm_Load(object sender, EventArgs e)
        {
            reportViewer1.ProcessingMode = ProcessingMode.Local;
            reportViewer1.ZoomMode = ZoomMode.Percent;
            reportViewer1.ZoomPercent = 100;
            reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);

            if (reportData == null || reportData.Rows.Count == 0)
            {
                MessageBox.Show("No data available for the report.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string reportPath = "NoLandholdingApp.rdlc.CertificationReport.rdlc";  // Default report

            if (reportType == "Scholarship")
            {
                reportPath = "NoLandholdingApp.rdlc.CertificationReport-Scholarship.rdlc";
            }
            else if (reportType == "Hospitalization")
            {
                reportPath = "NoLandholdingApp.rdlc.CertificationReport.rdlc";
            }

            reportViewer1.LocalReport.ReportEmbeddedResource = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("nolandholding_dataset", reportData));
            reportViewer1.LocalReport.Refresh();
            reportViewer1.RefreshReport();
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
    }
}
