using System.Data;
using MySql.Data.MySqlClient;
using System.Drawing.Drawing2D;

namespace Transport
{
    public partial class ProductReportForm : Form
    {
        private DatabaseConnection dbConnection;
        private DataGridView dgvProducts;
        private Panel filterPanel;
        private ComboBox cmbStockFilter;
        private ComboBox cmbPriceFilter;
        private TextBox txtMinPrice;
        private TextBox txtMaxPrice;
        private Button btnGenerateReport;
        private Button btnExport;
        private Button btnPrint;
        private Label lblTotalProducts;
        private Label lblLowStock;
        private Label lblTotalValue;
        private Label lblAveragePrice;

        public ProductReportForm()
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            LoadProductReport();
            LoadProductStats();
        }

        private void InitializeComponent()
        {
            this.dgvProducts = new DataGridView();
            this.filterPanel = new Panel();
            this.cmbStockFilter = new ComboBox();
            this.cmbPriceFilter = new ComboBox();
            this.txtMinPrice = new TextBox();
            this.txtMaxPrice = new TextBox();
            this.btnGenerateReport = new Button();
            this.btnExport = new Button();
            this.btnPrint = new Button();
            this.lblTotalProducts = new Label();
            this.lblLowStock = new Label();
            this.lblTotalValue = new Label();
            this.lblAveragePrice = new Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.SuspendLayout();

            // Form Properties
            this.Text = "Product Report - Transport Management System";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 250);

            // Header Panel
            Panel headerPanel = new Panel();
            headerPanel.Location = new Point(0, 0);
            headerPanel.Size = new Size(1200, 80);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.BackColor = Color.FromArgb(255, 193, 7);

            Label lblTitle = new Label();
            lblTitle.Text = "?? Product Report & Analytics";
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(30, 25);
            lblTitle.Size = new Size(400, 30);
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

            // Filter Panel
            this.filterPanel.Location = new Point(20, 100);
            this.filterPanel.Size = new Size(1160, 100);
            this.filterPanel.BackColor = Color.White;
            this.filterPanel.BorderStyle = BorderStyle.FixedSingle;

            Label lblFilter = new Label();
            lblFilter.Text = "Filter Options:";
            lblFilter.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblFilter.Location = new Point(20, 15);
            lblFilter.Size = new Size(120, 25);

            Label lblStock = new Label();
            lblStock.Text = "Stock Level:";
            lblStock.Location = new Point(20, 50);
            lblStock.Size = new Size(80, 25);

            this.cmbStockFilter.Location = new Point(105, 48);
            this.cmbStockFilter.Size = new Size(120, 25);
            this.cmbStockFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbStockFilter.Items.AddRange(new string[] { "All Stock", "In Stock (>0)", "Low Stock (<10)", "Out of Stock (0)" });
            this.cmbStockFilter.SelectedIndex = 0;

            Label lblPriceRange = new Label();
            lblPriceRange.Text = "Price Range:";
            lblPriceRange.Location = new Point(250, 50);
            lblPriceRange.Size = new Size(80, 25);

            this.cmbPriceFilter.Location = new Point(335, 48);
            this.cmbPriceFilter.Size = new Size(100, 25);
            this.cmbPriceFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbPriceFilter.Items.AddRange(new string[] { "All Prices", "Custom Range" });
            this.cmbPriceFilter.SelectedIndex = 0;
            this.cmbPriceFilter.SelectedIndexChanged += new EventHandler(this.cmbPriceFilter_SelectedIndexChanged);

            Label lblMinPrice = new Label();
            lblMinPrice.Text = "Min:";
            lblMinPrice.Location = new Point(450, 50);
            lblMinPrice.Size = new Size(30, 25);

            this.txtMinPrice.Location = new Point(485, 48);
            this.txtMinPrice.Size = new Size(80, 25);
            this.txtMinPrice.PlaceholderText = "0.00";
            this.txtMinPrice.Enabled = false;

            Label lblMaxPrice = new Label();
            lblMaxPrice.Text = "Max:";
            lblMaxPrice.Location = new Point(580, 50);
            lblMaxPrice.Size = new Size(30, 25);

            this.txtMaxPrice.Location = new Point(615, 48);
            this.txtMaxPrice.Size = new Size(80, 25);
            this.txtMaxPrice.PlaceholderText = "999.99";
            this.txtMaxPrice.Enabled = false;

