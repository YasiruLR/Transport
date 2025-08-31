using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data;
using MySql.Data.MySqlClient;

namespace Transport
{
    /// <summary>
    /// Crystal Reports Service for generating professional reports
    /// Handles all Crystal Reports integration and report generation
    /// </summary>
    public class CrystalReportsService : IDisposable
    {
        private DatabaseConnection dbConnection;
        private ReportDocument reportDocument;

        public CrystalReportsService()
        {
            dbConnection = new DatabaseConnection();
        }

        /// <summary>
        /// Generates Customer Report using Crystal Reports
        /// </summary>
        public ReportDocument GenerateCustomerReport(DateTime fromDate, DateTime toDate, string filterType = "All")
        {
            try
            {
                DataTable customerData = GetCustomerReportData(fromDate, toDate, filterType);
                
                // Try to load from template file first, then create programmatically
                ReportDocument report;
                if (CrystalReportTemplates.TemplateExists("CustomerReport"))
                {
                    report = CrystalReportTemplates.LoadReportTemplate("CustomerReport");
                }
                else
                {
                    report = CrystalReportTemplates.CreateCustomerReportTemplate(customerData);
                }
                
                // Set data source
                report.SetDataSource(customerData);
                
                // Set parameters
                CrystalReportTemplates.SetCommonParameters(report, "Customer Report");
                SetReportParameter(report, "FromDate", fromDate.ToString("MMM dd, yyyy"));
                SetReportParameter(report, "ToDate", toDate.ToString("MMM dd, yyyy"));
                SetReportParameter(report, "FilterType", filterType);
                SetReportParameter(report, "GeneratedDate", DateTime.Now.ToString("MMM dd, yyyy HH:mm"));
                
                // Apply professional formatting
                CrystalReportTemplates.ApplyProfessionalFormatting(report);
                
                reportDocument = report;
                return report;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating Customer Report: {ex.Message}");
            }
        }

        /// <summary>
        /// Generates Job Report using Crystal Reports
        /// </summary>
        public ReportDocument GenerateJobReport(DateTime fromDate, DateTime toDate, string statusFilter = "All", int customerId = 0)
        {
            try
            {
                DataTable jobData = GetJobReportData(fromDate, toDate, statusFilter, customerId);
                
                ReportDocument report;
                if (CrystalReportTemplates.TemplateExists("JobReport"))
                {
                    report = CrystalReportTemplates.LoadReportTemplate("JobReport");
                }
                else
                {
                    report = CrystalReportTemplates.CreateJobReportTemplate(jobData);
                }
                
                report.SetDataSource(jobData);
                
                CrystalReportTemplates.SetCommonParameters(report, "Job Report");
                SetReportParameter(report, "FromDate", fromDate.ToString("MMM dd, yyyy"));
                SetReportParameter(report, "ToDate", toDate.ToString("MMM dd, yyyy"));
                SetReportParameter(report, "StatusFilter", statusFilter);
                SetReportParameter(report, "GeneratedDate", DateTime.Now.ToString("MMM dd, yyyy HH:mm"));
                
                CrystalReportTemplates.ApplyProfessionalFormatting(report);
                
                reportDocument = report;
                return report;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating Job Report: {ex.Message}");
            }
        }

        /// <summary>
        /// Generates Load Report using Crystal Reports
        /// </summary>
        public ReportDocument GenerateLoadReport(DateTime fromDate, DateTime toDate, string statusFilter = "All", string truckFilter = "All")
        {
            try
            {
                DataTable loadData = GetLoadReportData(fromDate, toDate, statusFilter, truckFilter);
                
                ReportDocument report;
                if (CrystalReportTemplates.TemplateExists("LoadReport"))
                {
                    report = CrystalReportTemplates.LoadReportTemplate("LoadReport");
                }
                else
                {
                    report = CrystalReportTemplates.CreateLoadReportTemplate(loadData);
                }
                
                report.SetDataSource(loadData);
                
                CrystalReportTemplates.SetCommonParameters(report, "Load Report");
                SetReportParameter(report, "FromDate", fromDate.ToString("MMM dd, yyyy"));
                SetReportParameter(report, "ToDate", toDate.ToString("MMM dd, yyyy"));
                SetReportParameter(report, "StatusFilter", statusFilter);
                SetReportParameter(report, "TruckFilter", truckFilter);
                SetReportParameter(report, "GeneratedDate", DateTime.Now.ToString("MMM dd, yyyy HH:mm"));
                
                CrystalReportTemplates.ApplyProfessionalFormatting(report);
                
                reportDocument = report;
                return report;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating Load Report: {ex.Message}");
            }
        }

