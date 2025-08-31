using System.Data;
using MySql.Data.MySqlClient;
using System.Drawing.Drawing2D;

namespace Transport
{
    public partial class ExportOptionsForm : Form
    {
        private DatabaseConnection dbConnection;
        private ComboBox cmbReportType;
        private ComboBox cmbExportFormat;
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;
        private CheckedListBox clbColumns;
        private Button btnExport;
        private Button btnPreview;
        private ProgressBar progressBar;
        private Label lblStatus;

        public ExportOptionsForm()
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            LoadExportOptions();
        }

        private void InitializeComponent()
        {
            this.cmbReportType = new ComboBox();
            this.cmbExportFormat = new ComboBox();
            this.dtpFromDate = new DateTimePicker();
            this.dtpToDate = new DateTimePicker();
            this.clbColumns = new CheckedListBox();
            this.btnExport = new Button();
            this.btnPreview = new Button();
            this.progressBar = new ProgressBar();
            this.lblStatus = new Label();
            this.SuspendLayout();

            // Form Properties
            this.Text = "Export Options - Transport Management System";
            this.Size = new Size(800, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Header Panel
            Panel headerPanel = new Panel();
            headerPanel.Location = new Point(0, 0);
            headerPanel.Size = new Size(800, 80);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.BackColor = Color.FromArgb(108, 117, 125);

            Label lblTitle = new Label();
            lblTitle.Text = "?? Export Data Options";
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(30, 25);
            lblTitle.Size = new Size(400, 30);
            lblTitle.BackColor = Color.Transparent;

            Button btnClose = new Button();
            btnClose.Text = "?";
            btnClose.Location = new Point(750, 10);
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

            // Main Content Panel
            Panel contentPanel = new Panel();
            contentPanel.Location = new Point(20, 100);
            contentPanel.Size = new Size(760, 550);
            contentPanel.BackColor = Color.White;
            contentPanel.BorderStyle = BorderStyle.FixedSingle;

            // Report Type Selection
            Label lblReportType = new Label();
            lblReportType.Text = "Select Report Type:";
            lblReportType.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblReportType.Location = new Point(30, 30);
            lblReportType.Size = new Size(200, 25);

            this.cmbReportType.Location = new Point(30, 60);
            this.cmbReportType.Size = new Size(300, 25);
            this.cmbReportType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbReportType.Items.AddRange(new string[] { 
                "Customer Report", 
                "Job Report", 
                "Load Report", 
                "Product Report",
                "Complete System Export" 
            });
            this.cmbReportType.SelectedIndex = 0;
            this.cmbReportType.SelectedIndexChanged += new EventHandler(this.cmbReportType_SelectedIndexChanged);

            // Export Format Selection
            Label lblExportFormat = new Label();
            lblExportFormat.Text = "Export Format:";
            lblExportFormat.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblExportFormat.Location = new Point(400, 30);
            lblExportFormat.Size = new Size(150, 25);

            this.cmbExportFormat.Location = new Point(400, 60);
            this.cmbExportFormat.Size = new Size(200, 25);
            this.cmbExportFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbExportFormat.Items.AddRange(new string[] { 
                "CSV (Comma Separated)", 
                "Excel (XLSX)",
                "PDF Report",
                "JSON Data",
                "XML Data"
            });
            this.cmbExportFormat.SelectedIndex = 0;

            // Date Range Selection
            Label lblDateRange = new Label();
            lblDateRange.Text = "Date Range:";
            lblDateRange.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblDateRange.Location = new Point(30, 120);
            lblDateRange.Size = new Size(150, 25);

            Label lblFromDate = new Label();
            lblFromDate.Text = "From:";
            lblFromDate.Location = new Point(30, 150);
            lblFromDate.Size = new Size(50, 25);

            this.dtpFromDate.Location = new Point(85, 148);
            this.dtpFromDate.Size = new Size(200, 25);
            this.dtpFromDate.Value = DateTime.Now.AddMonths(-6);

            Label lblToDate = new Label();
            lblToDate.Text = "To:";
            lblToDate.Location = new Point(310, 150);
            lblToDate.Size = new Size(30, 25);

            this.dtpToDate.Location = new Point(345, 148);
            this.dtpToDate.Size = new Size(200, 25);
            this.dtpToDate.Value = DateTime.Now;

            // Column Selection
            Label lblColumns = new Label();
            lblColumns.Text = "Select Columns to Export:";
            lblColumns.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblColumns.Location = new Point(30, 200);
            lblColumns.Size = new Size(250, 25);

            this.clbColumns.Location = new Point(30, 230);
            this.clbColumns.Size = new Size(700, 180);
            this.clbColumns.CheckOnClick = true;
            this.clbColumns.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            // Progress Bar
            this.progressBar.Location = new Point(30, 430);
            this.progressBar.Size = new Size(500, 25);
            this.progressBar.Visible = false;

            // Status Label
            this.lblStatus.Text = "Ready to export";
            this.lblStatus.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.lblStatus.ForeColor = Color.FromArgb(108, 117, 125);
            this.lblStatus.Location = new Point(30, 465);
            this.lblStatus.Size = new Size(500, 25);

            // Buttons
            this.btnPreview.Text = "Preview Data";
            this.btnPreview.Location = new Point(550, 430);
            this.btnPreview.Size = new Size(100, 35);
            this.btnPreview.BackColor = Color.FromArgb(0, 123, 255);
            this.btnPreview.ForeColor = Color.White;
            this.btnPreview.FlatStyle = FlatStyle.Flat;
            this.btnPreview.FlatAppearance.BorderSize = 0;
            this.btnPreview.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnPreview.Cursor = Cursors.Hand;
            this.btnPreview.Click += new EventHandler(this.btnPreview_Click);

            this.btnExport.Text = "Export Now";
            this.btnExport.Location = new Point(660, 430);
            this.btnExport.Size = new Size(100, 35);
            this.btnExport.BackColor = Color.FromArgb(40, 167, 69);
            this.btnExport.ForeColor = Color.White;
            this.btnExport.FlatStyle = FlatStyle.Flat;
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnExport.Cursor = Cursors.Hand;
            this.btnExport.Click += new EventHandler(this.btnExport_Click);

            // Additional Options Panel
            Panel optionsPanel = new Panel();
            optionsPanel.Location = new Point(30, 500);
            optionsPanel.Size = new Size(700, 40);
            optionsPanel.BackColor = Color.FromArgb(248, 249, 250);

            CheckBox chkIncludeHeaders = new CheckBox();
            chkIncludeHeaders.Text = "Include column headers";
            chkIncludeHeaders.Location = new Point(10, 10);
            chkIncludeHeaders.Size = new Size(180, 20);
            chkIncludeHeaders.Checked = true;

            CheckBox chkOpenAfterExport = new CheckBox();
            chkOpenAfterExport.Text = "Open file after export";
            chkOpenAfterExport.Location = new Point(200, 10);
            chkOpenAfterExport.Size = new Size(180, 20);
            chkOpenAfterExport.Checked = true;

            CheckBox chkCreateBackup = new CheckBox();
            chkCreateBackup.Text = "Create backup copy";
            chkCreateBackup.Location = new Point(390, 10);
            chkCreateBackup.Size = new Size(150, 20);

            optionsPanel.Controls.Add(chkIncludeHeaders);
            optionsPanel.Controls.Add(chkOpenAfterExport);
            optionsPanel.Controls.Add(chkCreateBackup);

            contentPanel.Controls.Add(lblReportType);
            contentPanel.Controls.Add(this.cmbReportType);
            contentPanel.Controls.Add(lblExportFormat);
            contentPanel.Controls.Add(this.cmbExportFormat);
            contentPanel.Controls.Add(lblDateRange);
            contentPanel.Controls.Add(lblFromDate);
            contentPanel.Controls.Add(this.dtpFromDate);
            contentPanel.Controls.Add(lblToDate);
            contentPanel.Controls.Add(this.dtpToDate);
            contentPanel.Controls.Add(lblColumns);
            contentPanel.Controls.Add(this.clbColumns);
            contentPanel.Controls.Add(this.progressBar);
            contentPanel.Controls.Add(this.lblStatus);
            contentPanel.Controls.Add(this.btnPreview);
            contentPanel.Controls.Add(this.btnExport);
            contentPanel.Controls.Add(optionsPanel);

            // Add controls to form
            this.Controls.Add(headerPanel);
            this.Controls.Add(contentPanel);

            this.ResumeLayout(false);
        }

        private void LoadExportOptions()
        {
            LoadColumnOptions();
        }

        private void LoadColumnOptions()
        {
            // Load default columns based on selected report type
            string selectedReport = cmbReportType.SelectedItem?.ToString() ?? "Customer Report";
            clbColumns.Items.Clear();

            switch (selectedReport)
            {
                case "Customer Report":
                    clbColumns.Items.Add("Customer ID", true);
                    clbColumns.Items.Add("Customer Name", true);
                    clbColumns.Items.Add("Email", true);
                    clbColumns.Items.Add("Phone", true);
                    clbColumns.Items.Add("Address", false);
                    clbColumns.Items.Add("Registration Date", true);
                    clbColumns.Items.Add("Total Jobs", true);
                    clbColumns.Items.Add("Status", true);
                    break;

                case "Job Report":
                    clbColumns.Items.Add("Job ID", true);
                    clbColumns.Items.Add("Customer Name", true);
                    clbColumns.Items.Add("Start Location", true);
                    clbColumns.Items.Add("Destination", true);
                    clbColumns.Items.Add("Status", true);
                    clbColumns.Items.Add("Request Date", true);
                    clbColumns.Items.Add("Estimated Weight", false);
                    clbColumns.Items.Add("Preferred Date", false);
                    clbColumns.Items.Add("Urgency", false);
                    clbColumns.Items.Add("Assigned Truck", true);
                    clbColumns.Items.Add("Assigned Driver", true);
                    break;

                case "Load Report":
                    clbColumns.Items.Add("Load ID", true);
                    clbColumns.Items.Add("Job ID", true);
                    clbColumns.Items.Add("Customer Name", true);
                    clbColumns.Items.Add("Truck Number", true);
                    clbColumns.Items.Add("Driver Name", true);
                    clbColumns.Items.Add("Assistant Name", false);
                    clbColumns.Items.Add("Container Number", false);
                    clbColumns.Items.Add("Load Status", true);
                    clbColumns.Items.Add("Estimated Weight", false);
                    break;

                case "Product Report":
                    clbColumns.Items.Add("Product ID", true);
                    clbColumns.Items.Add("Product Name", true);
                    clbColumns.Items.Add("Description", false);
                    clbColumns.Items.Add("Price", true);
                    clbColumns.Items.Add("Stock", true);
                    clbColumns.Items.Add("Total Value", true);
                    clbColumns.Items.Add("Stock Status", true);
                    clbColumns.Items.Add("Price Category", false);
                    break;

                case "Complete System Export":
                    clbColumns.Items.Add("All Customer Data", true);
                    clbColumns.Items.Add("All Job Data", true);
                    clbColumns.Items.Add("All Load Data", true);
                    clbColumns.Items.Add("All Product Data", true);
                    clbColumns.Items.Add("System Statistics", false);
                    break;
            }
        }

        private void cmbReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadColumnOptions();
            lblStatus.Text = $"Ready to export {cmbReportType.SelectedItem}";
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "Loading preview...";
                Application.DoEvents();

                string selectedReport = cmbReportType.SelectedItem.ToString();
                DataTable previewData = GetReportData(selectedReport, true); // Limited preview

                if (previewData.Rows.Count > 0)
                {
                    ShowPreviewWindow(previewData, selectedReport);
                    lblStatus.Text = $"Preview loaded - {previewData.Rows.Count} records";
                }
                else
                {
                    lblStatus.Text = "No data found for the selected criteria";
                    MessageBox.Show("No data found for the selected criteria.", "Preview", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error loading preview";
                MessageBox.Show($"Error loading preview: {ex.Message}", "Preview Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (clbColumns.CheckedItems.Count == 0)
                {
                    MessageBox.Show("Please select at least one column to export.", "Export Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog();
                string selectedFormat = cmbExportFormat.SelectedItem.ToString();
                
                switch (selectedFormat)
                {
                    case "CSV (Comma Separated)":
                        saveDialog.Filter = "CSV files (*.csv)|*.csv";
                        saveDialog.DefaultExt = "csv";
                        break;
                    case "Excel (XLSX)":
                        saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                        saveDialog.DefaultExt = "xlsx";
                        break;
                    case "PDF Report":
                        saveDialog.Filter = "PDF files (*.pdf)|*.pdf";
                        saveDialog.DefaultExt = "pdf";
                        break;
                    case "JSON Data":
                        saveDialog.Filter = "JSON files (*.json)|*.json";
                        saveDialog.DefaultExt = "json";
                        break;
                    case "XML Data":
                        saveDialog.Filter = "XML files (*.xml)|*.xml";
                        saveDialog.DefaultExt = "xml";
                        break;
                }

                string reportType = cmbReportType.SelectedItem.ToString().Replace(" ", "");
                saveDialog.FileName = $"{reportType}_{DateTime.Now:yyyyMMdd}";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    PerformExport(saveDialog.FileName, selectedFormat);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Export failed";
                MessageBox.Show($"Error during export: {ex.Message}", "Export Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PerformExport(string fileName, string format)
        {
            progressBar.Visible = true;
            progressBar.Value = 0;
            lblStatus.Text = "Starting export...";
            Application.DoEvents();

            try
            {
                string selectedReport = cmbReportType.SelectedItem.ToString();
                DataTable exportData = GetReportData(selectedReport, false);

                progressBar.Value = 30;
                lblStatus.Text = "Processing data...";
                Application.DoEvents();

                // Filter columns based on selection
                DataTable filteredData = FilterSelectedColumns(exportData);

                progressBar.Value = 60;
                lblStatus.Text = "Generating export file...";
                Application.DoEvents();

                switch (format)
                {
                    case "CSV (Comma Separated)":
                        ExportToCsv(filteredData, fileName);
                        break;
                    case "Excel (XLSX)":
                        ExportToExcel(filteredData, fileName);
                        break;
                    case "PDF Report":
                        ExportToPdf(filteredData, fileName);
                        break;
                    case "JSON Data":
                        ExportToJson(filteredData, fileName);
                        break;
                    case "XML Data":
                        ExportToXml(filteredData, fileName);
                        break;
                }

                progressBar.Value = 100;
                lblStatus.Text = $"Export completed - {filteredData.Rows.Count} records exported";

                MessageBox.Show($"Export completed successfully!\n\nFile: {fileName}\nRecords: {filteredData.Rows.Count}", 
                    "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Option to open file
                DialogResult openResult = MessageBox.Show("Would you like to open the exported file?", 
                    "Open File", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (openResult == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = fileName,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Export failed: {ex.Message}");
            }
            finally
            {
                progressBar.Visible = false;
            }
        }

        private DataTable GetReportData(string reportType, bool isPreview)
        {
            string query = "";
            string limitClause = isPreview ? " LIMIT 10" : "";

            switch (reportType)
            {
                case "Customer Report":
                    query = @"
                        SELECT 
                            c.CustomerID,
                            c.Name as CustomerName,
                            c.Email,
                            c.Phone,
                            c.Address,
                            c.RegisteredDate,
                            COUNT(j.JobID) as TotalJobs,
                            CASE 
                                WHEN COUNT(j.JobID) > 0 THEN 'Active'
                                ELSE 'Inactive'
                            END as Status
                        FROM Customer c
                        LEFT JOIN Job j ON c.CustomerID = j.CustomerID
                        WHERE c.RegisteredDate BETWEEN @fromDate AND @toDate
                        GROUP BY c.CustomerID, c.Name, c.Email, c.Phone, c.Address, c.RegisteredDate
                        ORDER BY c.RegisteredDate DESC" + limitClause;
                    break;

                case "Job Report":
                    query = @"
                        SELECT 
                            j.JobID,
                            c.Name as CustomerName,
                            j.StartLocation,
                            j.Destination,
                            j.Status,
                            j.RequestDate,
                            j.EstimatedWeight,
                            j.PreferredDate,
                            j.Urgency,
                            COALESCE(ld.Lorry, 'Not Assigned') as AssignedTruck,
                            COALESCE(ld.Driver, 'Not Assigned') as AssignedDriver
                        FROM Job j
                        INNER JOIN Customer c ON j.CustomerID = c.CustomerID
                        LEFT JOIN LoadDetails ld ON j.JobID = ld.JobID
                        WHERE j.RequestDate BETWEEN @fromDate AND @toDate
                        ORDER BY j.RequestDate DESC" + limitClause;
                    break;

                case "Load Report":
                    query = @"
                        SELECT 
                            ld.LoadID,
                            j.JobID,
                            c.Name as CustomerName,
                            ld.Lorry as TruckNumber,
                            ld.Driver as DriverName,
                            ld.Assistant as AssistantName,
                            ld.Container as ContainerNumber,
                            j.Status as LoadStatus,
                            j.EstimatedWeight
                        FROM LoadDetails ld
                        INNER JOIN Job j ON ld.JobID = j.JobID
                        INNER JOIN Customer c ON j.CustomerID = c.CustomerID
                        WHERE j.RequestDate BETWEEN @fromDate AND @toDate
                        ORDER BY j.RequestDate DESC" + limitClause;
                    break;

                case "Product Report":
                    query = @"
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
                        ORDER BY ProductID" + limitClause;
                    break;

                default:
                    throw new ArgumentException("Invalid report type selected");
            }

            // Only use parameters for queries that need them
            if (reportType == "Product Report")
            {
                return dbConnection.ExecuteSelect(query);
            }
            else
            {
                MySqlParameter[] parameters = {
                    new MySqlParameter("@fromDate", dtpFromDate.Value.Date),
                    new MySqlParameter("@toDate", dtpToDate.Value.Date)
                };
                return dbConnection.ExecuteSelect(query, parameters);
            }
        }

        private DataTable FilterSelectedColumns(DataTable originalData)
        {
            DataTable filteredData = new DataTable();

            // Add selected columns
            foreach (object checkedItem in clbColumns.CheckedItems)
            {
                string columnName = checkedItem.ToString();
                
                // Map display names to actual column names
                string actualColumnName = GetActualColumnName(columnName);
                
                if (originalData.Columns.Contains(actualColumnName))
                {
                    filteredData.Columns.Add(columnName, originalData.Columns[actualColumnName].DataType);
                }
            }

            // Copy data for selected columns
            foreach (DataRow row in originalData.Rows)
            {
                DataRow newRow = filteredData.NewRow();
                
                foreach (object checkedItem in clbColumns.CheckedItems)
                {
                    string columnName = checkedItem.ToString();
                    string actualColumnName = GetActualColumnName(columnName);
                    
                    if (originalData.Columns.Contains(actualColumnName))
                    {
                        newRow[columnName] = row[actualColumnName];
                    }
                }
                
                filteredData.Rows.Add(newRow);
            }

            return filteredData;
        }

        private string GetActualColumnName(string displayName)
        {
            // Map display names to actual database column names
            var columnMappings = new Dictionary<string, string>
            {
                {"Customer ID", "CustomerID"},
                {"Customer Name", "CustomerName"},
                {"Email", "Email"},
                {"Phone", "Phone"},
                {"Address", "Address"},
                {"Registration Date", "RegisteredDate"},
                {"Total Jobs", "TotalJobs"},
                {"Status", "Status"},
                {"Job ID", "JobID"},
                {"Start Location", "StartLocation"},
                {"Destination", "Destination"},
                {"Request Date", "RequestDate"},
                {"Estimated Weight", "EstimatedWeight"},
                {"Preferred Date", "PreferredDate"},
                {"Urgency", "Urgency"},
                {"Assigned Truck", "AssignedTruck"},
                {"Assigned Driver", "AssignedDriver"},
                {"Load ID", "LoadID"},
                {"Truck Number", "TruckNumber"},
                {"Driver Name", "DriverName"},
                {"Assistant Name", "AssistantName"},
                {"Container Number", "ContainerNumber"},
                {"Load Status", "LoadStatus"},
                {"Product ID", "ProductID"},
                {"Product Name", "ProductName"},
                {"Description", "Description"},
                {"Price", "Price"},
                {"Stock", "Stock"},
                {"Total Value", "TotalValue"},
                {"Stock Status", "StockStatus"},
                {"Price Category", "PriceCategory"}
            };

            return columnMappings.ContainsKey(displayName) ? columnMappings[displayName] : displayName;
        }

        private void ShowPreviewWindow(DataTable data, string reportTitle)
        {
            Form previewForm = new Form();
            previewForm.Text = $"Preview: {reportTitle}";
            previewForm.Size = new Size(800, 600);
            previewForm.StartPosition = FormStartPosition.CenterParent;

            DataGridView previewGrid = new DataGridView();
            previewGrid.Dock = DockStyle.Fill;
            previewGrid.DataSource = data;
            previewGrid.ReadOnly = true;
            previewGrid.AllowUserToAddRows = false;
            previewGrid.AllowUserToDeleteRows = false;
            previewGrid.AutoResizeColumns();

            previewForm.Controls.Add(previewGrid);
            previewForm.ShowDialog(this);
        }

        private void ExportToCsv(DataTable data, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                // Write headers
                string[] headers = new string[data.Columns.Count];
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    headers[i] = data.Columns[i].ColumnName;
                }
                writer.WriteLine(string.Join(",", headers));

                // Write data
                foreach (DataRow row in data.Rows)
                {
                    string[] values = new string[data.Columns.Count];
                    for (int i = 0; i < data.Columns.Count; i++)
                    {
                        values[i] = row[i]?.ToString() ?? "";
                    }
                    writer.WriteLine(string.Join(",", values));
                }
            }
        }

        private void ExportToExcel(DataTable data, string fileName)
        {
            // For now, export as CSV with .xlsx extension
            // In a real implementation, you would use a library like EPPlus or ClosedXML
            MessageBox.Show("Excel export would be implemented using EPPlus or ClosedXML library.\nExporting as CSV for now.", 
                "Excel Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ExportToCsv(data, fileName.Replace(".xlsx", ".csv"));
        }

        private void ExportToPdf(DataTable data, string fileName)
        {
            MessageBox.Show("PDF export would be implemented using iTextSharp or similar library.\nExporting as CSV for now.", 
                "PDF Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ExportToCsv(data, fileName.Replace(".pdf", ".csv"));
        }

        private void ExportToJson(DataTable data, string fileName)
        {
            // Simple JSON export
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine("[");
                
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    writer.WriteLine("  {");
                    
                    for (int j = 0; j < data.Columns.Count; j++)
                    {
                        string value = data.Rows[i][j]?.ToString() ?? "";
                        writer.Write($"    \"{data.Columns[j].ColumnName}\": \"{value}\"");
                        
                        if (j < data.Columns.Count - 1)
                            writer.WriteLine(",");
                        else
                            writer.WriteLine();
                    }
                    
                    if (i < data.Rows.Count - 1)
                        writer.WriteLine("  },");
                    else
                        writer.WriteLine("  }");
                }
                
                writer.WriteLine("]");
            }
        }

        private void ExportToXml(DataTable data, string fileName)
        {
            data.TableName = "Records";
            data.WriteXml(fileName);
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