            this.btnGenerateReport.Text = "Generate Report";
            this.btnGenerateReport.Location = new Point(720, 45);
            this.btnGenerateReport.Size = new Size(120, 30);
            this.btnGenerateReport.BackColor = Color.FromArgb(255, 193, 7);
            this.btnGenerateReport.ForeColor = Color.White;
            this.btnGenerateReport.FlatStyle = FlatStyle.Flat;
            this.btnGenerateReport.FlatAppearance.BorderSize = 0;
            this.btnGenerateReport.Cursor = Cursors.Hand;
            this.btnGenerateReport.Click += new EventHandler(this.btnGenerateReport_Click);

            this.btnExport.Text = "Export";
            this.btnExport.Location = new Point(850, 45);
            this.btnExport.Size = new Size(80, 30);
            this.btnExport.BackColor = Color.FromArgb(40, 167, 69);
            this.btnExport.ForeColor = Color.White;
            this.btnExport.FlatStyle = FlatStyle.Flat;
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.Cursor = Cursors.Hand;
            this.btnExport.Click += new EventHandler(this.btnExport_Click);

            this.btnPrint.Text = "Print";
            this.btnPrint.Location = new Point(940, 45);
            this.btnPrint.Size = new Size(80, 30);
            this.btnPrint.BackColor = Color.FromArgb(108, 117, 125);
            this.btnPrint.ForeColor = Color.White;
            this.btnPrint.FlatStyle = FlatStyle.Flat;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.Cursor = Cursors.Hand;
            this.btnPrint.Click += new EventHandler(this.btnPrint_Click);

            this.filterPanel.Controls.Add(lblFilter);
            this.filterPanel.Controls.Add(lblStock);
            this.filterPanel.Controls.Add(this.cmbStockFilter);
            this.filterPanel.Controls.Add(lblPriceRange);
            this.filterPanel.Controls.Add(this.cmbPriceFilter);
            this.filterPanel.Controls.Add(lblMinPrice);
            this.filterPanel.Controls.Add(this.txtMinPrice);
            this.filterPanel.Controls.Add(lblMaxPrice);
            this.filterPanel.Controls.Add(this.txtMaxPrice);
            this.filterPanel.Controls.Add(this.btnGenerateReport);
            this.filterPanel.Controls.Add(this.btnExport);
            this.filterPanel.Controls.Add(this.btnPrint);

            // Stats Panel
            Panel statsPanel = new Panel();
            statsPanel.Location = new Point(20, 220);
            statsPanel.Size = new Size(1160, 80);
            statsPanel.BackColor = Color.FromArgb(248, 249, 250);
            statsPanel.BorderStyle = BorderStyle.FixedSingle;

            Label lblStatsTitle = new Label();
            lblStatsTitle.Text = "Product Statistics:";
            lblStatsTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblStatsTitle.Location = new Point(20, 15);
            lblStatsTitle.Size = new Size(150, 25);

            this.lblTotalProducts.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.lblTotalProducts.Location = new Point(20, 45);
            this.lblTotalProducts.Size = new Size(150, 20);

            this.lblLowStock.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.lblLowStock.Location = new Point(180, 45);
            this.lblLowStock.Size = new Size(150, 20);

            this.lblTotalValue.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.lblTotalValue.Location = new Point(340, 45);
            this.lblTotalValue.Size = new Size(200, 20);

            this.lblAveragePrice.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.lblAveragePrice.Location = new Point(550, 45);
            this.lblAveragePrice.Size = new Size(200, 20);

            statsPanel.Controls.Add(lblStatsTitle);
            statsPanel.Controls.Add(this.lblTotalProducts);
            statsPanel.Controls.Add(this.lblLowStock);
            statsPanel.Controls.Add(this.lblTotalValue);
            statsPanel.Controls.Add(this.lblAveragePrice);

