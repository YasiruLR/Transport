using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;
using System.Linq;

namespace Transport
{
    public partial class AdminDashboard : Form
    {
        private int adminId;
        private string adminRole;
        private Panel headerPanel;
        private Panel mainContentPanel;
        private Panel quickActionsPanel;
        private Panel statsPanel;
        private Panel sidebarPanel;
        private Label lblWelcomeTitle;
        private Label lblDateTime;
        private System.Windows.Forms.Timer timeTimer;
        private DatabaseConnection dbConnection;
        private Label lblWelcome;

        public AdminDashboard(int adminId, string role)
        {
            this.adminId = adminId;
            this.adminRole = role;
            this.dbConnection = new DatabaseConnection();
            InitializeComponent();
            SetupTimer();
            LoadDashboardStats();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Initialize components
            this.headerPanel = new Panel();
            this.mainContentPanel = new Panel();
            this.quickActionsPanel = new Panel();
            this.statsPanel = new Panel();
            this.sidebarPanel = new Panel();
            this.lblWelcomeTitle = new Label();
            this.lblDateTime = new Label();
            this.lblWelcome = new Label();

            // Form properties
            this.Text = "Transport Management System - Admin Dashboard";
            this.Size = new Size(1400, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.MinimumSize = new Size(1200, 700);

            // Create sidebar
            CreateSidebar();

            // Header Panel
            this.headerPanel.Location = new Point(250, 0);
            this.headerPanel.Size = new Size(1150, 80);
            this.headerPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.headerPanel.BackColor = Color.FromArgb(40, 40, 40);

            // Welcome Title
            this.lblWelcomeTitle.Text = $"Welcome, {adminRole}";
            this.lblWelcomeTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            this.lblWelcomeTitle.ForeColor = Color.White;
            this.lblWelcomeTitle.Location = new Point(30, 15);
            this.lblWelcomeTitle.Size = new Size(400, 30);
            this.lblWelcomeTitle.BackColor = Color.Transparent;

            // Subtitle
            this.lblWelcome.Text = "Admin Control Panel";
            this.lblWelcome.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            this.lblWelcome.ForeColor = Color.FromArgb(180, 180, 180);
            this.lblWelcome.Location = new Point(30, 50);
            this.lblWelcome.Size = new Size(400, 20);
            this.lblWelcome.BackColor = Color.Transparent;

            // Date Time
            this.lblDateTime.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            this.lblDateTime.ForeColor = Color.FromArgb(100, 150, 255);
            this.lblDateTime.Location = new Point(800, 30);
            this.lblDateTime.Size = new Size(300, 20);
            this.lblDateTime.TextAlign = ContentAlignment.MiddleRight;
            this.lblDateTime.BackColor = Color.Transparent;
            this.lblDateTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // Logout Button
            Button btnLogout = new Button
            {
                Text = "Logout",
                Location = new Point(1050, 25),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(220, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += LogoutBtn_Click;

            this.headerPanel.Controls.Add(this.lblWelcomeTitle);
            this.headerPanel.Controls.Add(this.lblWelcome);
            this.headerPanel.Controls.Add(this.lblDateTime);
            this.headerPanel.Controls.Add(btnLogout);

            // Main Content Panel
            this.mainContentPanel.Location = new Point(250, 80);
            this.mainContentPanel.Size = new Size(1150, 820);
            this.mainContentPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.mainContentPanel.BackColor = Color.FromArgb(30, 30, 30);
            this.mainContentPanel.Padding = new Padding(20);

            // Stats Panel
            this.statsPanel.Location = new Point(20, 20);
            this.statsPanel.Size = new Size(1110, 120);
            this.statsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.statsPanel.BackColor = Color.Transparent;

            // Quick Actions Panel
            this.quickActionsPanel.Location = new Point(20, 160);
            this.quickActionsPanel.Size = new Size(1110, 640);
            this.quickActionsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.quickActionsPanel.BackColor = Color.Transparent;

            this.mainContentPanel.Controls.Add(this.statsPanel);
            this.mainContentPanel.Controls.Add(this.quickActionsPanel);

            // Add all panels to form
            this.Controls.Add(this.sidebarPanel);
            this.Controls.Add(this.headerPanel);
            this.Controls.Add(this.mainContentPanel);

            // Create dashboard content
            CreateStatsCards();
            CreateActionCards();

            this.ResumeLayout(false);
        }

        private void CreateSidebar()
        {
            this.sidebarPanel.Location = new Point(0, 0);
            this.sidebarPanel.Size = new Size(250, 900);
            this.sidebarPanel.Dock = DockStyle.Left;
            this.sidebarPanel.BackColor = Color.FromArgb(50, 50, 50);

            // Logo section
            Label logoLabel = new Label
            {
                Text = "TMS Admin",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 150, 255),
                Location = new Point(20, 20),
                Size = new Size(200, 30),
                BackColor = Color.Transparent
            };

            // Navigation menu
            string[] menuItems = { "Dashboard", "Customers", "Products", "Jobs", "Loads", "Reports" };
            for (int i = 0; i < menuItems.Length; i++)
            {
                Button menuBtn = new Button
                {
                    Text = menuItems[i],
                    Size = new Size(230, 45),
                    Location = new Point(10, 80 + (i * 50)),
                    BackColor = i == 0 ? Color.FromArgb(100, 150, 255) : Color.Transparent,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(15, 0, 0, 0),
                    Cursor = Cursors.Hand,
                    Tag = menuItems[i]
                };

                menuBtn.FlatAppearance.BorderSize = 0;
                menuBtn.FlatAppearance.MouseOverBackColor = Color.FromArgb(100, 150, 255);
                menuBtn.Click += MenuButton_Click;

                this.sidebarPanel.Controls.Add(menuBtn);
            }

            this.sidebarPanel.Controls.Add(logoLabel);
        }

        private void CreateStatsCards()
        {
            string[] titles = { "Customers", "Jobs", "Products", "Loads" };
            Color[] colors = {
                Color.FromArgb(100, 150, 255),  // Blue
                Color.FromArgb(50, 200, 100),   // Green
                Color.FromArgb(255, 150, 50),   // Orange
                Color.FromArgb(200, 100, 255)   // Purple
            };

            for (int i = 0; i < titles.Length; i++)
            {
                Panel card = new Panel
                {
                    Size = new Size(260, 100),
                    Location = new Point(i * 280, 10),
                    BackColor = Color.FromArgb(50, 50, 50)
                };

                // Title
                Label titleLabel = new Label
                {
                    Text = titles[i],
                    Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                    ForeColor = Color.FromArgb(180, 180, 180),
                    Location = new Point(20, 20),
                    Size = new Size(100, 20),
                    BackColor = Color.Transparent
                };

                // Value
                Label valueLabel = new Label
                {
                    Name = "lblStatValue",
                    Text = "Loading...",
                    Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                    ForeColor = colors[i],
                    Location = new Point(20, 45),
                    Size = new Size(100, 30),
                    BackColor = Color.Transparent
                };

                card.Controls.Add(titleLabel);
                card.Controls.Add(valueLabel);
                statsPanel.Controls.Add(card);
            }
        }

        private void CreateActionCards()
        {
            // Title
            Label titleLabel = new Label
            {
                Text = "Quick Actions",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                Size = new Size(200, 30),
                BackColor = Color.Transparent
            };
            this.quickActionsPanel.Controls.Add(titleLabel);

            // Action cards
            var actions = new[]
            {
                new { Title = "Manage Customers", Color = Color.FromArgb(100, 150, 255), Action = "Customers" },
                new { Title = "Manage Products", Color = Color.FromArgb(50, 200, 100), Action = "Products" },
                new { Title = "Review Jobs", Color = Color.FromArgb(255, 150, 50), Action = "Jobs" },
                new { Title = "Track Loads", Color = Color.FromArgb(200, 100, 255), Action = "Loads" },
                new { Title = "View Reports", Color = Color.FromArgb(255, 100, 150), Action = "Reports" },
                new { Title = "System Settings", Color = Color.FromArgb(150, 150, 150), Action = "Settings" }
            };

            for (int i = 0; i < actions.Length; i++)
            {
                int row = i / 3;
                int col = i % 3;
                var action = actions[i];

                Button card = new Button
                {
                    Text = action.Title,
                    Size = new Size(350, 120),
                    Location = new Point(20 + col * 370, 70 + row * 140),
                    BackColor = action.Color,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Tag = action.Action
                };
                card.FlatAppearance.BorderSize = 0;
                card.Click += ActionCard_Click;

                this.quickActionsPanel.Controls.Add(card);
            }
        }

        // Timer setup
        private void SetupTimer()
        {
            this.timeTimer = new System.Windows.Forms.Timer();
            this.timeTimer.Interval = 1000;
            this.timeTimer.Tick += TimeTimer_Tick;
            this.timeTimer.Start();
        }

        private void TimeTimer_Tick(object sender, EventArgs e)
        {
            this.lblDateTime.Text = DateTime.Now.ToString("MMMM dd, yyyy • HH:mm:ss");
        }

        // Load dashboard stats from database
        private void LoadDashboardStats()
        {
            try
            {
                int customerCount = GetRecordCount("Customer");
                int jobCount = GetRecordCount("Job");
                int productCount = GetRecordCount("Product");
                int loadCount = GetRecordCount("LoadDetails");

                UpdateStatValue(0, customerCount);
                UpdateStatValue(1, jobCount);
                UpdateStatValue(2, productCount);
                UpdateStatValue(3, loadCount);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard statistics: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
                // Set fallback values
                for (int i = 0; i < 4; i++)
                {
                    UpdateStatValue(i, 0);
                }
            }
        }

        private int GetRecordCount(string tableName)
        {
            try
            {
                string query = $"SELECT COUNT(*) FROM {tableName}";
                var result = dbConnection.ExecuteScalar(query);
                return result != null ? Convert.ToInt32(result) : 0;
            }
            catch
            {
                return 0;
            }
        }

        private void UpdateStatValue(int cardIndex, int value)
        {
            if (statsPanel.Controls.Count > cardIndex)
            {
                var card = statsPanel.Controls[cardIndex];
                var valueLabel = card.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "lblStatValue");
                if (valueLabel != null)
                {
                    valueLabel.Text = value.ToString();
                }
            }
        }

        // Event Handlers
        private void MenuButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                string action = btn.Tag.ToString();
                HandleAction(action);
                
                // Update visual state
                foreach (Button menuBtn in sidebarPanel.Controls.OfType<Button>())
                {
                    menuBtn.BackColor = menuBtn == btn ? Color.FromArgb(100, 150, 255) : Color.Transparent;
                }
            }
        }

