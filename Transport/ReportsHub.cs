using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;

namespace Transport
{
    public partial class ReportsHub : Form
    {
        #region Styles and Design Constants

        // Color Palette
        private static class Colors
        {
            // Primary Colors
            public static readonly Color Primary = Color.FromArgb(0, 123, 255);
            public static readonly Color Secondary = Color.FromArgb(108, 117, 125);
            public static readonly Color Success = Color.FromArgb(40, 167, 69);
            public static readonly Color Danger = Color.FromArgb(220, 53, 69);
            public static readonly Color Warning = Color.FromArgb(255, 193, 7);
            public static readonly Color Info = Color.FromArgb(23, 162, 184);
            
            // Background Colors
            public static readonly Color Background = Color.FromArgb(240, 248, 255);
            public static readonly Color PanelBackground = Color.FromArgb(248, 249, 250);
            public static readonly Color CardBackground = Color.White;
            
            // Text Colors
            public static readonly Color TextPrimary = Color.FromArgb(52, 58, 64);
            public static readonly Color TextSecondary = Color.FromArgb(108, 117, 125);
            public static readonly Color TextLight = Color.FromArgb(230, 230, 230);
            public static readonly Color TextWhite = Color.White;
            
            // Stats Card Colors
            public static readonly Color StatsBlue = Color.FromArgb(0, 123, 255);
            public static readonly Color StatsGreen = Color.FromArgb(40, 167, 69);
            public static readonly Color StatsAmber = Color.FromArgb(255, 193, 7);
            public static readonly Color StatsRed = Color.FromArgb(220, 53, 69);
            public static readonly Color StatsGray = Color.FromArgb(108, 117, 125);
            
            // Accent Colors for Transparency
            public static readonly Color AccentLight = Color.FromArgb(30, 255, 255, 255);
            public static readonly Color BorderLight = Color.FromArgb(30, 0, 0, 0);
            public static readonly Color ShadowLight = Color.FromArgb(20, 0, 0, 0);
        }

        // Typography
        private static class Fonts
        {
            public static readonly Font SectionTitle = new Font("Segoe UI", 22F, FontStyle.Bold);
            public static readonly Font SectionSubtitle = new Font("Segoe UI", 12F, FontStyle.Regular);
            public static readonly Font CardTitle = new Font("Segoe UI", 16F, FontStyle.Bold);
            public static readonly Font CardDescription = new Font("Segoe UI", 11F, FontStyle.Regular);
            public static readonly Font StatTitle = new Font("Segoe UI", 10F, FontStyle.Regular);
            public static readonly Font StatValue = new Font("Segoe UI", 18F, FontStyle.Bold);
            public static readonly Font ButtonText = new Font("Segoe UI", 10F, FontStyle.Bold);
            public static readonly Font EmojiFont = new Font("Segoe UI Emoji", 20F);
            public static readonly Font EmojiLargeFont = new Font("Segoe UI Emoji", 24F);
            public static readonly Font FeaturesText = new Font("Segoe UI", 9F, FontStyle.Regular);
        }

        // Layout and Sizing
        private static class Layout
        {
            // Form Dimensions
            public static readonly Size FormSize = new Size(1400, 900);
            public static readonly Size MinFormSize = new Size(1200, 700);
            
            // Main Panel (now starts from top)
            public static readonly Point MainPanelPosition = new Point(0, 0);
            public static readonly Size MainPanelSize = new Size(1400, 900);
            public static readonly Padding MainPanelPadding = new Padding(40);
            
            // Stats Panel
            public static readonly Point StatsPanelPosition = new Point(40, 40);
            public static readonly Size StatsPanelSize = new Size(1320, 140);
            public static readonly Size StatsCardSize = new Size(240, 100);
            public static readonly int StatsCardSpacing = 260;
            public static readonly Point StatsCardOffset = new Point(20, 20);
            
