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
using TextBox = System.Windows.Forms.TextBox;
using Button = System.Windows.Forms.Button;
using System.Globalization;
using System.Windows.Forms.VisualStyles;
using System.Security.Cryptography;

namespace NoLandholdingApp
{
    public partial class formInput : Form
    {
        private PrintDocument printDocument; private int currentPage; private float scaleFactor;

        // Set default value on form load
        private void Form_Load(object sender, EventArgs e)
        {
            // Set the "Single" checkbox as checked by default
            checkBoxSingle.Checked = true;

        }

        // KeyPress event to allow user to type a date
        private void dateTimePickerDateIssued_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits, backspace, and slash
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '/' && e.KeyChar != '\b')
            {
                e.Handled = true;  // Ignore the key press if it's not a valid character
            }
        }

        // Optional: Validate the input date when text changes
        private void DateTimePickerDateIssued_TextChanged(object sender, EventArgs e)
        {
            string input = dateTimePickerDateIssued.Text;

            if (DateTime.TryParse(input, out DateTime validDate))
            {
                dateTimePickerDateIssued.Value = validDate;  // Update the DateTimePicker value
            }
            else
            {
                // Optional: Show a message if the input is invalid
                // For example, display an error message or set the date back to default
                dateTimePickerDateIssued.Value = DateTime.Now;  // Reset to current date or a default value
            }
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
            // Load database configuration
            var config = ConfigHelper.LoadConfig();
            string connectionString = ""; // Declare outside

            if (config.Count > 0)
            {
                connectionString = $"Server={config["Server"]};Database={config["Database"]};Uid={config["User"]};Pwd={config["Password"]};";
            }

            // Fetch only the latest record based on the highest ID (newest entry)
            string query = "SELECT MaritalStatus, ParentGuardian, ParentGuardian2, Barangay, Patient, Hospital, HospitalAddress, CertificationDate, CertificationTime, AmountPaid, ReceiptNo, ReceiptDateIssued, PlaceIssued, Type " +
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

        private DataGridView dataGridViewResults;

        public formInput()
        {
            InitializeComponent();

            SetFormProperties();
            SetUpPanel();
            SetTextBoxCharacterCasing();
            InitializePrintDocument();
            AttachEventHandlers();


            this.KeyPreview = true;
            // Subscribe to KeyDown event
            this.KeyDown += new KeyEventHandler(DatabaseReload_KeyDown);

            txtSearch.BringToFront();

            LoadDatabase();
        }

        private void SetFormProperties()
        {
            this.BackColor = ColorTranslator.FromHtml("#e8e8e2");
            this.Icon = Properties.Resources.logo;
        }

        private void SetUpPanel()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 650,
                Margin = new Padding(20),
                Padding = new Padding(40, 0, 40, 74)
            };

            // Add the DataGridView to the Panel
            SetUpDataGridView();
            panel.Controls.Add(dataGridViewResults);
            // Add the Panel to the form
            this.Controls.Add(panel);
        }

        private void dataGridViewResults_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
        }

        private void dataGridViewResults_MouseDown(object sender, MouseEventArgs e)
        {
            dataGridViewResults.ClearSelection();
        }

        private void SetUpDataGridView()
        {
            // Create and set up the DataGridView
            dataGridViewResults = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                Font = new Font("Tahoma", 8),
                EnableHeadersVisualStyles = false,
                AllowUserToAddRows = false,
                AllowDrop = false,
                AllowUserToOrderColumns = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AllowUserToResizeColumns = false,
                ContextMenuStrip = new ContextMenuStrip(),
                GridColor = ColorTranslator.FromHtml("#CCCCCC"),
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersDefaultCellStyle =
            {
                SelectionBackColor = ColorTranslator.FromHtml("#F0F0F0"), // Prevent header highlight
                BackColor = ColorTranslator.FromHtml("#F0F0F0") // Set a consistent header color
            }
            };

            // Set alternating row colors and default row colors
            dataGridViewResults.ReadOnly = true;
            // Set alternating row colors
            dataGridViewResults.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(235, 240, 244); // #EBF0F4
            dataGridViewResults.RowsDefaultCellStyle.BackColor = Color.White;

            // Enable full-row selection
            dataGridViewResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Define default selection colors (for entire rows)
            dataGridViewResults.DefaultCellStyle.SelectionBackColor = Color.DodgerBlue; // Default blue highlight
            dataGridViewResults.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridViewResults.RowHeadersDefaultCellStyle.SelectionBackColor = Color.DodgerBlue;
            dataGridViewResults.RowHeadersDefaultCellStyle.SelectionForeColor = Color.Tan; // Ensure the text is visible
            dataGridViewResults.RowTemplate.Height = 19;

            // Attach events
            dataGridViewResults.CellMouseDown += dataGridViewResults_CellMouseDown;
            dataGridViewResults.CellFormatting += dataGridViewResults_CellFormatting;
            dataGridViewResults.DragOver += dataGridViewResults_DragOver;
            dataGridViewResults.MouseDown += dataGridViewResults_MouseDown;

            // Set up appearance properties
            dataGridViewResults.CellBorderStyle = DataGridViewCellBorderStyle.None;
            //dataGridViewResults.RowHeadersVisible = false;
            dataGridViewResults.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            dataGridViewResults.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // Set column headers style
            dataGridViewResults.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#F0F0F0");


            // Enable double-buffering
            dataGridViewResults.GetType().InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.SetProperty,
                null, dataGridViewResults, new object[] { true });

            // Column separator custom painting
            dataGridViewResults.CellPainting += (sender, e) =>
            {
                if (e.RowIndex == -1) // Header row
                {
                    e.PaintBackground(e.CellBounds, true);

                    // Draw the right separator for column headers (already existing)
                    int separatorTop = e.CellBounds.Top + 4;
                    int separatorBottom = e.CellBounds.Bottom - 4;

                    using (Pen pen = new Pen(ColorTranslator.FromHtml("#cccccc"), 1))
                    {
                        e.Graphics.DrawLine(pen, e.CellBounds.Right - 1, separatorTop, e.CellBounds.Right - 1, separatorBottom);
                    }

                    // Draw the bottom border for the column header
                    int bottomBorderY = e.CellBounds.Bottom - 1;  // Position just below the header
                    using (Pen bottomPen = new Pen(ColorTranslator.FromHtml("#cccccc"), 1)) // Customize color here
                    {
                        e.Graphics.DrawLine(bottomPen, e.CellBounds.Left, bottomBorderY, e.CellBounds.Right, bottomBorderY);
                    }

                    e.PaintContent(e.CellBounds);
                    e.Handled = true;
                }
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // Ensure it's a valid cell
                {
                    // Default painting
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                    // If it's the last clicked cell, draw a broken (dashed) outline
                    if (e.RowIndex == _lastClickedRow && e.ColumnIndex == _lastClickedCol)
                    {
                        using (Pen dashedPen = new Pen(Color.FromArgb(150, 0, 0, 0), 1))
                        {
                            dashedPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash; // Broken outline
                            e.Graphics.DrawRectangle(dashedPen, e.CellBounds.X, e.CellBounds.Y,
                                e.CellBounds.Width - 1, e.CellBounds.Height - 1);
                        }
                    }

                    e.Handled = true; // Mark the event as handled
                }
            };

            // Persistent border for the DataGridView
            dataGridViewResults.Paint += (sender, e) =>
            {
                using (Pen borderPen = new Pen(ColorTranslator.FromHtml("#cccccc"), 1))
                {
                    e.Graphics.DrawRectangle(borderPen, 0, 0, dataGridViewResults.Width - 1, dataGridViewResults.Height - 1);
                }
            };

            // Handle resizing events
            dataGridViewResults.Resize += (sender, e) => dataGridViewResults.Invalidate();
        }

        private void ResizeDataGridViewColumns()
        {
            if (dataGridViewResults.Columns.Count == 0) return;

            dataGridViewResults.Columns["Patient"].Width = 200;
            dataGridViewResults.Columns["Marital Status"].Width = 100;
            dataGridViewResults.Columns["Parent / Guardian"].Width = 250;
            dataGridViewResults.Columns["Hospital"].Width = 250;
            dataGridViewResults.Columns["Hospital Address"].Width = 250;
            dataGridViewResults.Columns["Barangay"].Width = 250;
            dataGridViewResults.Columns["Certification Date"].Width = 100;
            dataGridViewResults.Columns["Certification Time"].Width = 100;
            dataGridViewResults.Columns["Amount Paid"].Width = 80;
            dataGridViewResults.Columns["Receipt No"].Width = 100;
            dataGridViewResults.Columns["Receipt Date Issued"].Width = 120;
            dataGridViewResults.Columns["Place Issued"].Width = 140;
            dataGridViewResults.Columns["Type"].Width = 150;
        }



        private void SetTextBoxCharacterCasing()
        {
            // Set CharacterCasing for all TextBox controls
            txtParentGuardian.CharacterCasing = CharacterCasing.Upper;
            txtParentGuardian2.CharacterCasing = CharacterCasing.Upper;
            txtAddress.CharacterCasing = CharacterCasing.Upper;
            txtPatientStudent.CharacterCasing = CharacterCasing.Upper;
            txtHospital.CharacterCasing = CharacterCasing.Upper;
            txtHospitalAddress.CharacterCasing = CharacterCasing.Upper;
            txtAmountPaid.Text = "₱0.00";
        }

        private void InitializePrintDocument()
        {
            printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;
            currentPage = 1;
            scaleFactor = 1.0f; // Default scale factor
        }

        private void AttachEventHandlers()
        {
            // Attach event handlers for form controls
            checkBoxSingle.CheckedChanged += checkBoxSingle_CheckedChanged;
            checkBoxMarried.CheckedChanged += checkBoxMarried_CheckedChanged;
            SearchBox();
            txtSearch.KeyDown += SearchBox_KeyDown;
            txtAmountPaid.Enter += txtAmountPaid_Enter;
            txtAmountPaid.KeyPress += txtAmountPaid_KeyPress;
            txtAmountPaid.Leave += txtAmountPaid_Leave;
            dataGridViewResults.KeyDown += dataGridViewResults_KeyDown;
            dataGridViewResults.CellDoubleClick += dataGridViewResults_CellDoubleClick;
            dataGridViewResults.DragOver += dataGridViewResults_DragOver;
            dataGridViewResults.MouseDown += dataGridViewResults_MouseDown;
            comboBoxTypeList.DropDownStyle = ComboBoxStyle.DropDownList;
            LoadComboBoxItemsFromMySQL();
        }

        private int _lastClickedRow = -1;
        private int _lastClickedCol = -1;

        private void dataGridViewResults_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.Button == MouseButtons.Left)
            {
                _lastClickedRow = e.RowIndex;
                _lastClickedCol = e.ColumnIndex;
                dataGridViewResults.Invalidate(); // Refresh to apply formatting
            }
        }

        private void dataGridViewResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            // Apply alternating row colors
            if (e.RowIndex % 2 == 0)
            {
                e.CellStyle.BackColor = Color.White;
            }
            else
            {
                e.CellStyle.BackColor = Color.FromArgb(235, 240, 244); // #EBF0F4
            }

            // Check if this is the last clicked cell
            if (e.RowIndex == _lastClickedRow && e.ColumnIndex == _lastClickedCol)
            {
                // Override both BackColor and SelectionBackColor for the clicked cell
                e.CellStyle.BackColor = Color.LightYellow;
                e.CellStyle.SelectionBackColor = Color.LightYellow;

                // Set text color to black to keep it readable
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.SelectionForeColor = Color.Black;  // Ensure selection keeps black text
            }
            else
            {
                e.CellStyle.ForeColor = Color.Black;
            }
        }

        public void LoadDatabase()
        {

            // Call the method to fetch data when the form loads
            string searchTerm = ""; // Empty or default search term
            DataTable reportData = GetReportData(searchTerm);

            // Bind the fetched data to the DataGridView
            dataGridViewResults.DataSource = reportData;
            ResizeDataGridViewColumns();
        }

        private void LoadDataFromDatabase()
        {
            // Fetch the filtered data (you can use an empty search term or adjust as needed)
            string searchTerm = string.Empty; // Or provide a specific search term if required
            DataTable reportData = GetReportData(searchTerm);

            // Check if any results were found
            if (reportData.Rows.Count > 0)
            {
                // Set the DataGridView's data source to display the results
                dataGridViewResults.DataSource = reportData;
            }
            else
            {
                MessageBox.Show("No results found.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSearch.Focus();
            }
        }


        // Event handler for double-clicking a row in the DataGridView
        private void dataGridViewResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = ((DataGridView)sender).Rows[e.RowIndex];

                // Extract necessary data from the selected row
                string maritalstatus = selectedRow.Cells["Marital Status"].Value.ToString();
                string parentguardian = selectedRow.Cells["Parent / Guardian"].Value.ToString();
                string patient = selectedRow.Cells["Patient"].Value.ToString();
                string hospital = selectedRow.Cells["Hospital"].Value.ToString();
                string hospitaladdress = selectedRow.Cells["Hospital Address"].Value.ToString();
                string barangay = selectedRow.Cells["Barangay"].Value.ToString();
                DateTime certificationDate = selectedRow.Cells["Certification Date"].Value != DBNull.Value
                    ? Convert.ToDateTime(selectedRow.Cells["Certification Date"].Value)
                    : DateTime.MinValue;

                DateTime certificationTime = selectedRow.Cells["Certification Time"].Value != DBNull.Value
                    ? Convert.ToDateTime(selectedRow.Cells["Certification Time"].Value)
                    : DateTime.MinValue;

                string amountPaid = selectedRow.Cells["Amount Paid"].Value.ToString();
                string receiptNo = selectedRow.Cells["Receipt No"].Value.ToString();
                string receiptDateIssued = selectedRow.Cells["Receipt Date Issued"].Value.ToString();
                string placeIssued = selectedRow.Cells["Place Issued"].Value.ToString();

                // Format the date and time before passing to the report
                string formattedDate = certificationDate.ToString("MM-dd-yyyy"); // Formatting Date
                string formattedTime = certificationTime.ToString("hh:mm:ss tt"); // Formatting Time to 12-hour format
                string typeset = selectedRow.Cells["Type"].Value.ToString();

                // Pass the selected data to the report form and show it
                DataTable selectedData = GetSelectedData(maritalstatus, parentguardian, patient, hospital, hospitaladdress, barangay, formattedDate, formattedTime, amountPaid, receiptNo, receiptDateIssued, placeIssued, typeset);
                SearchResultReportForm reportForm = new SearchResultReportForm(selectedData, typeset);
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
                DataGridViewRow selectedRow = ((DataGridView)sender).CurrentRow;

                if (selectedRow != null)
                {
                    // Extract necessary data from the selected row
                    string maritalstatus = selectedRow.Cells["Marital Status"].Value.ToString();
                    string parentguardian = selectedRow.Cells["Parent / Guardian"].Value.ToString();
                    string patient = selectedRow.Cells["Patient"].Value.ToString();
                    string hospital = selectedRow.Cells["Hospital"].Value.ToString();
                    string hospitaladdress = selectedRow.Cells["Hospital Address"].Value.ToString();
                    string barangay = selectedRow.Cells["Barangay"].Value.ToString();
                    DateTime certificationDate = selectedRow.Cells["Certification Date"].Value != DBNull.Value
                        ? Convert.ToDateTime(selectedRow.Cells["Certification Date"].Value)
                        : DateTime.MinValue;

                    DateTime certificationTime = selectedRow.Cells["Certification Time"].Value != DBNull.Value
                        ? Convert.ToDateTime(selectedRow.Cells["Certification Time"].Value)
                        : DateTime.MinValue;

                    string amountPaid = selectedRow.Cells["Amount Paid"].Value.ToString();
                    string receiptNo = selectedRow.Cells["Receipt No"].Value.ToString();
                    string receiptDateIssued = selectedRow.Cells["Receipt Date Issued"].Value.ToString();
                    string placeIssued = selectedRow.Cells["Place Issued"].Value.ToString();

                    // Format the date and time before passing to the report
                    string formattedDate = certificationDate.ToString("MM-dd-yyyy"); // Formatting Date
                    string formattedTime = certificationTime.ToString("hh:mm:ss tt"); // Formatting Time to 12-hour format
                    string typeset = selectedRow.Cells["Type"].Value.ToString();

                    // Pass the selected data to the report form and show it
                    DataTable selectedData = GetSelectedData(maritalstatus, parentguardian, patient, hospital, hospitaladdress, barangay, formattedDate, formattedTime, amountPaid, receiptNo, receiptDateIssued, placeIssued, typeset);
                    SearchResultReportForm reportForm = new SearchResultReportForm(selectedData, typeset);
                    reportForm.ShowDialog();
                }
            }
        }

        // Method to get selected data (same logic as in SearchResultsForm)
        private DataTable GetSelectedData(string maritalstatus, string parentguardian, string patient, string hospital, string hospitaladdress, string barangay, string certificationDate, string certificationTime, string amountPaid, string receiptNo, string receiptDateIssued, string placeIssued, string typeset)
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
            selectedData.Columns.Add("Type");

            // Add the selected row data into the DataTable
            selectedData.Rows.Add(maritalstatus, parentguardian, patient, hospital, hospitaladdress, barangay, certificationDate, certificationTime, amountPaid, receiptNo, receiptDateIssued, placeIssued, typeset);

            return selectedData;
        }

        private void txtAmountPaid_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only numbers, backspace, and a single decimal point
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Prevent multiple decimal points
            if (e.KeyChar == '.' && txtAmountPaid.Text.Contains("."))
            {
                e.Handled = true;
            }

            // Prevent the user from deleting the peso sign (₱) by backspace
            if (txtAmountPaid.Text.Length == 1 && e.KeyChar == '\b') // Only allow backspace if length > 1
            {
                e.Handled = true; // Don't let the user backspace if the text is just "₱"
            }
        }

        private void txtAmountPaid_Enter(object sender, EventArgs e)
        {
            // Remove amount part only if it's the default value but keep the peso sign
            if (txtAmountPaid.Text == "₱0.00")
            {
                txtAmountPaid.Text = "₱";  // Set to "₱" only, so user can start typing after it
                txtAmountPaid.SelectionStart = txtAmountPaid.Text.Length;  // Place cursor at the end
            }
        }

        private void txtAmountPaid_Leave(object sender, EventArgs e)
        {
            string input = txtAmountPaid.Text.Replace("₱", "").Trim(); // Remove peso sign before parsing

            if (decimal.TryParse(input, out decimal parsedAmount))
            {
                // Re-add the peso sign and format with two decimal places
                txtAmountPaid.Text = $"₱{parsedAmount:F2}";
            }
            else
            {
                // Default value if input is invalid, keeping the peso sign intact
                txtAmountPaid.Text = "₱0.00";
            }
        }

        private void btnPrintSave_Click(object sender, EventArgs e)
        {
            // Collect form data
            string maritalStatus = checkBoxSingle.Checked ? "SINGLE" : "MARRIED";
            string parentGuardian = txtParentGuardian.Text;
            string parentGuardian2 = txtParentGuardian2.Text;
            string barangay = txtAddress.Text;
            string patient = txtPatientStudent.Text;
            string hospital = txtHospital.Text;
            string hospitalAddress = txtHospitalAddress.Text;
            string certificationDate = DateTime.Now.ToString("MM-dd-yyyy");  // "2025-02-11"
            string certificationTime = DateTime.Now.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);
            string amountpaid = txtAmountPaid.Text;
            string receiptno = txtReceiptNo.Text;
            string receiptdateissued = dateTimePickerDateIssued.Value.ToString("MM-dd-yyyy");
            string placeissued = txtPlaceIssued.Text;
            string typeset = comboBoxTypeList.Text;


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
            // Load database configuration
            var config = ConfigHelper.LoadConfig();
            string connectionString = ""; // Declare outside

            if (config.Count > 0)
            {
                connectionString = $"Server={config["Server"]};Database={config["Database"]};Uid={config["User"]};Pwd={config["Password"]};";
            }

            // Insert query
            string query = "INSERT INTO certificationrecords_nolandholding (MaritalStatus, ParentGuardian, ParentGuardian2, Barangay, Patient, Hospital, HospitalAddress, CertificationDate, CertificationTime, AmountPaid, ReceiptNo, ReceiptDateIssued, PlaceIssued, Type) " +
                           "VALUES (@MaritalStatus, @ParentGuardian, @ParentGuardian2, @Barangay, @Patient, @Hospital, @HospitalAddress, @CertificationDate, @CertificationTime, @AmountPaid, @ReceiptNo, @ReceiptDateIssued, @PlaceIssued, @Type)";

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
                        cmd.Parameters.AddWithValue("@AmountPaid", amountpaid);
                        // Only add ReceiptDateIssued if AmountPaid is greater than ₱0.00
                        if (amountpaid != "₱0.00")
                        {
                            cmd.Parameters.AddWithValue("@ReceiptNo", receiptno);
                            cmd.Parameters.AddWithValue("@ReceiptDateIssued", receiptdateissued);
                            cmd.Parameters.AddWithValue("@PlaceIssued", placeissued);
                        }
                        else
                        {
                            // Prevent inserting the ReceiptDateIssued if the amount is zero
                            cmd.Parameters.AddWithValue("@ReceiptNo", DBNull.Value);
                            cmd.Parameters.AddWithValue("@ReceiptDateIssued", DBNull.Value);
                            cmd.Parameters.AddWithValue("@PlaceIssued", DBNull.Value);
                        }
                        cmd.Parameters.AddWithValue("@Type", typeset);

                        cmd.ExecuteNonQuery();
                    }
                }

                // Show success message
                //MessageBox.Show("Data Saved Successfully");
                LoadDatabase();

                // Fetch only the latest saved record for printing
                DataTable reportData = GetCertificationData();  // This method must return the latest entry

                if (reportData.Rows.Count == 0)
                {
                    MessageBox.Show("No records found for printing.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Open the Report Form with the latest record
                if (comboBoxTypeList.SelectedItem != null)
                {
                    string selectedType = comboBoxTypeList.SelectedItem.ToString(); // Get selected type
                    ReportForm reportForm = new ReportForm(reportData, selectedType); // Pass only reportData and selectedType
                    reportForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void radioOthers_CheckedChanged(object sender, EventArgs e)
        {

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

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private TextBox txtSearch;
        private PictureBox picSearch;
        private string placeholderText = "Enter search term...";

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtSearch.Focused)
            {
                PerformSearch(sender, e);
                dataGridViewResults.Focus();
            }
        }

        private void SearchBox()
        {
            // Create the TextBox (Search Box)
            txtSearch = new TextBox();
            txtSearch.Size = new Size(200, 17);
            txtSearch.Location = new Point(60, 279);
            txtSearch.BorderStyle = BorderStyle.FixedSingle;

            // Set placeholder initially
            txtSearch.Text = placeholderText;
            txtSearch.ForeColor = Color.Gray;

            // Handle Focus Enter (Remove Placeholder only if text is not entered)
            txtSearch.Enter += (s, e) =>
            {
                if (txtSearch.Text == placeholderText)
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = Color.Black;
                }
            };

            // Handle Focus Leave (Restore placeholder if TextBox is empty)
            txtSearch.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    // Restore the placeholder text when focus is lost and text is empty
                    txtSearch.Text = placeholderText;
                    txtSearch.ForeColor = Color.Gray;
                }

                // Explicitly make sure the TextBox loses focus
                txtSearch.Parent.Focus();  // This forces the TextBox to lose focus
            };

            // Ensure the placeholder is visible if no text has been entered and TextBox is clicked
            txtSearch.TextChanged += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    // Once the user starts typing, the placeholder text is removed
                    txtSearch.ForeColor = Color.Black;
                }
                else
                {
                    // If the text is empty, show the placeholder text again
                    txtSearch.ForeColor = Color.Gray;
                }
            };

            // Create the PictureBox (Magnifying Glass)
            picSearch = new PictureBox();
            picSearch.Size = new Size(17, 17);
            picSearch.Location = new Point(txtSearch.Width - 20, txtSearch.Top - 277); // Position inside TextBox
            picSearch.SizeMode = PictureBoxSizeMode.StretchImage;
            picSearch.BackColor = Color.Transparent; // Ensure no background

            picSearch.Image = Properties.Resources.magnifier; // Default image

            // Add hover effect
            picSearch.MouseEnter += (s, e) =>
            {
                picSearch.Image = Properties.Resources.magnifier_hover;
                picSearch.Cursor = Cursors.Default; // Change cursor for better UX
            };

            picSearch.MouseLeave += (s, e) =>
            {
                picSearch.Image = Properties.Resources.magnifier;
                picSearch.Cursor = Cursors.Default; // Reset cursor
            };

            // Add click event to perform search and highlight TextBox
            picSearch.Click += (s, e) =>
            {
                // Perform the search operation (for example)
                string searchQuery = txtSearch.Text;

                if (searchQuery != placeholderText && !string.IsNullOrWhiteSpace(searchQuery))
                {
                    // Perform search logic here (e.g., search the database or a list)
                    MessageBox.Show($"Searching for: {searchQuery}");
                }
                else
                {
                    MessageBox.Show("Please enter a valid search term.");
                }

                // Highlight TextBox background color to pastel yellow
                txtSearch.BackColor = Color.FromArgb(255, 255, 204); // Pastel yellow

                // Optionally, reset highlight after a delay or upon certain conditions
                Timer timer = new Timer();
                timer.Interval = 2000; // Reset after 2 seconds (2000ms)
                timer.Tick += (sender, args) =>
                {
                    txtSearch.BackColor = SystemColors.Window; // Reset to default background color
                    timer.Stop(); // Stop the timer
                };
                timer.Start();
            };

            // Ensure PictureBox stays inside TextBox
            txtSearch.Controls.Add(picSearch);
            picSearch.BringToFront();

            // Add to form
            this.Controls.Add(txtSearch);
        }

        private void PerformSearch(object sender, EventArgs e)
        {
            // Get the search term entered by the user
            string searchTerm = txtSearch.Text.Trim();

            // Fetch the filtered data based on the search term
            DataTable reportData = GetReportData(searchTerm);

            // Check if any results were found
            if (reportData.Rows.Count > 0)
            {
                // Populate the DataGridView in the current form with the results
                dataGridViewResults.DataSource = reportData;
            }
            else
            {
                MessageBox.Show("No results found.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Highlight the TextBox background color to pastel yellow
            txtSearch.BackColor = Color.FromArgb(255, 255, 204);

            // Reset highlight after 2 seconds
            Timer timer = new Timer();
            timer.Interval = 2000;
            timer.Tick += (object timerSender, EventArgs timerArgs) =>
            {
                txtSearch.BackColor = SystemColors.Window;  // Reset to default background color
                timer.Stop();  // Stop the timer
            };
            timer.Start();
        }


        private DataTable GetReportData(string searchTerm)
        {
            // Create a DataTable to hold the results
            DataTable dt = new DataTable();

            // Add columns to the DataTable
            dt.Columns.Add("Patient");
            dt.Columns.Add("Barangay");
            dt.Columns.Add("MaritalStatus");
            dt.Columns.Add("Type");
            dt.Columns.Add("ParentGuardian");
            dt.Columns.Add("Hospital");
            dt.Columns.Add("HospitalAddress");
            dt.Columns.Add("CertificationDate");
            dt.Columns.Add("CertificationTime");
            dt.Columns.Add("AmountPaid");
            dt.Columns.Add("ReceiptNo");
            dt.Columns.Add("ReceiptDateIssued");
            dt.Columns.Add("PlaceIssued");

            // Load database configuration
            var config = ConfigHelper.LoadConfig();
            string connectionString = ""; // Declare outside

            if (config.Count > 0)
            {
                connectionString = $"Server={config["Server"]};Database={config["Database"]};Uid={config["User"]};Pwd={config["Password"]};";
            }


            // SQL query to fetch data based on the search term
            string query = "SELECT MaritalStatus, ParentGuardian, Patient, Hospital, HospitalAddress, Barangay, CertificationDate, CertificationTime, AmountPaid, ReceiptNo, ReceiptDateIssued, PlaceIssued, Type " +
                           "FROM certificationrecords_nolandholding " +
                           "WHERE Patient LIKE @SearchTerm " +
                           "ORDER BY CertificationDate DESC, STR_TO_DATE(CertificationTime, '%h:%i:%s %p') DESC";  // Use DESC for descending order

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

                // Format the column headers with spaces (if needed)
                FormatColumnHeaders(dt);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                DatabaseSetup dbSetup = new DatabaseSetup();
                dbSetup.FormClosing += (s, args) => dbSetup.Hide(); // Prevent disposal
                dbSetup.ShowDialog();
            }

            // Return the populated DataTable
            return dt;

        }

        // Helper function to format column names
        private void FormatColumnHeaders(DataTable dt)
        {
            foreach (DataColumn column in dt.Columns)
            {
                column.ColumnName = FormatColumnName(column.ColumnName);
            }
        }

        // Helper function to add space between camelCase words
        private string FormatColumnName(string columnName)
        {
            if (columnName == "ParentGuardian")
            {
                return "Parent / Guardian";
            }
            var formattedName = System.Text.RegularExpressions.Regex.Replace(
                columnName,
                "([a-z])([A-Z])",
                "$1 $2"
            );
            return formattedName;
        }

        // Handle the KeyDown event
        private void DatabaseReload_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)  // Check if F5 was pressed
            {
                // Call ReloadData to refresh the data
                LoadDatabase();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void labelParentGuardian_Click(object sender, EventArgs e)
        {

        }

        private void labelParentGuardian2_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBoxDate_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void formInput_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            // Subscribe to KeyDown event
            this.KeyDown += new KeyEventHandler(DatabaseReload_KeyDown);
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void preferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Pass the reference of formInput if opened from preferences, else null
            SettingsForm settingsForm = new SettingsForm(); // Pass 'this' (formInput) reference
            settingsForm.ShowDialog();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDatabase();
        }

        private void shutdownToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void radioHospital_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void LoadComboBoxItemsFromMySQL()
        {
            // MySQL connection string
            // Load database configuration
            var config = ConfigHelper.LoadConfig();
            string connectionString = ""; // Declare outside

            if (config.Count > 0)
            {
                connectionString = $"Server={config["Server"]};Database={config["Database"]};Uid={config["User"]};Pwd={config["Password"]};";
            }

            string query = "SELECT typesets FROM sys_nolandholding_internaltypesets ";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        comboBoxTypeList.Items.Clear();

                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                comboBoxTypeList.Items.Add(reader.GetString(0));
                            }
                        }
                    }
                }

                if (comboBoxTypeList.Items.Count > 0)
                {
                    comboBoxTypeList.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data from MySQL: " + ex.Message);
                Console.WriteLine($"Exception: {ex}");
            }
        }
    }
}