        private void ActionCard_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                string action = btn.Tag.ToString();
                HandleAction(action);
            }
        }

        private void HandleAction(string action)
        {
            try
            {
                switch (action)
                {
                    case "Customers":
                        try
                        {
                            var customerForm = new CustomerManagementForm();
                            customerForm.ShowDialog();
                        }
                        catch
                        {
                            MessageBox.Show("Customer Management\n\nManage customer accounts and information.",
                                "Customer Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                    case "Products":
                        try
                        {
                            var productForm = new ProductManagementForm();
                            productForm.ShowDialog();
                        }
                        catch
                        {
                            MessageBox.Show("Product Management\n\nManage product catalog and inventory.",
                                "Product Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                    case "Jobs":
                        try
                        {
                            var jobForm = new JobManagementForm();
                            jobForm.ShowDialog();
                        }
                        catch
                        {
                            MessageBox.Show("Job Management\n\nReview and manage transport jobs.",
                                "Job Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                    case "Loads":
                        try
                        {
                            var loadForm = new LoadManagementForm();
                            loadForm.ShowDialog();
                        }
                        catch
                        {
                            MessageBox.Show("Load Management\n\nTrack and assign vehicle loads.",
                                "Load Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                    case "Reports":
                        try
                        {
                            var reportsHub = new ReportsHub();
                            reportsHub.ShowDialog();
                        }
                        catch
                        {
                            MessageBox.Show("📊 Reports Hub\n\nComprehensive database reports:\n\n" +
                                "• Customer Database Report\n" +
                                "• Job Status & Analytics\n" +
                                "• Product Inventory Report\n" +
                                "• Load & Transportation Report\n" +
                                "• Admin Users Report\n" +
                                "• Revenue & Financial Analytics\n\n" +
                                "All reports include filtering, statistics, and export features.",
                                "Reports Hub", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                    case "Settings":
                        MessageBox.Show("Settings\n\nSystem configuration and preferences.",
                            "Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case "Dashboard":
                        LoadDashboardStats();
                        break;
                }
                
                // Refresh stats after any action
                if (action != "Dashboard" && action != "Settings" && action != "Reports")
                {
                    LoadDashboardStats();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening {action}: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LogoutBtn_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?",
                "Logout Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.timeTimer?.Stop();
                this.Hide();
                
                var loginForm = new LoginForm();
                loginForm.ShowDialog();
                this.Close();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                timeTimer?.Stop();
                timeTimer?.Dispose();
                dbConnection?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