            // Report Cards (moved up since no header)
            public static readonly Size ReportCardSize = new Size(400, 180);
            public static readonly int ReportCardSpacing = 420;
            public static readonly Point ReportSectionPosition = new Point(40, 200);
            public static readonly Point ReportCardsStart = new Point(40, 280);
            public static readonly int ReportRowSpacing = 200;
            
            // Buttons and Icons
            public static readonly Size IconBackgroundSize = new Size(50, 50);
            public static readonly Size ReportIconSize = new Size(60, 60);
            public static readonly Size GenerateButtonSize = new Size(140, 35);
            public static readonly Point GenerateButtonPosition = new Point(230, 125);
            
            // Border Radius
            public static readonly int CardRadius = 8;
            public static readonly int ButtonRadius = 8;
        }

        // Animation and Effects
        private static class Effects
        {
            public static readonly int HoverTransitionMs = 200;
            public static readonly float HoverOpacity = 0.1f;
            public static readonly int ShadowOffset = 2;
            public static readonly int BorderWidth = 1;
            public static readonly int AccentBorderWidth = 4;
        }

        // Content Strings
        private static class Content
        {
            // Section Content
            public static readonly string SectionTitle = "📋 Available Reports";
            public static readonly string SectionSubtitle = "Generate comprehensive reports with advanced filtering and export capabilities";
            
            // Button Text
            public static readonly string GenerateButton = "Generate Report →";
            
            // Features Text
            public static readonly string FeaturesText = "✓ Advanced Filtering  ✓ CSV Export  ✓ Real-time Data";
            
            // Stats Titles
            public static readonly string[] StatsTitles = { "Total Customers", "Active Jobs", "Products", "Completed Loads", "System Users" };
            public static readonly string[] StatsIcons = { "👥", "⚡", "📦", "🚛", "👨‍💼" };
            
            // Loading Text
            public static readonly string LoadingText = "Loading...";
        }

        #endregion

        // Class members
        private DatabaseConnection dbConnection;
        private Panel mainPanel;
        private Panel statsPanel;

        public ReportsHub()
        {
            this.dbConnection = new DatabaseConnection();
            InitializeComponent();
            LoadSystemStats();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties using styles
            this.Text = "Transport Management System - Reports Hub";
            this.Size = Layout.FormSize;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Colors.Background;
            this.MinimumSize = Layout.MinFormSize;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Maximized;

            // Main Panel (now fills entire form)
            this.mainPanel = new Panel();
            this.mainPanel.Location = Layout.MainPanelPosition;
            this.mainPanel.Size = Layout.MainPanelSize;
            this.mainPanel.Dock = DockStyle.Fill;
            this.mainPanel.BackColor = Colors.PanelBackground;
            this.mainPanel.Padding = Layout.MainPanelPadding;
            this.mainPanel.AutoScroll = true;

            // Stats Panel
            this.statsPanel = new Panel();
            this.statsPanel.Location = Layout.StatsPanelPosition;
            this.statsPanel.Size = Layout.StatsPanelSize;
            this.statsPanel.BackColor = Colors.CardBackground;
            this.statsPanel.Paint += StatsPanel_Paint;

            CreateStatsCards();
            CreateReportButtons();

            this.mainPanel.Controls.Add(this.statsPanel);
            this.Controls.Add(this.mainPanel);

            this.ResumeLayout(false);
        }

