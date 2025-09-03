namespace Shop.Application.DTOs.Report
{
    public class ProductReportDTO
    {
        // Sản phẩm bán chạy nhất
        public string MostSoldProductName { get; set; }
        public string MostSoldProductImageUrl { get; set; }
        public long TotalSoldQuantity { get; set; } // Số lượng đã bán
        // Sản phẩm doanh thu cao nhất
        public string TopRevenueProductName { get; set; }
        public string TopRevenueProductImageUrl { get; set; }
        public long TotalRevenue { get; set; }
    }
}
