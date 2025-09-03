using Shop.Application.DTOs.ML;

namespace Shop.Application.Services.ML
{
    public interface IForecastService
    {
        Task<RevenueForecastRangeDto> GetRevenueForecastAsync(); // Dự đoán doanh thu
        Task<List<TopSellingPhoneDto>> GetTopSellingProductsAsync(); // Dự đoán sản phẩm bán chạy
        ///// <summary>
        ///// Dự đoán số lượng sản phẩm cần nhập kho dựa trên xu hướng tiêu thụ.
        ///// </summary>
        //Task<List<ProductRestockForecastDto>> ForecastProductRestockAsync();

        ///// <summary>
        ///// Phân loại và trả về danh sách khách hàng tiềm năng (dễ quay lại mua).
        ///// </summary>
        //Task<List<CustomerPotentialDto>> ClassifyPotentialCustomersAsync();

        /// <summary>
        /// Gợi ý danh sách sản phẩm phù hợp cho khách hàng dựa trên lịch sử mua hàng.
        /// </summary>
        //Task<List<RecommendedProductDto>> RecommendProductsAsync(int userId);

        ///// <summary>
        ///// Phát hiện những ngày có doanh thu bất thường (cao hoặc thấp bất thường).
        ///// </summary>
        //Task<List<AnomalyRevenueDto>> DetectRevenueAnomaliesAsync();

        ///// <summary>
        ///// Xếp hạng các sản phẩm có mức tăng trưởng mạnh theo thời gian.
        ///// </summary>
        //Task<List<ProductGrowthRankingDto>> RankGrowingProductsAsync();
    }

}
