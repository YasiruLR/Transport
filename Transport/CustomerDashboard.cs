using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using Timer = System.Windows.Forms.Timer;

namespace Transport
{
    public partial class CustomerDashboard : Form
    {
        private int customerId;
        private string customerName;
        private Panel headerPanel;
        private Panel mainContentPanel;
        private Panel quickActionsPanel;
        private Panel statsPanel;
        private Panel sidebarPanel;
        private Label lblWelcomeTitle;
        private Label lblDateTime;
        private Timer timeTimer;
        private PictureBox logoBox;
        private DatabaseConnection dbConnection;
        private Label lblWelcome;

        public CustomerDashboard(int customerId, string customerName)
        {
            this.customerId = customerId;
            this.customerName = customerName;
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
            this.logoBox = new PictureBox();
            this.lblWelcome = new Label();

            // Form properties
            this.Text = "Transport Management System - Customer Portal";
            this.Size = new Size(1600, 1000);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(15, 23, 42);
            this.MinimumSize = new Size(1400, 800);
            this.FormBorderStyle = FormBorderStyle.Sizable;

            // Create sidebar first
            CreateModernSidebar();

            // Header Panel
            this.headerPanel.Location = new Point(280, 0);
            this.headerPanel.Size = new Size(1320, 100);
            this.headerPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.headerPanel.BackColor = Color.FromArgb(30, 41, 59);
            this.headerPanel.Paint += HeaderPanel_Paint;

            // Logo
            this.logoBox.Location = new Point(20, 20);
            this.logoBox.Size = new Size(60, 60);
            this.logoBox.BackColor = Color.Transparent;
            this.logoBox.Paint += LogoBox_Paint;

            // Welcome Title
            this.lblWelcomeTitle.Text = $"Welcome Back, {customerName}";
            this.lblWelcomeTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            this.lblWelcomeTitle.ForeColor = Color.White;
            this.lblWelcomeTitle.Location = new Point(100, 15);
            this.lblWelcomeTitle.Size = new Size(500, 35);
            this.lblWelcomeTitle.BackColor = Color.Transparent;

            // Subtitle
            this.lblWelcome.Text = "Transport Management System • Customer Portal";
            this.lblWelcome.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            this.lblWelcome.ForeColor = Color.FromArgb(148, 163, 184);
            this.lblWelcome.Location = new Point(100, 55);
            this.lblWelcome.Size = new Size(500, 20);
            this.lblWelcome.BackColor = Color.Transparent;

            // Date Time
            this.lblDateTime.Font = new Font("Consolas", 11F, FontStyle.Regular);
            this.lblDateTime.ForeColor = Color.FromArgb(59, 130, 246);
            this.lblDateTime.Location = new Point(900, 20);
            this.lblDateTime.Size = new Size(300, 20);
            this.lblDateTime.TextAlign = ContentAlignment.MiddleRight;
            this.lblDateTime.BackColor = Color.Transparent;
            this.lblDateTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // Control buttons
            CreateHeaderButtons();

            // Add to Header Panel
            this.headerPanel.Controls.Add(this.logoBox);
            this.headerPanel.Controls.Add(this.lblWelcomeTitle);
            this.headerPanel.Controls.Add(this.lblWelcome);
            this.headerPanel.Controls.Add(this.lblDateTime);

            // Main Content Panel
            this.mainContentPanel.Location = new Point(280, 100);
            this.mainContentPanel.Size = new Size(1320, 900);
            this.mainContentPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.mainContentPanel.BackColor = Color.FromArgb(15, 23, 42);
            this.mainContentPanel.Padding = new Padding(20);
            this.mainContentPanel.AutoScroll = true;

            // Stats Panel
            this.statsPanel.Location = new Point(20, 20);
            this.statsPanel.Size = new Size(1280, 180);
            this.statsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.statsPanel.BackColor = Color.Transparent;

            // Quick Actions Panel
            this.quickActionsPanel.Location = new Point(20, 220);
            this.quickActionsPanel.Size = new Size(1280, 600);
            this.quickActionsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.quickActionsPanel.BackColor = Color.Transparent;

            this.mainContentPanel.Controls.Add(this.statsPanel);
            this.mainContentPanel.Controls.Add(this.quickActionsPanel);

            // Add all panels to form
            this.Controls.Add(this.sidebarPanel);
            this.Controls.Add(this.headerPanel);
            this.Controls.Add(this.mainContentPanel);

            // Create dashboard content
            CreateModernStatisticsCards();
            CreateCustomerGrid();

            this.ResumeLayout(false);
        }