        /// <summary>
        /// Generates Product Report using Crystal Reports
        /// </summary>
        public ReportDocument GenerateProductReport(string stockFilter = "All", decimal minPrice = 0, decimal maxPrice = 0)
        {
            try
            {
                DataTable productData = GetProductReportData(stockFilter, minPrice, maxPrice);
                
                ReportDocument report;
                if (CrystalReportTemplates.TemplateExists("ProductReport"))
                {
                    report = CrystalReportTemplates.LoadReportTemplate("ProductReport");
                }
                else
                {
                    report = CrystalReportTemplates.CreateProductReportTemplate(productData);
                }
                
                report.SetDataSource(productData);
                
                CrystalReportTemplates.SetCommonParameters(report, "Product Report");
                SetReportParameter(report, "StockFilter", stockFilter);
                SetReportParameter(report, "PriceRange", minPrice > 0 || maxPrice > 0 ? $"${minPrice:F2} - ${maxPrice:F2}" : "All Prices");
                SetReportParameter(report, "GeneratedDate", DateTime.Now.ToString("MMM dd, yyyy HH:mm"));
                
                CrystalReportTemplates.ApplyProfessionalFormatting(report);
                
                reportDocument = report;
                return report;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating Product Report: {ex.Message}");
            }
        }

        /// <summary>
        /// Generates Dashboard Summary Report
        /// </summary>
        public ReportDocument GenerateDashboardReport()
        {
            try
            {
                DataTable dashboardData = GetDashboardReportData();
                
                ReportDocument report;
                if (CrystalReportTemplates.TemplateExists("DashboardReport"))
                {
                    report = CrystalReportTemplates.LoadReportTemplate("DashboardReport");
                }
                else
                {
                    report = CrystalReportTemplates.CreateDashboardReportTemplate(dashboardData);
                }
                
                report.SetDataSource(dashboardData);
                
                CrystalReportTemplates.SetCommonParameters(report, "Dashboard Summary Report");
                SetReportParameter(report, "GeneratedDate", DateTime.Now.ToString("MMM dd, yyyy HH:mm"));
                
                CrystalReportTemplates.ApplyProfessionalFormatting(report);
                
                reportDocument = report;
                return report;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating Dashboard Report: {ex.Message}");
            }
        }

