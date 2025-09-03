namespace Shop.Application.DTOs.Report
{
    // Khách hàng mua nhiều nhất
    public class CustomerReportDTO
    {
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public long TotalItemsPurchased { get; set; }
        public long TotalPrice { get; set; }
    }

}
