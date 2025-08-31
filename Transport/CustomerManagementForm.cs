using MySql.Data.MySqlClient;
using System.Data;

namespace Transport
{
    public partial class CustomerManagementForm : Form
    {
        private DatabaseConnection dbConnection;
        private DataTable customerDataTable;

        public CustomerManagementForm()
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            LoadCustomers();
        }

        private void InitializeComponent()
        {
            this.dgvCustomers = new DataGridView();
            this.txtCustomerId = new TextBox();
            this.txtName = new TextBox();
            this.txtEmail = new TextBox();
            this.txtPhone = new TextBox();
            this.txtAddress = new TextBox();
            this.txtPassword = new TextBox();
            this.dtpRegisteredDate = new DateTimePicker();
            this.btnAdd = new Button();
            this.btnUpdate = new Button();
            this.btnDelete = new Button();
            this.btnClear = new Button();
            this.lblCustomerId = new Label();
            this.lblName = new Label();
            this.lblEmail = new Label();
            this.lblPhone = new Label();
            this.lblAddress = new Label();
            this.lblPassword = new Label();
            this.lblRegisteredDate = new Label();
            this.lblTitle = new Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).BeginInit();
            this.SuspendLayout();

            // Form
            this.Text = "Customer Management";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Title
            this.lblTitle.Text = "Customer Management";
            this.lblTitle.Font = new Font("Arial", 16F, FontStyle.Bold);
            this.lblTitle.Location = new Point(400, 10);
            this.lblTitle.Size = new Size(200, 30);

            // DataGridView
            this.dgvCustomers.Location = new Point(20, 50);
            this.dgvCustomers.Size = new Size(600, 400);
            this.dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvCustomers.MultiSelect = false;
            this.dgvCustomers.ReadOnly = true;
            this.dgvCustomers.AllowUserToAddRows = false;
            this.dgvCustomers.AllowUserToDeleteRows = false;
            this.dgvCustomers.SelectionChanged += new EventHandler(this.dgvCustomers_SelectionChanged);

            // Labels and TextBoxes
            int startX = 650;
            int startY = 80;
            int spacing = 40;

            this.lblCustomerId.Text = "Customer ID:";
            this.lblCustomerId.Location = new Point(startX, startY);
            this.lblCustomerId.Size = new Size(100, 23);

            this.txtCustomerId.Location = new Point(startX + 110, startY);
            this.txtCustomerId.Size = new Size(200, 23);
            this.txtCustomerId.ReadOnly = true;

            this.lblName.Text = "Name:";
            this.lblName.Location = new Point(startX, startY + spacing);
            this.lblName.Size = new Size(100, 23);

            this.txtName.Location = new Point(startX + 110, startY + spacing);
            this.txtName.Size = new Size(200, 23);

            this.lblEmail.Text = "Email:";
            this.lblEmail.Location = new Point(startX, startY + spacing * 2);
            this.lblEmail.Size = new Size(100, 23);

            this.txtEmail.Location = new Point(startX + 110, startY + spacing * 2);
            this.txtEmail.Size = new Size(200, 23);

            this.lblPhone.Text = "Phone:";
            this.lblPhone.Location = new Point(startX, startY + spacing * 3);
            this.lblPhone.Size = new Size(100, 23);

            this.txtPhone.Location = new Point(startX + 110, startY + spacing * 3);
            this.txtPhone.Size = new Size(200, 23);

            this.lblAddress.Text = "Address:";
            this.lblAddress.Location = new Point(startX, startY + spacing * 4);
            this.lblAddress.Size = new Size(100, 23);

            this.txtAddress.Location = new Point(startX + 110, startY + spacing * 4);
            this.txtAddress.Size = new Size(200, 60);
            this.txtAddress.Multiline = true;

            this.lblPassword.Text = "Password:";
            this.lblPassword.Location = new Point(startX, startY + spacing * 5 + 20);
            this.lblPassword.Size = new Size(100, 23);

            this.txtPassword.Location = new Point(startX + 110, startY + spacing * 5 + 20);
            this.txtPassword.Size = new Size(200, 23);
            this.txtPassword.UseSystemPasswordChar = true;

            this.lblRegisteredDate.Text = "Registered Date:";
            this.lblRegisteredDate.Location = new Point(startX, startY + spacing * 6 + 20);
            this.lblRegisteredDate.Size = new Size(100, 23);

            this.dtpRegisteredDate.Location = new Point(startX + 110, startY + spacing * 6 + 20);
            this.dtpRegisteredDate.Size = new Size(200, 23);

