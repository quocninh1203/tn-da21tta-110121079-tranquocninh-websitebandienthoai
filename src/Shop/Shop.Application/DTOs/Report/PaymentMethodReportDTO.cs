namespace Shop.Application.DTOs.Report
{
    // Phương thức thanh toán được sử dụng nhiều nhất
    public class PaymentMethodReportDTO
    {
        public string PaymentMethod { get; set; } // Tên phương thức
        public long TotalOrders { get; set; }
        public long TotalRevenue { get; set; }
    }

}
