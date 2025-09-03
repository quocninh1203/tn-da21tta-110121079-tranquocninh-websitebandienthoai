namespace Shop.Application.DTOs.Report
{
    public class ReportDTO
    {
        public RevenueReportDTO Revenue { get; set; }
        public ProductReportDTO Products { get; set; }
        public CustomerReportDTO Customers { get; set; }
        public PaymentMethodReportDTO PaymentMethods { get; set; }
    }
}
