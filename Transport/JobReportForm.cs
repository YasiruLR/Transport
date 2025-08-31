using System.Data;
using MySql.Data.MySqlClient;
using System.Drawing.Drawing2D;

namespace Transport
{
    public partial class JobReportForm : Form
    {
        private DatabaseConnection dbConnection;
        private DataGridView dgvJobs;
        private Panel filterPanel;
        private ComboBox cmbStatusFilter;
        private ComboBox cmbCustomerFilter;
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;
        private Button btnGenerateReport;
        private Button btnExport;
        private Button btnPrint;
        private Label lblTotalJobs;
        private Label lblPendingJobs;
        private Label lblCompletedJobs;
        private Label lblRevenue;

        public JobReportForm()
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            LoadCustomers();
            LoadJobReport();
            LoadJobStats();
        }

        private void InitializeComponent()
        {
            this.dgvJobs = new DataGridView();
            this.filterPanel = new Panel();
            this.cmbStatusFilter = new ComboBox();
            this.cmbCustomerFilter = new ComboBox();
            this.dtpFromDate = new DateTimePicker();
            this.dtpToDate = new DateTimePicker();
            this.btnGenerateReport = new Button();
            this.btnExport = new Button();
            this.btnPrint = new Button();
            this.lblTotalJobs = new Label();
            this.lblPendingJobs = new Label();
            this.lblCompletedJobs = new Label();
            this.lblRevenue = new Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJobs)).BeginInit();
            this.SuspendLayout();

            // Form Properties
            this.Text = "Job Report - Transport Management System";
            this.Size = new Size(1400, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.WindowState = FormWindowState.Maximized;

            // Header Panel
            Panel headerPanel = new Panel();
            headerPanel.Location = new Point(0, 0);
            headerPanel.Size = new Size(1400, 80);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.BackColor = Color.FromArgb(0, 123, 255);

            Label lblTitle = new Label();
            lblTitle.Text = "?? Job Report & Analytics";
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(30, 25);
            lblTitle.Size = new Size(400, 30);
            lblTitle.BackColor = Color.Transparent;

            Button btnClose = new Button();
            btnClose.Text = "?";
            btnClose.Location = new Point(1350, 10);
            btnClose.Size = new Size(40, 40);
            btnClose.BackColor = Color.FromArgb(220, 53, 69);
            btnClose.ForeColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnClose.Cursor = Cursors.Hand;
            btnClose.Click += (s, e) => this.Close();

            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(btnClose);

            // Filter Panel
            this.filterPanel.Location = new Point(20, 100);
            this.filterPanel.Size = new Size(1360, 100);
            this.filterPanel.BackColor = Color.White;
            this.filterPanel.BorderStyle = BorderStyle.FixedSingle;

            Label lblFilter = new Label();
            lblFilter.Text = "Filter Options:";
            lblFilter.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblFilter.Location = new Point(20, 15);
            lblFilter.Size = new Size(120, 25);

            // First Row of Filters
            Label lblStatus = new Label();
            lblStatus.Text = "Status:";
            lblStatus.Location = new Point(20, 50);
            lblStatus.Size = new Size(50, 25);

            this.cmbStatusFilter.Location = new Point(75, 48);
            this.cmbStatusFilter.Size = new Size(120, 25);
            this.cmbStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbStatusFilter.Items.AddRange(new string[] { "All Status", "Pending", "Approved", "In Progress", "Completed", "Cancelled" });
            this.cmbStatusFilter.SelectedIndex = 0;

            Label lblCustomer = new Label();
            lblCustomer.Text = "Customer:";
            lblCustomer.Location = new Point(220, 50);
            lblCustomer.Size = new Size(70, 25);

            this.cmbCustomerFilter.Location = new Point(295, 48);
            this.cmbCustomerFilter.Size = new Size(200, 25);
            this.cmbCustomerFilter.DropDownStyle = ComboBoxStyle.DropDownList;

            Label lblFromDate = new Label();
            lblFromDate.Text = "From:";
            lblFromDate.Location = new Point(520, 50);
            lblFromDate.Size = new Size(50, 25);

            this.dtpFromDate.Location = new Point(575, 48);
            this.dtpFromDate.Size = new Size(150, 25);
            this.dtpFromDate.Value = DateTime.Now.AddMonths(-3);

            Label lblToDate = new Label();
            lblToDate.Text = "To:";
            lblToDate.Location = new Point(745, 50);
            lblToDate.Size = new Size(30, 25);

            this.dtpToDate.Location = new Point(780, 48);
            this.dtpToDate.Size = new Size(150, 25);
            this.dtpToDate.Value = DateTime.Now;

            this.btnGenerateReport.Text = "Generate Report";
            this.btnGenerateReport.Location = new Point(950, 45);
            this.btnGenerateReport.Size = new Size(120, 30);
            this.btnGenerateReport.BackColor = Color.FromArgb(0, 123, 255);
            this.btnGenerateReport.ForeColor = Color.White;
            this.btnGenerateReport.FlatStyle = FlatStyle.Flat;
            this.btnGenerateReport.FlatAppearance.BorderSize = 0;
            this.btnGenerateReport.Cursor = Cursors.Hand;
            this.btnGenerateReport.Click += new EventHandler(this.btnGenerateReport_Click);

            this.btnExport.Text = "Export";
            this.btnExport.Location = new Point(1080, 45);
            this.btnExport.Size = new Size(80, 30);
            this.btnExport.BackColor = Color.FromArgb(40, 167, 69);
            this.btnExport.ForeColor = Color.White;
            this.btnExport.FlatStyle = FlatStyle.Flat;
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.Cursor = Cursors.Hand;
            this.btnExport.Click += new EventHandler(this.btnExport_Click);

            this.btnPrint.Text = "Print";
            this.btnPrint.Location = new Point(1170, 45);
            this.btnPrint.Size = new Size(80, 30);
            this.btnPrint.BackColor = Color.FromArgb(108, 117, 125);
            this.btnPrint.ForeColor = Color.White;
            this.btnPrint.FlatStyle = FlatStyle.Flat;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.Cursor = Cursors.Hand;
            this.btnPrint.Click += new EventHandler(this.btnPrint_Click);

            this.filterPanel.Controls.Add(lblFilter);
            this.filterPanel.Controls.Add(lblStatus);
            this.filterPanel.Controls.Add(this.cmbStatusFilter);
            this.filterPanel.Controls.Add(lblCustomer);
            this.filterPanel.Controls.Add(this.cmbCustomerFilter);
            this.filterPanel.Controls.Add(lblFromDate);
            this.filterPanel.Controls.Add(this.dtpFromDate);
            this.filterPanel.Controls.Add(lblToDate);
            this.filterPanel.Controls.Add(this.dtpToDate);
            this.filterPanel.Controls.Add(this.btnGenerateReport);
            this.filterPanel.Controls.Add(this.btnExport);
            this.filterPanel.Controls.Add(this.btnPrint);

            // Stats Panel
            Panel statsPanel = new Panel();
            statsPanel.Location = new Point(20, 220);
            statsPanel.Size = new Size(1360, 80);
            statsPanel.BackColor = Color.FromArgb(248, 249, 250);
            statsPanel.BorderStyle = BorderStyle.FixedSingle;

            Label lblStatsTitle = new Label();
            lblStatsTitle.Text = "Job Statistics:";
            lblStatsTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblStatsTitle.Location = new Point(20, 15);
            lblStatsTitle.Size = new Size(150, 25);

            this.lblTotalJobs.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.lblTotalJobs.Location = new Point(20, 45);
            this.lblTotalJobs.Size = new Size(150, 20);

            this.lblPendingJobs.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.lblPendingJobs.Location = new Point(180, 45);
            this.lblPendingJobs.Size = new Size(150, 20);

            this.lblCompletedJobs.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.lblCompletedJobs.Location = new Point(340, 45);
            this.lblCompletedJobs.Size = new Size(150, 20);

            this.lblRevenue.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.lblRevenue.Location = new Point(500, 45);
            this.lblRevenue.Size = new Size(200, 20);

            statsPanel.Controls.Add(lblStatsTitle);
            statsPanel.Controls.Add(this.lblTotalJobs);
            statsPanel.Controls.Add(this.lblPendingJobs);
            statsPanel.Controls.Add(this.lblCompletedJobs);
            statsPanel.Controls.Add(this.lblRevenue);

            // DataGridView
            this.dgvJobs.Location = new Point(20, 320);
            this.dgvJobs.Size = new Size(1360, 500);
            this.dgvJobs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvJobs.MultiSelect = false;
            this.dgvJobs.ReadOnly = true;
            this.dgvJobs.AllowUserToAddRows = false;
            this.dgvJobs.AllowUserToDeleteRows = false;
            this.dgvJobs.BackgroundColor = Color.White;
            this.dgvJobs.BorderStyle = BorderStyle.FixedSingle;
            this.dgvJobs.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 123, 255);
            this.dgvJobs.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvJobs.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.dgvJobs.EnableHeadersVisualStyles = false;
            this.dgvJobs.RowTemplate.Height = 30;

            // Add controls to form
            this.Controls.Add(headerPanel);
            this.Controls.Add(this.filterPanel);
            this.Controls.Add(statsPanel);
            this.Controls.Add(this.dgvJobs);

            ((System.ComponentModel.ISupportInitialize)(this.dgvJobs)).EndInit();
            this.ResumeLayout(false);
        }

        private void LoadCustomers()
        {
            try
            {
                string query = "SELECT CustomerID, Name FROM Customer ORDER BY Name";
                DataTable customerData = dbConnection.ExecuteSelect(query);

                DataRow allRow = customerData.NewRow();
                allRow["CustomerID"] = 0;
                allRow["Name"] = "All Customers";
                customerData.Rows.InsertAt(allRow, 0);

                cmbCustomerFilter.DisplayMember = "Name";
                cmbCustomerFilter.ValueMember = "CustomerID";
                cmbCustomerFilter.DataSource = customerData;
                cmbCustomerFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customers: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadJobReport()
        {
            try
            {
                string query = @"
                    SELECT 
                        j.JobID,
                        c.Name as CustomerName,
                        j.StartLocation,
                        j.Destination,
                        j.Status,
                        j.RequestDate,
                        j.EstimatedWeight,
                        j.PreferredDate,
                        j.Urgency,
                        COALESCE(ld.Lorry, 'Not Assigned') as AssignedTruck,
                        COALESCE(ld.Driver, 'Not Assigned') as AssignedDriver,
                        CASE j.Status
                            WHEN 'Pending' THEN 'Waiting for Approval'
                            WHEN 'Approved' THEN 'Ready for Assignment'
                            WHEN 'In Progress' THEN 'Currently Active'
                            WHEN 'Completed' THEN 'Successfully Completed'
                            WHEN 'Cancelled' THEN 'Cancelled by Admin'
                            ELSE j.Status
                        END as StatusDescription
                    FROM Job j
                    INNER JOIN Customer c ON j.CustomerID = c.CustomerID
                    LEFT JOIN LoadDetails ld ON j.JobID = ld.JobID
                    ORDER BY j.RequestDate DESC";

                DataTable jobData = dbConnection.ExecuteSelect(query);
                dgvJobs.DataSource = jobData;

                FormatJobGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading job report: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatJobGrid()
        {
            // Format columns
            if (dgvJobs.Columns["JobID"] != null)
            {
                dgvJobs.Columns["JobID"].HeaderText = "Job ID";
                dgvJobs.Columns["JobID"].Width = 80;
            }

            if (dgvJobs.Columns["CustomerName"] != null)
            {
                dgvJobs.Columns["CustomerName"].HeaderText = "Customer";
                dgvJobs.Columns["CustomerName"].Width = 150;
            }

            if (dgvJobs.Columns["StartLocation"] != null)
            {
                dgvJobs.Columns["StartLocation"].HeaderText = "From";
                dgvJobs.Columns["StartLocation"].Width = 120;
            }

            if (dgvJobs.Columns["Destination"] != null)
            {
                dgvJobs.Columns["Destination"].HeaderText = "To";
                dgvJobs.Columns["Destination"].Width = 120;
            }

            if (dgvJobs.Columns["Status"] != null)
            {
                dgvJobs.Columns["Status"].HeaderText = "Status";
                dgvJobs.Columns["Status"].Width = 100;
            }

            if (dgvJobs.Columns["RequestDate"] != null)
            {
                dgvJobs.Columns["RequestDate"].HeaderText = "Request Date";
                dgvJobs.Columns["RequestDate"].DefaultCellStyle.Format = "MMM dd, yyyy";
                dgvJobs.Columns["RequestDate"].Width = 120;
            }

            if (dgvJobs.Columns["EstimatedWeight"] != null)
            {
                dgvJobs.Columns["EstimatedWeight"].HeaderText = "Weight (kg)";
                dgvJobs.Columns["EstimatedWeight"].Width = 100;
            }

            if (dgvJobs.Columns["PreferredDate"] != null)
            {
                dgvJobs.Columns["PreferredDate"].HeaderText = "Preferred Date";
                dgvJobs.Columns["PreferredDate"].DefaultCellStyle.Format = "MMM dd, yyyy";
                dgvJobs.Columns["PreferredDate"].Width = 120;
            }

            if (dgvJobs.Columns["Urgency"] != null)
            {
                dgvJobs.Columns["Urgency"].HeaderText = "Priority";
                dgvJobs.Columns["Urgency"].Width = 80;
            }

            if (dgvJobs.Columns["AssignedTruck"] != null)
            {
                dgvJobs.Columns["AssignedTruck"].HeaderText = "Truck";
                dgvJobs.Columns["AssignedTruck"].Width = 120;
            }

            if (dgvJobs.Columns["AssignedDriver"] != null)
            {
                dgvJobs.Columns["AssignedDriver"].HeaderText = "Driver";
                dgvJobs.Columns["AssignedDriver"].Width = 120;
            }

            if (dgvJobs.Columns["StatusDescription"] != null)
            {
                dgvJobs.Columns["StatusDescription"].HeaderText = "Description";
                dgvJobs.Columns["StatusDescription"].Width = 150;
            }

            // Color code rows based on status
            foreach (DataGridViewRow row in dgvJobs.Rows)
            {
                if (row.Cells["Status"].Value != null)
                {
                    string status = row.Cells["Status"].Value.ToString();
                    switch (status)
                    {
                        case "Pending":
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 243, 205);
                            break;
                        case "Approved":
                            row.DefaultCellStyle.BackColor = Color.FromArgb(217, 237, 247);
                            break;
                        case "In Progress":
                            row.DefaultCellStyle.BackColor = Color.FromArgb(209, 231, 221);
                            break;
                        case "Completed":
                            row.DefaultCellStyle.BackColor = Color.FromArgb(212, 237, 218);
                            break;
                        case "Cancelled":
                            row.DefaultCellStyle.BackColor = Color.FromArgb(248, 215, 218);
                            break;
                    }
                }
            }
        }

        private void LoadJobStats()
        {
            try
            {
                // Total jobs
                string totalQuery = "SELECT COUNT(*) FROM Job";
                int totalJobs = Convert.ToInt32(dbConnection.ExecuteScalar(totalQuery));

                // Pending jobs
                string pendingQuery = "SELECT COUNT(*) FROM Job WHERE Status = 'Pending'";
                int pendingJobs = Convert.ToInt32(dbConnection.ExecuteScalar(pendingQuery));

                // Completed jobs
                string completedQuery = "SELECT COUNT(*) FROM Job WHERE Status = 'Completed'";
                int completedJobs = Convert.ToInt32(dbConnection.ExecuteScalar(completedQuery));

                // Calculate completion rate
                double completionRate = totalJobs > 0 ? (double)completedJobs / totalJobs * 100 : 0;

                lblTotalJobs.Text = $"Total Jobs: {totalJobs}";
                lblPendingJobs.Text = $"Pending: {pendingJobs}";
                lblCompletedJobs.Text = $"Completed: {completedJobs}";
                lblRevenue.Text = $"Completion Rate: {completionRate:F1}%";
            }
            catch (Exception ex)
            {
                lblTotalJobs.Text = "Stats unavailable";
                lblPendingJobs.Text = "";
                lblCompletedJobs.Text = "";
                lblRevenue.Text = "";
            }
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                string query = GetFilteredJobQuery();
                DataTable filteredData = dbConnection.ExecuteSelect(query);
                dgvJobs.DataSource = filteredData;
                FormatJobGrid();

                MessageBox.Show($"Report generated successfully! Found {filteredData.Rows.Count} jobs.", 
                    "Report Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetFilteredJobQuery()
        {
            string baseQuery = @"
                SELECT 
                    j.JobID,
                    c.Name as CustomerName,
                    j.StartLocation,
                    j.Destination,
                    j.Status,
                    j.RequestDate,
                    j.EstimatedWeight,
                    j.PreferredDate,
                    j.Urgency,
                    COALESCE(ld.Lorry, 'Not Assigned') as AssignedTruck,
                    COALESCE(ld.Driver, 'Not Assigned') as AssignedDriver,
                    CASE j.Status
                        WHEN 'Pending' THEN 'Waiting for Approval'
                        WHEN 'Approved' THEN 'Ready for Assignment'
                        WHEN 'In Progress' THEN 'Currently Active'
                        WHEN 'Completed' THEN 'Successfully Completed'
                        WHEN 'Cancelled' THEN 'Cancelled by Admin'
                        ELSE j.Status
                    END as StatusDescription
                FROM Job j
                INNER JOIN Customer c ON j.CustomerID = c.CustomerID
                LEFT JOIN LoadDetails ld ON j.JobID = ld.JobID";

            List<string> conditions = new List<string>();

            // Status filter
            string selectedStatus = cmbStatusFilter.SelectedItem.ToString();
            if (selectedStatus != "All Status")
            {
                conditions.Add($"j.Status = '{selectedStatus}'");
            }

            // Customer filter
            if (cmbCustomerFilter.SelectedValue != null && Convert.ToInt32(cmbCustomerFilter.SelectedValue) > 0)
            {
                conditions.Add($"j.CustomerID = {cmbCustomerFilter.SelectedValue}");
            }

            // Date range filter
            conditions.Add($"j.RequestDate BETWEEN '{dtpFromDate.Value:yyyy-MM-dd}' AND '{dtpToDate.Value:yyyy-MM-dd}'");

            if (conditions.Count > 0)
            {
                baseQuery += " WHERE " + string.Join(" AND ", conditions);
            }

            return baseQuery + " ORDER BY j.RequestDate DESC";
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                saveDialog.FileName = $"JobReport_{DateTime.Now:yyyyMMdd}.csv";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToCsv(saveDialog.FileName);
                    MessageBox.Show("Report exported successfully!", "Export Complete", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting report: {ex.Message}", "Export Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToCsv(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                // Write headers
                string[] headers = new string[dgvJobs.Columns.Count];
                for (int i = 0; i < dgvJobs.Columns.Count; i++)
                {
                    headers[i] = dgvJobs.Columns[i].HeaderText;
                }
                writer.WriteLine(string.Join(",", headers));

                // Write data
                foreach (DataGridViewRow row in dgvJobs.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string[] values = new string[row.Cells.Count];
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            values[i] = row.Cells[i].Value?.ToString() ?? "";
                        }
                        writer.WriteLine(string.Join(",", values));
                    }
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Print functionality would be implemented here.\nThis could integrate with Crystal Reports or Windows Printing API.", 
                "Print Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
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