using MediatR;
using Shop.Application.DTOs.Orders;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Orders
{
    public class GetOrderDetailByOrderIdRequest : IRequest<QueryResult<OrderDetailDTO>>
    {
        public int OrderId { get; set; }
    }
}
