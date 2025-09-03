namespace Shop.Application.DTOs.ML
{
    // DTO này được sử dụng cho cả huấn luyện và dự đoán
    public class ProductInteraction
    {
        // Các thuộc tính nên là int để khớp với dữ liệu gốc
        [Microsoft.ML.Data.LoadColumn(0)]
        public int UserId { get; set; }

        [Microsoft.ML.Data.LoadColumn(1)]
        public int ProductId { get; set; }

        [Microsoft.ML.Data.LoadColumn(2)]
        public float Label { get; set; }
    }

    public class ProductPrediction
    {
        // ML.NET sẽ tự động tạo cột Score
        public float Score { get; set; }
    }
}