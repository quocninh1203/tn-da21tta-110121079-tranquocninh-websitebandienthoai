namespace Shop.Application.DTOs.ML
{
    // Kết quả đầu ra từ mô hình dự đoán doanh thu
    public class RevenueForecast
    {
        public float[] ForecastedRevenue { get; set; } // Mảng kết quả dự đoán doanh thu từng ngày
    }

}
