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
    public partial class SearchResultsForm : Form
    {
        public SearchResultsForm(DataTable searchResults)
        {
            InitializeComponent();

            // Set DataGridView to display the search results
            dataGridViewResults.DataSource = searchResults;

            // Ensure that the DataGridView can capture key events
            dataGridViewResults.KeyDown += dataGridViewResults_KeyDown;
            this.Icon = Properties.Resources.logo;
        }

        // Event handler for double-clicking a row in the DataGridView
        private void dataGridViewResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = dataGridViewResults.Rows[e.RowIndex];

                // Extract necessary data from the selected row
                string maritalstatus = selectedRow.Cells["MaritalStatus"].Value.ToString();
                string parentguardian = selectedRow.Cells["ParentGuardian"].Value.ToString();
                string patient = selectedRow.Cells["Patient"].Value.ToString();
                string hospital = selectedRow.Cells["Hospital"].Value.ToString();
                string hospitaladdress = selectedRow.Cells["HospitalAddress"].Value.ToString();
                string barangay = selectedRow.Cells["Barangay"].Value.ToString();
                DateTime certificationDate = selectedRow.Cells["CertificationDate"].Value != DBNull.Value
                    ? Convert.ToDateTime(selectedRow.Cells["CertificationDate"].Value)
                    : DateTime.MinValue;

                DateTime certificationTime = selectedRow.Cells["CertificationTime"].Value != DBNull.Value
                    ? Convert.ToDateTime(selectedRow.Cells["CertificationTime"].Value)
                    : DateTime.MinValue;

                string amountPaid = selectedRow.Cells["AmountPaid"].Value.ToString();
                string receiptNo = selectedRow.Cells["ReceiptNo"].Value.ToString();
                string receiptDateIssued = selectedRow.Cells["ReceiptDateIssued"].Value.ToString();
                string placeIssued = selectedRow.Cells["PlaceIssued"].Value.ToString();

                // Format the date and time before passing to the report
                string formattedDate = certificationDate.ToString("yyyy-MM-dd"); // Formatting Date
                string formattedTime = certificationTime.ToString("hh:mm tt"); // Formatting Time to 12-hour format

                // Pass the selected data to the report form and show it
                DataTable selectedData = GetSelectedData(maritalstatus, parentguardian, patient, hospital, hospitaladdress, barangay, formattedDate, formattedTime, amountPaid, receiptNo, receiptDateIssued, placeIssued);
                SearchResultReportForm reportForm = new SearchResultReportForm(selectedData);
                reportForm.ShowDialog();
            }
        }

        // Trigger for the Enter key to open the report form (without moving to the next row)
        private void dataGridViewResults_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the Enter key was pressed
            if (e.KeyCode == Keys.Enter)
            {
                // Prevent the default behavior of Enter key (which moves focus to the next row)
                e.Handled = true;

                // Ensure there's a selected row. Here we get the row focused (even if it's not fully selected)
                DataGridViewRow selectedRow = dataGridViewResults.CurrentRow;

                if (selectedRow != null)
                {
                    // Extract necessary data from the selected row
                    string maritalstatus = selectedRow.Cells["MaritalStatus"].Value.ToString();
                    string parentguardian = selectedRow.Cells["ParentGuardian"].Value.ToString();
                    string patient = selectedRow.Cells["Patient"].Value.ToString();
                    string hospital = selectedRow.Cells["Hospital"].Value.ToString();
                    string hospitaladdress = selectedRow.Cells["HospitalAddress"].Value.ToString();
                    string barangay = selectedRow.Cells["Barangay"].Value.ToString();
                    DateTime certificationDate = selectedRow.Cells["CertificationDate"].Value != DBNull.Value
                        ? Convert.ToDateTime(selectedRow.Cells["CertificationDate"].Value)
                        : DateTime.MinValue;

                    DateTime certificationTime = selectedRow.Cells["CertificationTime"].Value != DBNull.Value
                        ? Convert.ToDateTime(selectedRow.Cells["CertificationTime"].Value)
                        : DateTime.MinValue;

                    string amountPaid = selectedRow.Cells["AmountPaid"].Value.ToString();
                    string receiptNo = selectedRow.Cells["ReceiptNo"].Value.ToString();
                    string receiptDateIssued = selectedRow.Cells["ReceiptDateIssued"].Value.ToString();
                    string placeIssued = selectedRow.Cells["PlaceIssued"].Value.ToString();

                    // Format the date and time before passing to the report
                    string formattedDate = certificationDate.ToString("yyyy-MM-dd"); // Formatting Date
                    string formattedTime = certificationTime.ToString("hh:mm tt"); // Formatting Time to 12-hour format

                    // Pass the selected data to the report form and show it
                    DataTable selectedData = GetSelectedData(maritalstatus, parentguardian, patient, hospital, hospitaladdress, barangay, formattedDate, formattedTime, amountPaid, receiptNo, receiptDateIssued, placeIssued);
                    SearchResultReportForm reportForm = new SearchResultReportForm(selectedData);
                    reportForm.ShowDialog();
                }
            }
        }

        // Method to get selected data (already explained earlier)
        private DataTable GetSelectedData(string maritalstatus, string parentguardian, string patient, string hospital, string hospitaladdress, string barangay, string certificationDate, string certificationTime, string amountPaid, string receiptNo, string receiptDateIssued, string placeIssued)
        {
            DataTable selectedData = new DataTable();
            selectedData.Columns.Add("MaritalStatus");
            selectedData.Columns.Add("ParentGuardian");
            selectedData.Columns.Add("Patient");
            selectedData.Columns.Add("Hospital");
            selectedData.Columns.Add("HospitalAddress");
            selectedData.Columns.Add("Barangay");
            selectedData.Columns.Add("CertificationDate");
            selectedData.Columns.Add("CertificationTime");
            selectedData.Columns.Add("AmountPaid");
            selectedData.Columns.Add("ReceiptNo");
            selectedData.Columns.Add("ReceiptDateIssued");
            selectedData.Columns.Add("PlaceIssued");

            // Add the selected row data into the DataTable
            selectedData.Rows.Add(maritalstatus, parentguardian, patient, hospital, hospitaladdress, barangay, certificationDate, certificationTime, amountPaid, receiptNo, receiptDateIssued, placeIssued);

            return selectedData;
        }
    }
}
