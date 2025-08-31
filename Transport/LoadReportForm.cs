using System.Data;
using MySql.Data.MySqlClient;
using System.Drawing.Drawing2D;

namespace Transport
{
    public partial class LoadReportForm : Form
    {
        private DatabaseConnection dbConnection;
        private DataGridView dgvLoads;
        private Panel filterPanel;
        private ComboBox cmbStatusFilter;
        private ComboBox cmbTruckFilter;
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;
        private Button btnGenerateReport;
        private Button btnExport;
        private Button btnPrint;
        private Label lblTotalLoads;
        private Label lblActiveLoads;
        private Label lblTruckUtilization;
        private Label lblAverageWeight;

        public LoadReportForm()
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            LoadTrucks();
            LoadLoadReport();
            LoadLoadStats();
        }

        private void InitializeComponent()
        {
            this.dgvLoads = new DataGridView();
            this.filterPanel = new Panel();
            this.cmbStatusFilter = new ComboBox();
            this.cmbTruckFilter = new ComboBox();
            this.dtpFromDate = new DateTimePicker();
            this.dtpToDate = new DateTimePicker();
            this.btnGenerateReport = new Button();
            this.btnExport = new Button();
            this.btnPrint = new Button();
            this.lblTotalLoads = new Label();
            this.lblActiveLoads = new Label();
            this.lblTruckUtilization = new Label();
            this.lblAverageWeight = new Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoads)).BeginInit();
            this.SuspendLayout();

            // Form Properties
            this.Text = "Load Report - Transport Management System";
            this.Size = new Size(1400, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.WindowState = FormWindowState.Maximized;

            // Header Panel
            Panel headerPanel = new Panel();
            headerPanel.Location = new Point(0, 0);
            headerPanel.Size = new Size(1400, 80);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.BackColor = Color.FromArgb(220, 53, 69);

            Label lblTitle = new Label();
            lblTitle.Text = "?? Load Report & Analytics";
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(30, 25);
            lblTitle.Size = new Size(400, 30);
            lblTitle.BackColor = Color.Transparent;

            Button btnClose = new Button();
            btnClose.Text = "?";
            btnClose.Location = new Point(1350, 10);
            btnClose.Size = new Size(40, 40);
            btnClose.BackColor = Color.FromArgb(108, 117, 125);
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

            Label lblJobStatus = new Label();
            lblJobStatus.Text = "Job Status:";
            lblJobStatus.Location = new Point(20, 50);
            lblJobStatus.Size = new Size(80, 25);

            this.cmbStatusFilter.Location = new Point(105, 48);
            this.cmbStatusFilter.Size = new Size(120, 25);
            this.cmbStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbStatusFilter.Items.AddRange(new string[] { "All Status", "In Progress", "Completed", "Cancelled" });
            this.cmbStatusFilter.SelectedIndex = 0;

            Label lblTruck = new Label();
            lblTruck.Text = "Truck:";
            lblTruck.Location = new Point(250, 50);
            lblTruck.Size = new Size(50, 25);

            this.cmbTruckFilter.Location = new Point(305, 48);
            this.cmbTruckFilter.Size = new Size(150, 25);
            this.cmbTruckFilter.DropDownStyle = ComboBoxStyle.DropDownList;

            Label lblFromDate = new Label();
            lblFromDate.Text = "From:";
            lblFromDate.Location = new Point(480, 50);
            lblFromDate.Size = new Size(50, 25);

            this.dtpFromDate.Location = new Point(535, 48);
            this.dtpFromDate.Size = new Size(150, 25);
            this.dtpFromDate.Value = DateTime.Now.AddMonths(-3);

            Label lblToDate = new Label();
            lblToDate.Text = "To:";
            lblToDate.Location = new Point(705, 50);
            lblToDate.Size = new Size(30, 25);

            this.dtpToDate.Location = new Point(740, 48);
            this.dtpToDate.Size = new Size(150, 25);
            this.dtpToDate.Value = DateTime.Now;

            this.btnGenerateReport.Text = "Generate Report";
            this.btnGenerateReport.Location = new Point(910, 45);
            this.btnGenerateReport.Size = new Size(120, 30);
            this.btnGenerateReport.BackColor = Color.FromArgb(220, 53, 69);
            this.btnGenerateReport.ForeColor = Color.White;
            this.btnGenerateReport.FlatStyle = FlatStyle.Flat;
            this.btnGenerateReport.FlatAppearance.BorderSize = 0;
            this.btnGenerateReport.Cursor = Cursors.Hand;
            this.btnGenerateReport.Click += new EventHandler(this.btnGenerateReport_Click);

            this.btnExport.Text = "Export";
            this.btnExport.Location = new Point(1040, 45);
            this.btnExport.Size = new Size(80, 30);
            this.btnExport.BackColor = Color.FromArgb(40, 167, 69);
            this.btnExport.ForeColor = Color.White;
            this.btnExport.FlatStyle = FlatStyle.Flat;
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.Cursor = Cursors.Hand;
            this.btnExport.Click += new EventHandler(this.btnExport_Click);

            this.btnPrint.Text = "Print";
            this.btnPrint.Location = new Point(1130, 45);
            this.btnPrint.Size = new Size(80, 30);
            this.btnPrint.BackColor = Color.FromArgb(108, 117, 125);
            this.btnPrint.ForeColor = Color.White;
            this.btnPrint.FlatStyle = FlatStyle.Flat;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.Cursor = Cursors.Hand;
            this.btnPrint.Click += new EventHandler(this.btnPrint_Click);

            this.filterPanel.Controls.Add(lblFilter);
            this.filterPanel.Controls.Add(lblJobStatus);
            this.filterPanel.Controls.Add(this.cmbStatusFilter);
            this.filterPanel.Controls.Add(lblTruck);
            this.filterPanel.Controls.Add(this.cmbTruckFilter);
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
            lblStatsTitle.Text = "Load Statistics:";
            lblStatsTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblStatsTitle.Location = new Point(20, 15);
            lblStatsTitle.Size = new Size(150, 25);

            this.lblTotalLoads.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.lblTotalLoads.Location = new Point(20, 45);
            this.lblTotalLoads.Size = new Size(150, 20);

            this.lblActiveLoads.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.lblActiveLoads.Location = new Point(180, 45);
            this.lblActiveLoads.Size = new Size(150, 20);

            this.lblTruckUtilization.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.lblTruckUtilization.Location = new Point(340, 45);
            this.lblTruckUtilization.Size = new Size(200, 20);

            this.lblAverageWeight.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.lblAverageWeight.Location = new Point(550, 45);
            this.lblAverageWeight.Size = new Size(200, 20);

            statsPanel.Controls.Add(lblStatsTitle);
            statsPanel.Controls.Add(this.lblTotalLoads);
            statsPanel.Controls.Add(this.lblActiveLoads);
            statsPanel.Controls.Add(this.lblTruckUtilization);
            statsPanel.Controls.Add(this.lblAverageWeight);

            // DataGridView
            this.dgvLoads.Location = new Point(20, 320);
            this.dgvLoads.Size = new Size(1360, 500);
            this.dgvLoads.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvLoads.MultiSelect = false;
            this.dgvLoads.ReadOnly = true;
            this.dgvLoads.AllowUserToAddRows = false;
            this.dgvLoads.AllowUserToDeleteRows = false;
            this.dgvLoads.BackgroundColor = Color.White;
            this.dgvLoads.BorderStyle = BorderStyle.FixedSingle;
            this.dgvLoads.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(220, 53, 69);
            this.dgvLoads.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvLoads.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.dgvLoads.EnableHeadersVisualStyles = false;
            this.dgvLoads.RowTemplate.Height = 30;

            // Add controls to form
            this.Controls.Add(headerPanel);
            this.Controls.Add(this.filterPanel);
            this.Controls.Add(statsPanel);
            this.Controls.Add(this.dgvLoads);

            ((System.ComponentModel.ISupportInitialize)(this.dgvLoads)).EndInit();
            this.ResumeLayout(false);
        }

        private void LoadTrucks()
        {
            try
            {
                string query = "SELECT DISTINCT Lorry FROM LoadDetails WHERE Lorry IS NOT NULL ORDER BY Lorry";
                DataTable truckData = dbConnection.ExecuteSelect(query);

                // Add "All Trucks" option
                DataTable combinedData = new DataTable();
                combinedData.Columns.Add("Lorry", typeof(string));
                
                DataRow allRow = combinedData.NewRow();
                allRow["Lorry"] = "All Trucks";
                combinedData.Rows.Add(allRow);

                foreach (DataRow row in truckData.Rows)
                {
                    DataRow newRow = combinedData.NewRow();
                    newRow["Lorry"] = row["Lorry"];
                    combinedData.Rows.Add(newRow);
                }

                cmbTruckFilter.DisplayMember = "Lorry";
                cmbTruckFilter.ValueMember = "Lorry";
                cmbTruckFilter.DataSource = combinedData;
                cmbTruckFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading trucks: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLoadReport()
        {
            try
            {
                string query = @"
                    SELECT 
                        ld.LoadID,
                        j.JobID,
                        c.Name as CustomerName,
                        j.StartLocation,
                        j.Destination,
                        j.Status as JobStatus,
                        ld.Lorry as TruckNumber,
                        ld.Driver as DriverName,
                        ld.Assistant as AssistantName,
                        ld.Container as ContainerNumber,
                        j.EstimatedWeight,
                        j.RequestDate,
                        j.PreferredDate,
                        j.Urgency,
                        CASE j.Status
                            WHEN 'In Progress' THEN 'Currently Loading/Transit'
                            WHEN 'Completed' THEN 'Delivered Successfully'
                            WHEN 'Cancelled' THEN 'Load Cancelled'
                            ELSE j.Status
                        END as LoadStatus,
                        CASE 
                            WHEN j.Status = 'In Progress' THEN DATEDIFF(CURDATE(), j.PreferredDate)
                            WHEN j.Status = 'Completed' THEN 0
                            ELSE NULL
                        END as DaysFromSchedule
                    FROM LoadDetails ld
                    INNER JOIN Job j ON ld.JobID = j.JobID
                    INNER JOIN Customer c ON j.CustomerID = c.CustomerID
                    WHERE j.Status IN ('In Progress', 'Completed', 'Cancelled')
                    ORDER BY j.RequestDate DESC";

                DataTable loadData = dbConnection.ExecuteSelect(query);
                dgvLoads.DataSource = loadData;

                FormatLoadGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading load report: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatLoadGrid()
        {
            // Format columns
            if (dgvLoads.Columns["LoadID"] != null)
            {
                dgvLoads.Columns["LoadID"].HeaderText = "Load ID";
                dgvLoads.Columns["LoadID"].Width = 80;
            }

            if (dgvLoads.Columns["JobID"] != null)
            {
                dgvLoads.Columns["JobID"].HeaderText = "Job ID";
                dgvLoads.Columns["JobID"].Width = 80;
            }

            if (dgvLoads.Columns["CustomerName"] != null)
            {
                dgvLoads.Columns["CustomerName"].HeaderText = "Customer";
                dgvLoads.Columns["CustomerName"].Width = 150;
            }

            if (dgvLoads.Columns["StartLocation"] != null)
            {
                dgvLoads.Columns["StartLocation"].HeaderText = "From";
                dgvLoads.Columns["StartLocation"].Width = 120;
            }

            if (dgvLoads.Columns["Destination"] != null)
            {
                dgvLoads.Columns["Destination"].HeaderText = "To";
                dgvLoads.Columns["Destination"].Width = 120;
            }

            if (dgvLoads.Columns["JobStatus"] != null)
            {
                dgvLoads.Columns["JobStatus"].HeaderText = "Status";
                dgvLoads.Columns["JobStatus"].Width = 100;
            }

            if (dgvLoads.Columns["TruckNumber"] != null)
            {
                dgvLoads.Columns["TruckNumber"].HeaderText = "Truck";
                dgvLoads.Columns["TruckNumber"].Width = 100;
            }

            if (dgvLoads.Columns["DriverName"] != null)
            {
                dgvLoads.Columns["DriverName"].HeaderText = "Driver";
                dgvLoads.Columns["DriverName"].Width = 120;
            }

            if (dgvLoads.Columns["AssistantName"] != null)
            {
                dgvLoads.Columns["AssistantName"].HeaderText = "Assistant";
                dgvLoads.Columns["AssistantName"].Width = 120;
            }

            if (dgvLoads.Columns["ContainerNumber"] != null)
            {
                dgvLoads.Columns["ContainerNumber"].HeaderText = "Container";
                dgvLoads.Columns["ContainerNumber"].Width = 100;
            }

            if (dgvLoads.Columns["EstimatedWeight"] != null)
            {
                dgvLoads.Columns["EstimatedWeight"].HeaderText = "Weight (kg)";
                dgvLoads.Columns["EstimatedWeight"].Width = 100;
            }

            if (dgvLoads.Columns["RequestDate"] != null)
            {
                dgvLoads.Columns["RequestDate"].HeaderText = "Request Date";
                dgvLoads.Columns["RequestDate"].DefaultCellStyle.Format = "MMM dd, yyyy";
                dgvLoads.Columns["RequestDate"].Width = 120;
            }

            if (dgvLoads.Columns["PreferredDate"] != null)
            {
                dgvLoads.Columns["PreferredDate"].HeaderText = "Scheduled Date";
                dgvLoads.Columns["PreferredDate"].DefaultCellStyle.Format = "MMM dd, yyyy";
                dgvLoads.Columns["PreferredDate"].Width = 120;
            }

            if (dgvLoads.Columns["Urgency"] != null)
            {
                dgvLoads.Columns["Urgency"].HeaderText = "Priority";
                dgvLoads.Columns["Urgency"].Width = 80;
            }

            if (dgvLoads.Columns["LoadStatus"] != null)
            {
                dgvLoads.Columns["LoadStatus"].HeaderText = "Load Status";
                dgvLoads.Columns["LoadStatus"].Width = 150;
            }

            if (dgvLoads.Columns["DaysFromSchedule"] != null)
            {
                dgvLoads.Columns["DaysFromSchedule"].HeaderText = "Schedule Variance";
                dgvLoads.Columns["DaysFromSchedule"].Width = 120;
            }

            // Color code rows based on status
            foreach (DataGridViewRow row in dgvLoads.Rows)
            {
                if (row.Cells["JobStatus"].Value != null)
                {
                    string status = row.Cells["JobStatus"].Value.ToString();
                    switch (status)
                    {
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

        private void LoadLoadStats()
        {
            try
            {
                // Total loads
                string totalQuery = "SELECT COUNT(*) FROM LoadDetails";
                int totalLoads = Convert.ToInt32(dbConnection.ExecuteScalar(totalQuery));

                // Active loads (In Progress)
                string activeQuery = @"
                    SELECT COUNT(*) 
                    FROM LoadDetails ld
                    INNER JOIN Job j ON ld.JobID = j.JobID
                    WHERE j.Status = 'In Progress'";
                int activeLoads = Convert.ToInt32(dbConnection.ExecuteScalar(activeQuery));

                // Unique trucks utilized
                string trucksQuery = "SELECT COUNT(DISTINCT Lorry) FROM LoadDetails WHERE Lorry IS NOT NULL";
                int uniqueTrucks = Convert.ToInt32(dbConnection.ExecuteScalar(trucksQuery));

                // Average weight
                string avgWeightQuery = @"
                    SELECT AVG(j.EstimatedWeight) 
                    FROM LoadDetails ld
                    INNER JOIN Job j ON ld.JobID = j.JobID
                    WHERE j.EstimatedWeight IS NOT NULL";
                var avgWeightResult = dbConnection.ExecuteScalar(avgWeightQuery);
                double avgWeight = avgWeightResult != DBNull.Value ? Convert.ToDouble(avgWeightResult) : 0;

                lblTotalLoads.Text = $"Total Loads: {totalLoads}";
                lblActiveLoads.Text = $"Active Loads: {activeLoads}";
                lblTruckUtilization.Text = $"Trucks Utilized: {uniqueTrucks}";
                lblAverageWeight.Text = $"Avg Weight: {avgWeight:F1} kg";
            }
            catch (Exception ex)
            {
                lblTotalLoads.Text = "Stats unavailable";
                lblActiveLoads.Text = "";
                lblTruckUtilization.Text = "";
                lblAverageWeight.Text = "";
            }
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                string query = GetFilteredLoadQuery();
                DataTable filteredData = dbConnection.ExecuteSelect(query);
                dgvLoads.DataSource = filteredData;
                FormatLoadGrid();

                MessageBox.Show($"Report generated successfully! Found {filteredData.Rows.Count} loads.", 
                    "Report Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetFilteredLoadQuery()
        {
            string baseQuery = @"
                SELECT 
                    ld.LoadID,
                    j.JobID,
                    c.Name as CustomerName,
                    j.StartLocation,
                    j.Destination,
                    j.Status as JobStatus,
                    ld.Lorry as TruckNumber,
                    ld.Driver as DriverName,
                    ld.Assistant as AssistantName,
                    ld.Container as ContainerNumber,
                    j.EstimatedWeight,
                    j.RequestDate,
                    j.PreferredDate,
                    j.Urgency,
                    CASE j.Status
                        WHEN 'In Progress' THEN 'Currently Loading/Transit'
                        WHEN 'Completed' THEN 'Delivered Successfully'
                        WHEN 'Cancelled' THEN 'Load Cancelled'
                        ELSE j.Status
                    END as LoadStatus,
                    CASE 
                        WHEN j.Status = 'In Progress' THEN DATEDIFF(CURDATE(), j.PreferredDate)
                        WHEN j.Status = 'Completed' THEN 0
                        ELSE NULL
                    END as DaysFromSchedule
                FROM LoadDetails ld
                INNER JOIN Job j ON ld.JobID = j.JobID
                INNER JOIN Customer c ON j.CustomerID = c.CustomerID";

            List<string> conditions = new List<string>();
            conditions.Add("j.Status IN ('In Progress', 'Completed', 'Cancelled')");

            // Status filter
            string selectedStatus = cmbStatusFilter.SelectedItem.ToString();
            if (selectedStatus != "All Status")
            {
                conditions.Add($"j.Status = '{selectedStatus}'");
            }

            // Truck filter
            if (cmbTruckFilter.SelectedValue != null && cmbTruckFilter.SelectedValue.ToString() != "All Trucks")
            {
                conditions.Add($"ld.Lorry = '{cmbTruckFilter.SelectedValue}'");
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
                saveDialog.FileName = $"LoadReport_{DateTime.Now:yyyyMMdd}.csv";

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
                string[] headers = new string[dgvLoads.Columns.Count];
                for (int i = 0; i < dgvLoads.Columns.Count; i++)
                {
                    headers[i] = dgvLoads.Columns[i].HeaderText;
                }
                writer.WriteLine(string.Join(",", headers));

                // Write data
                foreach (DataGridViewRow row in dgvLoads.Rows)
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