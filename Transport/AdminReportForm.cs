using MySql.Data.MySqlClient;
using System.Data;
using System.Drawing.Drawing2D;

namespace Transport
{
    public partial class AdminReportForm : Form
    {
        private DatabaseConnection dbConnection;
        private DataGridView dgvReport;
        private Panel headerPanel;
        private Panel filterPanel;
        private Panel statsPanel;
        private Panel exportPanel;
        private ComboBox cmbRoleFilter;
        private Button btnGenerateReport;
        private Button btnExportCsv;
        private Button btnExportJson;
        private Button btnExportXml;
        private Button btnPrint;
        private Label lblTotalAdmins;
        private Label lblRoleDistribution;
        private Label lblReportTitle;
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;

        public AdminReportForm()
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            LoadInitialData();
        }

        private void InitializeComponent()
        {
            this.dgvReport = new DataGridView();
            this.headerPanel = new Panel();
            this.filterPanel = new Panel();
            this.statsPanel = new Panel();
            this.exportPanel = new Panel();
            this.cmbRoleFilter = new ComboBox();
            this.btnGenerateReport = new Button();
            this.btnExportCsv = new Button();
            this.btnExportJson = new Button();
            this.btnExportXml = new Button();
            this.btnPrint = new Button();
            this.lblTotalAdmins = new Label();
            this.lblRoleDistribution = new Label();
            this.lblReportTitle = new Label();
            this.dtpFromDate = new DateTimePicker();
            this.dtpToDate = new DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.SuspendLayout();

            // Form Properties
            this.Text = "Admin Management Report - Transport System";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 250);

            // Header Panel
            this.headerPanel.Location = new Point(0, 0);
            this.headerPanel.Size = new Size(1000, 80);
            this.headerPanel.Dock = DockStyle.Top;
            this.headerPanel.BackColor = Color.FromArgb(102, 16, 242);
            this.headerPanel.Paint += new PaintEventHandler(this.headerPanel_Paint);

            this.lblReportTitle.Text = "?? Administrator Management Report";
            this.lblReportTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            this.lblReportTitle.ForeColor = Color.White;
            this.lblReportTitle.Location = new Point(30, 25);
            this.lblReportTitle.Size = new Size(500, 30);
            this.lblReportTitle.BackColor = Color.Transparent;

            Button btnClose = new Button();
            btnClose.Text = "?";
            btnClose.Location = new Point(950, 10);
            btnClose.Size = new Size(40, 40);
            btnClose.BackColor = Color.FromArgb(220, 53, 69);
            btnClose.ForeColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnClose.Cursor = Cursors.Hand;
            btnClose.Click += (s, e) => this.Close();

            this.headerPanel.Controls.Add(this.lblReportTitle);
            this.headerPanel.Controls.Add(btnClose);

            // Filter Panel
            this.filterPanel.Location = new Point(20, 90);
            this.filterPanel.Size = new Size(960, 60);
            this.filterPanel.BackColor = Color.White;
            this.filterPanel.BorderStyle = BorderStyle.FixedSingle;

            Label lblRoleFilter = new Label();
            lblRoleFilter.Text = "Filter by Role:";
            lblRoleFilter.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblRoleFilter.Location = new Point(20, 20);
            lblRoleFilter.Size = new Size(100, 25);

            this.cmbRoleFilter.Location = new Point(130, 18);
            this.cmbRoleFilter.Size = new Size(120, 25);
            this.cmbRoleFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbRoleFilter.Items.AddRange(new string[] { "All Roles", "Admin", "Manager", "Supervisor" });
            this.cmbRoleFilter.SelectedIndex = 0;
            this.cmbRoleFilter.Font = new Font("Segoe UI", 9F);

            Label lblDateRange = new Label();
            lblDateRange.Text = "Date Range:";
            lblDateRange.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDateRange.Location = new Point(280, 20);
            lblDateRange.Size = new Size(80, 25);

            this.dtpFromDate.Location = new Point(370, 18);
            this.dtpFromDate.Size = new Size(120, 25);
            this.dtpFromDate.Value = DateTime.Now.AddMonths(-3);
            this.dtpFromDate.Font = new Font("Segoe UI", 9F);

