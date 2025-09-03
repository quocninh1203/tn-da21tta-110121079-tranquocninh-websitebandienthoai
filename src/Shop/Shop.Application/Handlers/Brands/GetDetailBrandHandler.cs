using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shop.Application.DTOs.Brands;
using Shop.Application.DTOs.Phones;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Brands;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Brands
{
    public class GetDetailBrandHandler : IRequestHandler<GetDetailBrandRequest, QueryResult<BrandDetailDTO>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IPhoneRepository _phoneRepository;
        private readonly IHttpContextAccessor _httpContext;

        public GetDetailBrandHandler(
            IBrandRepository brandRepository,
            IPhoneRepository phoneRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _brandRepository = brandRepository;
            _phoneRepository = phoneRepository;
            _httpContext = httpContextAccessor;
        }

        public async Task<QueryResult<BrandDetailDTO>> Handle(GetDetailBrandRequest request, CancellationToken cancellationToken)
        {
            var response = new QueryResult<BrandDetailDTO>();

            var brand = await _brandRepository.GetByIdAsync(request.BrandId);
            if (brand == null)
            {
                response.Success = false;
                response.Message = "Id not found";
                response.Code = StatusCode.NotFound;
                response.Model = null;
                return response;
            }

            var phones = await _phoneRepository.GetAsync(
                predicate: p => p.BrandId == request.BrandId,
                include: query => query.Include(p => p.PhoneVariants)
            );

            var baseUrl = _httpContext.HttpContext != null
                ? $"{_httpContext.HttpContext.Request.Scheme}://{_httpContext.HttpContext.Request.Host}"
                : "";

            var phoneDTOs = phones.Select(phone =>
            {
                var lowestPrice = phone.PhoneVariants?
                    .Where(v => v.Price != null)
                    .Min(v => (int?)v.Price) ?? 0;

                var imageUrl = phone.ImageUrl;
                if (!string.IsNullOrWhiteSpace(imageUrl) && !imageUrl.StartsWith("http"))
                {
                    imageUrl = baseUrl + imageUrl;
                }

                return new PhoneDTO
                {
                    PhoneId = phone.Id,
                    BrandId = phone.BrandId,
                    Name = phone.Name,
                    PriceReview = lowestPrice,
                    ImageUrl = imageUrl,
                    Screen = phone.Screen,
                    Chip = phone.Chip,
                    Battery = phone.Battery,
                    Description = phone.Description,
                    Slug = phone.Slug,
                    IsActive = phone.IsActive,
                };
            }).ToList();

            var result = new BrandDetailDTO
            {
                BrandId = brand.Id,
                BrandName = brand.Name,
                Slug = brand.Slug,
                Phones = phoneDTOs
            };

            response.Success = true;
            response.Code = StatusCode.Ok;
            response.Model = result;
            return response;
        }
    }
}