        /// <summary>
        /// Safely sets a parameter value in a Crystal Report
        /// </summary>
        private void SetReportParameter(ReportDocument report, string parameterName, object value)
        {
            try
            {
                // Check if parameter exists before setting it
                foreach (ParameterField parameter in report.ParameterFields)
                {
                    if (parameter.Name.Equals(parameterName, StringComparison.OrdinalIgnoreCase))
                    {
                        report.SetParameterValue(parameterName, value);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log warning but don't throw - parameter may not exist
                System.Diagnostics.Debug.WriteLine($"Warning: Could not set parameter '{parameterName}': {ex.Message}");
            }
        }

        #region Data Retrieval Methods

        private DataTable GetCustomerReportData(DateTime fromDate, DateTime toDate, string filterType)
        {
            string query = @"
                SELECT 
                    c.CustomerID,
                    c.Name as CustomerName,
                    c.Email,
                    c.Phone,
                    c.Address,
                    c.RegisteredDate,
                    COUNT(j.JobID) as TotalJobs,
                    COALESCE(MAX(j.RequestDate), 'No Jobs') as LastJobDate,
                    CASE 
                        WHEN COUNT(j.JobID) > 0 THEN 'Active'
                        ELSE 'Inactive'
                    END as Status,
                    COALESCE(SUM(CASE WHEN j.Status = 'Completed' THEN 1 ELSE 0 END), 0) as CompletedJobs,
                    COALESCE(SUM(CASE WHEN j.Status = 'Pending' THEN 1 ELSE 0 END), 0) as PendingJobs
                FROM Customer c
                LEFT JOIN Job j ON c.CustomerID = j.CustomerID
                WHERE c.RegisteredDate BETWEEN @fromDate AND @toDate";

            switch (filterType)
            {
                case "Recent (30 days)":
                    query += " AND c.RegisteredDate >= DATE_SUB(NOW(), INTERVAL 30 DAY)";
                    break;
                case "Active (with jobs)":
                    query += " AND EXISTS (SELECT 1 FROM Job WHERE CustomerID = c.CustomerID)";
                    break;
                case "Inactive":
                    query += " AND NOT EXISTS (SELECT 1 FROM Job WHERE CustomerID = c.CustomerID)";
                    break;
            }

            query += " GROUP BY c.CustomerID, c.Name, c.Email, c.Phone, c.Address, c.RegisteredDate ORDER BY c.RegisteredDate DESC";

            MySqlParameter[] parameters = {
                new MySqlParameter("@fromDate", fromDate.Date),
                new MySqlParameter("@toDate", toDate.Date)
            };

            return dbConnection.ExecuteSelect(query, parameters);
        }

        private DataTable GetJobReportData(DateTime fromDate, DateTime toDate, string statusFilter, int customerId)
        {
            string query = @"
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
                    COALESCE(ld.Driver, 'Not Assigned') as AssignedDriver,
                    CASE j.Status
                        WHEN 'Pending' THEN 'Waiting for Approval'
                        WHEN 'Approved' THEN 'Ready for Assignment'
                        WHEN 'In Progress' THEN 'Currently Active'
                        WHEN 'Completed' THEN 'Successfully Completed'
                        WHEN 'Cancelled' THEN 'Cancelled by Admin'
                        ELSE j.Status
                    END as StatusDescription,
                    CASE 
                        WHEN j.Status = 'Completed' THEN 'On Time'
                        WHEN j.Status = 'In Progress' AND j.PreferredDate < CURDATE() THEN 'Delayed'
                        WHEN j.Status = 'In Progress' THEN 'On Schedule'
                        ELSE 'N/A'
                    END as ScheduleStatus
                FROM Job j
                INNER JOIN Customer c ON j.CustomerID = c.CustomerID
                LEFT JOIN LoadDetails ld ON j.JobID = ld.JobID
                WHERE j.RequestDate BETWEEN @fromDate AND @toDate";

            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@fromDate", fromDate.Date),
                new MySqlParameter("@toDate", toDate.Date)
            };

            if (statusFilter != "All Status")
            {
                query += " AND j.Status = @statusFilter";
                parameters.Add(new MySqlParameter("@statusFilter", statusFilter));
            }

            if (customerId > 0)
            {
                query += " AND j.CustomerID = @customerId";
                parameters.Add(new MySqlParameter("@customerId", customerId));
            }

            query += " ORDER BY j.RequestDate DESC";

            return dbConnection.ExecuteSelect(query, parameters.ToArray());
        }

        private DataTable GetLoadReportData(DateTime fromDate, DateTime toDate, string statusFilter, string truckFilter)
        {
            string query = @"
                SELECT 
                    ld.LoadID,
                    j.JobID,
                    c.Name as CustomerName,
                    j.StartLocation,
                    j.Destination,
                    j.Status as JobStatus,
                    ld.Lorry as TruckNumber,
                    ld.Driver as DriverName,
                    ld.Assistant as AssistantName,
                    ld.Container as ContainerNumber,
                    j.EstimatedWeight,
                    j.RequestDate,
                    j.PreferredDate,
                    j.Urgency,
                    CASE j.Status
                        WHEN 'In Progress' THEN 'Currently Loading/Transit'
                        WHEN 'Completed' THEN 'Delivered Successfully'
                        WHEN 'Cancelled' THEN 'Load Cancelled'
                        ELSE j.Status
                    END as LoadStatus,
                    CASE 
                        WHEN j.Status = 'In Progress' THEN DATEDIFF(CURDATE(), j.PreferredDate)
                        WHEN j.Status = 'Completed' THEN 0
                        ELSE NULL
                    END as DaysFromSchedule,
                    CASE 
                        WHEN j.Status = 'Completed' THEN 100
                        WHEN j.Status = 'In Progress' THEN 75
                        WHEN j.Status = 'Approved' THEN 25
                        ELSE 0
                    END as CompletionPercentage
                FROM LoadDetails ld
                INNER JOIN Job j ON ld.JobID = j.JobID
                INNER JOIN Customer c ON j.CustomerID = c.CustomerID
                WHERE j.Status IN ('In Progress', 'Completed', 'Cancelled')
                AND j.RequestDate BETWEEN @fromDate AND @toDate";

            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@fromDate", fromDate.Date),
                new MySqlParameter("@toDate", toDate.Date)
            };

            if (statusFilter != "All Status")
            {
                query += " AND j.Status = @statusFilter";
                parameters.Add(new MySqlParameter("@statusFilter", statusFilter));
            }

            if (truckFilter != "All Trucks")
            {
                query += " AND ld.Lorry = @truckFilter";
                parameters.Add(new MySqlParameter("@truckFilter", truckFilter));
            }

            query += " ORDER BY j.RequestDate DESC";

            return dbConnection.ExecuteSelect(query, parameters.ToArray());
        }

