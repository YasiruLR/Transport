using System.Data;
using System.Drawing.Drawing2D;

namespace Transport
{
    public partial class DashboardStatsForm : Form
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

        public DashboardStatsForm()
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
            this.Text = "Dashboard Statistics - Transport Management System";
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
            lblTitle.Text = "?? Dashboard Statistics & Analytics";
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(30, 25);
            lblTitle.Size = new Size(500, 30);
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

            Button btnRefresh = new Button();
            btnRefresh.Text = "?? Refresh";
            btnRefresh.Location = new Point(1040, 20);
            btnRefresh.Size = new Size(100, 40);
            btnRefresh.BackColor = Color.FromArgb(40, 167, 69);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Click += new EventHandler(this.btnRefresh_Click);

            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(btnClose);
            headerPanel.Controls.Add(btnRefresh);

            // Stats Overview Panel
            this.statsOverviewPanel.Location = new Point(20, 100);
            this.statsOverviewPanel.Size = new Size(1160, 300);
            this.statsOverviewPanel.BackColor = Color.White;
            this.statsOverviewPanel.BorderStyle = BorderStyle.FixedSingle;
            this.statsOverviewPanel.Paint += new PaintEventHandler(this.statsOverviewPanel_Paint);

            Label lblOverviewTitle = new Label();
            lblOverviewTitle.Text = "System Overview";
            lblOverviewTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblOverviewTitle.ForeColor = Color.FromArgb(52, 58, 64);
            lblOverviewTitle.Location = new Point(30, 20);
            lblOverviewTitle.Size = new Size(300, 30);

            // Create stat cards
            CreateStatCards();

            this.statsOverviewPanel.Controls.Add(lblOverviewTitle);

            // Chart Panel
            this.chartPanel.Location = new Point(20, 420);
            this.chartPanel.Size = new Size(1160, 300);
            this.chartPanel.BackColor = Color.White;
            this.chartPanel.BorderStyle = BorderStyle.FixedSingle;
            this.chartPanel.Paint += new PaintEventHandler(this.chartPanel_Paint);

            Label lblChartsTitle = new Label();
            lblChartsTitle.Text = "Visual Analytics";
            lblChartsTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblChartsTitle.ForeColor = Color.FromArgb(52, 58, 64);
            lblChartsTitle.Location = new Point(30, 20);
            lblChartsTitle.Size = new Size(300, 30);

            this.chartPanel.Controls.Add(lblChartsTitle);

            // Add controls to form
            this.Controls.Add(headerPanel);
            this.Controls.Add(this.statsOverviewPanel);
            this.Controls.Add(this.chartPanel);

