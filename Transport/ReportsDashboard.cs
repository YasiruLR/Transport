using System.Data;
using System.Drawing.Drawing2D;

namespace Transport
{
    public partial class ReportsDashboard : Form
    {
        private DatabaseConnection dbConnection;
        private Panel statsOverviewPanel;
        private Panel chartPanel;
        private Label lblCustomerCount;
        private Label lblJobCount;
        private Label lblProductCount;
        private Label lblLoadCount;
        private Label lblPendingJobs;
        private Label lblCompletedJobs;
        private Label lblActiveLoads;
        private Label lblRevenue;

        public ReportsDashboard()
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            LoadDashboardStats();
            CreateVisualCharts();
        }

        private void InitializeComponent()
        {
            this.statsOverviewPanel = new Panel();
            this.chartPanel = new Panel();
            this.lblCustomerCount = new Label();
            this.lblJobCount = new Label();
            this.lblProductCount = new Label();
            this.lblLoadCount = new Label();
            this.lblPendingJobs = new Label();
            this.lblCompletedJobs = new Label();
            this.lblActiveLoads = new Label();
            this.lblRevenue = new Label();
            this.SuspendLayout();

            // Form Properties
            this.Text = "Reports Dashboard - Transport Management System";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 250);

            // Header Panel
            Panel headerPanel = new Panel();
            headerPanel.Location = new Point(0, 0);
            headerPanel.Size = new Size(1200, 80);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.BackColor = Color.FromArgb(102, 16, 242);

            Label lblTitle = new Label();
            lblTitle.Text = "?? Reports & Analytics Dashboard";
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(30, 25);
            lblTitle.Size = new Size(600, 30);
            lblTitle.BackColor = Color.Transparent;

            Button btnClose = new Button();
            btnClose.Text = "?";
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

            // Main Content Panel
            Panel contentPanel = new Panel();
            contentPanel.Location = new Point(20, 100);
            contentPanel.Size = new Size(1160, 650);
            contentPanel.BackColor = Color.White;
            contentPanel.BorderStyle = BorderStyle.FixedSingle;

            // Reports Section Title
            Label lblReportsTitle = new Label();
            lblReportsTitle.Text = "Professional Report Generation";
            lblReportsTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblReportsTitle.ForeColor = Color.FromArgb(52, 58, 64);
            lblReportsTitle.Location = new Point(30, 20);
            lblReportsTitle.Size = new Size(400, 30);

            // Customer Report Button
            Button btnCustomerReport = CreateReportButton("?? Customer Report", "Comprehensive customer analytics with filtering", 
                new Point(80, 80), Color.FromArgb(40, 167, 69));
            btnCustomerReport.Click += new EventHandler(this.btnCustomerReport_Click);

            // Job Report Button
            Button btnJobReport = CreateReportButton("?? Job Report", "Job tracking, status monitoring & analytics", 
                new Point(400, 80), Color.FromArgb(0, 123, 255));
            btnJobReport.Click += new EventHandler(this.btnJobReport_Click);

            // Load Report Button
            Button btnLoadReport = CreateReportButton("?? Load Report", "Logistics, truck utilization & performance", 
                new Point(720, 80), Color.FromArgb(220, 53, 69));
            btnLoadReport.Click += new EventHandler(this.btnLoadReport_Click);

            // Product Report Button
            Button btnProductReport = CreateReportButton("?? Product Report", "Inventory management & stock analysis", 
                new Point(80, 250), Color.FromArgb(255, 193, 7));
            btnProductReport.Click += new EventHandler(this.btnProductReport_Click);

            // Dashboard Stats Button
            Button btnDashboardStats = CreateReportButton("?? Dashboard Report", "System overview & KPI analytics", 
                new Point(400, 250), Color.FromArgb(102, 16, 242));
            btnDashboardStats.Click += new EventHandler(this.btnDashboardStats_Click);

            // Export Options Button
            Button btnExportOptions = CreateReportButton("?? Export Center", "Multi-format export & data integration", 
                new Point(720, 250), Color.FromArgb(108, 117, 125));
            btnExportOptions.Click += new EventHandler(this.btnExportOptions_Click);

            // Reports Features Panel
            Panel featuresPanel = new Panel();
            featuresPanel.Location = new Point(80, 420);
            featuresPanel.Size = new Size(1000, 120);
            featuresPanel.BackColor = Color.FromArgb(248, 249, 250);
            featuresPanel.BorderStyle = BorderStyle.FixedSingle;

            Label lblFeaturesTitle = new Label();
            lblFeaturesTitle.Text = "? Report Features";
            lblFeaturesTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblFeaturesTitle.ForeColor = Color.FromArgb(52, 58, 64);
            lblFeaturesTitle.Location = new Point(20, 15);
            lblFeaturesTitle.Size = new Size(300, 25);

            Label lblFeatures = new Label();
            lblFeatures.Text = "• Professional Data Export (CSV, JSON, XML)  • Advanced Filtering & Search  • Real-time Analytics\n" +
                              "• Interactive Data Grids  • Status Color Coding  • Statistical Summaries  • Date Range Filtering";
            lblFeatures.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblFeatures.ForeColor = Color.FromArgb(108, 117, 125);
            lblFeatures.Location = new Point(20, 45);
            lblFeatures.Size = new Size(960, 60);

            featuresPanel.Controls.Add(lblFeaturesTitle);
            featuresPanel.Controls.Add(lblFeatures);

            // Add stats summary
            CreateStatsPanel(contentPanel);