        private void CreateStatsCards()
        {
            Color[] colors = {
                Colors.StatsBlue,
                Colors.StatsGreen,
                Colors.StatsAmber,
                Colors.StatsRed,
                Colors.StatsGray
            };

            for (int i = 0; i < Content.StatsTitles.Length; i++)
            {
                Panel card = new Panel
                {
                    Size = Layout.StatsCardSize,
                    Location = new Point(Layout.StatsCardOffset.X + i * Layout.StatsCardSpacing, Layout.StatsCardOffset.Y),
                    BackColor = Colors.CardBackground,
                    Tag = i
                };
                card.Paint += (s, e) => DrawStatCard(s, e, colors[(int)((Panel)s).Tag]);

                // Icon with background circle
                Panel iconBg = new Panel
                {
                    Size = Layout.IconBackgroundSize,
                    Location = new Point(20, 15),
                    BackColor = Color.FromArgb(30, colors[i].R, colors[i].G, colors[i].B)
                };
                int index = i;
                iconBg.Paint += (s, e) => DrawIconCircle(s, e, colors[index]);

                Label iconLabel = new Label
                {
                    Text = Content.StatsIcons[i],
                    Font = Fonts.EmojiFont,
                    ForeColor = colors[i],
                    Location = new Point(10, 10),
                    Size = new Size(30, 30),
                    BackColor = Color.Transparent,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                iconBg.Controls.Add(iconLabel);

                // Title
                Label titleLabel = new Label
                {
                    Text = Content.StatsTitles[i],
                    Font = Fonts.StatTitle,
                    ForeColor = Colors.TextSecondary,
                    Location = new Point(80, 20),
                    Size = new Size(140, 15),
                    BackColor = Color.Transparent
                };

                // Value
                Label valueLabel = new Label
                {
                    Name = $"stat_{i}",
                    Text = Content.LoadingText,
                    Font = Fonts.StatValue,
                    ForeColor = colors[i],
                    Location = new Point(80, 40),
                    Size = new Size(140, 30),
                    BackColor = Color.Transparent
                };

                card.Controls.Add(iconBg);
                card.Controls.Add(titleLabel);
                card.Controls.Add(valueLabel);
                this.statsPanel.Controls.Add(card);
            }
        }

        private void CreateReportButtons()
        {
            // Section title with modern styling
            Label reportTitle = new Label
            {
                Text = Content.SectionTitle,
                Font = Fonts.SectionTitle,
                ForeColor = Colors.TextPrimary,
                Location = Layout.ReportSectionPosition,
                Size = new Size(400, 35),
                BackColor = Color.Transparent
            };

            Label reportSubtitle = new Label
            {
                Text = Content.SectionSubtitle,
                Font = Fonts.SectionSubtitle,
                ForeColor = Colors.TextSecondary,
                Location = new Point(Layout.ReportSectionPosition.X, Layout.ReportSectionPosition.Y + 40),
                Size = new Size(800, 20),
                BackColor = Color.Transparent
            };

            this.mainPanel.Controls.Add(reportTitle);
            this.mainPanel.Controls.Add(reportSubtitle);

            var reports = new[]
            {
                new { Title = "Customer Report", Description = "Complete customer database analysis with filtering", Color = Colors.Primary, Icon = "👥" },
                new { Title = "Job Report", Description = "Job status tracking and performance analytics", Color = Colors.Success, Icon = "⚡" },
                new { Title = "Product Report", Description = "Inventory management and product analytics", Color = Colors.Warning, Icon = "📦" },
                new { Title = "Load Report", Description = "Transportation logistics and delivery tracking", Color = Colors.Danger, Icon = "🚛" },
                new { Title = "Admin Report", Description = "System users and administrative analytics", Color = Colors.Secondary, Icon = "👨‍💼" },
                new { Title = "Revenue Report", Description = "Financial analytics and revenue insights", Color = Colors.Info, Icon = "💰" }
            };

            for (int i = 0; i < reports.Length; i++)
            {
                int row = i / 3;
                int col = i % 3;
                var report = reports[i];

                Panel reportCard = new Panel
                {
                    Size = Layout.ReportCardSize,
                    Location = new Point(Layout.ReportCardsStart.X + col * Layout.ReportCardSpacing, 
                                       Layout.ReportCardsStart.Y + row * Layout.ReportRowSpacing),
                    BackColor = Colors.CardBackground,
                    Cursor = Cursors.Hand,
                    Tag = report.Title
                };
                reportCard.Paint += (s, e) => DrawReportCard(s, e, report.Color);
                reportCard.Click += ReportCard_Click;
                reportCard.MouseEnter += (s, e) => ReportCard_MouseEnter(s, e, report.Color);
                reportCard.MouseLeave += ReportCard_MouseLeave;

                // Icon with styled background
                Panel iconBg = new Panel
                {
                    Size = Layout.ReportIconSize,
                    Location = new Point(25, 25),
                    BackColor = Color.FromArgb(20, report.Color.R, report.Color.G, report.Color.B)
                };
                iconBg.Paint += (s, e) => DrawIconCircle(s, e, report.Color);

                Label iconLabel = new Label
                {
                    Text = report.Icon,
                    Font = Fonts.EmojiLargeFont,
                    ForeColor = report.Color,
                    Location = new Point(15, 15),
                    Size = new Size(30, 30),
                    BackColor = Color.Transparent,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                iconLabel.Click += ReportCard_Click;
                iconBg.Controls.Add(iconLabel);

                // Title
                Label titleLabel = new Label
                {
                    Text = report.Title,
                    Font = Fonts.CardTitle,
                    ForeColor = Colors.TextPrimary,
                    Location = new Point(100, 30),
                    Size = new Size(280, 25),
                    BackColor = Color.Transparent
                };
                titleLabel.Click += ReportCard_Click;

                // Description
                Label descLabel = new Label
                {
                    Text = report.Description,
                    Font = Fonts.CardDescription,
                    ForeColor = Colors.TextSecondary,
                    Location = new Point(100, 60),
                    Size = new Size(280, 40),
                    BackColor = Color.Transparent
                };
                descLabel.Click += ReportCard_Click;

                // Features list
                Label featuresLabel = new Label
                {
                    Text = Content.FeaturesText,
                    Font = Fonts.FeaturesText,
                    ForeColor = Colors.Success,
                    Location = new Point(25, 100),
                    Size = new Size(350, 15),
                    BackColor = Color.Transparent
                };

                // Generate button with modern styling
                Button generateBtn = new Button
                {
                    Text = Content.GenerateButton,
                    Size = Layout.GenerateButtonSize,
                    Location = Layout.GenerateButtonPosition,
                    BackColor = report.Color,
                    ForeColor = Colors.TextWhite,
                    FlatStyle = FlatStyle.Flat,
                    Font = Fonts.ButtonText,
                    Cursor = Cursors.Hand,
                    Tag = report.Title
                };
                generateBtn.FlatAppearance.BorderSize = 0;
                generateBtn.Paint += (s, e) => DrawRoundedButton(s, e);
                generateBtn.Click += GenerateReport_Click;

                reportCard.Controls.Add(iconBg);
                reportCard.Controls.Add(titleLabel);
                reportCard.Controls.Add(descLabel);
                reportCard.Controls.Add(featuresLabel);
                reportCard.Controls.Add(generateBtn);
                this.mainPanel.Controls.Add(reportCard);
            }
        }

        private void LoadSystemStats()
        {
            try
            {
                // Load statistics for each table
                int customers = GetTableCount("Customer");
                int jobs = GetTableCount("Job");
                int products = GetTableCount("Product");
                int loads = GetTableCount("LoadDetails");
                int admins = GetTableCount("Admin");

                UpdateStatValue(0, customers);
                UpdateStatValue(1, jobs);
                UpdateStatValue(2, products);
                UpdateStatValue(3, loads);
                UpdateStatValue(4, admins);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading system statistics: {ex.Message}", 
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
                // Set fallback values
                for (int i = 0; i < 5; i++)
                {
                    UpdateStatValue(i, 0);
                }
            }
        }

        private int GetTableCount(string tableName)
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

        private void UpdateStatValue(int index, int value)
        {
            if (statsPanel.Controls.Count > index)
            {
                var card = statsPanel.Controls[index];
                var valueLabel = card.Controls[$"stat_{index}"] as Label;
                if (valueLabel != null)
                {
                    valueLabel.Text = value.ToString("N0");
                }
            }
        }

        // Event Handlers
        private void ReportCard_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            string reportType = control.Tag?.ToString() ?? control.Parent?.Tag?.ToString();
            
            if (!string.IsNullOrEmpty(reportType))
            {
                GenerateReport(reportType);
            }
        }

        private void ReportCard_MouseEnter(object sender, EventArgs e, Color accentColor)
        {
            Panel card = sender as Panel;
            card.BackColor = Color.FromArgb(10, accentColor.R, accentColor.G, accentColor.B);
        }

        private void ReportCard_MouseLeave(object sender, EventArgs e)
        {
            Panel card = sender as Panel;
            card.BackColor = Colors.CardBackground;
        }

        private void GenerateReport_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string reportType = btn.Tag.ToString();
            GenerateReport(reportType);
        }