            Label lblTo = new Label();
            lblTo.Text = "to";
            lblTo.Location = new Point(500, 20);
            lblTo.Size = new Size(20, 25);
            lblTo.TextAlign = ContentAlignment.MiddleCenter;

            this.dtpToDate.Location = new Point(530, 18);
            this.dtpToDate.Size = new Size(120, 25);
            this.dtpToDate.Value = DateTime.Now;
            this.dtpToDate.Font = new Font("Segoe UI", 9F);

            this.btnGenerateReport.Text = "?? Generate Report";
            this.btnGenerateReport.Location = new Point(680, 15);
            this.btnGenerateReport.Size = new Size(130, 30);
            this.btnGenerateReport.BackColor = Color.FromArgb(102, 16, 242);
            this.btnGenerateReport.ForeColor = Color.White;
            this.btnGenerateReport.FlatStyle = FlatStyle.Flat;
            this.btnGenerateReport.FlatAppearance.BorderSize = 0;
            this.btnGenerateReport.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnGenerateReport.Cursor = Cursors.Hand;
            this.btnGenerateReport.Click += new EventHandler(this.btnGenerateReport_Click);

            this.filterPanel.Controls.Add(lblRoleFilter);
            this.filterPanel.Controls.Add(this.cmbRoleFilter);
            this.filterPanel.Controls.Add(lblDateRange);
            this.filterPanel.Controls.Add(this.dtpFromDate);
            this.filterPanel.Controls.Add(lblTo);
            this.filterPanel.Controls.Add(this.dtpToDate);
            this.filterPanel.Controls.Add(this.btnGenerateReport);

