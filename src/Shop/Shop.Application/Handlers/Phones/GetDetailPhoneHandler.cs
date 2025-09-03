using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shop.Application.DTOs.Phones;
using Shop.Application.DTOs.Reviews;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Phones;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Phones
{
    public class GetDetailPhoneHandler : IRequestHandler<GetDetailPhoneRequest, QueryResult<PhoneDetailDTO>>
    {
        private readonly IPhoneRepository _phoneRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRepository _userRepository;

        public GetDetailPhoneHandler(IPhoneRepository phoneRepository, IBrandRepository brandRepository, IHttpContextAccessor httpContext, IReviewRepository reviewRepository, IUserRepository userRepository)
        {
            _phoneRepository = phoneRepository;
            _brandRepository = brandRepository;
            _httpContext = httpContext;
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
        }

        public async Task<QueryResult<PhoneDetailDTO>> Handle(GetDetailPhoneRequest request, CancellationToken cancellationToken)
        {
            var phones = await _phoneRepository.GetAsync(
                predicate: p => p.Id == request.PhoneId,
                include: query => query
                    .Include(p => p.PhoneVariants)
                        .ThenInclude(v => v.Color)
                    .Include(p => p.PhoneVariants)
                        .ThenInclude(v => v.Ram)
                    .Include(p => p.PhoneVariants)
                        .ThenInclude(v => v.Storage)
                    .Include(p => p.PhoneImages)
            );

            var phone = phones.FirstOrDefault();

            var response = new QueryResult<PhoneDetailDTO>();

            if (phone == null)
            {
                response.Success = false;
                response.Message = string.Format(CommonMessages.NotFound, nameof(Phone));
                response.Code = StatusCode.NotFound;
                response.Model = null;
                return response;
            }

            var brand = await _brandRepository.GetByIdAsync(phone.BrandId);

            var baseUrl = _httpContext.HttpContext != null
                ? $"{_httpContext.HttpContext.Request.Scheme}://{_httpContext.HttpContext.Request.Host}"
                : "";

            // Lấy các image URL của Phone
            var imageUrls = new List<string>();

            if (!string.IsNullOrWhiteSpace(phone.ImageUrl))
            {
                var mainImageUrl = phone.ImageUrl.StartsWith("http") ? phone.ImageUrl : baseUrl + phone.ImageUrl;
                imageUrls.Add(mainImageUrl);
            }

            if (phone.PhoneImages != null && phone.PhoneImages.Any())
            {
                var additionalImages = phone.PhoneImages
                    .Where(img => !string.IsNullOrWhiteSpace(img.ImageUrl))
                    .Select(img => img.ImageUrl.StartsWith("http") ? img.ImageUrl : baseUrl + img.ImageUrl);

                imageUrls.AddRange(additionalImages);
            }

            // Tính Rating trung bình của phone từ các review
            var reviews = await _reviewRepository.GetAsync(r => r.PhoneId == phone.Id);
            var rating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;

            // Lấy thông tin reviews chi tiết
            var reviewDTOs = new List<ReviewDTO>();
            foreach (var review in reviews)
            {
                var user = await _userRepository.GetByIdAsync(review.UserId);
                var phoneVariant = phone.PhoneVariants.FirstOrDefault(v => v.Id == review.VariantId); // Lấy PhoneVariant từ review

                if (!string.IsNullOrWhiteSpace(review.ImageUrl))
                {
                    review.ImageUrl = review.ImageUrl.StartsWith("http") ? review.ImageUrl : baseUrl + review.ImageUrl;
                }
                if (!string.IsNullOrWhiteSpace(user.ImageUrl))
                {
                    user.ImageUrl = user.ImageUrl.StartsWith("http") ? user.ImageUrl : baseUrl + user.ImageUrl;
                }
                reviewDTOs.Add(new ReviewDTO
                {
                    FullName = user?.FullName ?? "Unknown",
                    UserImageUrl = user?.ImageUrl ?? "",
                    Rating = review.Rating,
                    Text = review.Text,
                    ReviewImageUrl = review.ImageUrl,
                    Color = phoneVariant?.Color?.Name ?? "N/A",  // Lấy thông tin màu từ PhoneVariant
                    Ram = phoneVariant?.Ram?.Size ?? "N/A",     // Lấy thông tin RAM từ PhoneVariant
                    Storage = phoneVariant?.Storage?.Size ?? "N/A",  // Lấy thông tin dung lượng lưu trữ từ PhoneVariant
                    ReviewDate = review.CreateDate
                });
            }

            // Lấy thông tin PhoneVariants
            var phoneVariants = phone.PhoneVariants.Select(v => new VariantDTO
            {
                PhoneVariantId = v.Id,
                Price = v.Price,
                StockQuantity = v.StockQuantity,
                ColorName = v.Color?.Name ?? "N/A",
                RamSize = v.Ram?.Size ?? "N/A",
                StorageSize = v.Storage?.Size ?? "N/A"
            }).ToList();

            var result = new PhoneDetailDTO
            {
                PhoneId = phone.Id,
                PhoneName = phone.Name,
                Screen = phone.Screen,
                Chip = phone.Chip,
                Battery = phone.Battery,
                Description = phone.Description,
                Slug = phone.Slug,
                IsActive = phone.IsActive,
                BrandName = brand?.Name ?? "Unknown",
                PhoneVariants = phoneVariants,
                PhoneImages = imageUrls,
                Rating = rating, // Rating trung bình
                Reviews = reviewDTOs // Danh sách các reviews
            };

            response.Success = true;
            response.Message = "Phone retrieved successfully";
            response.Code = StatusCode.Ok;
            response.Model = result;

            return response;
        }
    }
}