            this.ResumeLayout(false);
        }

        private void CreateStatCards()
        {
            // Customer Count Card
            Panel customerCard = CreateStatCard("Customers", "??", Color.FromArgb(40, 167, 69), 80, 70);
            this.lblCustomerCount.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            this.lblCustomerCount.ForeColor = Color.White;
            this.lblCustomerCount.Location = new Point(20, 80);
            this.lblCustomerCount.Size = new Size(150, 40);
            this.lblCustomerCount.TextAlign = ContentAlignment.MiddleCenter;
            customerCard.Controls.Add(this.lblCustomerCount);
            this.statsOverviewPanel.Controls.Add(customerCard);

            // Job Count Card
            Panel jobCard = CreateStatCard("Jobs", "??", Color.FromArgb(0, 123, 255), 320, 70);
            this.lblJobCount.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            this.lblJobCount.ForeColor = Color.White;
            this.lblJobCount.Location = new Point(20, 80);
            this.lblJobCount.Size = new Size(150, 40);
            this.lblJobCount.TextAlign = ContentAlignment.MiddleCenter;
            jobCard.Controls.Add(this.lblJobCount);
            this.statsOverviewPanel.Controls.Add(jobCard);

            // Product Count Card
            Panel productCard = CreateStatCard("Products", "??", Color.FromArgb(255, 193, 7), 560, 70);
            this.lblProductCount.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            this.lblProductCount.ForeColor = Color.White;
            this.lblProductCount.Location = new Point(20, 80);
            this.lblProductCount.Size = new Size(150, 40);
            this.lblProductCount.TextAlign = ContentAlignment.MiddleCenter;
            productCard.Controls.Add(this.lblProductCount);
            this.statsOverviewPanel.Controls.Add(productCard);

            // Load Count Card
            Panel loadCard = CreateStatCard("Loads", "??", Color.FromArgb(220, 53, 69), 800, 70);
            this.lblLoadCount.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            this.lblLoadCount.ForeColor = Color.White;
            this.lblLoadCount.Location = new Point(20, 80);
            this.lblLoadCount.Size = new Size(150, 40);
            this.lblLoadCount.TextAlign = ContentAlignment.MiddleCenter;
            loadCard.Controls.Add(this.lblLoadCount);
            this.statsOverviewPanel.Controls.Add(loadCard);

            // Secondary Stats
            Panel secondaryStats = new Panel();
            secondaryStats.Location = new Point(80, 200);
            secondaryStats.Size = new Size(1000, 80);
            secondaryStats.BackColor = Color.FromArgb(248, 249, 250);

            this.lblPendingJobs.Text = "Pending Jobs: 0";
            this.lblPendingJobs.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblPendingJobs.ForeColor = Color.FromArgb(255, 193, 7);
            this.lblPendingJobs.Location = new Point(20, 20);
            this.lblPendingJobs.Size = new Size(200, 25);

            this.lblCompletedJobs.Text = "Completed Jobs: 0";
            this.lblCompletedJobs.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblCompletedJobs.ForeColor = Color.FromArgb(40, 167, 69);
            this.lblCompletedJobs.Location = new Point(240, 20);
            this.lblCompletedJobs.Size = new Size(200, 25);

            this.lblActiveLoads.Text = "Active Loads: 0";
            this.lblActiveLoads.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblActiveLoads.ForeColor = Color.FromArgb(0, 123, 255);
            this.lblActiveLoads.Location = new Point(460, 20);
            this.lblActiveLoads.Size = new Size(200, 25);

            this.lblRevenue.Text = "System Health: Good";
            this.lblRevenue.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblRevenue.ForeColor = Color.FromArgb(102, 16, 242);
            this.lblRevenue.Location = new Point(680, 20);
            this.lblRevenue.Size = new Size(200, 25);

            secondaryStats.Controls.Add(this.lblPendingJobs);
            secondaryStats.Controls.Add(this.lblCompletedJobs);
            secondaryStats.Controls.Add(this.lblActiveLoads);
            secondaryStats.Controls.Add(this.lblRevenue);

            this.statsOverviewPanel.Controls.Add(secondaryStats);
        }

        private Panel CreateStatCard(string title, string icon, Color color, int x, int y)
        {
            Panel card = new Panel();
            card.Location = new Point(x, y);
            card.Size = new Size(190, 120);
            card.BackColor = color;
            card.Paint += new PaintEventHandler(this.statCard_Paint);

            Label lblIcon = new Label();
            lblIcon.Text = icon;
            lblIcon.Font = new Font("Segoe UI", 20F, FontStyle.Regular);
            lblIcon.ForeColor = Color.White;
            lblIcon.Location = new Point(20, 20);
            lblIcon.Size = new Size(40, 40);
            lblIcon.BackColor = Color.Transparent;

            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(70, 25);
            lblTitle.Size = new Size(100, 30);
            lblTitle.BackColor = Color.Transparent;

            card.Controls.Add(lblIcon);
            card.Controls.Add(lblTitle);

            return card;
        }

        private void LoadDashboardStats()
        {
            try
            {
                // Load customer count
                int customerCount = Convert.ToInt32(dbConnection.ExecuteScalar("SELECT COUNT(*) FROM Customer"));
                lblCustomerCount.Text = customerCount.ToString();

                // Load job count
                int jobCount = Convert.ToInt32(dbConnection.ExecuteScalar("SELECT COUNT(*) FROM Job"));
                lblJobCount.Text = jobCount.ToString();

                // Load product count
                int productCount = Convert.ToInt32(dbConnection.ExecuteScalar("SELECT COUNT(*) FROM Product"));
                lblProductCount.Text = productCount.ToString();

                // Load load count
                int loadCount = Convert.ToInt32(dbConnection.ExecuteScalar("SELECT COUNT(*) FROM LoadDetails"));
                lblLoadCount.Text = loadCount.ToString();

                // Load secondary stats
                int pendingJobs = Convert.ToInt32(dbConnection.ExecuteScalar("SELECT COUNT(*) FROM Job WHERE Status = 'Pending'"));
                lblPendingJobs.Text = $"Pending Jobs: {pendingJobs}";

                int completedJobs = Convert.ToInt32(dbConnection.ExecuteScalar("SELECT COUNT(*) FROM Job WHERE Status = 'Completed'"));
                lblCompletedJobs.Text = $"Completed Jobs: {completedJobs}";

                int activeLoads = Convert.ToInt32(dbConnection.ExecuteScalar(@"
                    SELECT COUNT(*) FROM LoadDetails ld 
                    INNER JOIN Job j ON ld.JobID = j.JobID 
                    WHERE j.Status = 'In Progress'"));
                lblActiveLoads.Text = $"Active Loads: {activeLoads}";

                // Calculate system health
                double completionRate = jobCount > 0 ? (double)completedJobs / jobCount * 100 : 0;
                string healthStatus = completionRate > 80 ? "Excellent" : completionRate > 60 ? "Good" : completionRate > 40 ? "Fair" : "Needs Attention";
                lblRevenue.Text = $"System Health: {healthStatus}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard stats: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateVisualCharts()
        {
            // Create simple bar chart representation
            Panel chartArea = new Panel();
            chartArea.Location = new Point(50, 60);
            chartArea.Size = new Size(1060, 200);
            chartArea.BackColor = Color.FromArgb(248, 249, 250);
            chartArea.Paint += new PaintEventHandler(this.chartArea_Paint);

            this.chartPanel.Controls.Add(chartArea);
        }

        private void chartArea_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            try
            {
                // Get data for chart
                int customers = Convert.ToInt32(dbConnection.ExecuteScalar("SELECT COUNT(*) FROM Customer"));
                int jobs = Convert.ToInt32(dbConnection.ExecuteScalar("SELECT COUNT(*) FROM Job"));
                int products = Convert.ToInt32(dbConnection.ExecuteScalar("SELECT COUNT(*) FROM Product"));
                int loads = Convert.ToInt32(dbConnection.ExecuteScalar("SELECT COUNT(*) FROM LoadDetails"));

                // Find max value for scaling
                int maxValue = Math.Max(Math.Max(customers, jobs), Math.Max(products, loads));
                if (maxValue == 0) maxValue = 1; // Prevent division by zero

                // Chart dimensions
                int chartWidth = panel.Width - 100;
                int chartHeight = panel.Height - 80;
                int barWidth = chartWidth / 5;
                int baseY = panel.Height - 40;

                // Colors for bars
                Color[] colors = {
                    Color.FromArgb(40, 167, 69),   // Green for customers
                    Color.FromArgb(0, 123, 255),    // Blue for jobs
                    Color.FromArgb(255, 193, 7),    // Yellow for products
                    Color.FromArgb(220, 53, 69)     // Red for loads
                };

                // Data and labels
                int[] values = { customers, jobs, products, loads };
                string[] labels = { "Customers", "Jobs", "Products", "Loads" };

                // Draw bars
                for (int i = 0; i < values.Length; i++)
                {
                    int barHeight = (int)((double)values[i] / maxValue * chartHeight);
                    int x = 50 + i * (barWidth + 20);
                    int y = baseY - barHeight;

                    // Draw bar
                    using (SolidBrush brush = new SolidBrush(colors[i]))
                    {
                        g.FillRectangle(brush, x, y, barWidth, barHeight);
                    }

                    // Draw value on top of bar
                    using (SolidBrush textBrush = new SolidBrush(Color.FromArgb(52, 58, 64)))
                    {
                        Font font = new Font("Segoe UI", 10F, FontStyle.Bold);
                        string valueText = values[i].ToString();
                        SizeF textSize = g.MeasureString(valueText, font);
                        g.DrawString(valueText, font, textBrush, 
                            x + (barWidth - textSize.Width) / 2, y - 25);
                    }

                    // Draw label below bar
                    using (SolidBrush textBrush = new SolidBrush(Color.FromArgb(52, 58, 64)))
                    {
                        Font font = new Font("Segoe UI", 9F, FontStyle.Regular);
                        SizeF textSize = g.MeasureString(labels[i], font);
                        g.DrawString(labels[i], font, textBrush, 
                            x + (barWidth - textSize.Width) / 2, baseY + 10);
                    }
                }

                // Draw chart title
                using (SolidBrush textBrush = new SolidBrush(Color.FromArgb(52, 58, 64)))
                {
                    Font titleFont = new Font("Segoe UI", 12F, FontStyle.Bold);
                    string title = "System Data Overview";
                    SizeF titleSize = g.MeasureString(title, titleFont);
                    g.DrawString(title, titleFont, textBrush, 
                        (panel.Width - titleSize.Width) / 2, 10);
                }
            }
            catch (Exception ex)
            {
                // Draw error message
                using (SolidBrush textBrush = new SolidBrush(Color.Red))
                {
                    Font font = new Font("Segoe UI", 10F, FontStyle.Regular);
                    g.DrawString("Unable to load chart data", font, textBrush, 50, 50);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDashboardStats();
            this.chartPanel.Invalidate(); // Refresh the chart
            MessageBox.Show("Dashboard statistics refreshed!", "Refresh Complete", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Custom Paint Events
        private void statsOverviewPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            
            using (SolidBrush brush = new SolidBrush(Color.White))
            {
                e.Graphics.FillRectangle(brush, panel.ClientRectangle);
            }
            
            using (Pen borderPen = new Pen(Color.FromArgb(100, 0, 0, 0), 1))
            {
                e.Graphics.DrawRectangle(borderPen, 0, 0, panel.Width - 1, panel.Height - 1);
            }
        }

        private void chartPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            
            using (SolidBrush brush = new SolidBrush(Color.White))
            {
                e.Graphics.FillRectangle(brush, panel.ClientRectangle);
            }
            
            using (Pen borderPen = new Pen(Color.FromArgb(100, 0, 0, 0), 1))
            {
                e.Graphics.DrawRectangle(borderPen, 0, 0, panel.Width - 1, panel.Height - 1);
            }
        }

        private void statCard_Paint(object sender, PaintEventArgs e)
        {
            Panel card = sender as Panel;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            
            // Create rounded corners
            using (GraphicsPath path = new GraphicsPath())
            {
                int radius = 10;
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(card.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(card.Width - radius, card.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, card.Height - radius, radius, radius, 90, 90);
                path.CloseFigure();
                
                card.Region = new Region(path);
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