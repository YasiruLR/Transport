using MySql.Data.MySqlClient;
using System.Data;

namespace Transport
{
    public partial class JobRequestForm : Form
    {
        private int customerId;
        private DatabaseConnection dbConnection;

        public JobRequestForm(int customerId)
        {
            this.customerId = customerId;
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            LoadCustomerInfo();
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.lblCustomerInfo = new Label();
            this.txtCustomerInfo = new TextBox();
            this.lblStartLocation = new Label();
            this.txtStartLocation = new TextBox();
            this.lblDestination = new Label();
            this.txtDestination = new TextBox();
            this.lblDescription = new Label();
            this.txtDescription = new TextBox();
            this.lblEstimatedWeight = new Label();
            this.txtEstimatedWeight = new TextBox();
            this.lblPreferredDate = new Label();
            this.dtpPreferredDate = new DateTimePicker();
            this.lblUrgency = new Label();
            this.cmbUrgency = new ComboBox();
            this.btnSubmitRequest = new Button();
            this.btnClear = new Button();
            this.btnCancel = new Button();
            this.groupBoxJobDetails = new GroupBox();
            this.groupBoxAdditionalInfo = new GroupBox();
            this.SuspendLayout();

            // Form
            this.Text = "Request New Transport Job";
            this.Size = new Size(800, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Title
            this.lblTitle.Text = "Request New Transport Job";
            this.lblTitle.Font = new Font("Arial", 18F, FontStyle.Bold);
            this.lblTitle.Location = new Point(250, 20);
            this.lblTitle.Size = new Size(350, 30);
            this.lblTitle.ForeColor = Color.DarkBlue;

            // Customer Info
            this.lblCustomerInfo.Text = "Customer Information:";
            this.lblCustomerInfo.Font = new Font("Arial", 12F, FontStyle.Bold);
            this.lblCustomerInfo.Location = new Point(30, 70);
            this.lblCustomerInfo.Size = new Size(200, 20);

            this.txtCustomerInfo.Location = new Point(30, 95);
            this.txtCustomerInfo.Size = new Size(720, 50);
            this.txtCustomerInfo.Multiline = true;
            this.txtCustomerInfo.ReadOnly = true;
            this.txtCustomerInfo.BackColor = Color.LightGray;

            // Job Details GroupBox
            this.groupBoxJobDetails.Text = "Transport Details";
            this.groupBoxJobDetails.Font = new Font("Arial", 11F, FontStyle.Bold);
            this.groupBoxJobDetails.Location = new Point(30, 160);
            this.groupBoxJobDetails.Size = new Size(720, 220);

            // Start Location
            this.lblStartLocation.Text = "Pickup Location:";
            this.lblStartLocation.Location = new Point(20, 35);
            this.lblStartLocation.Size = new Size(120, 23);

            this.txtStartLocation.Location = new Point(150, 35);
            this.txtStartLocation.Size = new Size(540, 23);
            this.txtStartLocation.PlaceholderText = "Enter full pickup address with city and state";

            // Destination
            this.lblDestination.Text = "Delivery Location:";
            this.lblDestination.Location = new Point(20, 75);
            this.lblDestination.Size = new Size(120, 23);

            this.txtDestination.Location = new Point(150, 75);
            this.txtDestination.Size = new Size(540, 23);
            this.txtDestination.PlaceholderText = "Enter full delivery address with city and state";

            // Description
            this.lblDescription.Text = "Cargo Description:";
            this.lblDescription.Location = new Point(20, 115);
            this.lblDescription.Size = new Size(120, 23);

            this.txtDescription.Location = new Point(150, 115);
            this.txtDescription.Size = new Size(540, 80);
            this.txtDescription.Multiline = true;
            this.txtDescription.PlaceholderText = "Describe the items to be transported, special handling requirements, etc.";

            this.groupBoxJobDetails.Controls.Add(this.lblStartLocation);
            this.groupBoxJobDetails.Controls.Add(this.txtStartLocation);
            this.groupBoxJobDetails.Controls.Add(this.lblDestination);
            this.groupBoxJobDetails.Controls.Add(this.txtDestination);
            this.groupBoxJobDetails.Controls.Add(this.lblDescription);
            this.groupBoxJobDetails.Controls.Add(this.txtDescription);

            // Additional Info GroupBox
            this.groupBoxAdditionalInfo.Text = "Additional Information";
            this.groupBoxAdditionalInfo.Font = new Font("Arial", 11F, FontStyle.Bold);
            this.groupBoxAdditionalInfo.Location = new Point(30, 390);
            this.groupBoxAdditionalInfo.Size = new Size(720, 180);

            // Estimated Weight
            this.lblEstimatedWeight.Text = "Estimated Weight (kg):";
            this.lblEstimatedWeight.Location = new Point(20, 35);
            this.lblEstimatedWeight.Size = new Size(150, 23);

            this.txtEstimatedWeight.Location = new Point(180, 35);
            this.txtEstimatedWeight.Size = new Size(150, 23);
            this.txtEstimatedWeight.PlaceholderText = "Enter weight in kg";

            // Preferred Date
            this.lblPreferredDate.Text = "Preferred Pickup Date:";
            this.lblPreferredDate.Location = new Point(350, 35);
            this.lblPreferredDate.Size = new Size(150, 23);

            this.dtpPreferredDate.Location = new Point(510, 35);
            this.dtpPreferredDate.Size = new Size(180, 23);
            this.dtpPreferredDate.Value = DateTime.Now.AddDays(1);
            this.dtpPreferredDate.MinDate = DateTime.Now;

            // Urgency
            this.lblUrgency.Text = "Urgency Level:";
            this.lblUrgency.Location = new Point(20, 80);
            this.lblUrgency.Size = new Size(120, 23);

            this.cmbUrgency.Location = new Point(150, 80);
            this.cmbUrgency.Size = new Size(150, 23);
            this.cmbUrgency.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbUrgency.Items.AddRange(new string[] { "Standard", "Priority", "Express", "Emergency" });
            this.cmbUrgency.SelectedIndex = 0;

            this.groupBoxAdditionalInfo.Controls.Add(this.lblEstimatedWeight);
            this.groupBoxAdditionalInfo.Controls.Add(this.txtEstimatedWeight);
            this.groupBoxAdditionalInfo.Controls.Add(this.lblPreferredDate);
            this.groupBoxAdditionalInfo.Controls.Add(this.dtpPreferredDate);
            this.groupBoxAdditionalInfo.Controls.Add(this.lblUrgency);
            this.groupBoxAdditionalInfo.Controls.Add(this.cmbUrgency);

            // Buttons
            this.btnSubmitRequest.Text = "Submit Request";
            this.btnSubmitRequest.Location = new Point(200, 600);
            this.btnSubmitRequest.Size = new Size(130, 35);
            this.btnSubmitRequest.BackColor = Color.Green;
            this.btnSubmitRequest.ForeColor = Color.White;
            this.btnSubmitRequest.Font = new Font("Arial", 10F, FontStyle.Bold);
            this.btnSubmitRequest.Click += new EventHandler(this.btnSubmitRequest_Click);

            this.btnClear.Text = "Clear Form";
            this.btnClear.Location = new Point(350, 600);
            this.btnClear.Size = new Size(100, 35);
            this.btnClear.BackColor = Color.Orange;
            this.btnClear.ForeColor = Color.White;
            this.btnClear.Font = new Font("Arial", 10F, FontStyle.Bold);
            this.btnClear.Click += new EventHandler(this.btnClear_Click);

            this.btnCancel.Text = "Cancel";
            this.btnCancel.Location = new Point(470, 600);
            this.btnCancel.Size = new Size(100, 35);
            this.btnCancel.BackColor = Color.Gray;
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.Font = new Font("Arial", 10F, FontStyle.Bold);
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

            // Add controls to form
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblCustomerInfo);
            this.Controls.Add(this.txtCustomerInfo);
            this.Controls.Add(this.groupBoxJobDetails);
            this.Controls.Add(this.groupBoxAdditionalInfo);
            this.Controls.Add(this.btnSubmitRequest);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnCancel);

