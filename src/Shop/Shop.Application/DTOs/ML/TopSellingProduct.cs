namespace Shop.Application.DTOs.ML
{
    public class TopSellingPhoneDto
    {
        public int PhoneId { get; set; }
        public string PhoneName { get; set; }
        public int PredictedSold { get; set; } // Số lượng dự đoán bán ra trong tháng tới
    }

}