            // Buttons
            int buttonY = startY + spacing * 7 + 40;
            this.btnAdd.Text = "Add";
            this.btnAdd.Location = new Point(startX, buttonY);
            this.btnAdd.Size = new Size(75, 30);
            this.btnAdd.BackColor = Color.Green;
            this.btnAdd.ForeColor = Color.White;
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);

            this.btnUpdate.Text = "Update";
            this.btnUpdate.Location = new Point(startX + 85, buttonY);
            this.btnUpdate.Size = new Size(75, 30);
            this.btnUpdate.BackColor = Color.Orange;
            this.btnUpdate.ForeColor = Color.White;
            this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);

            this.btnDelete.Text = "Delete";
            this.btnDelete.Location = new Point(startX + 170, buttonY);
            this.btnDelete.Size = new Size(75, 30);
            this.btnDelete.BackColor = Color.Red;
            this.btnDelete.ForeColor = Color.White;
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);

            this.btnClear.Text = "Clear";
            this.btnClear.Location = new Point(startX + 255, buttonY);
            this.btnClear.Size = new Size(75, 30);
            this.btnClear.BackColor = Color.Gray;
            this.btnClear.ForeColor = Color.White;
            this.btnClear.Click += new EventHandler(this.btnClear_Click);

            // Add controls to form
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.dgvCustomers);
            this.Controls.Add(this.lblCustomerId);
            this.Controls.Add(this.txtCustomerId);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblRegisteredDate);
            this.Controls.Add(this.dtpRegisteredDate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClear);

            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).EndInit();
            this.ResumeLayout(false);
        }

        private DataGridView dgvCustomers;
        private TextBox txtCustomerId, txtName, txtEmail, txtPhone, txtAddress, txtPassword;
        private DateTimePicker dtpRegisteredDate;
        private Button btnAdd, btnUpdate, btnDelete, btnClear;
        private Label lblCustomerId, lblName, lblEmail, lblPhone, lblAddress, lblPassword, lblRegisteredDate, lblTitle;

        private void LoadCustomers()
        {
            string query = "SELECT CustomerID, Name, Email, Phone, Address, RegisteredDate FROM Customer ORDER BY CustomerID";
            customerDataTable = dbConnection.ExecuteSelect(query);
            dgvCustomers.DataSource = customerDataTable;
        }

        private void dgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCustomers.CurrentRow != null)
            {
                DataGridViewRow row = dgvCustomers.CurrentRow;
                txtCustomerId.Text = row.Cells["CustomerID"].Value?.ToString();
                txtName.Text = row.Cells["Name"].Value?.ToString();
                txtEmail.Text = row.Cells["Email"].Value?.ToString();
                txtPhone.Text = row.Cells["Phone"].Value?.ToString();
                txtAddress.Text = row.Cells["Address"].Value?.ToString();
                if (row.Cells["RegisteredDate"].Value != null)
                {
                    dtpRegisteredDate.Value = Convert.ToDateTime(row.Cells["RegisteredDate"].Value);
                }
                txtPassword.Text = ""; // Don't show password for security
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                string query = @"INSERT INTO Customer (Name, Email, Phone, Address, RegisteredDate, Password) 
                                VALUES (@name, @email, @phone, @address, @registeredDate, @password)";

                MySqlParameter[] parameters = {
                    new MySqlParameter("@name", txtName.Text.Trim()),
                    new MySqlParameter("@email", txtEmail.Text.Trim()),
                    new MySqlParameter("@phone", txtPhone.Text.Trim()),
                    new MySqlParameter("@address", txtAddress.Text.Trim()),
                    new MySqlParameter("@registeredDate", dtpRegisteredDate.Value.Date),
                    new MySqlParameter("@password", txtPassword.Text.Trim())
                };

                if (dbConnection.ExecuteNonQuery(query, parameters))
                {
                    MessageBox.Show("Customer added successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCustomers();
                    ClearForm();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCustomerId.Text))
            {
                MessageBox.Show("Please select a customer to update.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ValidateInput())
            {
                string query = @"UPDATE Customer SET Name = @name, Email = @email, Phone = @phone, 
                                Address = @address, RegisteredDate = @registeredDate";
                
                var parameterList = new List<MySqlParameter> {
                    new MySqlParameter("@name", txtName.Text.Trim()),
                    new MySqlParameter("@email", txtEmail.Text.Trim()),
                    new MySqlParameter("@phone", txtPhone.Text.Trim()),
                    new MySqlParameter("@address", txtAddress.Text.Trim()),
                    new MySqlParameter("@registeredDate", dtpRegisteredDate.Value.Date),
                    new MySqlParameter("@customerId", Convert.ToInt32(txtCustomerId.Text))
                };

                if (!string.IsNullOrEmpty(txtPassword.Text.Trim()))
                {
                    query += ", Password = @password";
                    parameterList.Add(new MySqlParameter("@password", txtPassword.Text.Trim()));
                }

                query += " WHERE CustomerID = @customerId";

                if (dbConnection.ExecuteNonQuery(query, parameterList.ToArray()))
                {
                    MessageBox.Show("Customer updated successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCustomers();
                    ClearForm();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCustomerId.Text))
            {
                MessageBox.Show("Please select a customer to delete.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this customer?", 
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string query = "DELETE FROM Customer WHERE CustomerID = @customerId";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@customerId", Convert.ToInt32(txtCustomerId.Text))
                };

                if (dbConnection.ExecuteNonQuery(query, parameters))
                {
                    MessageBox.Show("Customer deleted successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCustomers();
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
            txtCustomerId.Text = "";
            txtName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            txtPassword.Text = "";
            dtpRegisteredDate.Value = DateTime.Now;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter customer name.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Please enter email address.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Please enter phone number.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Please enter address.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Only validate password for new customers
            if (string.IsNullOrEmpty(txtCustomerId.Text) && string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter password.", "Validation Error", 
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