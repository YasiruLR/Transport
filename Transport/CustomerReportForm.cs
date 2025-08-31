using System.Data;
using MySql.Data.MySqlClient;
using System.Drawing.Drawing2D;

namespace Transport
{
    public partial class CustomerReportForm : Form
    {
        private DatabaseConnection dbConnection;
        private DataGridView dgvCustomers;
        private Panel filterPanel;
        private ComboBox cmbFilter;
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;
        private Button btnGenerateReport;
        private Button btnExport;
        private Button btnPrint;
        private Label lblTotalCustomers;
        private Label lblActiveCustomers;
        private Label lblRecentCustomers;

        public CustomerReportForm()
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            LoadCustomerReport();
            LoadCustomerStats();
        }

        private void InitializeComponent()
        {
            this.dgvCustomers = new DataGridView();
            this.filterPanel = new Panel();
            this.cmbFilter = new ComboBox();
            this.dtpFromDate = new DateTimePicker();
            this.dtpToDate = new DateTimePicker();
            this.btnGenerateReport = new Button();
            this.btnExport = new Button();
            this.btnPrint = new Button();
            this.lblTotalCustomers = new Label();
            this.lblActiveCustomers = new Label();
            this.lblRecentCustomers = new Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).BeginInit();
            this.SuspendLayout();