            // DataGridView for Report
            this.dgvReport.Location = new Point(20, 160);
            this.dgvReport.Size = new Size(960, 350);
            this.dgvReport.BackgroundColor = Color.White;
            this.dgvReport.BorderStyle = BorderStyle.None;
            this.dgvReport.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 58, 64);
            this.dgvReport.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvReport.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.dgvReport.DefaultCellStyle.SelectionBackColor = Color.FromArgb(102, 16, 242);
            this.dgvReport.DefaultCellStyle.SelectionForeColor = Color.White;
            this.dgvReport.EnableHeadersVisualStyles = false;
            this.dgvReport.ReadOnly = true;
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvReport.MultiSelect = false;

            // Statistics Panel
            this.statsPanel.Location = new Point(20, 520);
            this.statsPanel.Size = new Size(960, 60);
            this.statsPanel.BackColor = Color.FromArgb(248, 249, 250);
            this.statsPanel.BorderStyle = BorderStyle.FixedSingle;

            this.lblTotalAdmins.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblTotalAdmins.ForeColor = Color.FromArgb(52, 58, 64);
            this.lblTotalAdmins.Location = new Point(20, 15);
            this.lblTotalAdmins.Size = new Size(300, 25);

            this.lblRoleDistribution.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            this.lblRoleDistribution.ForeColor = Color.FromArgb(108, 117, 125);
            this.lblRoleDistribution.Location = new Point(350, 15);
            this.lblRoleDistribution.Size = new Size(600, 25);

            this.statsPanel.Controls.Add(this.lblTotalAdmins);
            this.statsPanel.Controls.Add(this.lblRoleDistribution);

            // Export Panel
            this.exportPanel.Location = new Point(20, 590);
            this.exportPanel.Size = new Size(960, 60);
            this.exportPanel.BackColor = Color.White;
            this.exportPanel.BorderStyle = BorderStyle.FixedSingle;

            Label lblExport = new Label();
            lblExport.Text = "?? Export Options:";
            lblExport.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblExport.Location = new Point(20, 20);
            lblExport.Size = new Size(120, 25);

            this.btnExportCsv.Text = "CSV";
            this.btnExportCsv.Location = new Point(150, 15);
            this.btnExportCsv.Size = new Size(80, 30);
            this.btnExportCsv.BackColor = Color.FromArgb(40, 167, 69);
            this.btnExportCsv.ForeColor = Color.White;
            this.btnExportCsv.FlatStyle = FlatStyle.Flat;
            this.btnExportCsv.FlatAppearance.BorderSize = 0;
            this.btnExportCsv.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnExportCsv.Cursor = Cursors.Hand;
            this.btnExportCsv.Click += new EventHandler(this.btnExportCsv_Click);

            this.btnExportJson.Text = "JSON";
            this.btnExportJson.Location = new Point(240, 15);
            this.btnExportJson.Size = new Size(80, 30);
            this.btnExportJson.BackColor = Color.FromArgb(255, 193, 7);
            this.btnExportJson.ForeColor = Color.White;
            this.btnExportJson.FlatStyle = FlatStyle.Flat;
            this.btnExportJson.FlatAppearance.BorderSize = 0;
            this.btnExportJson.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnExportJson.Cursor = Cursors.Hand;
            this.btnExportJson.Click += new EventHandler(this.btnExportJson_Click);

            this.btnExportXml.Text = "XML";
            this.btnExportXml.Location = new Point(330, 15);
            this.btnExportXml.Size = new Size(80, 30);
            this.btnExportXml.BackColor = Color.FromArgb(0, 123, 255);
            this.btnExportXml.ForeColor = Color.White;
            this.btnExportXml.FlatStyle = FlatStyle.Flat;
            this.btnExportXml.FlatAppearance.BorderSize = 0;
            this.btnExportXml.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnExportXml.Cursor = Cursors.Hand;
            this.btnExportXml.Click += new EventHandler(this.btnExportXml_Click);

            this.btnPrint.Text = "??? Print";
            this.btnPrint.Location = new Point(420, 15);
            this.btnPrint.Size = new Size(80, 30);
            this.btnPrint.BackColor = Color.FromArgb(108, 117, 125);
            this.btnPrint.ForeColor = Color.White;
            this.btnPrint.FlatStyle = FlatStyle.Flat;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnPrint.Cursor = Cursors.Hand;
            this.btnPrint.Click += new EventHandler(this.btnPrint_Click);

            this.exportPanel.Controls.Add(lblExport);
            this.exportPanel.Controls.Add(this.btnExportCsv);
            this.exportPanel.Controls.Add(this.btnExportJson);
            this.exportPanel.Controls.Add(this.btnExportXml);
            this.exportPanel.Controls.Add(this.btnPrint);

            // Add all panels to form
            this.Controls.Add(this.headerPanel);
            this.Controls.Add(this.filterPanel);
            this.Controls.Add(this.dgvReport);
            this.Controls.Add(this.statsPanel);
            this.Controls.Add(this.exportPanel);

            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.ResumeLayout(false);
        }

        private void LoadInitialData()
        {
            GenerateAdminReport();
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            GenerateAdminReport();
        }

        private void GenerateAdminReport()
        {
            try
            {
                string query = @"
                    SELECT 
                        AdminID,
                        Username,
                        Role,
                        'Active' as Status,
                        CASE Role
                            WHEN 'Admin' THEN 'System Administrator'
                            WHEN 'Manager' THEN 'Operations Manager'
                            WHEN 'Supervisor' THEN 'Department Supervisor'
                            ELSE Role
                        END as RoleDescription
                    FROM Admin 
                    WHERE 1=1";

                List<MySqlParameter> parameters = new List<MySqlParameter>();

                // Apply role filter
                if (cmbRoleFilter.SelectedIndex > 0) // Not "All Roles"
                {
                    query += " AND Role = @role";
                    parameters.Add(new MySqlParameter("@role", cmbRoleFilter.Text));
                }

                query += " ORDER BY Role, Username";

                DataTable reportData = dbConnection.ExecuteSelect(query, parameters.ToArray());
                dgvReport.DataSource = reportData;

                // Style the columns
                if (dgvReport.Columns.Count > 0)
                {
                    dgvReport.Columns["AdminID"].HeaderText = "ID";
                    dgvReport.Columns["AdminID"].Width = 80;
                    dgvReport.Columns["Username"].HeaderText = "Username";
                    dgvReport.Columns["Username"].Width = 200;
                    dgvReport.Columns["Role"].HeaderText = "Role";
                    dgvReport.Columns["Role"].Width = 150;
                    dgvReport.Columns["Status"].HeaderText = "Status";
                    dgvReport.Columns["Status"].Width = 100;
                    dgvReport.Columns["RoleDescription"].HeaderText = "Description";
                    dgvReport.Columns["RoleDescription"].Width = 300;

                    // Color code rows based on role
                    foreach (DataGridViewRow row in dgvReport.Rows)
                    {
                        string role = row.Cells["Role"].Value?.ToString();
                        switch (role)
                        {
                            case "Admin":
                                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 240, 240);
                                break;
                            case "Manager":
                                row.DefaultCellStyle.BackColor = Color.FromArgb(240, 255, 240);
                                break;
                            case "Supervisor":
                                row.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 255);
                                break;
                        }
                    }
                }

                // Update statistics
                UpdateStatistics(reportData);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating admin report: {ex.Message}", "Report Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatistics(DataTable data)
        {
            try
            {
                int totalAdmins = data.Rows.Count;
                lblTotalAdmins.Text = $"?? Total Administrators: {totalAdmins}";

                // Calculate role distribution
                var roleGroups = data.AsEnumerable()
                    .GroupBy(row => row.Field<string>("Role"))
                    .Select(g => new { Role = g.Key, Count = g.Count() });

                string distribution = "?? Distribution: ";
                foreach (var group in roleGroups)
                {
                    double percentage = (double)group.Count / totalAdmins * 100;
                    distribution += $"{group.Role}: {group.Count} ({percentage:F1}%) | ";
                }

                lblRoleDistribution.Text = distribution.TrimEnd('|', ' ');
            }
            catch (Exception ex)
            {
                lblTotalAdmins.Text = "Error calculating statistics";
                lblRoleDistribution.Text = ex.Message;
            }
        }

        private void headerPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            using (LinearGradientBrush brush = new LinearGradientBrush(
                panel.ClientRectangle,
                Color.FromArgb(102, 16, 242),
                Color.FromArgb(52, 58, 64),
                LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, panel.ClientRectangle);
            }
        }

        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            ExportData("CSV files (*.csv)|*.csv", ExportToCsv);
        }

        private void btnExportJson_Click(object sender, EventArgs e)
        {
            ExportData("JSON files (*.json)|*.json", ExportToJson);
        }

        private void btnExportXml_Click(object sender, EventArgs e)
        {
            ExportData("XML files (*.xml)|*.xml", ExportToXml);
        }

        private void ExportData(string filter, Action<string> exportAction)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = filter;
                saveDialog.FileName = $"AdminReport_{DateTime.Now:yyyyMMdd_HHmmss}";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    exportAction(saveDialog.FileName);
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
                string[] headers = new string[dgvReport.Columns.Count];
                for (int i = 0; i < dgvReport.Columns.Count; i++)
                {
                    headers[i] = dgvReport.Columns[i].HeaderText;
                }
                writer.WriteLine(string.Join(",", headers));

                // Write data
                foreach (DataGridViewRow row in dgvReport.Rows)
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

                // Write summary
                writer.WriteLine();
                writer.WriteLine("REPORT SUMMARY:");
                writer.WriteLine($"Generated On,{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                writer.WriteLine($"Total Records,{dgvReport.Rows.Count - 1}");
                writer.WriteLine($"Filter Applied,{cmbRoleFilter.Text}");
            }
        }

        private void ExportToJson(string fileName)
        {
            DataTable data = (DataTable)dgvReport.DataSource;
            var adminList = new List<object>();

            foreach (DataRow row in data.Rows)
            {
                adminList.Add(new
                {
                    AdminID = Convert.ToInt32(row["AdminID"]),
                    Username = row["Username"].ToString(),
                    Role = row["Role"].ToString(),
                    Status = row["Status"].ToString(),
                    RoleDescription = row["RoleDescription"].ToString()
                });
            }

            var reportData = new
            {
                Title = "Administrator Management Report",
                GeneratedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Filter = cmbRoleFilter.Text,
                TotalCount = adminList.Count,
                Administrators = adminList
            };

            string json = System.Text.Json.JsonSerializer.Serialize(reportData, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileName, json);
        }

        private void ExportToXml(string fileName)
        {
            DataTable data = (DataTable)dgvReport.DataSource;
            DataSet dataSet = new DataSet("AdminReport");
            DataTable exportTable = data.Copy();
            exportTable.TableName = "Administrators";
            dataSet.Tables.Add(exportTable);
            dataSet.WriteXml(fileName);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Print functionality would integrate with Windows printing system.\nReport data is ready for printing.", 
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