            // DataGridView
            this.dgvProducts.Location = new Point(20, 320);
            this.dgvProducts.Size = new Size(1160, 400);
            this.dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.MultiSelect = false;
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.BackgroundColor = Color.White;
            this.dgvProducts.BorderStyle = BorderStyle.FixedSingle;
            this.dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 193, 7);
            this.dgvProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.dgvProducts.EnableHeadersVisualStyles = false;
            this.dgvProducts.RowTemplate.Height = 30;

            // Add controls to form
            this.Controls.Add(headerPanel);
            this.Controls.Add(this.filterPanel);
            this.Controls.Add(statsPanel);
            this.Controls.Add(this.dgvProducts);

            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.ResumeLayout(false);
        }

        private void LoadProductReport()
        {
            try
            {
                string query = @"
                    SELECT 
                        ProductID,
                        Name as ProductName,
                        Description,
                        Price,
                        Stock,
                        (Price * Stock) as TotalValue,
                        CASE 
                            WHEN Stock = 0 THEN 'Out of Stock'
                            WHEN Stock < 10 THEN 'Low Stock'
                            WHEN Stock < 50 THEN 'Medium Stock'
                            ELSE 'Well Stocked'
                        END as StockStatus,
                        CASE 
                            WHEN Price < 50 THEN 'Budget'
                            WHEN Price < 200 THEN 'Standard'
                            WHEN Price < 500 THEN 'Premium'
                            ELSE 'Luxury'
                        END as PriceCategory
                    FROM Product 
                    ORDER BY ProductID";

                DataTable productData = dbConnection.ExecuteSelect(query);
                dgvProducts.DataSource = productData;

                FormatProductGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading product report: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatProductGrid()
        {
            // Format columns
            if (dgvProducts.Columns["ProductID"] != null)
            {
                dgvProducts.Columns["ProductID"].HeaderText = "Product ID";
                dgvProducts.Columns["ProductID"].Width = 100;
            }

            if (dgvProducts.Columns["ProductName"] != null)
            {
                dgvProducts.Columns["ProductName"].HeaderText = "Product Name";
                dgvProducts.Columns["ProductName"].Width = 200;
            }

            if (dgvProducts.Columns["Description"] != null)
            {
                dgvProducts.Columns["Description"].HeaderText = "Description";
                dgvProducts.Columns["Description"].Width = 250;
            }

            if (dgvProducts.Columns["Price"] != null)
            {
                dgvProducts.Columns["Price"].HeaderText = "Price";
                dgvProducts.Columns["Price"].DefaultCellStyle.Format = "C2";
                dgvProducts.Columns["Price"].Width = 100;
            }

            if (dgvProducts.Columns["Stock"] != null)
            {
                dgvProducts.Columns["Stock"].HeaderText = "Stock Qty";
                dgvProducts.Columns["Stock"].Width = 100;
            }

            if (dgvProducts.Columns["TotalValue"] != null)
            {
                dgvProducts.Columns["TotalValue"].HeaderText = "Total Value";
                dgvProducts.Columns["TotalValue"].DefaultCellStyle.Format = "C2";
                dgvProducts.Columns["TotalValue"].Width = 120;
            }

            if (dgvProducts.Columns["StockStatus"] != null)
            {
                dgvProducts.Columns["StockStatus"].HeaderText = "Stock Status";
                dgvProducts.Columns["StockStatus"].Width = 120;
            }

            if (dgvProducts.Columns["PriceCategory"] != null)
            {
                dgvProducts.Columns["PriceCategory"].HeaderText = "Price Category";
                dgvProducts.Columns["PriceCategory"].Width = 120;
            }

            // Color code rows based on stock status
            foreach (DataGridViewRow row in dgvProducts.Rows)
            {
                if (row.Cells["StockStatus"].Value != null)
                {
                    string stockStatus = row.Cells["StockStatus"].Value.ToString();
                    switch (stockStatus)
                    {
                        case "Out of Stock":
                            row.DefaultCellStyle.BackColor = Color.FromArgb(248, 215, 218);
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(132, 32, 41);
                            break;
                        case "Low Stock":
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 243, 205);
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(133, 100, 4);
                            break;
                        case "Medium Stock":
                            row.DefaultCellStyle.BackColor = Color.FromArgb(217, 237, 247);
                            break;
                        case "Well Stocked":
                            row.DefaultCellStyle.BackColor = Color.FromArgb(212, 237, 218);
                            break;
                    }
                }
            }
        }

        private void LoadProductStats()
        {
            try
            {
                // Total products
                string totalQuery = "SELECT COUNT(*) FROM Product";
                int totalProducts = Convert.ToInt32(dbConnection.ExecuteScalar(totalQuery));

                // Low stock products
                string lowStockQuery = "SELECT COUNT(*) FROM Product WHERE Stock < 10";
                int lowStockProducts = Convert.ToInt32(dbConnection.ExecuteScalar(lowStockQuery));

                // Total inventory value
                string totalValueQuery = "SELECT SUM(Price * Stock) FROM Product";
                var totalValueResult = dbConnection.ExecuteScalar(totalValueQuery);
                decimal totalValue = totalValueResult != DBNull.Value ? Convert.ToDecimal(totalValueResult) : 0;

                // Average price
                string avgPriceQuery = "SELECT AVG(Price) FROM Product";
                var avgPriceResult = dbConnection.ExecuteScalar(avgPriceQuery);
                decimal avgPrice = avgPriceResult != DBNull.Value ? Convert.ToDecimal(avgPriceResult) : 0;

                lblTotalProducts.Text = $"Total Products: {totalProducts}";
                lblLowStock.Text = $"Low Stock: {lowStockProducts}";
                lblTotalValue.Text = $"Total Value: {totalValue:C2}";
                lblAveragePrice.Text = $"Avg Price: {avgPrice:C2}";
            }
            catch (Exception ex)
            {
                lblTotalProducts.Text = "Stats unavailable";
                lblLowStock.Text = "";
                lblTotalValue.Text = "";
                lblAveragePrice.Text = "";
            }
        }

        private void cmbPriceFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool enablePriceFields = cmbPriceFilter.SelectedItem.ToString() == "Custom Range";
            txtMinPrice.Enabled = enablePriceFields;
            txtMaxPrice.Enabled = enablePriceFields;
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                string query = GetFilteredProductQuery();
                DataTable filteredData = dbConnection.ExecuteSelect(query);
                dgvProducts.DataSource = filteredData;
                FormatProductGrid();

                MessageBox.Show($"Report generated successfully! Found {filteredData.Rows.Count} products.", 
                    "Report Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetFilteredProductQuery()
        {
            string baseQuery = @"
                SELECT 
                    ProductID,
                    Name as ProductName,
                    Description,
                    Price,
                    Stock,
                    (Price * Stock) as TotalValue,
                    CASE 
                        WHEN Stock = 0 THEN 'Out of Stock'
                        WHEN Stock < 10 THEN 'Low Stock'
                        WHEN Stock < 50 THEN 'Medium Stock'
                        ELSE 'Well Stocked'
                    END as StockStatus,
                    CASE 
                        WHEN Price < 50 THEN 'Budget'
                        WHEN Price < 200 THEN 'Standard'
                        WHEN Price < 500 THEN 'Premium'
                        ELSE 'Luxury'
                    END as PriceCategory
                FROM Product";

            List<string> conditions = new List<string>();

            // Stock filter
            string selectedStock = cmbStockFilter.SelectedItem.ToString();
            switch (selectedStock)
            {
                case "In Stock (>0)":
                    conditions.Add("Stock > 0");
                    break;
                case "Low Stock (<10)":
                    conditions.Add("Stock < 10");
                    break;
                case "Out of Stock (0)":
                    conditions.Add("Stock = 0");
                    break;
            }

            // Price filter
            if (cmbPriceFilter.SelectedItem.ToString() == "Custom Range")
            {
                if (!string.IsNullOrEmpty(txtMinPrice.Text) && decimal.TryParse(txtMinPrice.Text, out decimal minPrice))
                {
                    conditions.Add($"Price >= {minPrice}");
                }

                if (!string.IsNullOrEmpty(txtMaxPrice.Text) && decimal.TryParse(txtMaxPrice.Text, out decimal maxPrice))
                {
                    conditions.Add($"Price <= {maxPrice}");
                }
            }

            if (conditions.Count > 0)
            {
                baseQuery += " WHERE " + string.Join(" AND ", conditions);
            }

            return baseQuery + " ORDER BY ProductID";
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                saveDialog.FileName = $"ProductReport_{DateTime.Now:yyyyMMdd}.csv";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToCsv(saveDialog.FileName);
                    MessageBox.Show("Report exported successfully!", "Export Complete", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting report: {ex.Message}", "Export Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToCsv(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                // Write headers
                string[] headers = new string[dgvProducts.Columns.Count];
                for (int i = 0; i < dgvProducts.Columns.Count; i++)
                {
                    headers[i] = dgvProducts.Columns[i].HeaderText;
                }
                writer.WriteLine(string.Join(",", headers));

                // Write data
                foreach (DataGridViewRow row in dgvProducts.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string[] values = new string[row.Cells.Count];
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            values[i] = row.Cells[i].Value?.ToString() ?? "";
                        }
                        writer.WriteLine(string.Join(",", values));
                    }
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Print functionality would be implemented here.\nThis could integrate with Crystal Reports or Windows Printing API.", 
                "Print Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
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