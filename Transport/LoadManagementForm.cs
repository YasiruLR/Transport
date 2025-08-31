using MySql.Data.MySqlClient;
using System.Data;

namespace Transport
{
    public partial class LoadManagementForm : Form
    {
        private DatabaseConnection dbConnection;
        private DataTable loadDataTable;

        public LoadManagementForm()
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            LoadLoads();
            LoadJobs();
        }

        private void InitializeComponent()
        {
            this.dgvLoads = new DataGridView();
            this.txtLoadId = new TextBox();
            this.cmbJob = new ComboBox();
            this.txtLorry = new TextBox();
            this.txtDriver = new TextBox();
            this.txtAssistant = new TextBox();
            this.txtContainer = new TextBox();
            this.btnAdd = new Button();
            this.btnUpdate = new Button();
            this.btnDelete = new Button();
            this.btnClear = new Button();
            this.btnAssignLoad = new Button();
            this.lblLoadId = new Label();
            this.lblJob = new Label();
            this.lblLorry = new Label();
            this.lblDriver = new Label();
            this.lblAssistant = new Label();
            this.lblContainer = new Label();
            this.lblTitle = new Label();
            this.groupBoxQuickAssign = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoads)).BeginInit();
            this.SuspendLayout();

            // Form
            this.Text = "Load Management";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Title
            this.lblTitle.Text = "Load Management";
            this.lblTitle.Font = new Font("Arial", 16F, FontStyle.Bold);
            this.lblTitle.Location = new Point(500, 10);
            this.lblTitle.Size = new Size(200, 30);

            // DataGridView
            this.dgvLoads.Location = new Point(20, 50);
            this.dgvLoads.Size = new Size(750, 400);
            this.dgvLoads.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvLoads.MultiSelect = false;
            this.dgvLoads.ReadOnly = true;
            this.dgvLoads.AllowUserToAddRows = false;
            this.dgvLoads.AllowUserToDeleteRows = false;
            this.dgvLoads.SelectionChanged += new EventHandler(this.dgvLoads_SelectionChanged);

            // Labels and Controls
            int startX = 800;
            int startY = 80;
            int spacing = 50;

            this.lblLoadId.Text = "Load ID:";
            this.lblLoadId.Location = new Point(startX, startY);
            this.lblLoadId.Size = new Size(100, 23);

            this.txtLoadId.Location = new Point(startX + 110, startY);
            this.txtLoadId.Size = new Size(200, 23);
            this.txtLoadId.ReadOnly = true;

            this.lblJob.Text = "Job:";
            this.lblJob.Location = new Point(startX, startY + spacing);
            this.lblJob.Size = new Size(100, 23);

            this.cmbJob.Location = new Point(startX + 110, startY + spacing);
            this.cmbJob.Size = new Size(200, 23);
            this.cmbJob.DropDownStyle = ComboBoxStyle.DropDownList;

            this.lblLorry.Text = "Lorry:";
            this.lblLorry.Location = new Point(startX, startY + spacing * 2);
            this.lblLorry.Size = new Size(100, 23);

            this.txtLorry.Location = new Point(startX + 110, startY + spacing * 2);
            this.txtLorry.Size = new Size(200, 23);

            this.lblDriver.Text = "Driver:";
            this.lblDriver.Location = new Point(startX, startY + spacing * 3);
            this.lblDriver.Size = new Size(100, 23);

            this.txtDriver.Location = new Point(startX + 110, startY + spacing * 3);
            this.txtDriver.Size = new Size(200, 23);

            this.lblAssistant.Text = "Assistant:";
            this.lblAssistant.Location = new Point(startX, startY + spacing * 4);
            this.lblAssistant.Size = new Size(100, 23);

            this.txtAssistant.Location = new Point(startX + 110, startY + spacing * 4);
            this.txtAssistant.Size = new Size(200, 23);

            this.lblContainer.Text = "Container:";
            this.lblContainer.Location = new Point(startX, startY + spacing * 5);
            this.lblContainer.Size = new Size(100, 23);

            this.txtContainer.Location = new Point(startX + 110, startY + spacing * 5);
            this.txtContainer.Size = new Size(200, 23);

            // Quick Assign GroupBox
            this.groupBoxQuickAssign.Text = "Quick Assignment";
            this.groupBoxQuickAssign.Location = new Point(startX, startY + spacing * 6 + 10);
            this.groupBoxQuickAssign.Size = new Size(320, 60);

            this.btnAssignLoad.Text = "Auto Assign Load";
            this.btnAssignLoad.Location = new Point(10, 25);
            this.btnAssignLoad.Size = new Size(120, 30);
            this.btnAssignLoad.BackColor = Color.DodgerBlue;
            this.btnAssignLoad.ForeColor = Color.White;
            this.btnAssignLoad.Click += new EventHandler(this.btnAssignLoad_Click);

            this.groupBoxQuickAssign.Controls.Add(this.btnAssignLoad);

            // CRUD Buttons
            int buttonY = startY + spacing * 7 + 80;
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
            this.Controls.Add(this.dgvLoads);
            this.Controls.Add(this.lblLoadId);
            this.Controls.Add(this.txtLoadId);
            this.Controls.Add(this.lblJob);
            this.Controls.Add(this.cmbJob);
            this.Controls.Add(this.lblLorry);
            this.Controls.Add(this.txtLorry);
            this.Controls.Add(this.lblDriver);
            this.Controls.Add(this.txtDriver);
            this.Controls.Add(this.lblAssistant);
            this.Controls.Add(this.txtAssistant);
            this.Controls.Add(this.lblContainer);
            this.Controls.Add(this.txtContainer);
            this.Controls.Add(this.groupBoxQuickAssign);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClear);

            ((System.ComponentModel.ISupportInitialize)(this.dgvLoads)).EndInit();
            this.ResumeLayout(false);
        }

        private DataGridView dgvLoads;
        private TextBox txtLoadId, txtLorry, txtDriver, txtAssistant, txtContainer;
        private ComboBox cmbJob;
        private Button btnAdd, btnUpdate, btnDelete, btnClear, btnAssignLoad;
        private Label lblLoadId, lblJob, lblLorry, lblDriver, lblAssistant, lblContainer, lblTitle;
        private GroupBox groupBoxQuickAssign;

        private void LoadLoads()
        {
            string query = @"SELECT l.LoadID, l.JobID, 
                            CONCAT('Job #', l.JobID, ' - ', j.StartLocation, ' to ', j.Destination) as JobDetails,
                            l.Lorry, l.Driver, l.Assistant, l.Container,
                            j.Status as JobStatus
                            FROM LoadDetails l 
                            INNER JOIN Job j ON l.JobID = j.JobID 
                            ORDER BY l.LoadID";
            loadDataTable = dbConnection.ExecuteSelect(query);
            dgvLoads.DataSource = loadDataTable;

            // Hide JobID column
            if (dgvLoads.Columns["JobID"] != null)
            {
                dgvLoads.Columns["JobID"].Visible = false;
            }
        }

        private void LoadJobs()
        {
            string query = @"SELECT j.JobID, 
                            CONCAT('Job #', j.JobID, ' - ', c.Name, ' (', j.StartLocation, ' to ', j.Destination, ')') as JobDisplay
                            FROM Job j 
                            INNER JOIN Customer c ON j.CustomerID = c.CustomerID 
                            WHERE j.Status IN ('Approved', 'In Progress')
                            ORDER BY j.JobID";
            var jobData = dbConnection.ExecuteSelect(query);

            cmbJob.DisplayMember = "JobDisplay";
            cmbJob.ValueMember = "JobID";
            cmbJob.DataSource = jobData;
        }

        private void dgvLoads_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLoads.CurrentRow != null)
            {
                DataGridViewRow row = dgvLoads.CurrentRow;
                txtLoadId.Text = row.Cells["LoadID"].Value?.ToString();
                cmbJob.SelectedValue = row.Cells["JobID"].Value;
                txtLorry.Text = row.Cells["Lorry"].Value?.ToString();
                txtDriver.Text = row.Cells["Driver"].Value?.ToString();
                txtAssistant.Text = row.Cells["Assistant"].Value?.ToString();
                txtContainer.Text = row.Cells["Container"].Value?.ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                string query = @"INSERT INTO LoadDetails (JobID, Lorry, Driver, Assistant, Container) 
                                VALUES (@jobId, @lorry, @driver, @assistant, @container)";

                MySqlParameter[] parameters = {
                    new MySqlParameter("@jobId", cmbJob.SelectedValue),
                    new MySqlParameter("@lorry", txtLorry.Text.Trim()),
                    new MySqlParameter("@driver", txtDriver.Text.Trim()),
                    new MySqlParameter("@assistant", txtAssistant.Text.Trim()),
                    new MySqlParameter("@container", txtContainer.Text.Trim())
                };

                if (dbConnection.ExecuteNonQuery(query, parameters))
                {
                    // Update job status to "In Progress"
                    UpdateJobStatus((int)cmbJob.SelectedValue, "In Progress");

                    MessageBox.Show("Load assigned successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadLoads();
                    LoadJobs(); // Refresh jobs list
                    ClearForm();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLoadId.Text))
            {
                MessageBox.Show("Please select a load to update.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ValidateInput())
            {
                string query = @"UPDATE LoadDetails SET JobID = @jobId, Lorry = @lorry, 
                                Driver = @driver, Assistant = @assistant, Container = @container 
                                WHERE LoadID = @loadId";

                MySqlParameter[] parameters = {
                    new MySqlParameter("@jobId", cmbJob.SelectedValue),
                    new MySqlParameter("@lorry", txtLorry.Text.Trim()),
                    new MySqlParameter("@driver", txtDriver.Text.Trim()),
                    new MySqlParameter("@assistant", txtAssistant.Text.Trim()),
                    new MySqlParameter("@container", txtContainer.Text.Trim()),
                    new MySqlParameter("@loadId", Convert.ToInt32(txtLoadId.Text))
                };

                if (dbConnection.ExecuteNonQuery(query, parameters))
                {
                    MessageBox.Show("Load updated successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadLoads();
                    ClearForm();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLoadId.Text))
            {
                MessageBox.Show("Please select a load to delete.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this load assignment?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string query = "DELETE FROM LoadDetails WHERE LoadID = @loadId";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@loadId", Convert.ToInt32(txtLoadId.Text))
                };

                if (dbConnection.ExecuteNonQuery(query, parameters))
                {
                    MessageBox.Show("Load assignment deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadLoads();
                    LoadJobs(); // Refresh jobs list
                    ClearForm();
                }
            }
        }

        private void btnAssignLoad_Click(object sender, EventArgs e)
        {
            if (cmbJob.SelectedValue == null)
            {
                MessageBox.Show("Please select a job to assign.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Auto-generate load details
            Random rand = new Random();
            txtLorry.Text = $"TRK-{rand.Next(100, 999):D3}";
            txtDriver.Text = $"Driver-{rand.Next(1, 100):D2}";
            txtAssistant.Text = $"Assistant-{rand.Next(1, 100):D2}";
            txtContainer.Text = $"CNT-{rand.Next(100, 999):D3}";

            MessageBox.Show("Load details auto-generated! Please review and click 'Add' to confirm.",
                "Auto Assignment", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtLoadId.Text = "";
            if (cmbJob.Items.Count > 0) cmbJob.SelectedIndex = 0;
            txtLorry.Text = "";
            txtDriver.Text = "";
            txtAssistant.Text = "";
            txtContainer.Text = "";
        }

        private bool ValidateInput()
        {
            if (cmbJob.SelectedValue == null)
            {
                MessageBox.Show("Please select a job.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLorry.Text))
            {
                MessageBox.Show("Please enter lorry information.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDriver.Text))
            {
                MessageBox.Show("Please enter driver name.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtContainer.Text))
            {
                MessageBox.Show("Please enter container information.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void UpdateJobStatus(int jobId, string status)
        {
            string query = "UPDATE Job SET Status = @status WHERE JobID = @jobId";
            MySqlParameter[] parameters = {
                new MySqlParameter("@status", status),
                new MySqlParameter("@jobId", jobId)
            };
            dbConnection.ExecuteNonQuery(query, parameters);
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