using MySql.Data.MySqlClient;
using System.Data;
using System.Drawing.Drawing2D;

namespace Transport
{
    public partial class AdminManagementForm : Form
    {
        private DatabaseConnection dbConnection;
        private DataTable adminDataTable;
        private Panel reportPanel;
        private Button btnGenerateReport;
        private Button btnExportReport;
        private Label lblTotalAdmins;
        private Label lblAdminsByRole;
        private Label lblRecentAdmins;

        public AdminManagementForm()
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            LoadAdmins();
            SetupReportingPanel();
            GenerateAdminReport();
        }

        private void InitializeComponent()
        {
            this.dgvAdmins = new DataGridView();
            this.txtAdminId = new TextBox();
            this.txtUsername = new TextBox();
            this.txtPassword = new TextBox();
            this.cmbRole = new ComboBox();
            this.btnAdd = new Button();
            this.btnUpdate = new Button();
            this.btnDelete = new Button();
            this.btnClear = new Button();
            this.lblAdminId = new Label();
            this.lblUsername = new Label();
            this.lblPassword = new Label();
            this.lblRole = new Label();
            this.lblTitle = new Label();
            this.reportPanel = new Panel();
            this.btnGenerateReport = new Button();
            this.btnExportReport = new Button();
            this.lblTotalAdmins = new Label();
            this.lblAdminsByRole = new Label();
            this.lblRecentAdmins = new Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdmins)).BeginInit();
            this.SuspendLayout();

            // Form - Made larger to accommodate reporting
            this.Text = "Admin Management & Reporting";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 250);

            // Title
            this.lblTitle.Text = "Admin Management & Reporting System";
            this.lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.FromArgb(52, 58, 64);
            this.lblTitle.Location = new Point(400, 15);
            this.lblTitle.Size = new Size(400, 30);
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // DataGridView - Enhanced styling
            this.dgvAdmins.Location = new Point(20, 60);
            this.dgvAdmins.Size = new Size(550, 350);
            this.dgvAdmins.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvAdmins.MultiSelect = false;
            this.dgvAdmins.ReadOnly = true;
            this.dgvAdmins.AllowUserToAddRows = false;
            this.dgvAdmins.AllowUserToDeleteRows = false;
            this.dgvAdmins.BackgroundColor = Color.White;
            this.dgvAdmins.BorderStyle = BorderStyle.None;
            this.dgvAdmins.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(102, 16, 242);
            this.dgvAdmins.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvAdmins.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.dgvAdmins.DefaultCellStyle.SelectionBackColor = Color.FromArgb(102, 16, 242);
            this.dgvAdmins.DefaultCellStyle.SelectionForeColor = Color.White;
            this.dgvAdmins.EnableHeadersVisualStyles = false;
            this.dgvAdmins.SelectionChanged += new EventHandler(this.dgvAdmins_SelectionChanged);

            // Labels and Controls with enhanced styling
            int startX = 600;
            int startY = 80;
            int spacing = 50;

            this.lblAdminId.Text = "Admin ID:";
            this.lblAdminId.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblAdminId.ForeColor = Color.FromArgb(52, 58, 64);
            this.lblAdminId.Location = new Point(startX, startY);
            this.lblAdminId.Size = new Size(100, 23);

            this.txtAdminId.Location = new Point(startX + 110, startY);
            this.txtAdminId.Size = new Size(200, 25);
            this.txtAdminId.ReadOnly = true;
            this.txtAdminId.BackColor = Color.FromArgb(248, 249, 250);
            this.txtAdminId.Font = new Font("Segoe UI", 9F);

            this.lblUsername.Text = "Username:";
            this.lblUsername.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblUsername.ForeColor = Color.FromArgb(52, 58, 64);
            this.lblUsername.Location = new Point(startX, startY + spacing);
            this.lblUsername.Size = new Size(100, 23);

            this.txtUsername.Location = new Point(startX + 110, startY + spacing);
            this.txtUsername.Size = new Size(200, 25);
            this.txtUsername.Font = new Font("Segoe UI", 9F);

            this.lblPassword.Text = "Password:";
            this.lblPassword.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblPassword.ForeColor = Color.FromArgb(52, 58, 64);
            this.lblPassword.Location = new Point(startX, startY + spacing * 2);
            this.lblPassword.Size = new Size(100, 23);

            this.txtPassword.Location = new Point(startX + 110, startY + spacing * 2);
            this.txtPassword.Size = new Size(200, 25);
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.Font = new Font("Segoe UI", 9F);

            this.lblRole.Text = "Role:";
            this.lblRole.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblRole.ForeColor = Color.FromArgb(52, 58, 64);
            this.lblRole.Location = new Point(startX, startY + spacing * 3);
            this.lblRole.Size = new Size(100, 23);

            this.cmbRole.Location = new Point(startX + 110, startY + spacing * 3);
            this.cmbRole.Size = new Size(200, 25);
            this.cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbRole.Items.AddRange(new string[] { "Admin", "Manager", "Supervisor" });
            this.cmbRole.SelectedIndex = 0;
            this.cmbRole.Font = new Font("Segoe UI", 9F);

            // Buttons with enhanced styling
            int buttonY = startY + spacing * 4 + 20;
            this.btnAdd.Text = "Add Admin";
            this.btnAdd.Location = new Point(startX, buttonY);
            this.btnAdd.Size = new Size(90, 35);
            this.btnAdd.BackColor = Color.FromArgb(40, 167, 69);
            this.btnAdd.ForeColor = Color.White;
            this.btnAdd.FlatStyle = FlatStyle.Flat;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnAdd.Cursor = Cursors.Hand;
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);

            this.btnUpdate.Text = "Update";
            this.btnUpdate.Location = new Point(startX + 100, buttonY);
            this.btnUpdate.Size = new Size(90, 35);
            this.btnUpdate.BackColor = Color.FromArgb(255, 193, 7);
            this.btnUpdate.ForeColor = Color.White;
            this.btnUpdate.FlatStyle = FlatStyle.Flat;
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnUpdate.Cursor = Cursors.Hand;
            this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);

            this.btnDelete.Text = "Delete";
            this.btnDelete.Location = new Point(startX + 200, buttonY);
            this.btnDelete.Size = new Size(90, 35);
            this.btnDelete.BackColor = Color.FromArgb(220, 53, 69);
            this.btnDelete.ForeColor = Color.White;
            this.btnDelete.FlatStyle = FlatStyle.Flat;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnDelete.Cursor = Cursors.Hand;
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);

            this.btnClear.Text = "Clear";
            this.btnClear.Location = new Point(startX + 300, buttonY);
            this.btnClear.Size = new Size(90, 35);
            this.btnClear.BackColor = Color.FromArgb(108, 117, 125);
            this.btnClear.ForeColor = Color.White;
            this.btnClear.FlatStyle = FlatStyle.Flat;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnClear.Cursor = Cursors.Hand;
            this.btnClear.Click += new EventHandler(this.btnClear_Click);

            // Report Generation Buttons
            this.btnGenerateReport.Text = "?? Generate Report";
            this.btnGenerateReport.Location = new Point(startX, buttonY + 50);
            this.btnGenerateReport.Size = new Size(140, 35);
            this.btnGenerateReport.BackColor = Color.FromArgb(102, 16, 242);
            this.btnGenerateReport.ForeColor = Color.White;
            this.btnGenerateReport.FlatStyle = FlatStyle.Flat;
            this.btnGenerateReport.FlatAppearance.BorderSize = 0;
            this.btnGenerateReport.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnGenerateReport.Cursor = Cursors.Hand;
            this.btnGenerateReport.Click += new EventHandler(this.btnGenerateReport_Click);

            this.btnExportReport.Text = "?? Export Report";
            this.btnExportReport.Location = new Point(startX + 150, buttonY + 50);
            this.btnExportReport.Size = new Size(140, 35);
            this.btnExportReport.BackColor = Color.FromArgb(0, 123, 255);
            this.btnExportReport.ForeColor = Color.White;
            this.btnExportReport.FlatStyle = FlatStyle.Flat;
            this.btnExportReport.FlatAppearance.BorderSize = 0;
            this.btnExportReport.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnExportReport.Cursor = Cursors.Hand;
            this.btnExportReport.Click += new EventHandler(this.btnExportReport_Click);

            // Add controls to form
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.dgvAdmins);
            this.Controls.Add(this.lblAdminId);
            this.Controls.Add(this.txtAdminId);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblRole);
            this.Controls.Add(this.cmbRole);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnGenerateReport);
            this.Controls.Add(this.btnExportReport);

            ((System.ComponentModel.ISupportInitialize)(this.dgvAdmins)).EndInit();
            this.ResumeLayout(false);
        }

        private DataGridView dgvAdmins;
        private TextBox txtAdminId, txtUsername, txtPassword;
        private ComboBox cmbRole;
        private Button btnAdd, btnUpdate, btnDelete, btnClear;
        private Label lblAdminId, lblUsername, lblPassword, lblRole, lblTitle;

        private void SetupReportingPanel()
        {
            // Create reporting panel
            this.reportPanel = new Panel();
            this.reportPanel.Location = new Point(20, 430);
            this.reportPanel.Size = new Size(1150, 220);
            this.reportPanel.BackColor = Color.White;
            this.reportPanel.BorderStyle = BorderStyle.FixedSingle;
            this.reportPanel.Paint += new PaintEventHandler(this.reportPanel_Paint);

            // Report Title
            Label lblReportTitle = new Label();
            lblReportTitle.Text = "?? Admin Management Report";
            lblReportTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblReportTitle.ForeColor = Color.FromArgb(52, 58, 64);
            lblReportTitle.Location = new Point(20, 15);
            lblReportTitle.Size = new Size(400, 30);

            // Statistics Labels
            this.lblTotalAdmins = new Label();
            this.lblTotalAdmins.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            this.lblTotalAdmins.ForeColor = Color.FromArgb(52, 58, 64);
            this.lblTotalAdmins.Location = new Point(30, 60);
            this.lblTotalAdmins.Size = new Size(350, 25);

            this.lblAdminsByRole = new Label();
            this.lblAdminsByRole.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            this.lblAdminsByRole.ForeColor = Color.FromArgb(52, 58, 64);
            this.lblAdminsByRole.Location = new Point(30, 90);
            this.lblAdminsByRole.Size = new Size(1100, 50);

            this.lblRecentAdmins = new Label();
            this.lblRecentAdmins.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            this.lblRecentAdmins.ForeColor = Color.FromArgb(52, 58, 64);
            this.lblRecentAdmins.Location = new Point(30, 150);
            this.lblRecentAdmins.Size = new Size(1100, 50);

            // Add to report panel
            this.reportPanel.Controls.Add(lblReportTitle);
            this.reportPanel.Controls.Add(this.lblTotalAdmins);
            this.reportPanel.Controls.Add(this.lblAdminsByRole);
            this.reportPanel.Controls.Add(this.lblRecentAdmins);

            // Add report panel to form
            this.Controls.Add(this.reportPanel);
        }

        private void reportPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            
            // Add subtle shadow effect
            using (Pen shadowPen = new Pen(Color.FromArgb(50, 0, 0, 0), 2))
            {
                e.Graphics.DrawRectangle(shadowPen, 0, 0, panel.Width - 1, panel.Height - 1);
            }
        }

        private void LoadAdmins()
        {
            string query = "SELECT AdminID, Username, Role FROM Admin ORDER BY AdminID";
            adminDataTable = dbConnection.ExecuteSelect(query);
            dgvAdmins.DataSource = adminDataTable;
            
            // Style the DataGridView columns
            if (dgvAdmins.Columns.Count > 0)
            {
                dgvAdmins.Columns["AdminID"].HeaderText = "Admin ID";
                dgvAdmins.Columns["AdminID"].Width = 100;
                dgvAdmins.Columns["Username"].HeaderText = "Username";
                dgvAdmins.Columns["Username"].Width = 200;
                dgvAdmins.Columns["Role"].HeaderText = "Role";
                dgvAdmins.Columns["Role"].Width = 150;
            }
        }

        private void GenerateAdminReport()
        {
            try
            {
                // Get total admin count
                string totalQuery = "SELECT COUNT(*) FROM Admin";
                int totalAdmins = Convert.ToInt32(dbConnection.ExecuteScalar(totalQuery));
                
                // Get admin count by role
                string roleQuery = @"SELECT Role, COUNT(*) as Count FROM Admin GROUP BY Role ORDER BY Role";
                DataTable roleData = dbConnection.ExecuteSelect(roleQuery);
                
                // Get recent admins (if there's a created date field, otherwise show all)
                string recentQuery = "SELECT Username, Role FROM Admin ORDER BY AdminID DESC LIMIT 5";
                DataTable recentData = dbConnection.ExecuteSelect(recentQuery);

                // Update labels with report data
                this.lblTotalAdmins.Text = $"?? Total Administrators: {totalAdmins}";

                // Build role distribution text
                string roleDistribution = "?? Role Distribution: ";
                foreach (DataRow row in roleData.Rows)
                {
                    string role = row["Role"].ToString();
                    int count = Convert.ToInt32(row["Count"]);
                    double percentage = (double)count / totalAdmins * 100;
                    roleDistribution += $"{role}: {count} ({percentage:F1}%) | ";
                }
                this.lblAdminsByRole.Text = roleDistribution.TrimEnd('|', ' ');

                // Build recent admins text
                string recentAdmins = "?? Recent Administrators: ";
                foreach (DataRow row in recentData.Rows)
                {
                    recentAdmins += $"{row["Username"]} ({row["Role"]}) | ";
                }
                this.lblRecentAdmins.Text = recentAdmins.TrimEnd('|', ' ');

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating admin report: {ex.Message}", "Report Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                AdminReportForm reportForm = new AdminReportForm();
                reportForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening admin report: {ex.Message}", "Report Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportReport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV files (*.csv)|*.csv|JSON files (*.json)|*.json|XML files (*.xml)|*.xml";
                saveDialog.FileName = $"AdminReport_{DateTime.Now:yyyyMMdd_HHmmss}";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string extension = Path.GetExtension(saveDialog.FileName).ToLower();
                    
                    switch (extension)
                    {
                        case ".csv":
                            ExportToCsv(saveDialog.FileName);
                            break;
                        case ".json":
                            ExportToJson(saveDialog.FileName);
                            break;
                        case ".xml":
                            ExportToXml(saveDialog.FileName);
                            break;
                    }

                    MessageBox.Show("Admin report exported successfully!", "Export Complete", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting admin report: {ex.Message}", "Export Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToCsv(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                // Write headers
                writer.WriteLine("AdminID,Username,Role");

                // Write data
                foreach (DataRow row in adminDataTable.Rows)
                {
                    writer.WriteLine($"{row["AdminID"]},{row["Username"]},{row["Role"]}");
                }

                // Write summary statistics
                writer.WriteLine();
                writer.WriteLine("SUMMARY STATISTICS:");
                writer.WriteLine($"Total Administrators,{adminDataTable.Rows.Count}");
                writer.WriteLine($"Report Generated,{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            }
        }

        private void ExportToJson(string fileName)
        {
            var adminList = new List<object>();
            foreach (DataRow row in adminDataTable.Rows)
            {
                adminList.Add(new
                {
                    AdminID = Convert.ToInt32(row["AdminID"]),
                    Username = row["Username"].ToString(),
                    Role = row["Role"].ToString()
                });
            }

            var reportData = new
            {
                Title = "Admin Management Report",
                GeneratedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                TotalCount = adminDataTable.Rows.Count,
                Administrators = adminList
            };

            string json = System.Text.Json.JsonSerializer.Serialize(reportData, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileName, json);
        }

        private void ExportToXml(string fileName)
        {
            DataSet dataSet = new DataSet("AdminReport");
            DataTable exportTable = adminDataTable.Copy();
            exportTable.TableName = "Administrators";
            dataSet.Tables.Add(exportTable);
            dataSet.WriteXml(fileName);
        }

        private void dgvAdmins_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvAdmins.CurrentRow != null)
            {
                DataGridViewRow row = dgvAdmins.CurrentRow;
                txtAdminId.Text = row.Cells["AdminID"].Value?.ToString();
                txtUsername.Text = row.Cells["Username"].Value?.ToString();
                cmbRole.Text = row.Cells["Role"].Value?.ToString();
                txtPassword.Text = ""; // Don't show password for security
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                string query = @"INSERT INTO Admin (Username, Password, Role) 
                                VALUES (@username, @password, @role)";

                MySqlParameter[] parameters = {
                    new MySqlParameter("@username", txtUsername.Text.Trim()),
                    new MySqlParameter("@password", txtPassword.Text.Trim()),
                    new MySqlParameter("@role", cmbRole.Text)
                };

                if (dbConnection.ExecuteNonQuery(query, parameters))
                {
                    MessageBox.Show("Admin added successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAdmins();
                    GenerateAdminReport(); // Refresh report
                    ClearForm();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAdminId.Text))
            {
                MessageBox.Show("Please select an admin to update.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ValidateInput())
            {
                string query = @"UPDATE Admin SET Username = @username, Role = @role";
                
                var parameterList = new List<MySqlParameter> {
                    new MySqlParameter("@username", txtUsername.Text.Trim()),
                    new MySqlParameter("@role", cmbRole.Text),
                    new MySqlParameter("@adminId", Convert.ToInt32(txtAdminId.Text))
                };

                if (!string.IsNullOrEmpty(txtPassword.Text.Trim()))
                {
                    query += ", Password = @password";
                    parameterList.Add(new MySqlParameter("@password", txtPassword.Text.Trim()));
                }

                query += " WHERE AdminID = @adminId";

                if (dbConnection.ExecuteNonQuery(query, parameterList.ToArray()))
                {
                    MessageBox.Show("Admin updated successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAdmins();
                    GenerateAdminReport(); // Refresh report
                    ClearForm();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAdminId.Text))
            {
                MessageBox.Show("Please select an admin to delete.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this admin?", 
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string query = "DELETE FROM Admin WHERE AdminID = @adminId";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@adminId", Convert.ToInt32(txtAdminId.Text))
                };

                if (dbConnection.ExecuteNonQuery(query, parameters))
                {
                    MessageBox.Show("Admin deleted successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAdmins();
                    GenerateAdminReport(); // Refresh report
                    ClearForm();
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtAdminId.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            cmbRole.SelectedIndex = 0;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Please enter username.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Only validate password for new admins
            if (string.IsNullOrEmpty(txtAdminId.Text) && string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter password.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbRole.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a role.", "Validation Error", 
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