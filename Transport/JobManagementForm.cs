using MySql.Data.MySqlClient;
using System.Data;

namespace Transport
{
    public partial class JobManagementForm : Form
    {
        private DatabaseConnection dbConnection;
        private DataTable jobDataTable;

        public JobManagementForm()
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            LoadJobs();
            LoadCustomers();
        }

        private void InitializeComponent()
        {
            this.dgvJobs = new DataGridView();
            this.txtJobId = new TextBox();
            this.cmbCustomer = new ComboBox();
            this.txtStartLocation = new TextBox();
            this.txtDestination = new TextBox();
            this.cmbStatus = new ComboBox();
            this.btnAdd = new Button();
            this.btnUpdate = new Button();
            this.btnDelete = new Button();
            this.btnClear = new Button();
            this.btnApprove = new Button();
            this.btnReject = new Button();
            this.lblJobId = new Label();
            this.lblCustomer = new Label();
            this.lblStartLocation = new Label();
            this.lblDestination = new Label();
            this.lblStatus = new Label();
            this.lblTitle = new Label();
            this.groupBoxActions = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJobs)).BeginInit();
            this.SuspendLayout();

            // Form
            this.Text = "Job Management";
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Title
            this.lblTitle.Text = "Job Management";
            this.lblTitle.Font = new Font("Arial", 16F, FontStyle.Bold);
            this.lblTitle.Location = new Point(450, 10);
            this.lblTitle.Size = new Size(200, 30);

            // DataGridView
            this.dgvJobs.Location = new Point(20, 50);
            this.dgvJobs.Size = new Size(700, 400);
            this.dgvJobs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvJobs.MultiSelect = false;
            this.dgvJobs.ReadOnly = true;
            this.dgvJobs.AllowUserToAddRows = false;
            this.dgvJobs.AllowUserToDeleteRows = false;
            this.dgvJobs.SelectionChanged += new EventHandler(this.dgvJobs_SelectionChanged);

            // Labels and Controls
            int startX = 750;
            int startY = 80;
            int spacing = 50;

            this.lblJobId.Text = "Job ID:";
            this.lblJobId.Location = new Point(startX, startY);
            this.lblJobId.Size = new Size(100, 23);

            this.txtJobId.Location = new Point(startX + 110, startY);
            this.txtJobId.Size = new Size(200, 23);
            this.txtJobId.ReadOnly = true;

            this.lblCustomer.Text = "Customer:";
            this.lblCustomer.Location = new Point(startX, startY + spacing);
            this.lblCustomer.Size = new Size(100, 23);

            this.cmbCustomer.Location = new Point(startX + 110, startY + spacing);
            this.cmbCustomer.Size = new Size(200, 23);
            this.cmbCustomer.DropDownStyle = ComboBoxStyle.DropDownList;

            this.lblStartLocation.Text = "Start Location:";
            this.lblStartLocation.Location = new Point(startX, startY + spacing * 2);
            this.lblStartLocation.Size = new Size(100, 23);

            this.txtStartLocation.Location = new Point(startX + 110, startY + spacing * 2);
            this.txtStartLocation.Size = new Size(200, 23);

            this.lblDestination.Text = "Destination:";
            this.lblDestination.Location = new Point(startX, startY + spacing * 3);
            this.lblDestination.Size = new Size(100, 23);

            this.txtDestination.Location = new Point(startX + 110, startY + spacing * 3);
            this.txtDestination.Size = new Size(200, 23);

            this.lblStatus.Text = "Status:";
            this.lblStatus.Location = new Point(startX, startY + spacing * 4);
            this.lblStatus.Size = new Size(100, 23);

            this.cmbStatus.Location = new Point(startX + 110, startY + spacing * 4);
            this.cmbStatus.Size = new Size(200, 23);
            this.cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbStatus.Items.AddRange(new string[] { "Pending", "Approved", "In Progress", "Completed", "Cancelled" });
            this.cmbStatus.SelectedIndex = 0;

            // Action Buttons GroupBox
            this.groupBoxActions.Text = "Quick Actions";
            this.groupBoxActions.Location = new Point(startX, startY + spacing * 5 + 10);
            this.groupBoxActions.Size = new Size(320, 60);

            this.btnApprove.Text = "Approve";
            this.btnApprove.Location = new Point(10, 25);
            this.btnApprove.Size = new Size(75, 30);
            this.btnApprove.BackColor = Color.LimeGreen;
            this.btnApprove.ForeColor = Color.White;
            this.btnApprove.Click += new EventHandler(this.btnApprove_Click);

            this.btnReject.Text = "Reject";
            this.btnReject.Location = new Point(95, 25);
            this.btnReject.Size = new Size(75, 30);
            this.btnReject.BackColor = Color.OrangeRed;
            this.btnReject.ForeColor = Color.White;
            this.btnReject.Click += new EventHandler(this.btnReject_Click);

            this.groupBoxActions.Controls.Add(this.btnApprove);
            this.groupBoxActions.Controls.Add(this.btnReject);

            // CRUD Buttons
            int buttonY = startY + spacing * 6 + 80;
            this.btnAdd.Text = "Add";
            this.btnAdd.Location = new Point(startX, buttonY);
            this.btnAdd.Size = new Size(75, 30);
            this.btnAdd.BackColor = Color.Green;
            this.btnAdd.ForeColor = Color.White;
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);

            this.btnUpdate.Text = "Update";
            this.btnUpdate.Location = new Point(startX + 85, buttonY);
            this.btnUpdate.Size = new Size(75, 30);
            this.btnUpdate.BackColor = Color.Orange;
            this.btnUpdate.ForeColor = Color.White;
            this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);

            this.btnDelete.Text = "Delete";
            this.btnDelete.Location = new Point(startX + 170, buttonY);
            this.btnDelete.Size = new Size(75, 30);
            this.btnDelete.BackColor = Color.Red;
            this.btnDelete.ForeColor = Color.White;
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);

            this.btnClear.Text = "Clear";
            this.btnClear.Location = new Point(startX + 255, buttonY);
            this.btnClear.Size = new Size(75, 30);
            this.btnClear.BackColor = Color.Gray;
            this.btnClear.ForeColor = Color.White;
            this.btnClear.Click += new EventHandler(this.btnClear_Click);

            // Add controls to form
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.dgvJobs);
            this.Controls.Add(this.lblJobId);
            this.Controls.Add(this.txtJobId);
            this.Controls.Add(this.lblCustomer);
            this.Controls.Add(this.cmbCustomer);
            this.Controls.Add(this.lblStartLocation);
            this.Controls.Add(this.txtStartLocation);
            this.Controls.Add(this.lblDestination);
            this.Controls.Add(this.txtDestination);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.groupBoxActions);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClear);

            ((System.ComponentModel.ISupportInitialize)(this.dgvJobs)).EndInit();
            this.ResumeLayout(false);
        }

        private DataGridView dgvJobs;
        private TextBox txtJobId, txtStartLocation, txtDestination;
        private ComboBox cmbCustomer, cmbStatus;
        private Button btnAdd, btnUpdate, btnDelete, btnClear, btnApprove, btnReject;
        private Label lblJobId, lblCustomer, lblStartLocation, lblDestination, lblStatus, lblTitle;
        private GroupBox groupBoxActions;

        private void LoadJobs()
        {
            string query = @"SELECT j.JobID, j.CustomerID, c.Name as CustomerName, 
                            j.StartLocation, j.Destination, j.Status 
                            FROM Job j 
                            INNER JOIN Customer c ON j.CustomerID = c.CustomerID 
                            ORDER BY j.JobID";
            jobDataTable = dbConnection.ExecuteSelect(query);
            dgvJobs.DataSource = jobDataTable;

            // Hide CustomerID column
            if (dgvJobs.Columns["CustomerID"] != null)
            {
                dgvJobs.Columns["CustomerID"].Visible = false;
            }
        }

        private void LoadCustomers()
        {
            string query = "SELECT CustomerID, Name FROM Customer ORDER BY Name";
            var customerData = dbConnection.ExecuteSelect(query);
            
            cmbCustomer.DisplayMember = "Name";
            cmbCustomer.ValueMember = "CustomerID";
            cmbCustomer.DataSource = customerData;
        }

        private void dgvJobs_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvJobs.CurrentRow != null)
            {
                DataGridViewRow row = dgvJobs.CurrentRow;
                txtJobId.Text = row.Cells["JobID"].Value?.ToString();
                cmbCustomer.SelectedValue = row.Cells["CustomerID"].Value;
                txtStartLocation.Text = row.Cells["StartLocation"].Value?.ToString();
                txtDestination.Text = row.Cells["Destination"].Value?.ToString();
                cmbStatus.Text = row.Cells["Status"].Value?.ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                string query = @"INSERT INTO Job (CustomerID, StartLocation, Destination, Status) 
                                VALUES (@customerId, @startLocation, @destination, @status)";

                MySqlParameter[] parameters = {
                    new MySqlParameter("@customerId", cmbCustomer.SelectedValue),
                    new MySqlParameter("@startLocation", txtStartLocation.Text.Trim()),
                    new MySqlParameter("@destination", txtDestination.Text.Trim()),
                    new MySqlParameter("@status", cmbStatus.Text)
                };

                if (dbConnection.ExecuteNonQuery(query, parameters))
                {
                    MessageBox.Show("Job added successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadJobs();
                    ClearForm();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtJobId.Text))
            {
                MessageBox.Show("Please select a job to update.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ValidateInput())
            {
                string query = @"UPDATE Job SET CustomerID = @customerId, StartLocation = @startLocation, 
                                Destination = @destination, Status = @status WHERE JobID = @jobId";

                MySqlParameter[] parameters = {
                    new MySqlParameter("@customerId", cmbCustomer.SelectedValue),
                    new MySqlParameter("@startLocation", txtStartLocation.Text.Trim()),
                    new MySqlParameter("@destination", txtDestination.Text.Trim()),
                    new MySqlParameter("@status", cmbStatus.Text),
                    new MySqlParameter("@jobId", Convert.ToInt32(txtJobId.Text))
                };

                if (dbConnection.ExecuteNonQuery(query, parameters))
                {
                    MessageBox.Show("Job updated successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadJobs();
                    ClearForm();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtJobId.Text))
            {
                MessageBox.Show("Please select a job to delete.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this job?", 
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string query = "DELETE FROM Job WHERE JobID = @jobId";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@jobId", Convert.ToInt32(txtJobId.Text))
                };

                if (dbConnection.ExecuteNonQuery(query, parameters))
                {
                    MessageBox.Show("Job deleted successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadJobs();
                    ClearForm();
                }
            }
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            UpdateJobStatus("Approved");
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            UpdateJobStatus("Cancelled");
        }

        private void UpdateJobStatus(string status)
        {
            if (string.IsNullOrEmpty(txtJobId.Text))
            {
                MessageBox.Show("Please select a job to update.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "UPDATE Job SET Status = @status WHERE JobID = @jobId";
            MySqlParameter[] parameters = {
                new MySqlParameter("@status", status),
                new MySqlParameter("@jobId", Convert.ToInt32(txtJobId.Text))
            };

            if (dbConnection.ExecuteNonQuery(query, parameters))
            {
                MessageBox.Show($"Job {status.ToLower()} successfully!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadJobs();
                cmbStatus.Text = status;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtJobId.Text = "";
            if (cmbCustomer.Items.Count > 0) cmbCustomer.SelectedIndex = 0;
            txtStartLocation.Text = "";
            txtDestination.Text = "";
            cmbStatus.SelectedIndex = 0;
        }

        private bool ValidateInput()
        {
            if (cmbCustomer.SelectedValue == null)
            {
                MessageBox.Show("Please select a customer.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtStartLocation.Text))
            {
                MessageBox.Show("Please enter start location.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDestination.Text))
            {
                MessageBox.Show("Please enter destination.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a status.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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