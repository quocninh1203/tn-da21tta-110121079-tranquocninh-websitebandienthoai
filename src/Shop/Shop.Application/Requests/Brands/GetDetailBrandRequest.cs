using MediatR;
using Shop.Application.DTOs.Brands;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Brands
{
    public class GetDetailBrandRequest : IRequest<QueryResult<BrandDetailDTO>>
    {
        public int BrandId { get; set; }
    }
}