            this.ResumeLayout(false);
        }

        private Label lblTitle, lblCustomerInfo, lblStartLocation, lblDestination, lblDescription;
        private Label lblEstimatedWeight, lblPreferredDate, lblUrgency;
        private TextBox txtCustomerInfo, txtStartLocation, txtDestination, txtDescription, txtEstimatedWeight;
        private DateTimePicker dtpPreferredDate;
        private ComboBox cmbUrgency;
        private Button btnSubmitRequest, btnClear, btnCancel;
        private GroupBox groupBoxJobDetails, groupBoxAdditionalInfo;

        private void LoadCustomerInfo()
        {
            string query = "SELECT Name, Email, Phone, Address FROM Customer WHERE CustomerID = @customerId";
            MySqlParameter[] parameters = {
                new MySqlParameter("@customerId", customerId)
            };

            var customerData = dbConnection.ExecuteSelect(query, parameters);
            if (customerData.Rows.Count > 0)
            {
                DataRow row = customerData.Rows[0];
                txtCustomerInfo.Text = $"Customer ID: {customerId}\r\n" +
                                     $"Name: {row["Name"]}\r\n" +
                                     $"Email: {row["Email"]}\r\n" +
                                     $"Phone: {row["Phone"]}\r\n" +
                                     $"Address: {row["Address"]}";
            }
        }

