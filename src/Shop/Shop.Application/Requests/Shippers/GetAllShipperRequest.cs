using MediatR;
using Shop.Domain.Entities;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Shippers
{
    public class GetAllShipperRequest : IRequest<QueryResult<List<Shipper>>>
    {
    }
}
