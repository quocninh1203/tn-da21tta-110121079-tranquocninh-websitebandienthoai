using MediatR;
using Microsoft.AspNetCore.Http;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Brands;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Brands
{
    public class GetAllBrandHandler : IRequestHandler<GetAllBrandRequest, QueryResult<List<Brand>>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IHttpContextAccessor _httpContext;
        public GetAllBrandHandler(IBrandRepository brandRepository, IHttpContextAccessor httpContext)
        {
            _brandRepository = brandRepository;
            _httpContext = httpContext;
        }
        public async Task<QueryResult<List<Brand>>> Handle(GetAllBrandRequest request, CancellationToken cancellationToken)
        {
            var brands = await _brandRepository.GetAsync();
            var response = new QueryResult<List<Brand>>();

            if (brands == null || !brands.Any())
            {
                response.Success = false;
                response.Message = string.Format(CommonMessages.NoDataFound, nameof(Brand));
                response.Code = StatusCode.NotFound;
                response.Model = new List<Brand>();
            }

            var baseUrl = _httpContext.HttpContext != null
               ? $"{_httpContext.HttpContext.Request.Scheme}://{_httpContext.HttpContext.Request.Host}"
               : "";
            foreach (var brand in brands)
            {
                if (!string.IsNullOrWhiteSpace(brand.ImageUrl) && !brand.ImageUrl.StartsWith("http"))
                {
                    brand.ImageUrl = baseUrl + brand.ImageUrl;
                }
            }

            response.Success = true;
            response.Message = "Brands retrieved successfully";
            response.Code = StatusCode.Ok;
            response.Model = brands.ToList();

            return response;
        }
    }
}
