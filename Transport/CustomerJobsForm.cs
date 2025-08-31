using MySql.Data.MySqlClient;
using System.Data;

namespace Transport
{
    public partial class CustomerJobsForm : Form
    {
        private int customerId;
        private DatabaseConnection dbConnection;
        private DataTable jobDataTable;

        public CustomerJobsForm(int customerId)
        {
            this.customerId = customerId;
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            LoadCustomerJobs();
        }

        private void InitializeComponent()
        {
            this.dgvJobs = new DataGridView();
            this.dgvLoadDetails = new DataGridView();
            this.lblTitle = new Label();
            this.lblJobs = new Label();
            this.lblLoadDetails = new Label();
            this.lblStatus = new Label();
            this.cmbStatusFilter = new ComboBox();
            this.btnRefresh = new Button();
            this.btnViewLoadDetails = new Button();
            this.txtJobDetails = new TextBox();
            this.lblJobInfo = new Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJobs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoadDetails)).BeginInit();
            this.SuspendLayout();

            // Form
            this.Text = "My Jobs";
            this.Size = new Size(1200, 750);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Title
            this.lblTitle.Text = $"My Jobs - Customer ID: {customerId}";
            this.lblTitle.Font = new Font("Arial", 16F, FontStyle.Bold);
            this.lblTitle.Location = new Point(450, 10);
            this.lblTitle.Size = new Size(400, 30);

            // Status Filter
            this.lblStatus.Text = "Filter by Status:";
            this.lblStatus.Location = new Point(20, 50);
            this.lblStatus.Size = new Size(100, 23);

