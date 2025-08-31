using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Windows.Forms;
using CrystalDecisions.Shared;
using System.Drawing.Drawing2D;

namespace Transport
{
    public partial class CrystalReportViewerForm : Form
    {
        private CrystalReportViewer crystalReportViewer;
        private Panel headerPanel;
        private Panel toolbarPanel;
        private Button btnExportPdf;
        private Button btnExportExcel;
        private Button btnExportWord;
        private Button btnPrint;
        private Button btnClose;
        private Label lblReportTitle;
        private ReportDocument currentReport;
        private CrystalReportsService reportsService;

        public CrystalReportViewerForm()
        {
            InitializeComponent();
            reportsService = new CrystalReportsService();
        }

        public CrystalReportViewerForm(ReportDocument report, string reportTitle) : this()
        {
            LoadReport(report, reportTitle);
        }

        private void InitializeComponent()
        {
            this.crystalReportViewer = new CrystalReportViewer();
            this.headerPanel = new Panel();
            this.toolbarPanel = new Panel();
            this.btnExportPdf = new Button();
            this.btnExportExcel = new Button();
            this.btnExportWord = new Button();
            this.btnPrint = new Button();
            this.btnClose = new Button();
            this.lblReportTitle = new Label();
            this.SuspendLayout();

            // Form Properties
            this.Text = "Crystal Reports Viewer - Transport Management System";
            this.Size = new Size(1200, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.MinimumSize = new Size(800, 600);

            // Header Panel
            this.headerPanel.Location = new Point(0, 0);
            this.headerPanel.Size = new Size(1200, 80);
            this.headerPanel.Dock = DockStyle.Top;
            this.headerPanel.BackColor = Color.FromArgb(52, 58, 64);
            this.headerPanel.Paint += new PaintEventHandler(this.headerPanel_Paint);

            // Report Title
            this.lblReportTitle.Text = "Crystal Report Viewer";
            this.lblReportTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            this.lblReportTitle.ForeColor = Color.White;
            this.lblReportTitle.Location = new Point(30, 25);
            this.lblReportTitle.Size = new Size(600, 30);
            this.lblReportTitle.BackColor = Color.Transparent;

            // Close Button
            this.btnClose.Text = "?";
            this.btnClose.Location = new Point(1150, 10);
            this.btnClose.Size = new Size(40, 40);
            this.btnClose.BackColor = Color.FromArgb(220, 53, 69);
            this.btnClose.ForeColor = Color.White;
            this.btnClose.FlatStyle = FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.btnClose.Cursor = Cursors.Hand;
            this.btnClose.Click += (s, e) => this.Close();

            this.headerPanel.Controls.Add(this.lblReportTitle);
            this.headerPanel.Controls.Add(this.btnClose);

            // Toolbar Panel
            this.toolbarPanel.Location = new Point(0, 80);
            this.toolbarPanel.Size = new Size(1200, 50);
            this.toolbarPanel.Dock = DockStyle.Top;
            this.toolbarPanel.BackColor = Color.FromArgb(248, 249, 250);
            this.toolbarPanel.BorderStyle = BorderStyle.FixedSingle;

            // Export to PDF Button
            this.btnExportPdf.Text = "?? Export PDF";
            this.btnExportPdf.Location = new Point(20, 10);
            this.btnExportPdf.Size = new Size(120, 30);
            this.btnExportPdf.BackColor = Color.FromArgb(220, 53, 69);
            this.btnExportPdf.ForeColor = Color.White;
            this.btnExportPdf.FlatStyle = FlatStyle.Flat;
            this.btnExportPdf.FlatAppearance.BorderSize = 0;
            this.btnExportPdf.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnExportPdf.Cursor = Cursors.Hand;
            this.btnExportPdf.Click += new EventHandler(this.btnExportPdf_Click);

            // Export to Excel Button
            this.btnExportExcel.Text = "?? Export Excel";
            this.btnExportExcel.Location = new Point(150, 10);
            this.btnExportExcel.Size = new Size(120, 30);
            this.btnExportExcel.BackColor = Color.FromArgb(40, 167, 69);
            this.btnExportExcel.ForeColor = Color.White;
            this.btnExportExcel.FlatStyle = FlatStyle.Flat;
            this.btnExportExcel.FlatAppearance.BorderSize = 0;
            this.btnExportExcel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnExportExcel.Cursor = Cursors.Hand;
            this.btnExportExcel.Click += new EventHandler(this.btnExportExcel_Click);

            // Export to Word Button
            this.btnExportWord.Text = "?? Export Word";
            this.btnExportWord.Location = new Point(280, 10);
            this.btnExportWord.Size = new Size(120, 30);
            this.btnExportWord.BackColor = Color.FromArgb(0, 123, 255);
            this.btnExportWord.ForeColor = Color.White;
            this.btnExportWord.FlatStyle = FlatStyle.Flat;
            this.btnExportWord.FlatAppearance.BorderSize = 0;
            this.btnExportWord.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnExportWord.Cursor = Cursors.Hand;
            this.btnExportWord.Click += new EventHandler(this.btnExportWord_Click);

            // Print Button
            this.btnPrint.Text = "??? Print";
            this.btnPrint.Location = new Point(410, 10);
            this.btnPrint.Size = new Size(100, 30);
            this.btnPrint.BackColor = Color.FromArgb(108, 117, 125);
            this.btnPrint.ForeColor = Color.White;
            this.btnPrint.FlatStyle = FlatStyle.Flat;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnPrint.Cursor = Cursors.Hand;
            this.btnPrint.Click += new EventHandler(this.btnPrint_Click);

            this.toolbarPanel.Controls.Add(this.btnExportPdf);
            this.toolbarPanel.Controls.Add(this.btnExportExcel);
            this.toolbarPanel.Controls.Add(this.btnExportWord);
            this.toolbarPanel.Controls.Add(this.btnPrint);

            // Crystal Report Viewer
            this.crystalReportViewer.Location = new Point(0, 130);
            this.crystalReportViewer.Size = new Size(1200, 770);
            this.crystalReportViewer.Dock = DockStyle.Fill;
            this.crystalReportViewer.ToolPanelView = ToolPanelViewType.None;
            this.crystalReportViewer.ShowGroupTreeButton = false;
            this.crystalReportViewer.ShowParameterPanelButton = false;
            this.crystalReportViewer.ShowRefreshButton = true;
            this.crystalReportViewer.ShowCloseButton = false;
            this.crystalReportViewer.ShowExportButton = false;
            this.crystalReportViewer.ShowPrintButton = false;
            this.crystalReportViewer.ShowZoomButton = true;
            this.crystalReportViewer.ShowPageNavigateButtons = true;
            this.crystalReportViewer.ShowTextSearchButton = true;

            // Add controls to form
            this.Controls.Add(this.crystalReportViewer);
            this.Controls.Add(this.toolbarPanel);
            this.Controls.Add(this.headerPanel);

            this.ResumeLayout(false);
        }

        public void LoadReport(ReportDocument report, string reportTitle)
        {
            try
            {
                currentReport = report;
                lblReportTitle.Text = reportTitle;
                this.Text = $"{reportTitle} - Crystal Reports Viewer";

                crystalReportViewer.ReportSource = report;
                crystalReportViewer.Refresh();

                // Enable export buttons
                btnExportPdf.Enabled = true;
                btnExportExcel.Enabled = true;
                btnExportWord.Enabled = true;
                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading report: {ex.Message}", "Crystal Reports Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadCustomerReport(DateTime fromDate, DateTime toDate, string filterType = "All")
        {
            try
            {
                var report = reportsService.GenerateCustomerReport(fromDate, toDate, filterType);
                LoadReport(report, "Customer Report");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating Customer Report: {ex.Message}", "Report Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadJobReport(DateTime fromDate, DateTime toDate, string statusFilter = "All", int customerId = 0)
        {
            try
            {
                var report = reportsService.GenerateJobReport(fromDate, toDate, statusFilter, customerId);
                LoadReport(report, "Job Report");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating Job Report: {ex.Message}", "Report Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadLoadReport(DateTime fromDate, DateTime toDate, string statusFilter = "All", string truckFilter = "All")
        {
            try
            {
                var report = reportsService.GenerateLoadReport(fromDate, toDate, statusFilter, truckFilter);
                LoadReport(report, "Load Report");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating Load Report: {ex.Message}", "Report Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadProductReport(string stockFilter = "All", decimal minPrice = 0, decimal maxPrice = 0)
        {
            try
            {
                var report = reportsService.GenerateProductReport(stockFilter, minPrice, maxPrice);
                LoadReport(report, "Product Report");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating Product Report: {ex.Message}", "Report Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadDashboardReport()
        {
            try
            {
                var report = reportsService.GenerateDashboardReport();
                LoadReport(report, "Dashboard Summary Report");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating Dashboard Report: {ex.Message}", "Report Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            if (currentReport == null) return;

            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveDialog.FileName = $"{lblReportTitle.Text.Replace(" ", "")}_{DateTime.Now:yyyyMMdd}.pdf";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    reportsService.ExportToPdf(currentReport, saveDialog.FileName);
                    MessageBox.Show("Report exported to PDF successfully!", "Export Complete", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Ask if user wants to open the file
                    if (MessageBox.Show("Would you like to open the exported file?", "Open File", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                        {
                            FileName = saveDialog.FileName,
                            UseShellExecute = true
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to PDF: {ex.Message}", "Export Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (currentReport == null) return;

            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel files (*.xls)|*.xls";
                saveDialog.FileName = $"{lblReportTitle.Text.Replace(" ", "")}_{DateTime.Now:yyyyMMdd}.xls";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    reportsService.ExportToExcel(currentReport, saveDialog.FileName);
                    MessageBox.Show("Report exported to Excel successfully!", "Export Complete", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (MessageBox.Show("Would you like to open the exported file?", "Open File", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                        {
                            FileName = saveDialog.FileName,
                            UseShellExecute = true
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to Excel: {ex.Message}", "Export Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportWord_Click(object sender, EventArgs e)
        {
            if (currentReport == null) return;

            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Word files (*.doc)|*.doc";
                saveDialog.FileName = $"{lblReportTitle.Text.Replace(" ", "")}_{DateTime.Now:yyyyMMdd}.doc";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    reportsService.ExportToWord(currentReport, saveDialog.FileName);
                    MessageBox.Show("Report exported to Word successfully!", "Export Complete", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (MessageBox.Show("Would you like to open the exported file?", "Open File", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                        {
                            FileName = saveDialog.FileName,
                            UseShellExecute = true
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to Word: {ex.Message}", "Export Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (currentReport == null) return;

            try
            {
                crystalReportViewer.PrintReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing report: {ex.Message}", "Print Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                currentReport?.Close();
                currentReport?.Dispose();
                reportsService?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}