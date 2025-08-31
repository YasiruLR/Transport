using System.Drawing.Drawing2D;

namespace Transport
{
    public partial class ReportParametersForm : Form
    {
        private string reportType;
        private Panel contentPanel;
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;
        private ComboBox cmbFilterType;
        private ComboBox cmbStatusFilter;
        private ComboBox cmbStockFilter;
        private ComboBox cmbCustomerFilter;
        private CheckBox chkIncludeCharts;
        private CheckBox chkIncludeDetails;
        private CheckBox chkColorCoding;
        private Button btnGenerate;
        private Button btnCancel;

        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }
        public string FilterType { get; private set; }
        public string StatusFilter { get; private set; }
        public string StockFilter { get; private set; }
        public int SelectedCustomerId { get; private set; }
        public bool IncludeCharts { get; private set; }
        public bool IncludeDetails { get; private set; }
        public bool ColorCoding { get; private set; }

        public ReportParametersForm(string reportType)
        {
            this.reportType = reportType;
            InitializeComponent();
            SetupParametersForReportType();
        }

        private void InitializeComponent()
        {
            this.contentPanel = new Panel();
            this.dtpFromDate = new DateTimePicker();
            this.dtpToDate = new DateTimePicker();
            this.cmbFilterType = new ComboBox();
            this.cmbStatusFilter = new ComboBox();
            this.cmbStockFilter = new ComboBox();
            this.cmbCustomerFilter = new ComboBox();
            this.chkIncludeCharts = new CheckBox();
            this.chkIncludeDetails = new CheckBox();
            this.chkColorCoding = new CheckBox();
            this.btnGenerate = new Button();
            this.btnCancel = new Button();
            this.SuspendLayout();

            // Form Properties
            this.Text = $"Crystal Report Parameters - {reportType}";
            this.Size = new Size(500, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Header Panel
            Panel headerPanel = new Panel();
            headerPanel.Location = new Point(0, 0);
            headerPanel.Size = new Size(500, 80);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.BackColor = Color.FromArgb(102, 16, 242);
            headerPanel.Paint += new PaintEventHandler(this.headerPanel_Paint);

            Label lblTitle = new Label();
            lblTitle.Text = $"?? {reportType} Parameters";
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(30, 25);
            lblTitle.Size = new Size(400, 30);
            lblTitle.BackColor = Color.Transparent;

            headerPanel.Controls.Add(lblTitle);

            // Content Panel
            this.contentPanel.Location = new Point(20, 100);
            this.contentPanel.Size = new Size(460, 420);
            this.contentPanel.BackColor = Color.White;
            this.contentPanel.BorderStyle = BorderStyle.FixedSingle;

            // Date Range Section
            Label lblDateRange = new Label();
            lblDateRange.Text = "?? Date Range Selection";
            lblDateRange.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblDateRange.ForeColor = Color.FromArgb(52, 58, 64);
            lblDateRange.Location = new Point(20, 20);
            lblDateRange.Size = new Size(200, 25);

            Label lblFromDate = new Label();
            lblFromDate.Text = "From Date:";
            lblFromDate.Location = new Point(30, 55);
            lblFromDate.Size = new Size(80, 25);

            this.dtpFromDate.Location = new Point(115, 53);
            this.dtpFromDate.Size = new Size(150, 25);
            this.dtpFromDate.Value = DateTime.Now.AddMonths(-3);

            Label lblToDate = new Label();
            lblToDate.Text = "To Date:";
            lblToDate.Location = new Point(280, 55);
            lblToDate.Size = new Size(60, 25);

            this.dtpToDate.Location = new Point(345, 53);
            this.dtpToDate.Size = new Size(150, 25);
            this.dtpToDate.Value = DateTime.Now;

            this.contentPanel.Controls.Add(lblDateRange);
            this.contentPanel.Controls.Add(lblFromDate);
            this.contentPanel.Controls.Add(this.dtpFromDate);
            this.contentPanel.Controls.Add(lblToDate);
            this.contentPanel.Controls.Add(this.dtpToDate);

            // Filter Section (will be customized per report type)
            Label lblFilters = new Label();
            lblFilters.Text = "?? Filter Options";
            lblFilters.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblFilters.ForeColor = Color.FromArgb(52, 58, 64);
            lblFilters.Location = new Point(20, 100);
            lblFilters.Size = new Size(200, 25);
            this.contentPanel.Controls.Add(lblFilters);

            // Report Options Section
            Label lblOptions = new Label();
            lblOptions.Text = "?? Report Options";
            lblOptions.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblOptions.ForeColor = Color.FromArgb(52, 58, 64);
            lblOptions.Location = new Point(20, 240);
            lblOptions.Size = new Size(200, 25);

            this.chkIncludeCharts.Text = "Include Charts & Graphs";
            this.chkIncludeCharts.Location = new Point(30, 275);
            this.chkIncludeCharts.Size = new Size(180, 25);
            this.chkIncludeCharts.Checked = true;

            this.chkIncludeDetails.Text = "Include Detailed Data";
            this.chkIncludeDetails.Location = new Point(30, 305);
            this.chkIncludeDetails.Size = new Size(180, 25);
            this.chkIncludeDetails.Checked = true;

            this.chkColorCoding.Text = "Enable Color Coding";
            this.chkColorCoding.Location = new Point(30, 335);
            this.chkColorCoding.Size = new Size(180, 25);
            this.chkColorCoding.Checked = true;

            this.contentPanel.Controls.Add(lblOptions);
            this.contentPanel.Controls.Add(this.chkIncludeCharts);
            this.contentPanel.Controls.Add(this.chkIncludeDetails);
            this.contentPanel.Controls.Add(this.chkColorCoding);

            // Buttons
            this.btnGenerate.Text = "?? Generate Report";
            this.btnGenerate.Location = new Point(200, 380);
            this.btnGenerate.Size = new Size(130, 35);
            this.btnGenerate.BackColor = Color.FromArgb(40, 167, 69);
            this.btnGenerate.ForeColor = Color.White;
            this.btnGenerate.FlatStyle = FlatStyle.Flat;
            this.btnGenerate.FlatAppearance.BorderSize = 0;
            this.btnGenerate.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnGenerate.Cursor = Cursors.Hand;
            this.btnGenerate.Click += new EventHandler(this.btnGenerate_Click);

            this.btnCancel.Text = "Cancel";
            this.btnCancel.Location = new Point(340, 380);
            this.btnCancel.Size = new Size(80, 35);
            this.btnCancel.BackColor = Color.FromArgb(108, 117, 125);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnCancel.Cursor = Cursors.Hand;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

            this.contentPanel.Controls.Add(this.btnGenerate);
            this.contentPanel.Controls.Add(this.btnCancel);

            // Add main panels to form
            this.Controls.Add(headerPanel);
            this.Controls.Add(this.contentPanel);

            this.ResumeLayout(false);
        }

        private void SetupParametersForReportType()
        {
            int yPos = 135;

            switch (reportType)
            {
                case "Customer Report":
                    SetupCustomerReportParameters(yPos);
                    break;
                case "Job Report":
                    SetupJobReportParameters(yPos);
                    break;
                case "Load Report":
                    SetupLoadReportParameters(yPos);
                    break;
                case "Product Report":
                    SetupProductReportParameters(yPos);
                    break;
                default:
                    // Default parameters
                    break;
            }
        }

        private void SetupCustomerReportParameters(int yPos)
        {
            Label lblFilterType = new Label();
            lblFilterType.Text = "Customer Filter:";
            lblFilterType.Location = new Point(30, yPos);
            lblFilterType.Size = new Size(100, 25);

            this.cmbFilterType.Location = new Point(135, yPos - 2);
            this.cmbFilterType.Size = new Size(200, 25);
            this.cmbFilterType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbFilterType.Items.AddRange(new string[] { 
                "All Customers", 
                "Active (with jobs)", 
                "Inactive", 
                "Recent (30 days)",
                "High Value Customers"
            });
            this.cmbFilterType.SelectedIndex = 0;

            this.contentPanel.Controls.Add(lblFilterType);
            this.contentPanel.Controls.Add(this.cmbFilterType);
        }

        private void SetupJobReportParameters(int yPos)
        {
            Label lblStatusFilter = new Label();
            lblStatusFilter.Text = "Job Status:";
            lblStatusFilter.Location = new Point(30, yPos);
            lblStatusFilter.Size = new Size(80, 25);

            this.cmbStatusFilter.Location = new Point(115, yPos - 2);
            this.cmbStatusFilter.Size = new Size(150, 25);
            this.cmbStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbStatusFilter.Items.AddRange(new string[] { 
                "All Status", 
                "Pending", 
                "Approved", 
                "In Progress", 
                "Completed", 
                "Cancelled" 
            });
            this.cmbStatusFilter.SelectedIndex = 0;

            Label lblCustomer = new Label();
            lblCustomer.Text = "Customer:";
            lblCustomer.Location = new Point(280, yPos);
            lblCustomer.Size = new Size(70, 25);

            this.cmbCustomerFilter.Location = new Point(355, yPos - 2);
            this.cmbCustomerFilter.Size = new Size(140, 25);
            this.cmbCustomerFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            LoadCustomers();

            this.contentPanel.Controls.Add(lblStatusFilter);
            this.contentPanel.Controls.Add(this.cmbStatusFilter);
            this.contentPanel.Controls.Add(lblCustomer);
            this.contentPanel.Controls.Add(this.cmbCustomerFilter);
        }

        private void SetupLoadReportParameters(int yPos)
        {
            Label lblStatusFilter = new Label();
            lblStatusFilter.Text = "Load Status:";
            lblStatusFilter.Location = new Point(30, yPos);
            lblStatusFilter.Size = new Size(80, 25);

            this.cmbStatusFilter.Location = new Point(115, yPos - 2);
            this.cmbStatusFilter.Size = new Size(150, 25);
            this.cmbStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbStatusFilter.Items.AddRange(new string[] { 
                "All Status", 
                "In Progress", 
                "Completed", 
                "Cancelled" 
            });
            this.cmbStatusFilter.SelectedIndex = 0;

            this.contentPanel.Controls.Add(lblStatusFilter);
            this.contentPanel.Controls.Add(this.cmbStatusFilter);
        }

        private void SetupProductReportParameters(int yPos)
        {
            Label lblStockFilter = new Label();
            lblStockFilter.Text = "Stock Filter:";
            lblStockFilter.Location = new Point(30, yPos);
            lblStockFilter.Size = new Size(80, 25);

            this.cmbStockFilter.Location = new Point(115, yPos - 2);
            this.cmbStockFilter.Size = new Size(200, 25);
            this.cmbStockFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbStockFilter.Items.AddRange(new string[] { 
                "All Stock Levels", 
                "In Stock (>0)", 
                "Low Stock (<10)", 
                "Out of Stock (0)",
                "Overstocked"
            });
            this.cmbStockFilter.SelectedIndex = 0;

            // Remove date controls for product report since it doesn't need them
            dtpFromDate.Visible = false;
            dtpToDate.Visible = false;
            contentPanel.Controls.Cast<Control>().Where(c => c.Text.Contains("Date")).ToList().ForEach(c => c.Visible = false);

            this.contentPanel.Controls.Add(lblStockFilter);
            this.contentPanel.Controls.Add(this.cmbStockFilter);
        }

        private void LoadCustomers()
        {
            try
            {
                DatabaseConnection db = new DatabaseConnection();
                string query = "SELECT CustomerID, Name FROM Customer ORDER BY Name";
                var customerData = db.ExecuteSelect(query);

                // Add "All Customers" option
                var allCustomersRow = customerData.NewRow();
                allCustomersRow["CustomerID"] = 0;
                allCustomersRow["Name"] = "All Customers";
                customerData.Rows.InsertAt(allCustomersRow, 0);

                cmbCustomerFilter.DisplayMember = "Name";
                cmbCustomerFilter.ValueMember = "CustomerID";
                cmbCustomerFilter.DataSource = customerData;
                cmbCustomerFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                // Handle error gracefully
                cmbCustomerFilter.Items.Add("All Customers");
                cmbCustomerFilter.SelectedIndex = 0;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate date range
                if (dtpFromDate.Visible && dtpToDate.Visible && dtpFromDate.Value > dtpToDate.Value)
                {
                    MessageBox.Show("From Date cannot be later than To Date.", "Invalid Date Range", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Set properties based on selections
                FromDate = dtpFromDate.Value.Date;
                ToDate = dtpToDate.Value.Date;
                FilterType = cmbFilterType.SelectedItem?.ToString() ?? "All";
                StatusFilter = cmbStatusFilter.SelectedItem?.ToString() ?? "All Status";
                StockFilter = cmbStockFilter.SelectedItem?.ToString() ?? "All Stock Levels";
                SelectedCustomerId = cmbCustomerFilter.SelectedValue != null ? 
                    Convert.ToInt32(cmbCustomerFilter.SelectedValue) : 0;
                IncludeCharts = chkIncludeCharts.Checked;
                IncludeDetails = chkIncludeDetails.Checked;
                ColorCoding = chkColorCoding.Checked;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing parameters: {ex.Message}", "Parameter Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void headerPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            using (LinearGradientBrush brush = new LinearGradientBrush(
                panel.ClientRectangle,
                Color.FromArgb(102, 16, 242),
                Color.FromArgb(52, 58, 64),
                LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, panel.ClientRectangle);
            }
        }
    }
}