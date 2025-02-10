using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MySql.Data.MySqlClient;
using Microsoft.Reporting.WebForms;
using Microsoft.Reporting.WinForms;

namespace NoLandholdingApp
{
    public partial class formInput : Form
    {
        private PrintDocument printDocument;
        private int currentPage;
        private float scaleFactor;

        // Set default value on form load
        private void Form_Load(object sender, EventArgs e)
        {
            // Set the "Single" checkbox as checked by default
            checkBoxSingle.Checked = true;
        }

        // Handle the Married checkbox change
        private void checkBoxMarried_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMarried.Checked)
            {
                // Uncheck the "Single" checkbox if "Married" is checked
                checkBoxSingle.Checked = false;
            }
            else
            {
                // Keep "Single" checked if "Married" is unchecked
                checkBoxSingle.Checked = true;
            }
        }

        // Handle the Single checkbox change
        private void checkBoxSingle_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSingle.Checked)
            {
                // Uncheck the "Married" checkbox if "Single" is checked
                checkBoxMarried.Checked = false;
            }
        }

        private DataTable GetCertificationData()
        {
            DataTable dt = new DataTable();
            string connectionString = "Server=localhost;Database=CertificationDB;Uid=root;Pwd=;";

            // Fetch only the latest record based on the highest ID (newest entry)
            string query = "SELECT MaritalStatus, ParentGuardian, ParentGuardian2, Barangay, Patient, Hospital, HospitalAddress, CertificationDate, CertificationTime " +
                           "FROM certificationrecords_nolandholding " +
                           "ORDER BY ID DESC LIMIT 1";  // Order by ID to get the latest

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching data: " + ex.Message);
            }

            return dt;
        }


        public formInput()
        {
            InitializeComponent();
            txtSearch.KeyDown += txtSearch_KeyDown;

            // Set CharacterCasing for all TextBox controls
            txtParentGuardian.CharacterCasing = CharacterCasing.Upper;
            txtParentGuardian2.CharacterCasing = CharacterCasing.Upper;
            txtAddress.CharacterCasing = CharacterCasing.Upper;
            txtPatientStudent.CharacterCasing = CharacterCasing.Upper;
            txtHospital.CharacterCasing = CharacterCasing.Upper;
            txtHospitalAddress.CharacterCasing = CharacterCasing.Upper;

            txtOthersEditor.Visible = false;

            // Initialize PrintDocument
            printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;

            currentPage = 1;
            scaleFactor = 1.0f; // Default scale factor

            // Attach event handlers to ensure only one checkbox is checked at a time
            checkBoxSingle.CheckedChanged += checkBoxSingle_CheckedChanged;
            checkBoxMarried.CheckedChanged += checkBoxMarried_CheckedChanged;
        }


        private void btnPrintSave_Click(object sender, EventArgs e)
        {
            // Collect form data
            string maritalStatus = checkBoxSingle.Checked ? "" : "SPOUSES";
            string parentGuardian = txtParentGuardian.Text;
            string parentGuardian2 = txtParentGuardian2.Text;
            string barangay = txtAddress.Text;
            string patient = txtPatientStudent.Text;
            string hospital = txtHospital.Text;
            string hospitalAddress = txtHospitalAddress.Text;
            DateTime certificationDate = DateTime.Now;
            TimeSpan certificationTime = DateTime.Now.TimeOfDay;  // Use TimeSpan for only the time part

            // Combine parent guardians with "AND" if both are filled, otherwise use the non-empty one
            string combinedParentGuardian = string.Empty;

            if (!string.IsNullOrEmpty(parentGuardian) && !string.IsNullOrEmpty(parentGuardian2))
            {
                // Both have data, so concatenate with "AND"
                combinedParentGuardian = parentGuardian + " AND " + parentGuardian2;
            }
            else
            {
                // Use whichever has data, if any
                combinedParentGuardian = !string.IsNullOrEmpty(parentGuardian) ? parentGuardian : parentGuardian2;
            }

            // MySQL connection string
            string connectionString = "Server=localhost;Database=CertificationDB;Uid=root;Pwd=;";

            // Insert query
            string query = "INSERT INTO certificationrecords_nolandholding (MaritalStatus, ParentGuardian, ParentGuardian2, Barangay, Patient, Hospital, HospitalAddress, CertificationDate, CertificationTime) " +
                           "VALUES (@MaritalStatus, @ParentGuardian, @ParentGuardian2, @Barangay, @Patient, @Hospital, @HospitalAddress, @CertificationDate, @CertificationTime)";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Use the combined ParentGuardian value
                        cmd.Parameters.AddWithValue("@MaritalStatus", maritalStatus);
                        cmd.Parameters.AddWithValue("@ParentGuardian", combinedParentGuardian + " ");
                        cmd.Parameters.AddWithValue("@ParentGuardian2", string.Empty);  // We don't need this anymore
                        cmd.Parameters.AddWithValue("@Barangay", barangay);
                        cmd.Parameters.AddWithValue("@Patient", patient);
                        cmd.Parameters.AddWithValue("@Hospital", hospital);
                        cmd.Parameters.AddWithValue("@HospitalAddress", hospitalAddress);
                        cmd.Parameters.AddWithValue("@CertificationDate", certificationDate);
                        cmd.Parameters.AddWithValue("@CertificationTime", certificationTime);  // Add the TimeSpan parameter directly

                        cmd.ExecuteNonQuery();
                    }
                }

                // Show success message
                MessageBox.Show("Data Saved Successfully");

                // Fetch only the latest saved record for printing
                DataTable reportData = GetCertificationData();  // This method must return the latest entry

                if (reportData.Rows.Count == 0)
                {
                    MessageBox.Show("No records found for printing.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Open the Report Form with the latest record
                ReportForm reportForm = new ReportForm(reportData);
                reportForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioOthers_CheckedChanged(object sender, EventArgs e)
        {
            txtOthersEditor.Visible = radioOthers.Checked;
            txtOthersEditor.Height = 300;
            txtOthersEditor.Multiline = true;
            txtOthersEditor.ScrollBars = ScrollBars.Vertical;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDocument;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            //// Get the values from the form controls
            //string maritalStatus = "";
            //if (checkBoxSingle.Checked)
            //    maritalStatus = "";
            //else if (checkBoxMarried.Checked)
            //    maritalStatus = "SPOUSES";    

            //string parentGuardian = txtParentGuardian.Text;
            //string parentGuardian2 = txtParentGuardian2.Text;
            //string barangay = txtAddress.Text;
            //string patient = txtPatientStudent.Text;
            //string hospital = txtHospital.Text;
            //string hospitalAddress = txtHospitalAddress.Text;

            //// Conditionally add "AND" only when both parentGuardian and parentGuardian2 have text
            //string parentString = $"{parentGuardian}";
            //if (!string.IsNullOrEmpty(parentGuardian) && !string.IsNullOrEmpty(parentGuardian2))
            //{
            //    parentString += " AND " + parentGuardian2 + " "; // Add "AND" if both have values
            //}
            //else
            //{
            //    parentString += parentGuardian2; // Just append the second parentGuardian if only one has a value
            //}

            //// Static content of the certificate
            //string header = "Republic of the Philippines\nPROVINCE OF BUKIDNON\nMunicipality of Kitaotao";
            //string pageHeader = "OFFICE OF THE MUNICIPAL ASSESSOR";
            //string pageHeader2 = "CERTIFICATION";
            //string line1Body = "TO WHOM IT MAY CONCERN:";
            //string body = "THIS IS TO CERTIFY that {maritalStatus} {parentGuardian}of Barangay {barangay} " +
            //  "this municipality has no declared real property ownership such as building, land, and machinery as far as our record is concerned.\n\n" +
            //  "This CERTIFICATION is being done to facilitate their request for financial/medical assistance of " +
            //  "{patient} (PATIENT) at {hospital} – {hospitalAddress}.\n" +
            //  "Done and Given this 22nd day of February, 2024 at Office of the Municipal Assessor, Poblacion, Kitaotao, Bukidnon.\n\n" +
            //  "ROEL H. MOLO, MMREM, REA REB, LPT\n" +
            //  "Municipal Assessor\n\n" +
            //  "Amount Paid : 0.00\n" +
            //  "Receipt No. :\n" +
            //  "Date Issued :\n" +
            //  "Place Issued :\n" +
            //  "Prepared By : JRC 02/06/2025 @ 03:46:06 PM\n\n" +
            //  "Note: This certification is not valid if it has a mark of erasure or alteration.";

            //// Define the fonts
            //Font headerFont = new Font("Bookman Old Style", 14);
            //Font pageHeaderFont = new Font("Berlin Sans FB", 14);
            //Font spaceHeaderFont = new Font("Berlin Sans FB", 8);
            //Font pageHeader2Font = new Font("Bernard MT Condensed", 20);
            //Font bodyFont = new Font("Bookman Old Style", 12);
            //Font line1BodyFont = new Font("Bookman Old Style", 12);
            //Font boldFont = new Font("Bookman Old Style", 12, FontStyle.Bold);

            //// Prepare the body with the actual values inserted
            //body = body.Replace("{maritalStatus}", maritalStatus)
            //           .Replace("{parentGuardian}", parentString) // Use the updated parentString with conditional "AND"
            //           .Replace("{barangay}", barangay)
            //           .Replace("{patient}", patient)
            //           .Replace("{hospital}", hospital)
            //           .Replace("{hospitalAddress}", hospitalAddress);

            //// Set starting position for header text
            //float topMargin = e.MarginBounds.Top;
            //float leftMargin = e.MarginBounds.Left;
            //float currentY = topMargin;

            //// Split the body into lines by newlines for easier printing
            //string[] bodyLines = body.Split(new[] { '\n' }, StringSplitOptions.None);

            //// Loop through each line of the header
            //string[] headerLines = header.Split(new[] { '\n' }, StringSplitOptions.None);
            //foreach (string line in headerLines)
            //{
            //    float lineWidth = e.Graphics.MeasureString(line, headerFont).Width;
            //    float centerX = (e.PageBounds.Width - lineWidth) / 2;
            //    e.Graphics.DrawString(line, headerFont, Brushes.Black, centerX, currentY);
            //    currentY += headerFont.GetHeight(e.Graphics);
            //}

            //// Draw a line below the header
            //float headerBottomY = currentY + 10;
            //e.Graphics.DrawLine(Pens.Black, leftMargin, headerBottomY, e.PageBounds.Width - leftMargin, headerBottomY);
            //currentY = headerBottomY + 10;

            //// Draw page header
            //string[] pageHeaderLines = pageHeader.Split(new[] { '\n' }, StringSplitOptions.None);
            //foreach (string line in pageHeaderLines)
            //{
            //    float lineWidth = e.Graphics.MeasureString(line, pageHeaderFont).Width;
            //    float centerX = (e.PageBounds.Width - lineWidth) / 2;
            //    e.Graphics.DrawString(line, pageHeaderFont, Brushes.Black, centerX, currentY);
            //    currentY += pageHeaderFont.GetHeight(e.Graphics) + 20;
            //}

            //// Draw pageHeader2 centered
            //float pageHeader2Width = e.Graphics.MeasureString(pageHeader2, pageHeader2Font).Width;
            //float pageHeader2X = (e.PageBounds.Width - pageHeader2Width) / 2;
            //e.Graphics.DrawString(pageHeader2, pageHeader2Font, Brushes.Black, pageHeader2X, currentY);
            //currentY += pageHeader2Font.GetHeight(e.Graphics) + 30; // Adjust for spacing

            //// Draw first line body centered
            //float line1BodyWidth = e.Graphics.MeasureString(line1Body, pageHeader2Font).Width;
            //float line1BodyX = leftMargin;
            //e.Graphics.DrawString(line1Body, line1BodyFont, Brushes.Black, line1BodyX, currentY);
            //currentY += line1BodyFont.GetHeight(e.Graphics) + 15; // Adjust for spacing

            //// Set the max width of the page, subtracting margins
            //float maxLineWidth = e.PageBounds.Width - leftMargin - 10; // Add a small margin for safety

            //bool isFirstLine = true; // Track if first line
            //float rightMargin = 50f;
            //float maxTextWidth = maxLineWidth - rightMargin; // Ensure right margin consistency

            //foreach (string line in bodyLines)
            //{
            //    float justifiedX = isFirstLine ? leftMargin + 50f : leftMargin;
            //    float adjustedMaxTextWidth = isFirstLine ? maxTextWidth - 50f : maxTextWidth;
            //    float lineHeight = bodyFont.GetHeight(e.Graphics);
            //    int index = 0;

            //    List<(string word, Font font)> words = new List<(string, Font)>();

            //    while (index < line.Length)
            //    {
            //        string nextWord = "";
            //        Font currentFont = bodyFont;

            //        // Check for bold words, treating them as individual units for spacing
            //        if (!string.IsNullOrEmpty(parentGuardian) &&
            //            line.IndexOf(parentGuardian, index, StringComparison.OrdinalIgnoreCase) == index)
            //        {
            //            nextWord = parentGuardian.Trim();
            //            currentFont = boldFont;
            //        }
            //        else if (!string.IsNullOrEmpty(parentGuardian2) &&
            //                 line.IndexOf(parentGuardian2, index, StringComparison.OrdinalIgnoreCase) == index)
            //        {
            //            nextWord = parentGuardian2.Trim();
            //            currentFont = boldFont;
            //        }
            //        else if (!string.IsNullOrEmpty(patient) &&
            //                 line.IndexOf(patient, index, StringComparison.OrdinalIgnoreCase) == index)
            //        {
            //            nextWord = patient.Trim();
            //            currentFont = boldFont;
            //        }
            //        else if (!string.IsNullOrEmpty(hospital) &&
            //                 line.IndexOf(hospital, index, StringComparison.OrdinalIgnoreCase) == index)
            //        {
            //            nextWord = hospital.Trim();
            //            currentFont = boldFont;
            //        }
            //        else if (!string.IsNullOrEmpty(hospitalAddress) &&
            //                 line.IndexOf(hospitalAddress, index, StringComparison.OrdinalIgnoreCase) == index)
            //        {
            //            nextWord = hospitalAddress.Trim();
            //            currentFont = boldFont;
            //        }
            //        else if (!string.IsNullOrEmpty(barangay) &&
            //                 line.IndexOf(barangay, index, StringComparison.OrdinalIgnoreCase) == index)
            //        {
            //            nextWord = barangay.Trim();
            //            currentFont = boldFont;
            //        }
            //        else if (line.IndexOf("AND", index, StringComparison.Ordinal) == index) // Case-sensitive
            //        {
            //            nextWord = "AND";
            //            currentFont = boldFont;
            //        }
            //        else
            //        {
            //            // Extract normal words
            //            int nextSpace = line.IndexOf(' ', index);
            //            if (nextSpace == -1) nextSpace = line.Length;
            //            nextWord = line.Substring(index, nextSpace - index).Trim(); // Ensure clean spacing
            //        }

            //        words.Add((nextWord, currentFont));
            //        index += nextWord.Length + 1; // Move index forward
            //    }

            //    // Justify each line
            //    List<(string word, Font font)> currentLineWords = new List<(string, Font)>();
            //    float currentLineWidth = 0;

            //    foreach (var (word, font) in words)
            //    {
            //        float wordWidth = e.Graphics.MeasureString(word, font).Width;

            //        if (justifiedX + currentLineWidth + wordWidth > adjustedMaxTextWidth)
            //        {
            //            if (currentLineWords.Count > 1)
            //            {
            //                float totalSpacing = adjustedMaxTextWidth - currentLineWidth;
            //                float extraSpace = totalSpacing / (currentLineWords.Count - 1);

            //                float justifyPosX = justifiedX;
            //                foreach (var (justifiedWord, justifiedFont) in currentLineWords)
            //                {
            //                    e.Graphics.DrawString(justifiedWord, justifiedFont, Brushes.Black, justifyPosX, currentY);
            //                    justifyPosX += e.Graphics.MeasureString(justifiedWord, justifiedFont).Width + extraSpace;
            //                }
            //            }
            //            else
            //            {
            //                e.Graphics.DrawString(currentLineWords[0].word, currentLineWords[0].font, Brushes.Black, justifiedX, currentY);
            //            }

            //            currentY += lineHeight;
            //            justifiedX = leftMargin;
            //            isFirstLine = false;
            //            adjustedMaxTextWidth = maxTextWidth;

            //            currentLineWords.Clear();
            //            currentLineWidth = 0;
            //        }

            //        currentLineWords.Add((word, font));
            //        currentLineWidth += wordWidth;
            //    }

            //    // Draw last line
            //    float lastLineX = justifiedX;
            //    foreach (var (lastWord, lastFont) in currentLineWords)
            //    {
            //        e.Graphics.DrawString(lastWord, lastFont, Brushes.Black, lastLineX, currentY);
            //        lastLineX += e.Graphics.MeasureString(lastWord, lastFont).Width + 5f;
            //    }

            //    currentY += lineHeight;
            //    isFirstLine = false;
            //}
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            PrintPreviewDialog previewDialog = new PrintPreviewDialog
            {
                Document = printDocument,
                Width = 800,   // Set preview window width
                Height = 600   // Set preview window height
            };

            previewDialog.ShowDialog(); // Show the print preview window
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            // Increase the scaling factor for zooming in
            scaleFactor += 0.1f;
            printDocument.DefaultPageSettings.Landscape = true;
            printDocument.Print(); // Re-render the document with the new scale
        }

        private void btnZoomOut_CLick(object sender, EventArgs e)
        {
            // Decrease the scaling factor for zooming out
            scaleFactor -= 0.1f;
            printDocument.DefaultPageSettings.Landscape = false;
            printDocument.Print(); // Re-render the document with the new scale
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtSearch.Focused)
            {
                btnSearch_Click(sender, e );
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Get the search term entered by the user
            string searchTerm = txtSearch.Text.Trim();

            // Fetch the filtered data based on the search term
            DataTable reportData = GetReportData(searchTerm);

            // Check if any results were found
            if (reportData.Rows.Count > 0)
            {
                // Show the search results in a new form
                SearchResultsForm searchResultsForm = new SearchResultsForm(reportData);
                searchResultsForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No results found.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private DataTable GetReportData(string searchTerm)
        {
            // Create a DataTable to hold the results
            DataTable dt = new DataTable();

            // Add columns to the DataTable
            dt.Columns.Add("Patient");
            dt.Columns.Add("Hospital");
            dt.Columns.Add("Barangay");
            dt.Columns.Add("CertificationDate");
            dt.Columns.Add("CertificationTime");

            // MySQL connection string
            string connectionString = "Server=localhost;Database=CertificationDB;Uid=root;Pwd=;";

            // SQL query to fetch data based on the search term
            string query = "SELECT Patient, Hospital, Barangay, CertificationDate, CertificationTime FROM certificationrecords_nolandholding " +
                           "WHERE Patient LIKE @SearchTerm";

            try
            {
                // Create a connection to the database
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    // Open the connection
                    conn.Open();

                    // Create a command to execute the query
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Add the search term parameter to the query
                        cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                        // Execute the query and fetch data
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Load data into the DataTable
                            dt.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            // Return the populated DataTable
            return dt;
        }

    }
}
