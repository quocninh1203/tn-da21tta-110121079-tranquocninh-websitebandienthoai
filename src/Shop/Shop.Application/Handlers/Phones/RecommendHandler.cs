using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shop.Application.DTOs.ML;
using Shop.Application.DTOs.Phones;
using Shop.Application.Extension;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Phones;
using Shop.Application.Services.ML;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Phones
{
    public class RecommendHandler : IRequestHandler<RecommendRequest, QueryResult<List<PhoneDTO>>>
    {
        private readonly RecommendationService _recommendationService;
        private readonly IPhoneRepository _phoneRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IHttpContextAccessor _httpContext;

        public RecommendHandler(RecommendationService recommendationService, IPhoneRepository phoneRepository, IReviewRepository reviewRepository, IHttpContextAccessor httpContext)
        {
            _recommendationService = recommendationService;
            _phoneRepository = phoneRepository;
            _reviewRepository = reviewRepository;
            _httpContext = httpContext;
        }

        public async Task<QueryResult<List<PhoneDTO>>> Handle(RecommendRequest request, CancellationToken cancellationToken)
        {
            var result = new QueryResult<List<PhoneDTO>>();

            // Kiểm tra mô hình
            var trainedModel = _recommendationService.GetTrainedModel();
            if (trainedModel == null)
            {
                result.Success = false;
                result.Message = "Mô hình gợi ý chưa được huấn luyện hoặc tải.";
                result.Code = StatusCode.Conflict;
                return result;
            }

            // Gọi CreatePredictionEngine trên MLContext
            // Sửa lại dòng này để sử dụng ProductInteraction làm kiểu dữ liệu đầu vào
            var predictionEngine = _recommendationService.GetMLContext().Model.CreatePredictionEngine<ProductInteraction, ProductPrediction>(trainedModel);

            // Lấy tất cả điện thoại và các biến thể của chúng
            var allPhones = await _phoneRepository.GetAsync(
                include: query => query.Include(p => p.PhoneVariants)
            );

            // Lấy tất cả review cho các phone đã lấy được (để tránh N+1 query)
            var phoneIds = allPhones.Select(p => p.Id).ToList();
            var reviews = await _reviewRepository.GetAsync(r => phoneIds.Contains(r.PhoneId));

            var recommendedProducts = new List<Tuple<float, PhoneDTO>>();

            foreach (var phone in allPhones)
            {
                // Tính toán PriceReview
                var minPrice = phone.PhoneVariants?.Where(v => v.Price != null).Min(v => (int?)v.Price) ?? 0;

                // Tính toán Rating trung bình
                var phoneReviews = reviews.Where(r => r.PhoneId == phone.Id).ToList();
                var rating = phoneReviews.Any() ? phoneReviews.Average(r => r.Rating) : 5;

                // Tạo đối tượng ProductInteraction
                var predictionInput = new ProductInteraction
                {
                    UserId = request.UserId,
                    ProductId = phone.Id
                };

                var prediction = predictionEngine.Predict(predictionInput);

                // Tạo PhoneDTO với các giá trị đã tính toán
                var phoneDto = phone.ToDto(_httpContext);
                phoneDto.PriceReview = minPrice;
                phoneDto.Rating = rating;

                recommendedProducts.Add(new Tuple<float, PhoneDTO>(prediction.Score, phoneDto));
            }

            var topRecommendations = recommendedProducts
                .OrderByDescending(p => p.Item1)
                .Take(5)
                .OrderBy(p => Guid.NewGuid())
                .Select(p => p.Item2)
                .ToList();

            if (!topRecommendations.Any())
            {
                result.Success = false;
                result.Message = "Không có gợi ý nào được tìm thấy.";
                result.Code = StatusCode.NotFound;
                return result;
            }

            result.Success = true;
            result.Code = StatusCode.Ok;
            result.Model = topRecommendations;
            return result;
        }
    }
}