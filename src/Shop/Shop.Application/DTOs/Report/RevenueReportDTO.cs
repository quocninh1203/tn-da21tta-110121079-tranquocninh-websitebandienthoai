namespace Shop.Application.DTOs.Report
{
    //public class RevenueReportDTO
    //{
    //    public decimal TotalRevenue { get; set; } // Tổng doanh thu từ các đơn hàng(đã thanh toán và chưa thanh toán - không bao gồm các đơn hàng đã hủy)
    //    public decimal TotalSoldRevenue { get; set; } // Tổng doanh thu các đơn hàng đã bán
    //    public int TotalProductsSold { get; set; } // Tổng số lượng sản phẩm đã bán(quantity)
    //    public int TotalCancelledOrders { get; set; } // Số lượng đơn hàng đã hủy
    //    public int TotalUsers { get; set; } // Số lượng người dùng đã mua hàng
    //    public DateTime StartDate { get; set; }
    //    public DateTime EndDate { get; set; }
    //    public string PeriodDescription { get; set; }
    //}

    public class RevenueReportDTO
    {
        public long TotalRevenue { get; set; }
        public long TotalCancelledOrdersAmount { get; set; }
        public long TotalOrders { get; set; }
        public long CompletedOrders { get; set; }
        public long CancelledOrders { get; set; }
        public long UndeliveredOrders { get; set; }
        public long PendingOrders { get; set; }
    }
}