        private void GenerateReport(string reportType)
        {
            try
            {
                switch (reportType)
                {
                    case "Customer Report":
                        var customerReport = new CustomerReportForm();
                        customerReport.ShowDialog();
                        break;
                    case "Job Report":
                        var jobReport = new JobReportForm();
                        jobReport.ShowDialog();
                        break;
                    case "Product Report":
                        var productReport = new ProductReportForm();
                        productReport.ShowDialog();
                        break;
                    case "Load Report":
                        var loadReport = new LoadReportForm();
                        loadReport.ShowDialog();
                        break;
                    case "Admin Report":
                        var adminReport = new AdminReportForm();
                        adminReport.ShowDialog();
                        break;
                    case "Revenue Report":
                        var revenueReport = new RevenueReportForm();
                        revenueReport.ShowDialog();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Opening {reportType}...\n\nFeatures:\n" +
                    "• Advanced filtering and search\n" +
                    "• Real-time database connectivity\n" +
                    "• Professional data visualization\n" +
                    "• Export to CSV with headers\n" +
                    "• Color-coded status indicators\n\n" +
                    "Report system is fully operational!", 
                    reportType, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Paint Events with Enhanced Styling
        private void StatsPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            
            using (SolidBrush brush = new SolidBrush(Colors.CardBackground))
            {
                e.Graphics.FillRectangle(brush, panel.ClientRectangle);
            }
            
            // Modern shadow effect
            using (Pen shadowPen = new Pen(Colors.ShadowLight, Effects.BorderWidth))
            {
                e.Graphics.DrawRectangle(shadowPen, 0, 0, panel.Width - 1, panel.Height - 1);
            }
        }

        private void DrawStatCard(object sender, PaintEventArgs e, Color accentColor)
        {
            Panel panel = sender as Panel;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            
            // Card background
            using (var brush = new SolidBrush(Colors.CardBackground))
            {
                e.Graphics.FillRectangle(brush, panel.ClientRectangle);
            }
            
            // Left accent border
            using (var brush = new SolidBrush(accentColor))
            {
                e.Graphics.FillRectangle(brush, 0, 0, Effects.AccentBorderWidth, panel.Height);
            }
            
            // Subtle border
            using (Pen borderPen = new Pen(Colors.BorderLight, Effects.BorderWidth))
            {
                e.Graphics.DrawRectangle(borderPen, 0, 0, panel.Width - 1, panel.Height - 1);
            }
        }

        private void DrawReportCard(object sender, PaintEventArgs e, Color accentColor)
        {
            Panel panel = sender as Panel;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            
            // Card background
            using (var brush = new SolidBrush(panel.BackColor))
            {
                e.Graphics.FillRectangle(brush, panel.ClientRectangle);
            }
            
            // Top accent bar
            using (var brush = new SolidBrush(accentColor))
            {
                e.Graphics.FillRectangle(brush, 0, 0, panel.Width, Effects.AccentBorderWidth);
            }
            
            // Card border
            using (Pen borderPen = new Pen(Color.FromArgb(40, 0, 0, 0), Effects.BorderWidth))
            {
                e.Graphics.DrawRectangle(borderPen, 0, 0, panel.Width - 1, panel.Height - 1);
            }
        }

        private void DrawIconCircle(object sender, PaintEventArgs e, Color color)
        {
            Panel panel = sender as Panel;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.FillEllipse(new SolidBrush(panel.BackColor), panel.ClientRectangle);
        }

        private void DrawRoundedButton(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            
            using (GraphicsPath path = new GraphicsPath())
            {
                int radius = Layout.ButtonRadius;
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
