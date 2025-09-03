using MediatR;
using Shop.Application.DTOs.Orders;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Orders
{
    public class GetOrdersByUserIdRequest : IRequest<QueryResult<List<OrderDetailDTO>>>
    {
        public int UserId { get; set; }
    }
}
