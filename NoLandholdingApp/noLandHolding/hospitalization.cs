using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoLandholdingApp.noLandHolding
{
    public partial class hospitalization : Form
    {
        public event Action OnPrintSave;
        public hospitalization()
        {
            InitializeComponent();
            SetTextBoxCharacterCasing();
            AttachEventHandlers();
            this.Icon = Properties.Resources.logo;

            this.KeyPreview = true;
            this.KeyDown += btnPrintSave_KeyDown;
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
            txtPlaceIssued.CharacterCasing= CharacterCasing.Upper;
            txtAmountPaid.Text = "₱0.00";
        }

        private void AttachEventHandlers()
        {
            checkBoxSingle.CheckedChanged += checkBoxSingle_CheckedChanged;
            checkBoxMarried.CheckedChanged += checkBoxMarried_CheckedChanged;
            txtAmountPaid.Enter += txtAmountPaid_Enter;
            txtAmountPaid.KeyPress += txtAmountPaid_KeyPress;
            txtAmountPaid.Leave += txtAmountPaid_Leave;
        }

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

        private void btnPrintSave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Trigger the btnTestConnection_Click event when Enter is pressed
                btnPrintSave_Click(sender, e);
            }
        }

        private bool ValidateRequiredFields()
        {
            bool isValid = true;

            // Array of text boxes and their corresponding labels
            (TextBox textBox, Label label)[] fields = {
        (txtParentGuardian, lblParentGuardian),
        (txtPatientStudent, lblPatientStudent),
        (txtHospital, lblHospital),
        (txtAddress, lblAddress),
        (txtHospitalAddress, lblHospitalAddress)
    };

            foreach (var (textBox, label) in fields)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    HighlightTextBox(textBox, true); // Highlight in yellow
                    label.Visible = true; // Show the red asterisk
                    isValid = false;
                }
                else
                {
                    HighlightTextBox(textBox, false); // Reset to default
                    label.Visible = false; // Hide the red asterisk
                }
            }

            return isValid;
        }

        private void HighlightTextBox(TextBox textBox, bool highlight)
        {
            textBox.BackColor = highlight ? Color.LightYellow : SystemColors.Window;
        }

        private void btnPrintSave_Click(object sender, EventArgs e)
        {
            // Validate required fields
            if (!ValidateRequiredFields())
            {
                MessageBox.Show("Please fill in all mandatory fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Exit the method if validation fails
            }

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
            string typeset = "Hospitalization";


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
                

                // Fetch only the latest saved record for printing
                formInput inputForm = new formInput();
                DataTable reportData = inputForm.GetCertificationData();  // This method must return the latest entry
                inputForm.LoadDatabase();

                if (reportData.Rows.Count == 0)
                {
                    MessageBox.Show("No records found for printing.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string selectedType = typeset;
                ReportForm reportForm = new ReportForm(reportData, selectedType); // Pass only reportData and selectedType
                reportForm.ShowDialog();
                this.Close();

                OnPrintSave?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