        private void btnSubmitRequest_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                // Create job request
                string jobQuery = @"INSERT INTO Job (CustomerID, StartLocation, Destination, Status) 
                                   VALUES (@customerId, @startLocation, @destination, 'Pending')";

                MySqlParameter[] jobParameters = {
                    new MySqlParameter("@customerId", customerId),
                    new MySqlParameter("@startLocation", txtStartLocation.Text.Trim()),
                    new MySqlParameter("@destination", txtDestination.Text.Trim())
                };

                if (dbConnection.ExecuteNonQuery(jobQuery, jobParameters))
                {
                    // Get the newly created job ID
                    string getJobIdQuery = "SELECT LAST_INSERT_ID()";
                    var jobId = dbConnection.ExecuteScalar(getJobIdQuery);

                    // Log additional job request details (you could create a separate table for this)
                    LogJobRequestDetails(Convert.ToInt32(jobId));

                    MessageBox.Show($"Job request submitted successfully!\n\n" +
                                  $"Job ID: {jobId}\n" +
                                  $"Status: Pending\n\n" +
                                  $"Your request will be reviewed by our team. " +
                                  $"You can track the status in 'My Jobs'.", 
                                  "Request Submitted", 
                                  MessageBoxButtons.OK, 
                                  MessageBoxIcon.Information);

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to submit job request. Please try again.", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LogJobRequestDetails(int jobId)
        {
            // You could create a JobRequestDetails table to store additional information
            // For now, we'll just log to console or could extend the Job table
            string details = $"Job {jobId} Details:\n" +
                           $"Description: {txtDescription.Text}\n" +
                           $"Estimated Weight: {txtEstimatedWeight.Text} kg\n" +
                           $"Preferred Date: {dtpPreferredDate.Value:yyyy-MM-dd}\n" +
                           $"Urgency: {cmbUrgency.Text}";
            
            // In a real application, you might want to store this in a separate table
            Console.WriteLine(details);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel? All entered data will be lost.", 
                "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void ClearForm()
        {
            txtStartLocation.Text = "";
            txtDestination.Text = "";
            txtDescription.Text = "";
            txtEstimatedWeight.Text = "";
            dtpPreferredDate.Value = DateTime.Now.AddDays(1);
            cmbUrgency.SelectedIndex = 0;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtStartLocation.Text))
            {
                MessageBox.Show("Please enter the pickup location.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStartLocation.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDestination.Text))
            {
                MessageBox.Show("Please enter the delivery location.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDestination.Focus();
                return false;
            }

            if (txtStartLocation.Text.Trim().Equals(txtDestination.Text.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Pickup and delivery locations cannot be the same.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDestination.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Please provide a description of the cargo.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDescription.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtEstimatedWeight.Text))
            {
                if (!decimal.TryParse(txtEstimatedWeight.Text.Trim(), out decimal weight) || weight <= 0)
                {
                    MessageBox.Show("Please enter a valid weight (positive number) or leave empty.", "Validation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEstimatedWeight.Focus();
                    return false;
                }
            }

            if (dtpPreferredDate.Value.Date < DateTime.Now.Date)
            {
                MessageBox.Show("Preferred pickup date cannot be in the past.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpPreferredDate.Focus();
                return false;
            }

            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbConnection?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}