            // Form Properties
            this.Text = "Customer Report - Transport Management System";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 250);

            // Header Panel
            Panel headerPanel = new Panel();
            headerPanel.Location = new Point(0, 0);
            headerPanel.Size = new Size(1200, 80);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.BackColor = Color.FromArgb(40, 167, 69);

            Label lblTitle = new Label();
            lblTitle.Text = "Customer Report & Analytics";
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(30, 25);
            lblTitle.Size = new Size(400, 30);
            lblTitle.BackColor = Color.Transparent;

            Button btnClose = new Button();
            btnClose.Text = "X";
            btnClose.Location = new Point(1150, 10);
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
            this.filterPanel.Size = new Size(1160, 100);
            this.filterPanel.BackColor = Color.White;
            this.filterPanel.BorderStyle = BorderStyle.FixedSingle;

            Label lblFilter = new Label();
            lblFilter.Text = "Filter Options:";
            lblFilter.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblFilter.Location = new Point(20, 15);
            lblFilter.Size = new Size(120, 25);

            Label lblFilterType = new Label();
            lblFilterType.Text = "Filter By:";
            lblFilterType.Location = new Point(20, 50);
            lblFilterType.Size = new Size(60, 25);

            this.cmbFilter.Location = new Point(90, 48);
            this.cmbFilter.Size = new Size(150, 25);
            this.cmbFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbFilter.Items.AddRange(new string[] { "All Customers", "Recent (30 days)", "Active (with jobs)", "Inactive" });
            this.cmbFilter.SelectedIndex = 0;

            Label lblFromDate = new Label();
            lblFromDate.Text = "From:";
            lblFromDate.Location = new Point(270, 50);
            lblFromDate.Size = new Size(50, 25);

            this.dtpFromDate.Location = new Point(325, 48);
            this.dtpFromDate.Size = new Size(150, 25);
            this.dtpFromDate.Value = DateTime.Now.AddMonths(-6);

            Label lblToDate = new Label();
            lblToDate.Text = "To:";
            lblToDate.Location = new Point(495, 50);
            lblToDate.Size = new Size(30, 25);

            this.dtpToDate.Location = new Point(530, 48);
            this.dtpToDate.Size = new Size(150, 25);
            this.dtpToDate.Value = DateTime.Now;

            this.btnGenerateReport.Text = "Generate Report";
            this.btnGenerateReport.Location = new Point(700, 45);
            this.btnGenerateReport.Size = new Size(120, 30);
            this.btnGenerateReport.BackColor = Color.FromArgb(0, 123, 255);
            this.btnGenerateReport.ForeColor = Color.White;
            this.btnGenerateReport.FlatStyle = FlatStyle.Flat;
            this.btnGenerateReport.FlatAppearance.BorderSize = 0;
            this.btnGenerateReport.Cursor = Cursors.Hand;
            this.btnGenerateReport.Click += new EventHandler(this.btnGenerateReport_Click);

            this.btnExport.Text = "Export";
            this.btnExport.Location = new Point(830, 45);
            this.btnExport.Size = new Size(80, 30);
            this.btnExport.BackColor = Color.FromArgb(40, 167, 69);
            this.btnExport.ForeColor = Color.White;
            this.btnExport.FlatStyle = FlatStyle.Flat;
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.Cursor = Cursors.Hand;
            this.btnExport.Click += new EventHandler(this.btnExport_Click);

            this.btnPrint.Text = "Print";
            this.btnPrint.Location = new Point(920, 45);
            this.btnPrint.Size = new Size(80, 30);
            this.btnPrint.BackColor = Color.FromArgb(108, 117, 125);
            this.btnPrint.ForeColor = Color.White;
            this.btnPrint.FlatStyle = FlatStyle.Flat;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.Cursor = Cursors.Hand;
            this.btnPrint.Click += new EventHandler(this.btnPrint_Click);

            this.filterPanel.Controls.Add(lblFilter);
            this.filterPanel.Controls.Add(lblFilterType);
            this.filterPanel.Controls.Add(this.cmbFilter);
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
            statsPanel.Size = new Size(1160, 80);
            statsPanel.BackColor = Color.FromArgb(248, 249, 250);
            statsPanel.BorderStyle = BorderStyle.FixedSingle;

            Label lblStatsTitle = new Label();
            lblStatsTitle.Text = "Customer Statistics:";
            lblStatsTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblStatsTitle.Location = new Point(20, 15);
            lblStatsTitle.Size = new Size(150, 25);

            this.lblTotalCustomers.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.lblTotalCustomers.Location = new Point(20, 45);
            this.lblTotalCustomers.Size = new Size(200, 20);

            this.lblActiveCustomers.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.lblActiveCustomers.Location = new Point(240, 45);
            this.lblActiveCustomers.Size = new Size(200, 20);

            this.lblRecentCustomers.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.lblRecentCustomers.Location = new Point(460, 45);
            this.lblRecentCustomers.Size = new Size(200, 20);

            statsPanel.Controls.Add(lblStatsTitle);
            statsPanel.Controls.Add(this.lblTotalCustomers);
            statsPanel.Controls.Add(this.lblActiveCustomers);
            statsPanel.Controls.Add(this.lblRecentCustomers);

            // DataGridView
            this.dgvCustomers.Location = new Point(20, 320);
            this.dgvCustomers.Size = new Size(1160, 400);
            this.dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvCustomers.MultiSelect = false;
            this.dgvCustomers.ReadOnly = true;
            this.dgvCustomers.AllowUserToAddRows = false;
            this.dgvCustomers.AllowUserToDeleteRows = false;
            this.dgvCustomers.BackgroundColor = Color.White;
            this.dgvCustomers.BorderStyle = BorderStyle.FixedSingle;
            this.dgvCustomers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(40, 167, 69);
            this.dgvCustomers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvCustomers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.dgvCustomers.EnableHeadersVisualStyles = false;

            // Add controls to form
            this.Controls.Add(headerPanel);
            this.Controls.Add(this.filterPanel);
            this.Controls.Add(statsPanel);
            this.Controls.Add(this.dgvCustomers);

            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).EndInit();
            this.ResumeLayout(false);
        }

        private void LoadCustomerReport()
        {
            try
            {
                string query = @"
                    SELECT 
                        c.CustomerID,
                        c.Name,
                        c.Email,
                        c.Phone,
                        c.Address,
                        c.RegisteredDate,
                        COUNT(j.JobID) as TotalJobs,
                        COALESCE(MAX(j.RequestDate), 'No Jobs') as LastJobDate,
                        CASE 
                            WHEN COUNT(j.JobID) > 0 THEN 'Active'
                            ELSE 'Inactive'
                        END as Status
                    FROM Customer c
                    LEFT JOIN Job j ON c.CustomerID = j.CustomerID
                    GROUP BY c.CustomerID, c.Name, c.Email, c.Phone, c.Address, c.RegisteredDate
                    ORDER BY c.RegisteredDate DESC";

                DataTable customerData = dbConnection.ExecuteSelect(query);
                dgvCustomers.DataSource = customerData;

                // Format columns
                if (dgvCustomers.Columns["CustomerID"] != null)
                    dgvCustomers.Columns["CustomerID"].HeaderText = "ID";
                
                if (dgvCustomers.Columns["Name"] != null)
                    dgvCustomers.Columns["Name"].HeaderText = "Customer Name";
                
                if (dgvCustomers.Columns["Email"] != null)
                    dgvCustomers.Columns["Email"].HeaderText = "Email Address";
                
                if (dgvCustomers.Columns["Phone"] != null)
                    dgvCustomers.Columns["Phone"].HeaderText = "Phone Number";
                
                if (dgvCustomers.Columns["Address"] != null)
                    dgvCustomers.Columns["Address"].HeaderText = "Address";
                
                if (dgvCustomers.Columns["RegisteredDate"] != null)
                {
                    dgvCustomers.Columns["RegisteredDate"].HeaderText = "Registration Date";
                    dgvCustomers.Columns["RegisteredDate"].DefaultCellStyle.Format = "MMM dd, yyyy";
                }
                
                if (dgvCustomers.Columns["TotalJobs"] != null)
                    dgvCustomers.Columns["TotalJobs"].HeaderText = "Total Jobs";
                
                if (dgvCustomers.Columns["LastJobDate"] != null)
                    dgvCustomers.Columns["LastJobDate"].HeaderText = "Last Job Date";
                
                if (dgvCustomers.Columns["Status"] != null)
                    dgvCustomers.Columns["Status"].HeaderText = "Status";

                // Auto resize columns
                dgvCustomers.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customer report: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCustomerStats()
        {
            try
            {
                // Total customers
                string totalQuery = "SELECT COUNT(*) FROM Customer";
                int totalCustomers = Convert.ToInt32(dbConnection.ExecuteScalar(totalQuery));

                // Active customers (with jobs)
                string activeQuery = @"
                    SELECT COUNT(DISTINCT c.CustomerID) 
                    FROM Customer c 
                    INNER JOIN Job j ON c.CustomerID = j.CustomerID";
                int activeCustomers = Convert.ToInt32(dbConnection.ExecuteScalar(activeQuery));

                // Recent customers (last 30 days)
                string recentQuery = "SELECT COUNT(*) FROM Customer WHERE RegisteredDate >= DATE_SUB(NOW(), INTERVAL 30 DAY)";
                int recentCustomers = Convert.ToInt32(dbConnection.ExecuteScalar(recentQuery));

                lblTotalCustomers.Text = $"Total Customers: {totalCustomers}";
                lblActiveCustomers.Text = $"Active Customers: {activeCustomers}";
                lblRecentCustomers.Text = $"Recent (30 days): {recentCustomers}";
            }
            catch (Exception ex)
            {
                lblTotalCustomers.Text = "Stats unavailable";
                lblActiveCustomers.Text = "";
                lblRecentCustomers.Text = "";
            }
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                string query = GetFilteredQuery();
                DataTable filteredData = dbConnection.ExecuteSelect(query);
                dgvCustomers.DataSource = filteredData;

                MessageBox.Show($"Report generated successfully! Found {filteredData.Rows.Count} customers.", 
                    "Report Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetFilteredQuery()
        {
            string baseQuery = @"
                SELECT 
                    c.CustomerID,
                    c.Name,
                    c.Email,
                    c.Phone,
                    c.Address,
                    c.RegisteredDate,
                    COUNT(j.JobID) as TotalJobs,
                    COALESCE(MAX(j.RequestDate), 'No Jobs') as LastJobDate,
                    CASE 
                        WHEN COUNT(j.JobID) > 0 THEN 'Active'
                        ELSE 'Inactive'
                    END as Status
                FROM Customer c
                LEFT JOIN Job j ON c.CustomerID = j.CustomerID";

            string whereClause = "";
            string selectedFilter = cmbFilter.SelectedItem.ToString();

            switch (selectedFilter)
            {
                case "Recent (30 days)":
                    whereClause = " WHERE c.RegisteredDate >= DATE_SUB(NOW(), INTERVAL 30 DAY)";
                    break;
                case "Active (with jobs)":
                    whereClause = " WHERE EXISTS (SELECT 1 FROM Job WHERE CustomerID = c.CustomerID)";
                    break;
                case "Inactive":
                    whereClause = " WHERE NOT EXISTS (SELECT 1 FROM Job WHERE CustomerID = c.CustomerID)";
                    break;
            }

            // Add date range filter
            if (!string.IsNullOrEmpty(whereClause))
            {
                whereClause += $" AND c.RegisteredDate BETWEEN '{dtpFromDate.Value:yyyy-MM-dd}' AND '{dtpToDate.Value:yyyy-MM-dd}'";
            }
            else
            {
                whereClause = $" WHERE c.RegisteredDate BETWEEN '{dtpFromDate.Value:yyyy-MM-dd}' AND '{dtpToDate.Value:yyyy-MM-dd}'";
            }

            return baseQuery + whereClause + " GROUP BY c.CustomerID, c.Name, c.Email, c.Phone, c.Address, c.RegisteredDate ORDER BY c.RegisteredDate DESC";
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                saveDialog.FileName = $"CustomerReport_{DateTime.Now:yyyyMMdd}.csv";

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
                string[] headers = new string[dgvCustomers.Columns.Count];
                for (int i = 0; i < dgvCustomers.Columns.Count; i++)
                {
                    headers[i] = dgvCustomers.Columns[i].HeaderText;
                }
                writer.WriteLine(string.Join(",", headers));

                // Write data
                foreach (DataGridViewRow row in dgvCustomers.Rows)
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