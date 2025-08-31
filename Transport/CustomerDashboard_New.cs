using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
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
        private Button btnMyJobs;
        private Button btnRequestJob;
        private Button btnLogout;
        private Label lblWelcomeTitle;
        private Label lblDateTime;
        private Timer timeTimer;
        private PictureBox logoBox;

        public CustomerDashboard(int customerId, string customerName)
        {
            this.customerId = customerId;
            this.customerName = customerName;
            InitializeComponent();
            SetupTimer();
        }

        private void InitializeComponent()
        {
            this.menuStrip = new MenuStrip();
            this.myJobsToolStripMenuItem = new ToolStripMenuItem();
            this.requestJobToolStripMenuItem = new ToolStripMenuItem();
            this.logoutToolStripMenuItem = new ToolStripMenuItem();
            this.lblWelcome = new Label();
            this.headerPanel = new Panel();
            this.mainContentPanel = new Panel();
            this.quickActionsPanel = new Panel();
            this.btnMyJobs = new Button();
            this.btnRequestJob = new Button();
            this.btnLogout = new Button();
            this.lblWelcomeTitle = new Label();
            this.lblDateTime = new Label();
            this.logoBox = new PictureBox();
            this.timeTimer = new Timer();
            ((System.ComponentModel.ISupportInitialize)(this.logoBox)).BeginInit();
            this.SuspendLayout();

            // Form Properties
            this.Text = "Transport Management System - Customer Portal";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(240, 248, 255); // Alice Blue background
            this.MinimumSize = new Size(1000, 600);

            // Header Panel with Gradient
            this.headerPanel.Location = new Point(0, 24);
            this.headerPanel.Size = new Size(1200, 120);
            this.headerPanel.Dock = DockStyle.Top;
            this.headerPanel.Paint += new PaintEventHandler(this.headerPanel_Paint);

            // Logo/Icon
            this.logoBox.Location = new Point(30, 20);
            this.logoBox.Size = new Size(80, 80);
            this.logoBox.BackColor = Color.White;
            this.logoBox.BorderStyle = BorderStyle.FixedSingle;
            this.logoBox.Paint += new PaintEventHandler(this.logoBox_Paint);

            // Welcome Title
            this.lblWelcomeTitle.Text = $"Welcome back, {customerName}!";
            this.lblWelcomeTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            this.lblWelcomeTitle.ForeColor = Color.White;
            this.lblWelcomeTitle.Location = new Point(130, 25);
            this.lblWelcomeTitle.Size = new Size(600, 40);
            this.lblWelcomeTitle.BackColor = Color.Transparent;

            // Subtitle
            this.lblWelcome.Text = "Manage your transport requests and track shipments";
            this.lblWelcome.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            this.lblWelcome.ForeColor = Color.FromArgb(230, 230, 230);
            this.lblWelcome.Location = new Point(130, 70);
            this.lblWelcome.Size = new Size(500, 25);
            this.lblWelcome.BackColor = Color.Transparent;

            // Date Time Label
            this.lblDateTime.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            this.lblDateTime.ForeColor = Color.White;
            this.lblDateTime.Location = new Point(850, 30);
            this.lblDateTime.Size = new Size(250, 25);
            this.lblDateTime.TextAlign = ContentAlignment.MiddleRight;
            this.lblDateTime.BackColor = Color.Transparent;

            // Logout Button in Header
            this.btnLogout.Text = "Logout";
            this.btnLogout.Location = new Point(1050, 65);
            this.btnLogout.Size = new Size(100, 35);
            this.btnLogout.BackColor = Color.FromArgb(220, 53, 69); // Bootstrap danger red
            this.btnLogout.ForeColor = Color.White;
            this.btnLogout.FlatStyle = FlatStyle.Flat;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnLogout.Cursor = Cursors.Hand;
            this.btnLogout.Click += new EventHandler(this.btnLogout_Click);

            this.headerPanel.Controls.Add(this.logoBox);
            this.headerPanel.Controls.Add(this.lblWelcomeTitle);
            this.headerPanel.Controls.Add(this.lblWelcome);
            this.headerPanel.Controls.Add(this.lblDateTime);
            this.headerPanel.Controls.Add(this.btnLogout);

            // Main Content Panel
            this.mainContentPanel.Location = new Point(0, 144);
            this.mainContentPanel.Size = new Size(1200, 600);
            this.mainContentPanel.Dock = DockStyle.Fill;
            this.mainContentPanel.BackColor = Color.Transparent;
            this.mainContentPanel.Padding = new Padding(40);

            // Quick Actions Panel
            this.quickActionsPanel.Location = new Point(50, 50);
            this.quickActionsPanel.Size = new Size(1100, 400);
            this.quickActionsPanel.Anchor = AnchorStyles.None;
            this.quickActionsPanel.BackColor = Color.White;
            this.quickActionsPanel.Paint += new PaintEventHandler(this.quickActionsPanel_Paint);

            // Quick Actions Title
            Label lblQuickActions = new Label();
            lblQuickActions.Text = "Quick Actions";
            lblQuickActions.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblQuickActions.ForeColor = Color.FromArgb(52, 58, 64);
            lblQuickActions.Location = new Point(40, 30);
            lblQuickActions.Size = new Size(300, 35);
            this.quickActionsPanel.Controls.Add(lblQuickActions);

            // My Jobs Button
            this.btnMyJobs.Text = "View My Jobs";
            this.btnMyJobs.Location = new Point(150, 120);
            this.btnMyJobs.Size = new Size(350, 120);
            this.btnMyJobs.BackColor = Color.FromArgb(40, 167, 69); // Bootstrap success green
            this.btnMyJobs.ForeColor = Color.White;
            this.btnMyJobs.FlatStyle = FlatStyle.Flat;
            this.btnMyJobs.FlatAppearance.BorderSize = 0;
            this.btnMyJobs.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.btnMyJobs.Cursor = Cursors.Hand;
            this.btnMyJobs.Click += new EventHandler(this.btnMyJobs_Click);
            this.btnMyJobs.Paint += new PaintEventHandler(this.button_Paint);

            // Request Job Button
            this.btnRequestJob.Text = "Request New Job";
            this.btnRequestJob.Location = new Point(550, 120);
            this.btnRequestJob.Size = new Size(350, 120);
            this.btnRequestJob.BackColor = Color.FromArgb(0, 123, 255); // Bootstrap primary blue
            this.btnRequestJob.ForeColor = Color.White;
            this.btnRequestJob.FlatStyle = FlatStyle.Flat;
            this.btnRequestJob.FlatAppearance.BorderSize = 0;
            this.btnRequestJob.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.btnRequestJob.Cursor = Cursors.Hand;
            this.btnRequestJob.Click += new EventHandler(this.btnRequestJob_Click);
            this.btnRequestJob.Paint += new PaintEventHandler(this.button_Paint);

            this.quickActionsPanel.Controls.Add(this.btnMyJobs);
            this.quickActionsPanel.Controls.Add(this.btnRequestJob);

            this.mainContentPanel.Controls.Add(this.quickActionsPanel);

            // Enhanced MenuStrip - Reports functionality removed
            this.menuStrip.Items.AddRange(new ToolStripItem[] {
                this.myJobsToolStripMenuItem,
                this.requestJobToolStripMenuItem,
                this.logoutToolStripMenuItem
            });
            this.menuStrip.Location = new Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new Size(1200, 24);
            this.menuStrip.BackColor = Color.FromArgb(52, 58, 64); // Dark gray
            this.menuStrip.ForeColor = Color.White;

            // Style menu items
            StyleMenuItem(this.myJobsToolStripMenuItem, "My Jobs", "");
            StyleMenuItem(this.requestJobToolStripMenuItem, "Request Job", "");
            StyleMenuItem(this.logoutToolStripMenuItem, "Logout", "");

            this.myJobsToolStripMenuItem.Click += new EventHandler(this.myJobsToolStripMenuItem_Click);
            this.requestJobToolStripMenuItem.Click += new EventHandler(this.requestJobToolStripMenuItem_Click);
            this.logoutToolStripMenuItem.Click += new EventHandler(this.logoutToolStripMenuItem_Click);

            // Add hover effects
            AddHoverEffects();

            // Add controls to form
            this.Controls.Add(this.mainContentPanel);
            this.Controls.Add(this.headerPanel);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;

            ((System.ComponentModel.ISupportInitialize)(this.logoBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private MenuStrip menuStrip;
        private ToolStripMenuItem myJobsToolStripMenuItem;
        private ToolStripMenuItem requestJobToolStripMenuItem;
        private ToolStripMenuItem logoutToolStripMenuItem;
        private Label lblWelcome;

        private void StyleMenuItem(ToolStripMenuItem item, string text, string icon)
        {
            item.Text = $"{icon} {text}";
            item.ForeColor = Color.White;
            item.BackColor = Color.FromArgb(52, 58, 64);
            item.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
        }

        private void AddHoverEffects()
        {
            // My Jobs Button Hover
            this.btnMyJobs.MouseEnter += (s, e) => {
                this.btnMyJobs.BackColor = Color.FromArgb(34, 139, 58);
                // Simple size increase without using Transform extension
                this.btnMyJobs.Size = new Size(365, 126);
            };
            this.btnMyJobs.MouseLeave += (s, e) => {
                this.btnMyJobs.BackColor = Color.FromArgb(40, 167, 69);
                this.btnMyJobs.Size = new Size(350, 120);
            };

            // Request Job Button Hover
            this.btnRequestJob.MouseEnter += (s, e) => {
                this.btnRequestJob.BackColor = Color.FromArgb(0, 105, 217);
                this.btnRequestJob.Size = new Size(365, 126);
            };
            this.btnRequestJob.MouseLeave += (s, e) => {
                this.btnRequestJob.BackColor = Color.FromArgb(0, 123, 255);
                this.btnRequestJob.Size = new Size(350, 120);
            };

            // Logout Button Hover
            this.btnLogout.MouseEnter += (s, e) => {
                this.btnLogout.BackColor = Color.FromArgb(200, 45, 61);
            };
            this.btnLogout.MouseLeave += (s, e) => {
                this.btnLogout.BackColor = Color.FromArgb(220, 53, 69);
            };
        }

        private void SetupTimer()
        {
            this.timeTimer.Interval = 1000; // Update every second
            this.timeTimer.Tick += (s, e) => {
                this.lblDateTime.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy HH:mm:ss");
            };
            this.timeTimer.Start();
            this.lblDateTime.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy HH:mm:ss");
        }

        // Custom Paint Events for Gradient Effects
        private void headerPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            using (LinearGradientBrush brush = new LinearGradientBrush(
                panel.ClientRectangle,
                Color.FromArgb(0, 123, 255), // Primary blue
                Color.FromArgb(108, 117, 125), // Secondary gray
                LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, panel.ClientRectangle);
            }
        }

        private void quickActionsPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            
            using (SolidBrush brush = new SolidBrush(Color.White))
            {
                e.Graphics.FillRectangle(brush, panel.ClientRectangle);
            }
            
            // Add subtle shadow effect
            using (Pen shadowPen = new Pen(Color.FromArgb(50, 0, 0, 0), 2))
            {
                e.Graphics.DrawRectangle(shadowPen, 2, 2, panel.Width - 4, panel.Height - 4);
            }
        }

        private void logoBox_Paint(object sender, PaintEventArgs e)
        {
            PictureBox box = sender as PictureBox;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            
            // Draw a truck icon
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(0, 123, 255)))
            {
                // Truck body
                Rectangle truckBody = new Rectangle(15, 30, 40, 20);
                e.Graphics.FillRectangle(brush, truckBody);
                
                // Truck cab
                Rectangle truckCab = new Rectangle(55, 35, 15, 15);
                e.Graphics.FillRectangle(brush, truckCab);
                
                // Wheels
                using (SolidBrush wheelBrush = new SolidBrush(Color.FromArgb(52, 58, 64)))
                {
                    e.Graphics.FillEllipse(wheelBrush, 20, 48, 8, 8);
                    e.Graphics.FillEllipse(wheelBrush, 42, 48, 8, 8);
                    e.Graphics.FillEllipse(wheelBrush, 58, 48, 8, 8);
                }
            }
        }

        private void button_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            
            // Add rounded corners effect
            using (GraphicsPath path = new GraphicsPath())
            {
                int radius = 10;
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);
                path.CloseFigure();
                
                btn.Region = new Region(path);
            }
        }

        private void myJobsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomerJobsForm jobsForm = new CustomerJobsForm(customerId);
            jobsForm.ShowDialog();
        }

        private void requestJobToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JobRequestForm requestForm = new JobRequestForm(customerId);
            requestForm.ShowDialog();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogoutUser();
        }

        private void btnMyJobs_Click(object sender, EventArgs e)
        {
            CustomerJobsForm jobsForm = new CustomerJobsForm(customerId);
            jobsForm.ShowDialog();
        }

        private void btnRequestJob_Click(object sender, EventArgs e)
        {
            JobRequestForm requestForm = new JobRequestForm(customerId);
            requestForm.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            LogoutUser();
        }

        private void LogoutUser()
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?", 
                "Logout Confirmation", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.timeTimer?.Stop();
                this.Hide();
                LoginForm loginForm = new LoginForm();
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
            }
            base.Dispose(disposing);
        }
    }
}