        private void CreateModernSidebar()
        {
            this.sidebarPanel.Location = new Point(0, 0);
            this.sidebarPanel.Size = new Size(280, 1000);
            this.sidebarPanel.Dock = DockStyle.Left;
            this.sidebarPanel.BackColor = Color.FromArgb(20, 33, 61);
            this.sidebarPanel.Paint += SidebarPanel_Paint;

            // Logo section
            Panel logoSection = new Panel
            {
                Size = new Size(280, 80),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(15, 23, 42)
            };

            Label logoLabel = new Label
            {
                Text = "🚛 TMS",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(59, 130, 246),
                Location = new Point(20, 25),
                Size = new Size(100, 30),
                BackColor = Color.Transparent
            };

            Label logoSubLabel = new Label
            {
                Text = "Customer Portal",
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = Color.FromArgb(148, 163, 184),
                Location = new Point(130, 30),
                Size = new Size(120, 20),
                BackColor = Color.Transparent
            };

            logoSection.Controls.Add(logoLabel);
            logoSection.Controls.Add(logoSubLabel);

            // Navigation menu
            string[] menuItems = { "📊 Dashboard", "📋 My Jobs", "⚡ New Request", "🚛 Shipments", "📦 Track Orders", "💰 Billing", "📞 Support", "⚙️ Settings" };
            string[] menuActions = { "Dashboard", "MyJobs", "NewRequest", "Shipments", "TrackOrders", "Billing", "Support", "Settings" };

            for (int i = 0; i < menuItems.Length; i++)
            {
                Button menuBtn = new Button
                {
                    Text = menuItems[i],
                    Size = new Size(260, 50),
                    Location = new Point(10, 90 + (i * 55)),
                    BackColor = i == 0 ? Color.FromArgb(59, 130, 246) : Color.Transparent,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(20, 0, 0, 0),
                    Cursor = Cursors.Hand,
                    Tag = menuActions[i]
                };

                menuBtn.FlatAppearance.BorderSize = 0;
                menuBtn.FlatAppearance.MouseOverBackColor = Color.FromArgb(59, 130, 246);
                menuBtn.Click += MenuButton_Click;
                menuBtn.Paint += MenuButton_Paint;

                this.sidebarPanel.Controls.Add(menuBtn);
            }

            this.sidebarPanel.Controls.Add(logoSection);

            // User info section at bottom
            Panel userSection = new Panel
            {
                Size = new Size(260, 80),
                Location = new Point(10, 800),
                BackColor = Color.FromArgb(15, 23, 42),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };

            Label userLabel = new Label
            {
                Text = $"👤 {customerName}",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(15, 15),
                Size = new Size(200, 20),
                BackColor = Color.Transparent
            };

            Button logoutBtn = new Button
            {
                Text = "🚪 Logout",
                Size = new Size(230, 35),
                Location = new Point(15, 40),
                BackColor = Color.FromArgb(220, 38, 127),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            logoutBtn.FlatAppearance.BorderSize = 0;
            logoutBtn.Click += LogoutBtn_Click;

            userSection.Controls.Add(userLabel);
            userSection.Controls.Add(logoutBtn);
            this.sidebarPanel.Controls.Add(userSection);
        }

        private void CreateHeaderButtons()
        {
            // Refresh Button
            Button btnRefresh = new Button
            {
                Text = "🔄",
                Location = new Point(1150, 30),
                Size = new Size(40, 40),
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14F, FontStyle.Regular),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => RefreshDashboardStats();

            // Notifications Button
            Button btnNotifications = new Button
            {
                Text = "🔔",
                Location = new Point(1200, 30),
                Size = new Size(40, 40),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14F, FontStyle.Regular),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnNotifications.FlatAppearance.BorderSize = 0;
            btnNotifications.Click += (s, e) => ShowNotifications();

            // Service status
            Label lblServiceStatus = new Label
            {
                Text = "🟢 Online",
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = Color.FromArgb(34, 197, 94),
                Location = new Point(1000, 50),
                Size = new Size(100, 20),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            this.headerPanel.Controls.Add(btnRefresh);
            this.headerPanel.Controls.Add(btnNotifications);
            this.headerPanel.Controls.Add(lblServiceStatus);
        }

        private void CreateModernStatisticsCards()
        {
            string[] titles = { "Total Jobs", "Active Jobs", "Completed Jobs", "Pending Payments", "This Month" };
            string[] icons = { "📋", "⚡", "✅", "💰", "📊" };
            Color[] colors = {
                Color.FromArgb(59, 130, 246),   // Blue
                Color.FromArgb(245, 158, 11),   // Amber
                Color.FromArgb(34, 197, 94),    // Green
                Color.FromArgb(239, 68, 68),    // Red
                Color.FromArgb(139, 92, 246)    // Purple
            };

            for (int i = 0; i < titles.Length; i++)
            {
                Panel card = new Panel
                {
                    Size = new Size(240, 140),
                    Location = new Point(i * 255, 20),
                    BackColor = Color.FromArgb(30, 41, 59),
                    Tag = i
                };
                card.Paint += (s, e) => DrawStatCard(s, e, colors[(int)((Panel)s).Tag]);

                // Icon
                Label statIconLabel = new Label
                {
                    Text = icons[i],
                    Font = new Font("Segoe UI Emoji", 24F),
                    ForeColor = colors[i],
                    Location = new Point(20, 20),
                    Size = new Size(40, 40),
                    BackColor = Color.Transparent
                };

                // Title
                Label statTitleLabel = new Label
                {
                    Text = titles[i],
                    Font = new Font("Segoe UI", 11F, FontStyle.Regular),
                    ForeColor = Color.FromArgb(148, 163, 184),
                    Location = new Point(20, 70),
                    Size = new Size(200, 20),
                    BackColor = Color.Transparent
                };

                // Value
                Label statValueLabel = new Label
                {
                    Name = "lblStatValue",
                    Text = "Loading...",
                    Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(20, 95),
                    Size = new Size(150, 30),
                    BackColor = Color.Transparent
                };

                card.Controls.Add(statIconLabel);
                card.Controls.Add(statTitleLabel);
                card.Controls.Add(statValueLabel);
                statsPanel.Controls.Add(card);
            }
        }

        private void CreateCustomerGrid()
        {
            // Title - using separate scope to avoid variable name conflicts
            {
                Label custTitleLabel = new Label
                {
                    Text = "🎯 Customer Services",
                    Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(20, 20),
                    Size = new Size(300, 30),
                    BackColor = Color.Transparent
                };
                this.quickActionsPanel.Controls.Add(custTitleLabel);
            }

            // Customer service cards
            var customerItems = new[]
            {
                new { Title = "My Jobs", Icon = "📋", Description = "View and track all your transport requests", Color = Color.FromArgb(59, 130, 246), Action = "MyJobs" },
                new { Title = "New Request", Icon = "⚡", Description = "Create a new transport job request", Color = Color.FromArgb(16, 185, 129), Action = "NewRequest" },
                new { Title = "Track Shipments", Icon = "🚛", Description = "Real-time tracking of your shipments", Color = Color.FromArgb(245, 158, 11), Action = "Shipments" },
                new { Title = "Order Status", Icon = "📦", Description = "Check status of current orders", Color = Color.FromArgb(139, 92, 246), Action = "TrackOrders" },
                new { Title = "Billing & Payments", Icon = "💰", Description = "View invoices and payment history", Color = Color.FromArgb(239, 68, 68), Action = "Billing" },
                new { Title = "Customer Support", Icon = "📞", Description = "Get help and contact support team", Color = Color.FromArgb(107, 114, 128), Action = "Support" }
            };

            for (int i = 0; i < customerItems.Length; i++)
            {
                int row = i / 3;
                int col = i % 3;
                var item = customerItems[i];

                Panel card = new Panel
                {
                    Size = new Size(400, 180),
                    Location = new Point(20 + col * 420, 70 + row * 200),
                    BackColor = Color.FromArgb(30, 41, 59),
                    Cursor = Cursors.Hand,
                    Tag = item.Action
                };
                card.Click += CustomerCard_Click;
                card.Paint += (s, e) => DrawCustomerCard(s, e, item.Color);

                // Icon background
                Panel iconBg = new Panel
                {
                    Size = new Size(60, 60),
                    Location = new Point(20, 20),
                    BackColor = Color.FromArgb(50, item.Color.R, item.Color.G, item.Color.B)
                };
                iconBg.Paint += (s, e) => DrawIconBackground(s, e);

                Label iconLabel = new Label
                {
                    Text = item.Icon,
                    Font = new Font("Segoe UI Emoji", 20F),
                    ForeColor = item.Color,
                    Location = new Point(15, 15),
                    Size = new Size(30, 30),
                    BackColor = Color.Transparent
                };
                iconBg.Controls.Add(iconLabel);

                // Title
                Label cardTitleLabel = new Label
                {
                    Text = item.Title,
                    Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(100, 25),
                    Size = new Size(280, 25),
                    BackColor = Color.Transparent
                };

                // Description
                Label descLabel = new Label
                {
                    Text = item.Description,
                    Font = new Font("Segoe UI", 11F, FontStyle.Regular),
                    ForeColor = Color.FromArgb(148, 163, 184),
                    Location = new Point(100, 55),
                    Size = new Size(280, 40),
                    BackColor = Color.Transparent
                };

                // Action button
                Button actionBtn = new Button
                {
                    Text = "Open →",
                    Size = new Size(120, 35),
                    Location = new Point(20, 120),
                    BackColor = item.Color,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Tag = item.Action
                };
                actionBtn.FlatAppearance.BorderSize = 0;
                actionBtn.Click += CustomerButton_Click;

                // Quick stats
                Label statsLabel = new Label
                {
                    Text = "Loading...",
                    Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                    ForeColor = Color.FromArgb(148, 163, 184),
                    Location = new Point(160, 130),
                    Size = new Size(200, 20),
                    BackColor = Color.Transparent,
                    Name = $"stats_{item.Action}"
                };

                card.Controls.Add(iconBg);
                card.Controls.Add(cardTitleLabel);
                card.Controls.Add(descLabel);
                card.Controls.Add(actionBtn);
                card.Controls.Add(statsLabel);

                this.quickActionsPanel.Controls.Add(card);
            }
        }

        // Timer setup
        private void SetupTimer()
        {
            this.timeTimer = new Timer();
            this.timeTimer.Interval = 1000;
            this.timeTimer.Tick += TimeTimer_Tick;
            this.timeTimer.Start();
        }

        private void TimeTimer_Tick(object sender, EventArgs e)
        {
            this.lblDateTime.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy • HH:mm:ss");
        }

        // Load dashboard stats from database
        private void LoadDashboardStats()
        {
            try
            {
                int totalJobs = GetCustomerJobCount("");
                int activeJobs = GetCustomerJobCount("Pending");
                int completedJobs = GetCustomerJobCount("Completed");
                int pendingPayments = GetPendingPaymentsCount();
                int monthlyJobs = GetMonthlyJobCount();

                UpdateStatValue(0, totalJobs);
                UpdateStatValue(1, activeJobs);
                UpdateStatValue(2, completedJobs);
                UpdateStatValue(3, pendingPayments);
                UpdateStatValue(4, monthlyJobs);

                // Update service card stats
                UpdateServiceStats("MyJobs", totalJobs);
                UpdateServiceStats("NewRequest", 0);
                UpdateServiceStats("Shipments", activeJobs);
                UpdateServiceStats("TrackOrders", activeJobs);
                UpdateServiceStats("Billing", pendingPayments);
                UpdateServiceStats("Support", 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading dashboard statistics: {ex.Message}");

                // Set fallback values
                for (int i = 0; i < 5; i++)
                {
                    UpdateStatValue(i, 0);
                }
            }
        }

        private int GetCustomerJobCount(string status)
        {
            try
            {
                string query = string.IsNullOrEmpty(status) 
                    ? "SELECT COUNT(*) FROM Job WHERE CustomerID = @customerId"
                    : "SELECT COUNT(*) FROM Job WHERE CustomerID = @customerId AND Status = @status";
                
                var parameters = string.IsNullOrEmpty(status)
                    ? new MySqlParameter[] { new MySqlParameter("@customerId", customerId) }
                    : new MySqlParameter[] { 
                        new MySqlParameter("@customerId", customerId),
                        new MySqlParameter("@status", status) 
                    };
                
                var result = dbConnection.ExecuteScalar(query, parameters);
                return result != null ? Convert.ToInt32(result) : 0;
            }
            catch
            {
                return 0;
            }
        }

        private int GetPendingPaymentsCount()
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM Job 
                               WHERE CustomerID = @customerId 
                               AND Status = 'Completed'";
                
                MySqlParameter[] parameters = { new MySqlParameter("@customerId", customerId) };
                var result = dbConnection.ExecuteScalar(query, parameters);
                return result != null ? Convert.ToInt32(result) : 0;
            }
            catch
            {
                return 0;
            }
        }

        private int GetMonthlyJobCount()
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM Job 
                               WHERE CustomerID = @customerId 
                               AND MONTH(RequestDate) = MONTH(CURDATE()) 
                               AND YEAR(RequestDate) = YEAR(CURDATE())";
                
                MySqlParameter[] parameters = { new MySqlParameter("@customerId", customerId) };
                var result = dbConnection.ExecuteScalar(query, parameters);
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
                    valueLabel.Text = value >= 1000 ? $"{value / 1000.0:0.0}K" : value.ToString();
                }
            }
        }

        private void UpdateServiceStats(string section, int count)
        {
            var statsLabel = this.quickActionsPanel.Controls
                .OfType<Panel>()
                .SelectMany(p => p.Controls.OfType<Label>())
                .FirstOrDefault(l => l.Name == $"stats_{section}");

            if (statsLabel != null)
            {
                switch (section)
                {
                    case "MyJobs":
                        statsLabel.Text = $"{count} total jobs";
                        break;
                    case "NewRequest":
                        statsLabel.Text = "Ready to submit";
                        break;
                    case "Shipments":
                        statsLabel.Text = $"{count} active shipments";
                        break;
                    case "TrackOrders":
                        statsLabel.Text = $"{count} in progress";
                        break;
                    case "Billing":
                        statsLabel.Text = $"{count} pending payments";
                        break;
                    case "Support":
                        statsLabel.Text = "24/7 available";
                        break;
                }
            }
        }

        private void RefreshDashboardStats()
        {
            LoadDashboardStats();
            lblDateTime.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy • HH:mm:ss") + " (Refreshed)";

            Timer resetTimer = new Timer();
            resetTimer.Interval = 2000;
            resetTimer.Tick += (s, e) =>
            {
                lblDateTime.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy • HH:mm:ss");
                resetTimer.Stop();
                resetTimer.Dispose();
            };
            resetTimer.Start();
        }

        // Event Handlers
        private void MenuButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                string action = btn.Tag.ToString();
                HandleMenuAction(action);

                // Update visual state
                foreach (Button menuBtn in sidebarPanel.Controls.OfType<Button>())
                {
                    menuBtn.BackColor = menuBtn == btn ? Color.FromArgb(59, 130, 246) : Color.Transparent;
                }
            }
        }

        private void CustomerCard_Click(object sender, EventArgs e)
        {
            if (sender is Panel panel)
            {
                string action = panel.Tag.ToString();
                HandleMenuAction(action);
            }
        }

        private void CustomerButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                string action = btn.Tag.ToString();
                HandleMenuAction(action);
            }
        }

        private void HandleMenuAction(string action)
        {
            try
            {
                switch (action)
                {
                    case "MyJobs":
                        try
                        {
                            var jobsForm = new CustomerJobsForm(customerId);
                            jobsForm.ShowDialog();
                        }
                        catch
                        {
                            MessageBox.Show("📋 My Jobs\n\n" +
                                "View and manage your transport requests:\n" +
                                "• Track job status and progress\n" +
                                "• View job details and timeline\n" +
                                "• Download job documents\n" +
                                "• Contact support for updates\n\n" +
                                "Jobs module is ready for integration!",
                                "My Jobs", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                    case "NewRequest":
                        try
                        {
                            var requestForm = new JobRequestForm(customerId);
                            requestForm.ShowDialog();
                        }
                        catch
                        {
                            MessageBox.Show("⚡ New Transport Request\n\n" +
                                "Create a new transport job:\n" +
                                "• Specify pickup and delivery locations\n" +
                                "• Select transport type and urgency\n" +
                                "• Add special instructions\n" +
                                "• Get instant quote estimation\n\n" +
                                "Request module is ready for integration!",
                                "New Request", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                    case "Shipments":
                        ShowShipmentTracking();
                        break;
                    case "TrackOrders":
                        ShowOrderTracking();
                        break;
                    case "Billing":
                        ShowBillingInfo();
                        break;
                    case "Support":
                        ShowSupportOptions();
                        break;
                    case "Settings":
                        ShowCustomerSettings();
                        break;
                    case "Dashboard":
                        RefreshDashboardStats();
                        break;
                }

                // Refresh stats after any action
                if (action != "Dashboard" && action != "Settings" && action != "Support")
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

        private void ShowShipmentTracking()
        {
            MessageBox.Show("🚛 Shipment Tracking\n\n" +
                "Real-time tracking features:\n" +
                "• Live GPS tracking of your shipments\n" +
                "• Estimated delivery times\n" +
                "• Route optimization updates\n" +
                "• Driver contact information\n" +
                "• Delivery confirmation photos\n\n" +
                "Tracking system is being integrated!",
                "Shipment Tracking", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowOrderTracking()
        {
            MessageBox.Show("📦 Order Status Tracking\n\n" +
                "Track your orders through every stage:\n" +
                "• Order confirmed and processing\n" +
                "• Pickup scheduled and completed\n" +
                "• In transit with live updates\n" +
                "• Out for delivery notifications\n" +
                "• Delivery confirmation\n\n" +
                "Order tracking system ready!",
                "Order Tracking", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowBillingInfo()
        {
            MessageBox.Show("💰 Billing & Payments\n\n" +
                "Manage your account finances:\n" +
                "• View current invoices and statements\n" +
                "• Payment history and receipts\n" +
                "• Multiple payment methods\n" +
                "• Automatic billing notifications\n" +
                "• Download tax documents\n\n" +
                "Billing system is being prepared!",
                "Billing & Payments", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowSupportOptions()
        {
            var supportMenu = new ContextMenuStrip();
            supportMenu.Items.Add("📞 Call Support", null, (s, e) => ShowCallSupport());
            supportMenu.Items.Add("💬 Live Chat", null, (s, e) => ShowLiveChat());
            supportMenu.Items.Add("📧 Email Support", null, (s, e) => ShowEmailSupport());
            supportMenu.Items.Add("❓ FAQ", null, (s, e) => ShowFAQ());
            supportMenu.Items.Add("📋 Submit Ticket", null, (s, e) => ShowTicketForm());

            supportMenu.Show(Cursor.Position);
        }

        private void ShowCallSupport()
        {
            MessageBox.Show("📞 Call Support\n\n" +
                "24/7 Customer Support Hotline:\n" +
                "📞 1-800-TRANSPORT (1-800-872-6776)\n\n" +
                "Business Hours: Mon-Fri 8AM-8PM\n" +
                "Emergency Support: 24/7 availability\n" +
                "Average wait time: < 2 minutes\n\n" +
                "Have your Customer ID ready: " + customerId,
                "Call Support", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowLiveChat()
        {
            MessageBox.Show("💬 Live Chat Support\n\n" +
                "Chat with our support team:\n" +
                "• Instant responses during business hours\n" +
                "• Screen sharing for technical issues\n" +
                "• File sharing for documents\n" +
                "• Chat history saved automatically\n\n" +
                "Live chat widget will open here!",
                "Live Chat", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowEmailSupport()
        {
            MessageBox.Show("📧 Email Support\n\n" +
                "Send us an email:\n" +
                "📧 support@transportms.com\n" +
                "📧 billing@transportms.com\n" +
                "📧 emergency@transportms.com\n\n" +
                "Response time: Within 4 hours\n" +
                "Include your Customer ID: " + customerId,
                "Email Support", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowFAQ()
        {
            MessageBox.Show("❓ Frequently Asked Questions\n\n" +
                "Common topics:\n" +
                "• How to track my shipment?\n" +
                "• What are the shipping rates?\n" +
                "• How to modify my order?\n" +
                "• Payment methods accepted\n" +
                "• Delivery time estimates\n\n" +
                "FAQ section will open here!",
                "FAQ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowTicketForm()
        {
            MessageBox.Show("📋 Submit Support Ticket\n\n" +
                "Create a support ticket:\n" +
                "• Detailed issue description\n" +
                "• Priority level selection\n" +
                "• File attachments supported\n" +
                "• Automatic ticket tracking\n" +
                "• Email notifications\n\n" +
                "Ticket form will open here!",
                "Submit Ticket", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowCustomerSettings()
        {
            var settingsMenu = new ContextMenuStrip();
            settingsMenu.Items.Add("👤 Profile Settings", null, (s, e) => ShowProfileSettings());
            settingsMenu.Items.Add("🔔 Notifications", null, (s, e) => ShowNotificationSettings());
            settingsMenu.Items.Add("🔒 Security", null, (s, e) => ShowSecuritySettings());
            settingsMenu.Items.Add("📍 Addresses", null, (s, e) => ShowAddressBook());

            settingsMenu.Show(Cursor.Position);
        }

        private void ShowProfileSettings()
        {
            MessageBox.Show("👤 Profile Settings\n\n" +
                "Manage your account information:\n" +
                "• Update contact details\n" +
                "• Change email preferences\n" +
                "• Modify company information\n" +
                "• Set default shipping preferences\n\n" +
                "Profile management coming soon!",
                "Profile Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowNotificationSettings()
        {
            MessageBox.Show("🔔 Notification Preferences\n\n" +
                "Customize your notifications:\n" +
                "• Email notifications for job updates\n" +
                "• SMS alerts for urgent matters\n" +
                "• Push notifications for mobile app\n" +
                "• Weekly summary reports\n\n" +
                "Notification settings ready!",
                "Notification Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowSecuritySettings()
        {
            MessageBox.Show("🔒 Security Settings\n\n" +
                "Protect your account:\n" +
                "• Change password\n" +
                "• Enable two-factor authentication\n" +
                "• View login history\n" +
                "• Manage API access tokens\n\n" +
                "Security panel coming soon!",
                "Security Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowAddressBook()
        {
            MessageBox.Show("📍 Address Book\n\n" +
                "Manage your addresses:\n" +
                "• Frequently used pickup locations\n" +
                "• Default delivery addresses\n" +
                "• Contact information for each location\n" +
                "• Quick selection for new requests\n\n" +
                "Address management ready!",
                "Address Book", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowNotifications()
        {
            try
            {
                int pendingJobs = GetCustomerJobCount("Pending");
                int completedJobs = GetCustomerJobCount("Completed");

                string notifications = "🔔 Your Notifications\n\n";
                notifications += $"• {pendingJobs} jobs in progress\n";
                notifications += $"• {completedJobs} jobs completed this month\n";
                notifications += $"• Account status: Active\n";
                notifications += $"• Last update: {DateTime.Now:HH:mm:ss}\n\n";
                notifications += "All your shipments are on track! ✅";

                MessageBox.Show(notifications, "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("🔔 Your Notifications\n\n" +
                    "• Welcome to Transport Management System\n" +
                    "• No urgent notifications\n" +
                    "• All services are operational\n\n" +
                    "Have a great day! 😊",
                    "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LogoutBtn_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("🔐 Secure Logout\n\nAre you sure you want to end your session?",
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

        // Paint Events
        private void HeaderPanel_Paint(object sender, PaintEventArgs e)
        {
            using (var brush = new LinearGradientBrush(
                headerPanel.ClientRectangle,
                Color.FromArgb(30, 41, 59),
                Color.FromArgb(15, 23, 42),
                LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, headerPanel.ClientRectangle);
            }
        }

        private void SidebarPanel_Paint(object sender, PaintEventArgs e)
        {
            using (var brush = new LinearGradientBrush(
                sidebarPanel.ClientRectangle,
                Color.FromArgb(20, 33, 61),
                Color.FromArgb(15, 23, 42),
                LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, sidebarPanel.ClientRectangle);
            }
        }

        private void LogoBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (var brush = new LinearGradientBrush(
                logoBox.ClientRectangle,
                Color.FromArgb(59, 130, 246),
                Color.FromArgb(139, 92, 246),
                LinearGradientMode.Vertical))
            {
                e.Graphics.FillEllipse(brush, 10, 10, 40, 40);
            }

            using (var font = new Font("Segoe UI", 16F, FontStyle.Bold))
            using (var textBrush = new SolidBrush(Color.White))
            {
                e.Graphics.DrawString("T", font, textBrush, 23, 20);
            }
        }

        private void MenuButton_Paint(object sender, PaintEventArgs e)
        {
            var btn = sender as Button;
            if (btn.BackColor != Color.Transparent)
            {
                using (var brush = new LinearGradientBrush(
                    btn.ClientRectangle,
                    Color.FromArgb(59, 130, 246),
                    Color.FromArgb(37, 99, 235),
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, btn.ClientRectangle);
                }
            }
        }

        private void DrawStatCard(object sender, PaintEventArgs e, Color accentColor)
        {
            var panel = sender as Panel;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Background
            using (var brush = new LinearGradientBrush(
                panel.ClientRectangle,
                Color.FromArgb(30, 41, 59),
                Color.FromArgb(20, 31, 49),
                LinearGradientMode.Vertical))
            {
                e.Graphics.FillRoundedRectangle(brush, 0, 0, panel.Width, panel.Height, 12);
            }

            // Accent border
            using (var pen = new Pen(accentColor, 2))
            {
                e.Graphics.DrawRoundedRectangle(pen, 1, 1, panel.Width - 2, panel.Height - 2, 12);
            }
        }

        private void DrawCustomerCard(object sender, PaintEventArgs e, Color accentColor)
        {
            var panel = sender as Panel;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (var brush = new LinearGradientBrush(
                panel.ClientRectangle,
                Color.FromArgb(30, 41, 59),
                Color.FromArgb(20, 31, 49),
                LinearGradientMode.Vertical))
            {
                e.Graphics.FillRoundedRectangle(brush, 0, 0, panel.Width, panel.Height, 15);
            }

            using (var pen = new Pen(Color.FromArgb(50, 255, 255, 255), 1))
            {
                e.Graphics.DrawRoundedRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1, 15);
            }
        }

        private void DrawIconBackground(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.FillEllipse(new SolidBrush(panel.BackColor), panel.ClientRectangle);
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

    // Extension methods for rounded rectangles (shared with AdminDashboard)
    public static class CustomerGraphicsExtensions
    {
        public static void FillRoundedRectangle(this Graphics graphics, Brush brush, float x, float y, float width, float height, float radius)
        {
            using (var path = new GraphicsPath())
            {
                path.AddArc(x, y, radius, radius, 180, 90);
                path.AddArc(x + width - radius, y, radius, radius, 270, 90);
                path.AddArc(x + width - radius, y + height - radius, radius, radius, 0, 90);
                path.AddArc(x, y + height - radius, radius, radius, 90, 90);
                path.CloseFigure();
                graphics.FillPath(brush, path);
            }
        }

        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, float x, float y, float width, float height, float radius)
        {
            using (var path = new GraphicsPath())
            {
                path.AddArc(x, y, radius, radius, 180, 90);
                path.AddArc(x + width - radius, y, radius, radius, 270, 90);
                path.AddArc(x + width - radius, y + height - radius, radius, radius, 0, 90);
                path.AddArc(x, y + height - radius, radius, radius, 90, 90);
                path.CloseFigure();
                graphics.DrawPath(pen, path);
            }
        }
    }
}
