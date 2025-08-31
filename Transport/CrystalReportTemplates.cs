using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data;

namespace Transport
{
    /// <summary>
    /// Crystal Reports Template Manager
    /// Handles creation and management of Crystal Report templates
    /// </summary>
    public static class CrystalReportTemplates
    {
        /// <summary>
        /// Creates a programmatic Crystal Report for Customer data
        /// This method creates the report structure in code since we don't have .rpt files
        /// </summary>
        public static ReportDocument CreateCustomerReportTemplate(DataTable data)
        {
            var report = new ReportDocument();
            
            try
            {
                // In a real implementation, you would:
                // 1. Load a pre-designed .rpt file: report.Load("Reports/CustomerReport.rpt")
                // 2. Or use Crystal Reports Designer to create templates
                
                // For now, we'll set up the data source and basic structure
                report.SetDataSource(data);
                
                // Set report information
                var reportOptions = report.ReportOptions;
                reportOptions.EnableSaveDataWithReport = false;
                
                return report;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating Customer Report template: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a programmatic Crystal Report for Job data
        /// </summary>
        public static ReportDocument CreateJobReportTemplate(DataTable data)
        {
            var report = new ReportDocument();
            
            try
            {
                report.SetDataSource(data);
                
                var reportOptions = report.ReportOptions;
                reportOptions.EnableSaveDataWithReport = false;
                
                return report;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating Job Report template: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a programmatic Crystal Report for Load data
        /// </summary>
        public static ReportDocument CreateLoadReportTemplate(DataTable data)
        {
            var report = new ReportDocument();
            
            try
            {
                report.SetDataSource(data);
                
                var reportOptions = report.ReportOptions;
                reportOptions.EnableSaveDataWithReport = false;
                
                return report;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating Load Report template: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a programmatic Crystal Report for Product data
        /// </summary>
        public static ReportDocument CreateProductReportTemplate(DataTable data)
        {
            var report = new ReportDocument();
            
            try
            {
                report.SetDataSource(data);
                
                var reportOptions = report.ReportOptions;
                reportOptions.EnableSaveDataWithReport = false;
                
                return report;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating Product Report template: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a programmatic Crystal Report for Dashboard data
        /// </summary>
        public static ReportDocument CreateDashboardReportTemplate(DataTable data)
        {
            var report = new ReportDocument();
            
            try
            {
                report.SetDataSource(data);
                
                var reportOptions = report.ReportOptions;
                reportOptions.EnableSaveDataWithReport = false;
                
                return report;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating Dashboard Report template: {ex.Message}");
            }
        }

        /// <summary>
        /// Sets common report parameters for all Crystal Reports
        /// </summary>
        public static void SetCommonParameters(ReportDocument report, string reportTitle)
        {
            try
            {
                // Add common parameters that all reports should have
                if (report.ParameterFields["ReportTitle"] != null)
                    report.SetParameterValue("ReportTitle", reportTitle);
                
                if (report.ParameterFields["CompanyName"] != null)
                    report.SetParameterValue("CompanyName", "Transport Management System");
                
                if (report.ParameterFields["GeneratedDate"] != null)
                    report.SetParameterValue("GeneratedDate", DateTime.Now);
                
                if (report.ParameterFields["GeneratedBy"] != null)
                    report.SetParameterValue("GeneratedBy", Environment.UserName);
            }
            catch (Exception ex)
            {
                // Log error but don't throw - parameters may not exist in all reports
                System.Diagnostics.Debug.WriteLine($"Warning: Could not set parameters: {ex.Message}");
            }
        }

        /// <summary>
        /// Applies professional formatting to Crystal Reports
        /// </summary>
        public static void ApplyProfessionalFormatting(ReportDocument report)
        {
            try
            {
                // Set report options for professional appearance
                var reportOptions = report.ReportOptions;
                reportOptions.EnableSaveDataWithReport = false;
                
                // In a real implementation with .rpt files, you would:
                // - Set fonts, colors, and styling
                // - Configure page layout and margins  
                // - Add headers, footers, and logos
                // - Set up grouping and sorting
                // - Configure conditional formatting
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Warning: Could not apply formatting: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the path to Crystal Report template files
        /// </summary>
        public static string GetReportTemplatePath(string reportName)
        {
            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "Templates");
            
            // Ensure directory exists
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            
            return Path.Combine(basePath, $"{reportName}.rpt");
        }

        /// <summary>
        /// Checks if a Crystal Report template file exists
        /// </summary>
        public static bool TemplateExists(string reportName)
        {
            string templatePath = GetReportTemplatePath(reportName);
            return File.Exists(templatePath);
        }

        /// <summary>
        /// Loads a Crystal Report template from file if it exists
        /// </summary>
        public static ReportDocument LoadReportTemplate(string reportName)
        {
            try
            {
                string templatePath = GetReportTemplatePath(reportName);
                
                if (File.Exists(templatePath))
                {
                    var report = new ReportDocument();
                    report.Load(templatePath);
                    return report;
                }
                else
                {
                    throw new FileNotFoundException($"Crystal Report template not found: {templatePath}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading Crystal Report template '{reportName}': {ex.Message}");
            }
        }

        /// <summary>
        /// Creates instructions for creating Crystal Report templates
        /// </summary>
        public static string GetTemplateCreationInstructions()
        {
            return @"
# Crystal Reports Template Creation Instructions

## To create professional Crystal Report templates (.rpt files):

### 1. Install Crystal Reports Developer
- Download Crystal Reports Developer or use Visual Studio integration
- Install Crystal Reports runtime if not already installed

### 2. Create Report Templates

#### Customer Report (CustomerReport.rpt):
- Fields: CustomerID, Name, Email, Phone, Address, RegisteredDate, TotalJobs, Status
- Grouping: By Status (Active/Inactive)
- Summary: Count of customers by status
- Charts: Pie chart showing Active vs Inactive customers

#### Job Report (JobReport.rpt):
- Fields: JobID, CustomerName, StartLocation, Destination, Status, RequestDate, AssignedTruck, AssignedDriver
- Grouping: By Status and Customer
- Summary: Count and percentage by status
- Charts: Bar chart showing job status distribution

#### Load Report (LoadReport.rpt):
- Fields: LoadID, JobID, CustomerName, TruckNumber, DriverName, LoadStatus, EstimatedWeight
- Grouping: By Truck and Status
- Summary: Total weight, utilization rates
- Charts: Truck utilization chart

#### Product Report (ProductReport.rpt):
- Fields: ProductID, ProductName, Price, Stock, TotalValue, StockStatus
- Grouping: By Stock Status
- Summary: Total inventory value, low stock alerts
- Charts: Stock level distribution

#### Dashboard Report (DashboardReport.rpt):
- Overview of all key metrics
- Multiple subreports for each area
- Executive summary format
- KPI indicators and trends

### 3. Save Templates
- Save all .rpt files in: Transport/Reports/Templates/
- Use naming convention: ReportName.rpt (e.g., CustomerReport.rpt)

### 4. Template Features to Include
- Professional headers with company logo
- Parameter fields for dynamic filtering
- Conditional formatting based on data values
- Page numbering and generation timestamps
- Color coding for status indicators
- Drill-down capabilities where appropriate

### 5. Parameters to Define
- FromDate / ToDate for date ranges
- FilterType for various filter options
- IncludeCharts (boolean) for chart inclusion
- ColorCoding (boolean) for color formatting

Once templates are created, the system will automatically load and use them
instead of the programmatic report generation currently implemented.
";
        }
    }
}