            contentPanel.Controls.Add(lblReportsTitle);
            contentPanel.Controls.Add(btnCustomerReport);
            contentPanel.Controls.Add(btnJobReport);
            contentPanel.Controls.Add(btnLoadReport);
            contentPanel.Controls.Add(btnProductReport);
            contentPanel.Controls.Add(btnDashboardStats);
            contentPanel.Controls.Add(btnExportOptions);
            contentPanel.Controls.Add(featuresPanel);

            // Add controls to form
            this.Controls.Add(headerPanel);
            this.Controls.Add(contentPanel);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Button CreateReportButton(string title, string description, Point location, Color color)
        {
            Button btn = new Button();
            btn.Location = location;
            btn.Size = new Size(280, 120);
            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.Paint += new PaintEventHandler(this.button_Paint);

            // Create custom text display
            btn.Paint += (s, e) =>
            {
                Button button = s as Button;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // Draw title
                using (SolidBrush titleBrush = new SolidBrush(Color.White))
                {
                    Font titleFont = new Font("Segoe UI", 12F, FontStyle.Bold);
                    e.Graphics.DrawString(title, titleFont, titleBrush, new RectangleF(10, 20, 260, 30));
                }

                // Draw description
                using (SolidBrush descBrush = new SolidBrush(Color.FromArgb(230, 230, 230)))
                {
                    Font descFont = new Font("Segoe UI", 9F, FontStyle.Regular);
                    e.Graphics.DrawString(description, descFont, descBrush, new RectangleF(10, 55, 260, 50));
                }
            };

            // Add hover effect
            Color hoverColor = ControlPaint.Dark(color, 0.1f);
            btn.MouseEnter += (s, e) => { btn.BackColor = hoverColor; };
            btn.MouseLeave += (s, e) => { btn.BackColor = color; };

            return btn;
        }

        private void CreateStatsPanel(Panel parentPanel)
        {
            Panel statsPanel = new Panel();
            statsPanel.Location = new Point(80, 560);
            statsPanel.Size = new Size(1000, 60);
            statsPanel.BackColor = Color.FromArgb(248, 249, 250);

            try
            {
                int totalCustomers = GetRecordCount("Customer");
                int totalJobs = GetRecordCount("Job");
                int totalProducts = GetRecordCount("Product");
                int totalLoads = GetRecordCount("LoadDetails");

                Label lblStats = new Label();
                lblStats.Text = $"?? System Stats: {totalCustomers} Customers | {totalJobs} Jobs | {totalProducts} Products | {totalLoads} Loads | Reports Enabled ?";
                lblStats.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                lblStats.ForeColor = Color.FromArgb(52, 58, 64);
                lblStats.Location = new Point(20, 20);
                lblStats.Size = new Size(960, 25);
                lblStats.TextAlign = ContentAlignment.MiddleCenter;

                statsPanel.Controls.Add(lblStats);
            }
            catch (Exception ex)
            {
                Label lblError = new Label();
                lblError.Text = "Reports Ready - Statistics loading...";
                lblError.Font = new Font("Segoe UI", 12F, FontStyle.Italic);
                lblError.ForeColor = Color.FromArgb(102, 16, 242);
                lblError.Location = new Point(20, 20);
                lblError.Size = new Size(960, 25);
                lblError.TextAlign = ContentAlignment.MiddleCenter;
                statsPanel.Controls.Add(lblError);
            }

            parentPanel.Controls.Add(statsPanel);
        }

        private int GetRecordCount(string tableName)
        {
            try
            {
                string query = $"SELECT COUNT(*) FROM {tableName}";
                var result = dbConnection.ExecuteScalar(query);
                return Convert.ToInt32(result);
            }
            catch
            {
                return 0;
            }
        }

        // Event Handlers for Reports
        private void btnCustomerReport_Click(object sender, EventArgs e)
        {
            try
            {
                CustomerReportForm customerReport = new CustomerReportForm();
                customerReport.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Customer Report: {ex.Message}", "Report Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnJobReport_Click(object sender, EventArgs e)
        {
            try
            {
                JobReportForm jobReport = new JobReportForm();
                jobReport.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Job Report: {ex.Message}", "Report Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoadReport_Click(object sender, EventArgs e)
        {
            try
            {
                LoadReportForm loadReport = new LoadReportForm();
                loadReport.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Load Report: {ex.Message}", "Report Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnProductReport_Click(object sender, EventArgs e)
        {
            try
            {
                ProductReportForm productReport = new ProductReportForm();
                productReport.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Product Report: {ex.Message}", "Report Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDashboardStats_Click(object sender, EventArgs e)
        {
            try
            {
                DashboardStatsForm dashboardStats = new DashboardStatsForm();
                dashboardStats.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Dashboard Stats: {ex.Message}", "Report Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportOptions_Click(object sender, EventArgs e)
        {
            try
            {
                ExportOptionsForm exportOptions = new ExportOptionsForm();
                exportOptions.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Export Options: {ex.Message}", "Export Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDashboardStats()
        {
            // Load dashboard statistics
            try
            {
                // Implementation for loading stats
            }
            catch (Exception ex)
            {
                // Handle errors gracefully
            }
        }

        private void CreateVisualCharts()
        {
            // Create visual charts
            try
            {
                // Implementation for charts
            }
            catch (Exception ex)
            {
                // Handle errors gracefully
            }
        }

        // Custom Paint Events
        private void button_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            
            using (GraphicsPath path = new GraphicsPath())
            {
                int radius = 15;
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);
                path.CloseFigure();
                
                btn.Region = new Region(path);
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