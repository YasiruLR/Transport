using MySql.Data.MySqlClient;
using System.Drawing.Drawing2D;

namespace Transport
{
    public partial class LoginForm : Form
    {
        private DatabaseConnection dbConnection;
        private Panel mainPanel;
        private Panel leftPanel;
        private Panel rightPanel;
        private PictureBox logoBox;
        private Label lblTitle;
        private Label lblSubtitle;

        public LoginForm()
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection();
        }

        private void InitializeComponent()
        {
            this.txtUsername = new TextBox();
            this.txtPassword = new TextBox();
            this.btnLogin = new Button();
            this.lblUsername = new Label();
            this.lblPassword = new Label();
            this.lblTitle = new Label();
            this.lblSubtitle = new Label();
            this.rbAdmin = new RadioButton();
            this.rbCustomer = new RadioButton();
            this.groupBoxUserType = new GroupBox();
            this.mainPanel = new Panel();
            this.leftPanel = new Panel();
            this.rightPanel = new Panel();
            this.logoBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.logoBox)).BeginInit();
            this.SuspendLayout();

            // Form Properties
            this.Text = "Transport Management System - Login";
            this.Size = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(240, 248, 255);

            // Main Panel
            this.mainPanel.Size = new Size(1000, 650);
            this.mainPanel.Location = new Point(0, 0);
            this.mainPanel.BackColor = Color.White;
            this.mainPanel.Paint += new PaintEventHandler(this.mainPanel_Paint);

            // Left Panel
            this.leftPanel.Size = new Size(500, 650);
            this.leftPanel.Location = new Point(0, 0);
            this.leftPanel.Paint += new PaintEventHandler(this.leftPanel_Paint);

            // Logo
            this.logoBox.Location = new Point(150, 150);
            this.logoBox.Size = new Size(200, 200);
            this.logoBox.BackColor = Color.Transparent;
            this.logoBox.Paint += new PaintEventHandler(this.logoBox_Paint);

            // Brand Title
            Label lblBrandTitle = new Label();
            lblBrandTitle.Text = "TRANSPORT";
            lblBrandTitle.Font = new Font("Segoe UI", 32F, FontStyle.Bold);
            lblBrandTitle.ForeColor = Color.White;
            lblBrandTitle.Location = new Point(100, 380);
            lblBrandTitle.Size = new Size(300, 50);
            lblBrandTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblBrandTitle.BackColor = Color.Transparent;

            Label lblBrandSubtitle = new Label();
            lblBrandSubtitle.Text = "Management System";
            lblBrandSubtitle.Font = new Font("Segoe UI", 18F, FontStyle.Regular);
            lblBrandSubtitle.ForeColor = Color.FromArgb(230, 230, 230);
            lblBrandSubtitle.Location = new Point(100, 430);
            lblBrandSubtitle.Size = new Size(300, 30);
            lblBrandSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            lblBrandSubtitle.BackColor = Color.Transparent;

            Label lblVersion = new Label();
            lblVersion.Text = "Version 1.0";
            lblVersion.Font = new Font("Segoe UI", 10F, FontStyle.Italic);
            lblVersion.ForeColor = Color.FromArgb(200, 200, 200);
            lblVersion.Location = new Point(100, 480);
            lblVersion.Size = new Size(300, 20);
            lblVersion.TextAlign = ContentAlignment.MiddleCenter;
            lblVersion.BackColor = Color.Transparent;

            this.leftPanel.Controls.Add(this.logoBox);
            this.leftPanel.Controls.Add(lblBrandTitle);
            this.leftPanel.Controls.Add(lblBrandSubtitle);
            this.leftPanel.Controls.Add(lblVersion);

            // Right Panel - Login
            this.rightPanel.Size = new Size(500, 650);
            this.rightPanel.Location = new Point(500, 0);
            this.rightPanel.BackColor = Color.White;

            // Title
            this.lblTitle.Text = "Welcome Back!";
            this.lblTitle.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.FromArgb(52, 58, 64);
            this.lblTitle.Location = new Point(50, 80);
            this.lblTitle.Size = new Size(400, 50);

            this.lblSubtitle.Text = "Please sign in to your account";
            this.lblSubtitle.Font = new Font("Segoe UI", 14F, FontStyle.Regular);
            this.lblSubtitle.ForeColor = Color.FromArgb(108, 117, 125);
            this.lblSubtitle.Location = new Point(50, 130);
            this.lblSubtitle.Size = new Size(400, 30);

            // User Type
            this.groupBoxUserType.Text = "Login As";
            this.groupBoxUserType.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.groupBoxUserType.ForeColor = Color.FromArgb(52, 58, 64);
            this.groupBoxUserType.Location = new Point(50, 190);
            this.groupBoxUserType.Size = new Size(400, 70);
            this.groupBoxUserType.BackColor = Color.FromArgb(248, 249, 250);

            this.rbAdmin.Text = "Administrator";
            this.rbAdmin.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            this.rbAdmin.ForeColor = Color.FromArgb(102, 16, 242);
            this.rbAdmin.Location = new Point(30, 30);
            this.rbAdmin.Checked = true;

            this.rbCustomer.Text = "Customer";
            this.rbCustomer.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            this.rbCustomer.ForeColor = Color.FromArgb(0, 123, 255);
            this.rbCustomer.Location = new Point(220, 30);

            this.groupBoxUserType.Controls.Add(this.rbAdmin);
            this.groupBoxUserType.Controls.Add(this.rbCustomer);

            // Username
            this.lblUsername.Text = "Username";
            this.lblUsername.Font = new Font("Segoe UI", 11F);
            this.lblUsername.Location = new Point(50, 290);

            this.txtUsername.Location = new Point(50, 320);
            this.txtUsername.Size = new Size(400, 35);
            this.txtUsername.Font = new Font("Segoe UI", 12F);
            this.txtUsername.BorderStyle = BorderStyle.FixedSingle;
            this.txtUsername.BackColor = Color.FromArgb(248, 249, 250);
            this.txtUsername.PlaceholderText = "Enter your username or email";

            // Password
            this.lblPassword.Text = "Password";
            this.lblPassword.Font = new Font("Segoe UI", 11F);
            this.lblPassword.Location = new Point(50, 380);

            this.txtPassword.Location = new Point(50, 410);
            this.txtPassword.Size = new Size(400, 35);
            this.txtPassword.Font = new Font("Segoe UI", 12F);
            this.txtPassword.BorderStyle = BorderStyle.FixedSingle;
            this.txtPassword.BackColor = Color.FromArgb(248, 249, 250);
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.PlaceholderText = "Enter your password";

            // Login Button
            this.btnLogin.Text = "🚀 Sign In";
            this.btnLogin.Location = new Point(50, 480);
            this.btnLogin.Size = new Size(400, 50);
            this.btnLogin.BackColor = Color.FromArgb(0, 123, 255);
            this.btnLogin.ForeColor = Color.White;
            this.btnLogin.FlatStyle = FlatStyle.Flat;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.btnLogin.Click += new EventHandler(this.btnLogin_Click);
            this.btnLogin.Paint += new PaintEventHandler(this.btnLogin_Paint);

            // Exit Button
            Button btnExit = new Button();
            btnExit.Text = "Exit";
            btnExit.Location = new Point(50, 550);
            btnExit.Size = new Size(400, 50);
            btnExit.BackColor = Color.FromArgb(220, 53, 69);
            btnExit.ForeColor = Color.White;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnExit.Cursor = Cursors.Hand;
            btnExit.Click += (s, e) =>
            {
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to exit the Transport Management System?",
                    "Exit Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                }
            };

            // Add controls
            this.rightPanel.Controls.Add(this.lblTitle);
            this.rightPanel.Controls.Add(this.lblSubtitle);
            this.rightPanel.Controls.Add(this.groupBoxUserType);
            this.rightPanel.Controls.Add(this.lblUsername);
            this.rightPanel.Controls.Add(this.txtUsername);
            this.rightPanel.Controls.Add(this.lblPassword);
            this.rightPanel.Controls.Add(this.txtPassword);
            this.rightPanel.Controls.Add(this.btnLogin);
            this.rightPanel.Controls.Add(btnExit);

            this.mainPanel.Controls.Add(this.leftPanel);
            this.mainPanel.Controls.Add(this.rightPanel);

            this.Controls.Add(this.mainPanel);

            ((System.ComponentModel.ISupportInitialize)(this.logoBox)).EndInit();
            this.ResumeLayout(false);
        }

        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblUsername;
        private Label lblPassword;
        private RadioButton rbAdmin;
        private RadioButton rbCustomer;
        private GroupBox groupBoxUserType;

        // Paints
        private void mainPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            using (Pen borderPen = new Pen(Color.FromArgb(0, 123, 255), 3))
            {
                e.Graphics.DrawRectangle(borderPen, 0, 0, panel.Width - 1, panel.Height - 1);
            }
        }

        private void leftPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            using (LinearGradientBrush brush = new LinearGradientBrush(
                panel.ClientRectangle,
                Color.FromArgb(102, 16, 242),
                Color.FromArgb(0, 123, 255),
                LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, panel.ClientRectangle);
            }
        }

        private void logoBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (SolidBrush brush = new SolidBrush(Color.White))
            {
                Rectangle truckBody = new Rectangle(40, 80, 120, 60);
                e.Graphics.FillRectangle(brush, truckBody);

                Rectangle truckCab = new Rectangle(160, 90, 40, 40);
                e.Graphics.FillRectangle(brush, truckCab);

                using (SolidBrush wheelBrush = new SolidBrush(Color.FromArgb(52, 58, 64)))
                {
                    e.Graphics.FillEllipse(wheelBrush, 60, 130, 20, 20);
                    e.Graphics.FillEllipse(wheelBrush, 120, 130, 20, 20);
                    e.Graphics.FillEllipse(wheelBrush, 170, 130, 20, 20);
                }
            }
        }

        private void btnLogin_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = new GraphicsPath())
            {
                int radius = 25;
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);
                path.CloseFigure();

                btn.Region = new Region(path);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowErrorMessage("Please enter both username and password.");
                return;
            }

            if (rbAdmin.Checked)
                ValidateAdminLogin(username, password);
            else
                ValidateCustomerLogin(username, password);
        }

        private void ValidateAdminLogin(string username, string password)
        {
            string query = "SELECT AdminID, Role FROM Admin WHERE Username = @username AND Password = @password";
            MySqlParameter[] parameters = {
                new MySqlParameter("@username", username),
                new MySqlParameter("@password", password)
            };

            var result = dbConnection.ExecuteSelect(query, parameters);

            if (result.Rows.Count > 0)
            {
                string role = result.Rows[0]["Role"].ToString();
                int adminId = Convert.ToInt32(result.Rows[0]["AdminID"]);
                ShowSuccessMessage($"Welcome {role}!");

                AdminDashboard adminDashboard = new AdminDashboard(adminId, role);
                this.Hide();
                adminDashboard.ShowDialog();
                this.Close();
            }
            else
            {
                ShowErrorMessage("Invalid admin credentials!");
            }
        }

        private void ValidateCustomerLogin(string email, string password)
        {
            string query = "SELECT CustomerID, Name FROM Customer WHERE Email = @email AND Password = @password";
            MySqlParameter[] parameters = {
                new MySqlParameter("@email", email),
                new MySqlParameter("@password", password)
            };

            var result = dbConnection.ExecuteSelect(query, parameters);

            if (result.Rows.Count > 0)
            {
                int customerId = Convert.ToInt32(result.Rows[0]["CustomerID"]);
                string customerName = result.Rows[0]["Name"].ToString();
                ShowSuccessMessage($"Welcome {customerName}!");

                CustomerDashboard customerDashboard = new CustomerDashboard(customerId, customerName);
                this.Hide();
                customerDashboard.ShowDialog();
                this.Close();
            }
            else
            {
                ShowErrorMessage("Invalid customer credentials!");
            }
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Login Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Login Successful",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
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