            this.cmbStatusFilter.Location = new Point(130, 50);
            this.cmbStatusFilter.Size = new Size(150, 23);
            this.cmbStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbStatusFilter.Items.AddRange(new string[] { "All", "Pending", "Approved", "In Progress", "Completed", "Cancelled" });
            this.cmbStatusFilter.SelectedIndex = 0;
            this.cmbStatusFilter.SelectedIndexChanged += new EventHandler(this.cmbStatusFilter_SelectedIndexChanged);

            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Location = new Point(300, 50);
            this.btnRefresh.Size = new Size(80, 25);
            this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);

            // Jobs Label and DataGridView
            this.lblJobs.Text = "My Transport Jobs:";
            this.lblJobs.Font = new Font("Arial", 12F, FontStyle.Bold);
            this.lblJobs.Location = new Point(20, 85);
            this.lblJobs.Size = new Size(200, 20);

            this.dgvJobs.Location = new Point(20, 110);
            this.dgvJobs.Size = new Size(700, 250);
            this.dgvJobs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvJobs.MultiSelect = false;
            this.dgvJobs.ReadOnly = true;
            this.dgvJobs.AllowUserToAddRows = false;
            this.dgvJobs.AllowUserToDeleteRows = false;
            this.dgvJobs.SelectionChanged += new EventHandler(this.dgvJobs_SelectionChanged);

            // Job Details
            this.lblJobInfo.Text = "Job Information:";
            this.lblJobInfo.Font = new Font("Arial", 12F, FontStyle.Bold);
            this.lblJobInfo.Location = new Point(750, 85);
            this.lblJobInfo.Size = new Size(150, 20);

            this.txtJobDetails.Location = new Point(750, 110);
            this.txtJobDetails.Size = new Size(400, 100);
            this.txtJobDetails.Multiline = true;
            this.txtJobDetails.ReadOnly = true;
            this.txtJobDetails.ScrollBars = ScrollBars.Vertical;

            this.btnViewLoadDetails.Text = "View Load Details";
            this.btnViewLoadDetails.Location = new Point(750, 220);
            this.btnViewLoadDetails.Size = new Size(150, 30);
            this.btnViewLoadDetails.BackColor = Color.DodgerBlue;
            this.btnViewLoadDetails.ForeColor = Color.White;
            this.btnViewLoadDetails.Click += new EventHandler(this.btnViewLoadDetails_Click);

            // Load Details Label and DataGridView
            this.lblLoadDetails.Text = "Load Details (Truck Assignment):";
            this.lblLoadDetails.Font = new Font("Arial", 12F, FontStyle.Bold);
            this.lblLoadDetails.Location = new Point(20, 380);
            this.lblLoadDetails.Size = new Size(300, 20);

            this.dgvLoadDetails.Location = new Point(20, 405);
            this.dgvLoadDetails.Size = new Size(1130, 200);
            this.dgvLoadDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvLoadDetails.MultiSelect = false;
            this.dgvLoadDetails.ReadOnly = true;
            this.dgvLoadDetails.AllowUserToAddRows = false;
            this.dgvLoadDetails.AllowUserToDeleteRows = false;

            // Add controls to form
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.cmbStatusFilter);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lblJobs);
            this.Controls.Add(this.dgvJobs);
            this.Controls.Add(this.lblJobInfo);
            this.Controls.Add(this.txtJobDetails);
            this.Controls.Add(this.btnViewLoadDetails);
            this.Controls.Add(this.lblLoadDetails);
            this.Controls.Add(this.dgvLoadDetails);

            ((System.ComponentModel.ISupportInitialize)(this.dgvJobs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoadDetails)).EndInit();
            this.ResumeLayout(false);
        }

        private DataGridView dgvJobs, dgvLoadDetails;
        private Label lblTitle, lblJobs, lblLoadDetails, lblStatus, lblJobInfo;
        private ComboBox cmbStatusFilter;
        private Button btnRefresh, btnViewLoadDetails;
        private TextBox txtJobDetails;

        private void LoadCustomerJobs()
        {
            string statusFilter = cmbStatusFilter.Text == "All" ? "" : $" AND Status = '{cmbStatusFilter.Text}'";
            
            string query = $@"SELECT JobID, StartLocation, Destination, Status, 
                             DATE_FORMAT(NOW(), '%Y-%m-%d') as RequestDate
                             FROM Job 
                             WHERE CustomerID = @customerId{statusFilter}
                             ORDER BY JobID DESC";

            MySqlParameter[] parameters = {
                new MySqlParameter("@customerId", customerId)
            };

            jobDataTable = dbConnection.ExecuteSelect(query, parameters);
            dgvJobs.DataSource = jobDataTable;

            // Apply status-based row coloring
            ApplyStatusColoring();
        }

        private void ApplyStatusColoring()
        {
            foreach (DataGridViewRow row in dgvJobs.Rows)
            {
                if (row.Cells["Status"].Value != null)
                {
                    string status = row.Cells["Status"].Value.ToString();
                    switch (status)
                    {
                        case "Pending":
                            row.DefaultCellStyle.BackColor = Color.LightYellow;
                            break;
                        case "Approved":
                            row.DefaultCellStyle.BackColor = Color.LightGreen;
                            break;
                        case "In Progress":
                            row.DefaultCellStyle.BackColor = Color.LightBlue;
                            break;
                        case "Completed":
                            row.DefaultCellStyle.BackColor = Color.LightGray;
                            break;
                        case "Cancelled":
                            row.DefaultCellStyle.BackColor = Color.LightPink;
                            break;
                    }
                }
            }
        }

        private void dgvJobs_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvJobs.CurrentRow != null)
            {
                DataGridViewRow row = dgvJobs.CurrentRow;
                string jobId = row.Cells["JobID"].Value?.ToString();
                string startLocation = row.Cells["StartLocation"].Value?.ToString();
                string destination = row.Cells["Destination"].Value?.ToString();
                string status = row.Cells["Status"].Value?.ToString();

                txtJobDetails.Text = $"Job ID: {jobId}\r\n" +
                                   $"From: {startLocation}\r\n" +
                                   $"To: {destination}\r\n" +
                                   $"Status: {status}\r\n" +
                                   $"Customer ID: {customerId}\r\n\r\n" +
                                   GetStatusDescription(status);

                LoadJobLoadDetails(jobId);
            }
        }

        private string GetStatusDescription(string status)
        {
            return status switch
            {
                "Pending" => "Your job request is being reviewed by our team.",
                "Approved" => "Your job has been approved and is awaiting truck assignment.",
                "In Progress" => "Your transport is currently in progress. Check load details below.",
                "Completed" => "Your transport has been completed successfully.",
                "Cancelled" => "This job has been cancelled.",
                _ => "Status information not available."
            };
        }

        private void LoadJobLoadDetails(string jobId)
        {
            if (string.IsNullOrEmpty(jobId)) return;

            string query = @"SELECT LoadID, Lorry, Driver, Assistant, Container 
                            FROM LoadDetails 
                            WHERE JobID = @jobId";

            MySqlParameter[] parameters = {
                new MySqlParameter("@jobId", Convert.ToInt32(jobId))
            };

            var loadData = dbConnection.ExecuteSelect(query, parameters);
            dgvLoadDetails.DataSource = loadData;

            if (loadData.Rows.Count == 0)
            {
                // Create empty row with message
                DataTable emptyTable = new DataTable();
                emptyTable.Columns.Add("Message");
                emptyTable.Rows.Add("No load details assigned yet.");
                dgvLoadDetails.DataSource = emptyTable;
            }
        }

        private void cmbStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCustomerJobs();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadCustomerJobs();
            MessageBox.Show("Jobs refreshed successfully!", "Refresh", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnViewLoadDetails_Click(object sender, EventArgs e)
        {
            if (dgvJobs.CurrentRow != null)
            {
                string jobId = dgvJobs.CurrentRow.Cells["JobID"].Value?.ToString();
                string status = dgvJobs.CurrentRow.Cells["Status"].Value?.ToString();

                if (status == "In Progress" || status == "Completed")
                {
                    LoadJobLoadDetails(jobId);
                    MessageBox.Show("Load details updated below.", "Load Details", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Load details are only available for jobs that are 'In Progress' or 'Completed'.", 
                        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a job first.", "Selection Required", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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