using MediatR;
using Shop.Domain.Entities;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Orders
{
    public class GetAllOrderRequest : IRequest<QueryResult<List<Order>>>
    {
    }
}
