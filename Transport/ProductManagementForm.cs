using MySql.Data.MySqlClient;
using System.Data;

namespace Transport
{
    public partial class ProductManagementForm : Form
    {
        private DatabaseConnection dbConnection;
        private DataTable productDataTable;

        public ProductManagementForm()
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            LoadProducts();
        }

        private void InitializeComponent()
        {
            this.dgvProducts = new DataGridView();
            this.txtProductId = new TextBox();
            this.txtName = new TextBox();
            this.txtDescription = new TextBox();
            this.txtPrice = new TextBox();
            this.txtStock = new TextBox();
            this.btnAdd = new Button();
            this.btnUpdate = new Button();
            this.btnDelete = new Button();
            this.btnClear = new Button();
            this.lblProductId = new Label();
            this.lblName = new Label();
            this.lblDescription = new Label();
            this.lblPrice = new Label();
            this.lblStock = new Label();
            this.lblTitle = new Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.SuspendLayout();

            // Form
            this.Text = "Product Management";
            this.Size = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Title
            this.lblTitle.Text = "Product Management";
            this.lblTitle.Font = new Font("Arial", 16F, FontStyle.Bold);
            this.lblTitle.Location = new Point(400, 10);
            this.lblTitle.Size = new Size(200, 30);

            // DataGridView
            this.dgvProducts.Location = new Point(20, 50);
            this.dgvProducts.Size = new Size(600, 400);
            this.dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.MultiSelect = false;
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.SelectionChanged += new EventHandler(this.dgvProducts_SelectionChanged);

            // Labels and TextBoxes
            int startX = 650;
            int startY = 80;
            int spacing = 50;

            this.lblProductId.Text = "Product ID:";
            this.lblProductId.Location = new Point(startX, startY);
            this.lblProductId.Size = new Size(100, 23);

            this.txtProductId.Location = new Point(startX + 110, startY);
            this.txtProductId.Size = new Size(200, 23);
            this.txtProductId.ReadOnly = true;

            this.lblName.Text = "Name:";
            this.lblName.Location = new Point(startX, startY + spacing);
            this.lblName.Size = new Size(100, 23);

            this.txtName.Location = new Point(startX + 110, startY + spacing);
            this.txtName.Size = new Size(200, 23);

            this.lblDescription.Text = "Description:";
            this.lblDescription.Location = new Point(startX, startY + spacing * 2);
            this.lblDescription.Size = new Size(100, 23);

            this.txtDescription.Location = new Point(startX + 110, startY + spacing * 2);
            this.txtDescription.Size = new Size(200, 60);
            this.txtDescription.Multiline = true;

            this.lblPrice.Text = "Price:";
            this.lblPrice.Location = new Point(startX, startY + spacing * 3 + 10);
            this.lblPrice.Size = new Size(100, 23);

            this.txtPrice.Location = new Point(startX + 110, startY + spacing * 3 + 10);
            this.txtPrice.Size = new Size(200, 23);

            this.lblStock.Text = "Stock:";
            this.lblStock.Location = new Point(startX, startY + spacing * 4 + 10);
            this.lblStock.Size = new Size(100, 23);

            this.txtStock.Location = new Point(startX + 110, startY + spacing * 4 + 10);
            this.txtStock.Size = new Size(200, 23);

            // Buttons
            int buttonY = startY + spacing * 5 + 20;
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
            this.Controls.Add(this.dgvProducts);
            this.Controls.Add(this.lblProductId);
            this.Controls.Add(this.txtProductId);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.lblStock);
            this.Controls.Add(this.txtStock);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClear);

            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.ResumeLayout(false);
        }

        private DataGridView dgvProducts;
        private TextBox txtProductId, txtName, txtDescription, txtPrice, txtStock;
        private Button btnAdd, btnUpdate, btnDelete, btnClear;
        private Label lblProductId, lblName, lblDescription, lblPrice, lblStock, lblTitle;

        private void LoadProducts()
        {
            string query = "SELECT ProductID, Name, Description, Price, Stock FROM Product ORDER BY ProductID";
            productDataTable = dbConnection.ExecuteSelect(query);
            dgvProducts.DataSource = productDataTable;

            // Format Price column
            if (dgvProducts.Columns["Price"] != null)
            {
                dgvProducts.Columns["Price"].DefaultCellStyle.Format = "C2";
            }
        }

        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow != null)
            {
                DataGridViewRow row = dgvProducts.CurrentRow;
                txtProductId.Text = row.Cells["ProductID"].Value?.ToString();
                txtName.Text = row.Cells["Name"].Value?.ToString();
                txtDescription.Text = row.Cells["Description"].Value?.ToString();
                txtPrice.Text = row.Cells["Price"].Value?.ToString();
                txtStock.Text = row.Cells["Stock"].Value?.ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                string query = @"INSERT INTO Product (Name, Description, Price, Stock) 
                                VALUES (@name, @description, @price, @stock)";

                MySqlParameter[] parameters = {
                    new MySqlParameter("@name", txtName.Text.Trim()),
                    new MySqlParameter("@description", txtDescription.Text.Trim()),
                    new MySqlParameter("@price", Convert.ToDecimal(txtPrice.Text.Trim())),
                    new MySqlParameter("@stock", Convert.ToInt32(txtStock.Text.Trim()))
                };

                if (dbConnection.ExecuteNonQuery(query, parameters))
                {
                    MessageBox.Show("Product added successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts();
                    ClearForm();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtProductId.Text))
            {
                MessageBox.Show("Please select a product to update.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ValidateInput())
            {
                string query = @"UPDATE Product SET Name = @name, Description = @description, 
                                Price = @price, Stock = @stock WHERE ProductID = @productId";

                MySqlParameter[] parameters = {
                    new MySqlParameter("@name", txtName.Text.Trim()),
                    new MySqlParameter("@description", txtDescription.Text.Trim()),
                    new MySqlParameter("@price", Convert.ToDecimal(txtPrice.Text.Trim())),
                    new MySqlParameter("@stock", Convert.ToInt32(txtStock.Text.Trim())),
                    new MySqlParameter("@productId", Convert.ToInt32(txtProductId.Text))
                };

                if (dbConnection.ExecuteNonQuery(query, parameters))
                {
                    MessageBox.Show("Product updated successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts();
                    ClearForm();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtProductId.Text))
            {
                MessageBox.Show("Please select a product to delete.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this product?", 
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string query = "DELETE FROM Product WHERE ProductID = @productId";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@productId", Convert.ToInt32(txtProductId.Text))
                };

                if (dbConnection.ExecuteNonQuery(query, parameters))
                {
                    MessageBox.Show("Product deleted successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts();
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
            txtProductId.Text = "";
            txtName.Text = "";
            txtDescription.Text = "";
            txtPrice.Text = "";
            txtStock.Text = "";
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter product name.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Please enter price.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text.Trim(), out decimal price) || price < 0)
            {
                MessageBox.Show("Please enter a valid price (positive number).", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtStock.Text))
            {
                MessageBox.Show("Please enter stock quantity.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtStock.Text.Trim(), out int stock) || stock < 0)
            {
                MessageBox.Show("Please enter a valid stock quantity (non-negative integer).", "Validation Error", 
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