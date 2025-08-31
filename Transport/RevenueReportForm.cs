using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;

namespace Transport
{
    public partial class RevenueReportForm : Form
    {
        private DatabaseConnection dbConnection;
        private DataGridView dataGridView;
        private Panel headerPanel;
        private Panel filterPanel;
        private Panel dataPanel;
        private Panel summaryPanel;
        private ComboBox cmbPeriodFilter;
        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;
        private Button btnGenerate;
        private Button btnExport;
        private Label lblRecordCount;
        private Label lblTotalRevenue;
        private Label lblAverageValue;
        private Label lblCompletedJobs;

        public RevenueReportForm()
        {
            this.dbConnection = new DatabaseConnection();
            InitializeComponent();
            LoadRevenueData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = "Revenue & Financial Report";
            this.Size = new Size(1300, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 248, 255);

            // Header Panel
            this.headerPanel = new Panel();
            this.headerPanel.Size = new Size(1300, 80);
            this.headerPanel.Dock = DockStyle.Top;
            this.headerPanel.Paint += HeaderPanel_Paint;

            Label titleLabel = new Label
            {
                Text = "?? Revenue & Financial Analytics",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                Size = new Size(500, 30),
                BackColor = Color.Transparent
            };

            Button closeBtn = new Button
            {
                Text = "?",
                Location = new Point(1240, 20),
                Size = new Size(40, 40),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            closeBtn.FlatAppearance.BorderSize = 0;
            closeBtn.Click += (s, e) => this.Close();

            this.headerPanel.Controls.Add(titleLabel);
            this.headerPanel.Controls.Add(closeBtn);

            // Filter Panel
            this.filterPanel = new Panel();
            this.filterPanel.Size = new Size(1300, 60);
            this.filterPanel.Location = new Point(0, 80);
            this.filterPanel.Dock = DockStyle.Top;
            this.filterPanel.BackColor = Color.White;

            Label periodLabel = new Label
            {
                Text = "Period:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(50, 20)
            };

            this.cmbPeriodFilter = new ComboBox
            {
                Location = new Point(80, 18),
                Size = new Size(120, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.cmbPeriodFilter.Items.AddRange(new[] { "All Time", "This Month", "Last 3 Months", "This Year", "Custom Range" });
            this.cmbPeriodFilter.SelectedIndex = 1;
            this.cmbPeriodFilter.SelectedIndexChanged += CmbPeriodFilter_SelectedIndexChanged;

            Label fromLabel = new Label
            {
                Text = "From:",
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                Location = new Point(220, 20),
                Size = new Size(40, 20)
            };

            this.dtpFrom = new DateTimePicker
            {
                Location = new Point(265, 18),
                Size = new Size(120, 25),
                Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
            };

            Label toLabel = new Label
            {
                Text = "To:",
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                Location = new Point(400, 20),
                Size = new Size(25, 20)
            };

            this.dtpTo = new DateTimePicker
            {
                Location = new Point(430, 18),
                Size = new Size(120, 25),
                Value = DateTime.Now
            };

            this.btnGenerate = new Button
            {
                Text = "?? Generate",
                Location = new Point(570, 15),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(23, 162, 184),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            this.btnGenerate.FlatAppearance.BorderSize = 0;
            this.btnGenerate.Click += BtnGenerate_Click;

            this.btnExport = new Button
            {
                Text = "?? Export CSV",
                Location = new Point(680, 15),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.Click += BtnExport_Click;

            this.lblRecordCount = new Label
            {
                Text = "Total Records: 0",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(23, 162, 184),
                Location = new Point(1050, 20),
                Size = new Size(200, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            this.filterPanel.Controls.Add(periodLabel);
            this.filterPanel.Controls.Add(this.cmbPeriodFilter);
            this.filterPanel.Controls.Add(fromLabel);
            this.filterPanel.Controls.Add(this.dtpFrom);
            this.filterPanel.Controls.Add(toLabel);
            this.filterPanel.Controls.Add(this.dtpTo);
            this.filterPanel.Controls.Add(this.btnGenerate);
            this.filterPanel.Controls.Add(this.btnExport);
            this.filterPanel.Controls.Add(this.lblRecordCount);

            // Summary Panel
            this.summaryPanel = new Panel();
            this.summaryPanel.Size = new Size(1300, 80);
            this.summaryPanel.Location = new Point(0, 140);
            this.summaryPanel.Dock = DockStyle.Top;
            this.summaryPanel.BackColor = Color.FromArgb(248, 249, 250);

            CreateSummaryCards();

            // Data Panel
            this.dataPanel = new Panel();
            this.dataPanel.Location = new Point(0, 220);
            this.dataPanel.Size = new Size(1300, 580);
            this.dataPanel.Dock = DockStyle.Fill;
            this.dataPanel.BackColor = Color.White;
            this.dataPanel.Padding = new Padding(20);

            // DataGridView
            this.dataGridView = new DataGridView();
            this.dataGridView.Location = new Point(20, 20);
            this.dataGridView.Size = new Size(1260, 540);
            this.dataGridView.Dock = DockStyle.Fill;
            this.dataGridView.BackgroundColor = Color.White;
            this.dataGridView.BorderStyle = BorderStyle.None;
            this.dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(23, 162, 184);
            this.dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.dataGridView.ColumnHeadersHeight = 40;
            this.dataGridView.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            this.dataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 248, 255);
            this.dataGridView.DefaultCellStyle.SelectionForeColor = Color.FromArgb(23, 162, 184);
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.ReadOnly = true;
            this.dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            this.dataPanel.Controls.Add(this.dataGridView);

            // Add panels to form
            this.Controls.Add(this.dataPanel);
            this.Controls.Add(this.summaryPanel);
            this.Controls.Add(this.filterPanel);
            this.Controls.Add(this.headerPanel);

            this.ResumeLayout(false);
        }

        private void CreateSummaryCards()
        {
            string[] titles = { "Total Revenue", "Completed Jobs", "Average Job Value", "This Month Growth" };
            string[] icons = { "??", "?", "??", "??" };
            Color[] colors = {
                Color.FromArgb(23, 162, 184),   // Teal
                Color.FromArgb(40, 167, 69),    // Green
                Color.FromArgb(255, 193, 7),    // Yellow
                Color.FromArgb(220, 53, 69)     // Red
            };

            for (int i = 0; i < titles.Length; i++)
            {
                Panel card = new Panel
                {
                    Size = new Size(300, 60),
                    Location = new Point(20 + i * 320, 10),
                    BackColor = colors[i]
                };

                Label iconLabel = new Label
                {
                    Text = icons[i],
                    Font = new Font("Segoe UI Emoji", 16F),
                    ForeColor = Color.White,
                    Location = new Point(15, 10),
                    Size = new Size(25, 25),
                    BackColor = Color.Transparent
                };

                Label titleLabel = new Label
                {
                    Text = titles[i],
                    Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                    ForeColor = Color.White,
                    Location = new Point(50, 8),
                    Size = new Size(200, 15),
                    BackColor = Color.Transparent
                };

                Label valueLabel = new Label
                {
                    Name = $"summary_{i}",
                    Text = "Loading...",
                    Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(50, 25),
                    Size = new Size(200, 25),
                    BackColor = Color.Transparent
                };

                switch (i)
                {
                    case 0: this.lblTotalRevenue = valueLabel; break;
                    case 1: this.lblCompletedJobs = valueLabel; break;
                    case 2: this.lblAverageValue = valueLabel; break;
                }

                card.Controls.Add(iconLabel);
                card.Controls.Add(titleLabel);
                card.Controls.Add(valueLabel);
                this.summaryPanel.Controls.Add(card);
            }
        }

        private void LoadRevenueData()
        {
            try
            {
                // Since we don't have a Payment table, we'll simulate revenue based on completed jobs
                string query = @"
                    SELECT 
                        j.JobID as 'Job ID',
                        CONCAT(c.FirstName, ' ', c.LastName) as 'Customer Name',
                        j.PickupLocation as 'Pickup Location',
                        j.DeliveryLocation as 'Delivery Location',
                        j.Weight,
                        j.RequestDate as 'Request Date',
                        j.PreferredDate as 'Completion Date',
                        j.Status,
                        -- Simulate revenue calculation based on weight and distance
                        ROUND((j.Weight * 2.5) + 
                              (LENGTH(j.PickupLocation) + LENGTH(j.DeliveryLocation)) * 0.5 + 
                              CASE j.Urgency 
                                  WHEN 'High' THEN 50 
                                  WHEN 'Medium' THEN 25 
                                  ELSE 0 
                              END, 2) as 'Estimated Revenue',
                        j.Urgency,
                        ld.TruckNumber as 'Truck Number',
                        ld.DriverName as 'Driver Name'
                    FROM Job j
                    INNER JOIN Customer c ON j.CustomerID = c.CustomerID
                    LEFT JOIN LoadDetails ld ON j.JobID = ld.JobID
                    WHERE j.Status = 'Completed'
                    ORDER BY j.PreferredDate DESC";

                DataTable revenueData = dbConnection.ExecuteSelect(query);
                this.dataGridView.DataSource = revenueData;
                
                this.lblRecordCount.Text = $"Total Records: {revenueData.Rows.Count:N0}";
                UpdateRevenueSummary(revenueData);
                
                if (revenueData.Rows.Count > 0)
                {
                    FormatRevenueDataGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading revenue data: {ex.Message}", 
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.lblRecordCount.Text = "Total Records: 0";
            }
        }

        private void UpdateRevenueSummary(DataTable revenueData)
        {
            try
            {
                decimal totalRevenue = 0;
                int completedJobs = revenueData.Rows.Count;

                foreach (DataRow row in revenueData.Rows)
                {
                    if (decimal.TryParse(row["Estimated Revenue"].ToString(), out decimal revenue))
                    {
                        totalRevenue += revenue;
                    }
                }

                decimal averageValue = completedJobs > 0 ? totalRevenue / completedJobs : 0;

                this.lblTotalRevenue.Text = $"${totalRevenue:N2}";
                this.lblCompletedJobs.Text = completedJobs.ToString("N0");
                this.lblAverageValue.Text = $"${averageValue:N2}";

                // Calculate this month's data for growth
                var thisMonthData = revenueData.AsEnumerable()
                    .Where(row => {
                        if (DateTime.TryParse(row["Completion Date"].ToString(), out DateTime date))
                        {
                            return date.Month == DateTime.Now.Month && date.Year == DateTime.Now.Year;
                        }
                        return false;
                    });

                decimal thisMonthRevenue = 0;
                foreach (var row in thisMonthData)
                {
                    if (decimal.TryParse(row["Estimated Revenue"].ToString(), out decimal revenue))
                    {
                        thisMonthRevenue += revenue;
                    }
                }

                // Update the growth card
                var growthCard = this.summaryPanel.Controls[3];
                var growthLabel = growthCard.Controls[2] as Label;
                if (growthLabel != null)
                {
                    growthLabel.Text = $"${thisMonthRevenue:N2}";
                }
            }
            catch
            {
                this.lblTotalRevenue.Text = "$0.00";
                this.lblCompletedJobs.Text = "0";
                this.lblAverageValue.Text = "$0.00";
            }
        }

        private void FormatRevenueDataGrid()
        {
            this.dataGridView.Columns["Job ID"].Width = 70;
            this.dataGridView.Columns["Customer Name"].Width = 150;
            this.dataGridView.Columns["Weight"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView.Columns["Weight"].DefaultCellStyle.Format = "N2";
            this.dataGridView.Columns["Request Date"].DefaultCellStyle.Format = "MM/dd/yyyy";
            this.dataGridView.Columns["Completion Date"].DefaultCellStyle.Format = "MM/dd/yyyy";
            this.dataGridView.Columns["Estimated Revenue"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView.Columns["Estimated Revenue"].DefaultCellStyle.Format = "C2";
            this.dataGridView.Columns["Estimated Revenue"].DefaultCellStyle.BackColor = Color.FromArgb(230, 255, 230);
            this.dataGridView.Columns["Status"].Width = 80;
            this.dataGridView.Columns["Urgency"].Width = 80;
        }

        private void CmbPeriodFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = this.cmbPeriodFilter.SelectedItem.ToString();
            switch (selected)
            {
                case "This Month":
                    this.dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    this.dtpTo.Value = DateTime.Now;
                    this.dtpFrom.Enabled = false;
                    this.dtpTo.Enabled = false;
                    break;
                case "Last 3 Months":
                    this.dtpFrom.Value = DateTime.Now.AddMonths(-3);
                    this.dtpTo.Value = DateTime.Now;
                    this.dtpFrom.Enabled = false;
                    this.dtpTo.Enabled = false;
                    break;
                case "This Year":
                    this.dtpFrom.Value = new DateTime(DateTime.Now.Year, 1, 1);
                    this.dtpTo.Value = DateTime.Now;
                    this.dtpFrom.Enabled = false;
                    this.dtpTo.Enabled = false;
                    break;
                case "Custom Range":
                    this.dtpFrom.Enabled = true;
                    this.dtpTo.Enabled = true;
                    break;
                default: // All Time
                    this.dtpFrom.Value = DateTime.Now.AddYears(-5);
                    this.dtpTo.Value = DateTime.Now;
                    this.dtpFrom.Enabled = false;
                    this.dtpTo.Enabled = false;
                    break;
            }
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                string query = @"
                    SELECT 
                        j.JobID as 'Job ID',
                        CONCAT(c.FirstName, ' ', c.LastName) as 'Customer Name',
                        j.PickupLocation as 'Pickup Location',
                        j.DeliveryLocation as 'Delivery Location',
                        j.Weight,
                        j.RequestDate as 'Request Date',
                        j.PreferredDate as 'Completion Date',
                        j.Status,
                        ROUND((j.Weight * 2.5) + 
                              (LENGTH(j.PickupLocation) + LENGTH(j.DeliveryLocation)) * 0.5 + 
                              CASE j.Urgency 
                                  WHEN 'High' THEN 50 
                                  WHEN 'Medium' THEN 25 
                                  ELSE 0 
                              END, 2) as 'Estimated Revenue',
                        j.Urgency,
                        ld.TruckNumber as 'Truck Number',
                        ld.DriverName as 'Driver Name'
                    FROM Job j
                    INNER JOIN Customer c ON j.CustomerID = c.CustomerID
                    LEFT JOIN LoadDetails ld ON j.JobID = ld.JobID
                    WHERE j.Status = 'Completed'";

                // Apply date range
                query += $" AND j.PreferredDate BETWEEN '{this.dtpFrom.Value:yyyy-MM-dd}' AND '{this.dtpTo.Value:yyyy-MM-dd}'";
                query += " ORDER BY j.PreferredDate DESC";

                DataTable filteredData = dbConnection.ExecuteSelect(query);
                this.dataGridView.DataSource = filteredData;

                this.lblRecordCount.Text = $"Total Records: {filteredData.Rows.Count:N0}";
                UpdateRevenueSummary(filteredData);

                if (filteredData.Rows.Count > 0)
                {
                    FormatRevenueDataGrid();
                }

                MessageBox.Show($"Report generated successfully!\nShowing {filteredData.Rows.Count} completed jobs.", 
                    "Report Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV Files (*.csv)|*.csv";
                saveDialog.FileName = $"RevenueReport_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToCSV(saveDialog.FileName);
                    MessageBox.Show($"Report exported successfully to:\n{saveDialog.FileName}", 
                        "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting report: {ex.Message}", 
                    "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToCSV(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                // Write summary header
                writer.WriteLine("Revenue Report Summary");
                writer.WriteLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                writer.WriteLine($"Period: {this.dtpFrom.Value:yyyy-MM-dd} to {this.dtpTo.Value:yyyy-MM-dd}");
                writer.WriteLine($"Total Revenue: {this.lblTotalRevenue.Text}");
                writer.WriteLine($"Completed Jobs: {this.lblCompletedJobs.Text}");
                writer.WriteLine($"Average Job Value: {this.lblAverageValue.Text}");
                writer.WriteLine("");

                // Write headers
                string[] headers = new string[dataGridView.Columns.Count];
                for (int i = 0; i < dataGridView.Columns.Count; i++)
                {
                    headers[i] = dataGridView.Columns[i].HeaderText;
                }
                writer.WriteLine(string.Join(",", headers));

                // Write data rows
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string[] values = new string[row.Cells.Count];
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            values[i] = row.Cells[i].Value?.ToString() ?? "";
                            if (values[i].Contains(",") || values[i].Contains("\""))
                            {
                                values[i] = "\"" + values[i].Replace("\"", "\"\"") + "\"";
                            }
                        }
                        writer.WriteLine(string.Join(",", values));
                    }
                }
            }
        }

        private void HeaderPanel_Paint(object sender, PaintEventArgs e)
        {
            using (var brush = new LinearGradientBrush(
                headerPanel.ClientRectangle,
                Color.FromArgb(23, 162, 184),
                Color.FromArgb(19, 132, 150),
                LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, headerPanel.ClientRectangle);
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