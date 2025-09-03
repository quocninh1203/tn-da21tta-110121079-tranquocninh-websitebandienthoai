using MediatR;
using Shop.Domain.Entities;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Brands
{
    public class GetAllBrandRequest : IRequest<QueryResult<List<Brand>>>
    {
    }
}
