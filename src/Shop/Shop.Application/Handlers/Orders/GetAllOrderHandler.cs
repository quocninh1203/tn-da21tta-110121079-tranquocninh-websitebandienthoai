using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Orders;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Orders
{
    public class GetAllOrderHandler : IRequestHandler<GetAllOrderRequest, QueryResult<List<Order>>>
    {
        private readonly IOrderRepository _orderRepository;
        public GetAllOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<QueryResult<List<Order>>> Handle(GetAllOrderRequest request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetAsync();
            var response = new QueryResult<List<Order>>();

            if (orders == null || !orders.Any())
            {
                response.Success = false;
                response.Message = string.Format(CommonMessages.NoDataFound, nameof(Order));
                response.Code = StatusCode.NotFound;
                response.Model = new List<Order>();
            }
            response.Model = (List<Order>)orders;

            return response;
        }
    }
}