        private DataTable GetProductReportData(string stockFilter, decimal minPrice, decimal maxPrice)
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
                    END as PriceCategory,
                    CASE 
                        WHEN Stock = 0 THEN 'Critical'
                        WHEN Stock < 10 THEN 'Urgent'
                        WHEN Stock < 50 THEN 'Normal'
                        ELSE 'Good'
                    END as RestockPriority
                FROM Product WHERE 1=1";

            List<MySqlParameter> parameters = new List<MySqlParameter>();

            switch (stockFilter)
            {
                case "In Stock (>0)":
                    query += " AND Stock > 0";
                    break;
                case "Low Stock (<10)":
                    query += " AND Stock < 10";
                    break;
                case "Out of Stock (0)":
                    query += " AND Stock = 0";
                    break;
            }

            if (minPrice > 0)
            {
                query += " AND Price >= @minPrice";
                parameters.Add(new MySqlParameter("@minPrice", minPrice));
            }

            if (maxPrice > 0)
            {
                query += " AND Price <= @maxPrice";
                parameters.Add(new MySqlParameter("@maxPrice", maxPrice));
            }

            query += " ORDER BY ProductID";

            return dbConnection.ExecuteSelect(query, parameters.ToArray());
        }

        private DataTable GetDashboardReportData()
        {
            string query = @"
                SELECT 
                    'System Overview' as ReportType,
                    (SELECT COUNT(*) FROM Customer) as TotalCustomers,
                    (SELECT COUNT(*) FROM Job) as TotalJobs,
                    (SELECT COUNT(*) FROM Product) as TotalProducts,
                    (SELECT COUNT(*) FROM LoadDetails) as TotalLoads,
                    (SELECT COUNT(*) FROM Job WHERE Status = 'Pending') as PendingJobs,
                    (SELECT COUNT(*) FROM Job WHERE Status = 'Completed') as CompletedJobs,
                    (SELECT COUNT(*) FROM Job WHERE Status = 'In Progress') as ActiveJobs,
                    (SELECT COUNT(*) FROM Job WHERE Status = 'Cancelled') as CancelledJobs,
                    (SELECT COUNT(DISTINCT c.CustomerID) FROM Customer c INNER JOIN Job j ON c.CustomerID = j.CustomerID) as ActiveCustomers,
                    (SELECT COUNT(*) FROM Product WHERE Stock < 10) as LowStockProducts,
                    (SELECT COALESCE(SUM(Price * Stock), 0) FROM Product) as TotalInventoryValue,
                    (SELECT COALESCE(AVG(EstimatedWeight), 0) FROM Job WHERE EstimatedWeight IS NOT NULL) as AverageJobWeight,
                    (SELECT COUNT(DISTINCT Lorry) FROM LoadDetails WHERE Lorry IS NOT NULL) as ActiveTrucks,
                    (SELECT COUNT(DISTINCT Driver) FROM LoadDetails WHERE Driver IS NOT NULL) as ActiveDrivers";

            return dbConnection.ExecuteSelect(query);
        }

        #endregion

        #region Report Template Creation (Programmatic)

        private void CreateCustomerReportTemplate()
        {
            // This would be where you'd load a .rpt file if you had one
            // For now, we'll create the report structure programmatically
            // In a real implementation, you'd have .rpt files created in Crystal Reports Designer
        }

        private void CreateJobReportTemplate()
        {
            // Job report template creation
        }

        private void CreateLoadReportTemplate()
        {
            // Load report template creation
        }

        private void CreateProductReportTemplate()
        {
            // Product report template creation
        }

        private void CreateDashboardReportTemplate()
        {
            // Dashboard report template creation
        }

        #endregion

        public void Dispose()
        {
            reportDocument?.Close();
            reportDocument?.Dispose();
            dbConnection?.Dispose();
        }
    }
}