using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shop.Application.DTOs.Phones;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Phones;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Phones
{
    public class GetAllPhoneHandler : IRequestHandler<GetAllPhoneRequest, QueryResult<List<PhoneDTO>>>
    {
        private readonly IPhoneRepository _phoneRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IReviewRepository _reviewRepository;
        public GetAllPhoneHandler(IPhoneRepository phoneRepository, IHttpContextAccessor httpContextAccessor, IReviewRepository reviewRepository)
        {
            _phoneRepository = phoneRepository;
            _httpContext = httpContextAccessor;
            _reviewRepository = reviewRepository;
        }

        public async Task<QueryResult<List<PhoneDTO>>> Handle(GetAllPhoneRequest request, CancellationToken cancellationToken)
        {
            var result = new QueryResult<List<PhoneDTO>>();

            var phones = await _phoneRepository.GetAsync(include: query => query.Include(p => p.PhoneVariants));

            if (phones == null || !phones.Any())
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NoDataFound, nameof(Phone));
                result.Code = StatusCode.NotFound;
                result.Model = new List<PhoneDTO>();
                return result;
            }

            var baseUrl = _httpContext.HttpContext != null
                ? $"{_httpContext.HttpContext.Request.Scheme}://{_httpContext.HttpContext.Request.Host}"
                : "";

            // Lấy tất cả review cho các phone trong một lần
            var phoneIds = phones.Select(p => p.Id).ToList(); // Lấy danh sách phoneId
            var reviews = await _reviewRepository.GetAsync(r => phoneIds.Contains(r.PhoneId));

            var phoneDTOs = phones.Select(phone =>
            {
                var minPrice = phone.PhoneVariants?
                    .Where(v => v.Price != null)
                    .Min(v => (int?)v.Price) ?? 0;

                var imageUrl = phone.ImageUrl;
                if (!string.IsNullOrWhiteSpace(imageUrl) && !imageUrl.StartsWith("http"))
                {
                    imageUrl = baseUrl + imageUrl;
                }

                // Lấy Rating trung bình từ các review của Phone
                var phoneReviews = reviews.Where(r => r.PhoneId == phone.Id).ToList();
                var rating = phoneReviews.Any() ? phoneReviews.Average(r => r.Rating) : 5;

                return new PhoneDTO
                {
                    PhoneId = phone.Id,
                    BrandId = phone.BrandId,
                    Name = phone.Name,
                    PriceReview = minPrice,
                    ImageUrl = imageUrl,
                    Screen = phone.Screen,
                    Chip = phone.Chip,
                    Battery = phone.Battery,
                    Description = phone.Description,
                    Slug = phone.Slug,
                    Rating = rating,
                    IsActive = phone.IsActive,
                };
            }).ToList();

            result.Success = true;
            result.Code = StatusCode.Ok;
            result.Model = phoneDTOs;
            return result;
        }
    }
}
