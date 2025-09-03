using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms.TimeSeries;
using Shop.Application.DTOs.ML;
using Shop.Application.Interfaces;
using Shop.Application.Services.ML;

namespace Shop.Application.Services
{
    public class ForecastService : IForecastService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IOrderDetailRepository _orderDetailRepo;
        private readonly IPhoneRepository _phoneRepo;
        private readonly IPhoneVariantRepository _phoneVariantRepo;
        private readonly MLContext _mlContext;

        public ForecastService(
            IOrderRepository orderRepo,
            IOrderDetailRepository orderDetailRepo,
            IPhoneRepository phoneRepo,
            IPhoneVariantRepository phoneVariantRepo)
        {
            _orderRepo = orderRepo;
            _orderDetailRepo = orderDetailRepo;
            _phoneRepo = phoneRepo;
            _phoneVariantRepo = phoneVariantRepo;
            _mlContext = new MLContext();
        }

        public async Task<RevenueForecastRangeDto> GetRevenueForecastAsync()
        {
            var now = DateTime.UtcNow;

            var orders = await _orderRepo.GetAsync(o => o.OrderDate >= now.AddMonths(-3));
            var orderIds = orders.Select(o => o.Id).ToList();

            var details = await _orderDetailRepo.GetAsync(d => orderIds.Contains(d.OrderId));

            var revenueByDate = details
                .Join(orders, d => d.OrderId, o => o.Id, (d, o) => new { o.OrderDate.Date, Revenue = d.Quantity * d.PriceAtOrder })
                .GroupBy(x => x.Date)
                .Select(g => new RevenueData { Date = g.Key, Revenue = g.Sum(x => x.Revenue) })
                .OrderBy(r => r.Date)
                .ToList();

            if (revenueByDate.Count <= 7)
                throw new InvalidOperationException("Không đủ dữ liệu để dự đoán.");

            var dataView = _mlContext.Data.LoadFromEnumerable(revenueByDate);

            int windowSize = Math.Min(7, Math.Max(2, revenueByDate.Count / 3));
            int seriesLength = revenueByDate.Count;

            var pipeline = _mlContext.Forecasting.ForecastBySsa(
                outputColumnName: "ForecastedRevenue",
                inputColumnName: nameof(RevenueData.Revenue),
                windowSize: windowSize,
                seriesLength: seriesLength,
                trainSize: seriesLength,
                horizon: 30);

            var model = pipeline.Fit(dataView);
            var forecastEngine = model.CreateTimeSeriesEngine<RevenueData, RevenueForecast>(_mlContext);
            var forecast = forecastEngine.Predict();

            var values = forecast.ForecastedRevenue;

            // Tính trung bình và độ lệch chuẩn
            float average = values.Average();
            float stdDev = (float)Math.Sqrt(values.Select(v => Math.Pow(v - average, 2)).Average());

            // Ước lượng tổng doanh thu: average ± stdDev nhân với 30 ngày
            float min = (average - stdDev) * 30;
            float max = (average + stdDev) * 30;

            return new RevenueForecastRangeDto
            {
                MinRevenue = Math.Max(min, 0),
                MaxRevenue = Math.Max(max, 0)
            };
        }

