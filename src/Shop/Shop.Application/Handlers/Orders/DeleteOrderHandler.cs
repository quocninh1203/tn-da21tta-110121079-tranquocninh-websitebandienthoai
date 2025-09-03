using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Orders;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Orders
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderRequest, CommandResult>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        public DeleteOrderHandler(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
        }
        public async Task<CommandResult> Handle(DeleteOrderRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var order = await _orderRepository.GetSingleAsync(
                o => o.Id == request.OrderId,
                o => o.OrderDetails
            );

            if (order is null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(Order));
                result.Code = StatusCode.NotFound;
                return result;
            }

            if (order.OrderDetails != null && order.OrderDetails.Any())
            {
                var orderDetailsList = order.OrderDetails.ToList();
                foreach (var orderDetail in orderDetailsList)
                {
                    await _orderDetailRepository.Delete(orderDetail);
                }
            }

            await _orderRepository.Delete(order);

            return result;
        }
    }
}