        public async Task<List<TopSellingPhoneDto>> GetTopSellingProductsAsync()
        {
            var now = DateTime.UtcNow;
            var orders = await _orderRepo.GetAsync(o => o.OrderDate >= now.AddMonths(-3));
            var orderIds = orders.Select(o => o.Id).ToList();
            var details = await _orderDetailRepo.GetAsync(d => orderIds.Contains(d.OrderId));

            var variants = await _phoneVariantRepo.GetAsync(v => true, q => q.Include(v => v.Phone));
            var variantDict = variants.ToDictionary(v => v.Id, v => v.Phone);

            // Tính tổng số lượng bán theo Phone
            var quantityByPhone = details
                .Where(d => variantDict.ContainsKey(d.VariantId))
                .GroupBy(d => variantDict[d.VariantId].Id)
                .Select(g => new
                {
                    PhoneId = g.Key,
                    PhoneName = variantDict[g.First().VariantId].Name,
                    TotalSold = g.Sum(d => d.Quantity)
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(10)
                .Select(x => new TopSellingPhoneDto
                {
                    PhoneId = x.PhoneId,
                    PhoneName = x.PhoneName,
                    PredictedSold = x.TotalSold / 3 // Ước lượng theo trung bình tháng
                })
                .ToList();

            return quantityByPhone;
        }

        //public async Task<List<RecommendedProductDto>> RecommendProductsAsync(int userId)
        //{
        //    // Bước 1: Lấy dữ liệu đơn hàng và chi tiết đơn hàng
        //    var orders = await _orderRepo.GetAsync(o => true);
        //    var orderIds = orders.Select(o => o.Id).ToList();
        //    var orderDetails = await _orderDetailRepo.GetAsync(d => orderIds.Contains(d.OrderId));

        //    var allVariants = await _phoneVariantRepo.GetAsync(v => true); // Bao gồm VariantId -> PhoneId

        //    // Bước 2: Tạo dữ liệu huấn luyện: (UserId, PhoneId, Label)
        //    var trainingData = orderDetails
        //        .Join(orders, d => d.OrderId, o => o.Id, (d, o) => new { o.UserId, d.VariantId })
        //        .Join(allVariants, x => x.VariantId, v => v.Id, (x, v) => new ProductRatingData
        //        {
        //            UserId = (uint)x.UserId,
        //            ProductId = (uint)v.PhoneId, // PhoneId là ProductId
        //            Label = 1f
        //        })
        //        .Distinct()
        //        .ToList();

        //    if (!trainingData.Any())
        //        return new List<RecommendedProductDto>();

        //    var dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

        //    // Bước 3: Huấn luyện mô hình
        //    var options = new MatrixFactorizationTrainer.Options
        //    {
        //        MatrixColumnIndexColumnName = nameof(ProductRatingData.UserId),
        //        MatrixRowIndexColumnName = nameof(ProductRatingData.ProductId),
        //        LabelColumnName = nameof(ProductRatingData.Label),
        //        NumberOfIterations = 20,
        //        ApproximationRank = 100
        //    };

        //    var estimator = _mlContext.Recommendation().Trainers.MatrixFactorization(options);
        //    var model = estimator.Fit(dataView);
        //    var predictionEngine = _mlContext.Model.CreatePredictionEngine<ProductRatingData, ProductRatingPrediction>(model);

        //    // Bước 4: Lấy danh sách sản phẩm và lọc ra những sản phẩm chưa mua
        //    var allPhones = await _phoneRepo.GetAsync();
        //    var purchasedProductIds = trainingData
        //        .Where(x => x.UserId == (uint)userId)
        //        .Select(x => x.ProductId)
        //        .ToHashSet();

        //    var unseenPhones = allPhones
        //        .Where(p => !purchasedProductIds.Contains((uint)p.Id))
        //        .ToList();

        //    // Bước 5: Dự đoán điểm gợi ý
        //    var scoredPhones = unseenPhones
        //        .Select(p => new
        //        {
        //            Product = p,
        //            Score = predictionEngine.Predict(new ProductRatingData
        //            {
        //                UserId = (uint)userId,
        //                ProductId = (uint)p.Id
        //            }).Score
        //        })
        //        .Where(x => !float.IsNaN(x.Score) && !float.IsInfinity(x.Score)) // <- LỌC
        //        .OrderByDescending(x => x.Score)
        //        .Take(10)
        //        .ToList();

        //    // Bước 6: Trả về
        //    return scoredPhones.Select(x => new RecommendedProductDto
        //    {
        //        ProductId = x.Product.Id,
        //        ProductName = x.Product.Name,
        //        Description = x.Product.Description,
        //        Score = x.Score
        //    }).ToList();
        //}





    }



    // Class dữ liệu huấn luyện
    //public class ProductRatingData
    //{
    //    [KeyType(count: 1000)]
    //    public uint UserId { get; set; }

    //    [KeyType(count: 1000)]
    //    public uint ProductId { get; set; }

    //    public float Label { get; set; }
    //}

    //// Class dự đoán
    //public class ProductRatingPrediction
    //{
    //    public float Score { get; set; }
    //}

    //// DTO cho kết quả gợi ý
    //public class RecommendedProductDto
    //{
    //    public int ProductId { get; set; }
    //    public string ProductName { get; set; } = string.Empty;
    //    public string? Description { get; set; }
    //    public float Score { get; set; }
    //}
}
