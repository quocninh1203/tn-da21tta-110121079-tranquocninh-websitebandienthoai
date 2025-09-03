using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.OrderDetails;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.OrderDetails
{
    public class CreateOrderDetailHandler : IRequestHandler<CreateOrderDetailRequest, CommandResult>
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPhoneVariantRepository _phoneVariantRepository;
        public CreateOrderDetailHandler(
            IOrderDetailRepository orderDetailRepository,
            IOrderRepository orderRepository,
            IPhoneVariantRepository phoneVariantRepository)
        {
            _orderDetailRepository = orderDetailRepository;
            _orderRepository = orderRepository; 
            _phoneVariantRepository = phoneVariantRepository;
        }
        public async Task<CommandResult> Handle(CreateOrderDetailRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order == null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(Order));
                result.Code = StatusCode.NotFound;
                return result;
            }

            var variant = await _phoneVariantRepository.GetByIdAsync(request.VariantId);
            if (variant == null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(PhoneVariant));
                result.Code = StatusCode.NotFound;
                return result;
            }

            if(request.Quantity > variant.StockQuantity)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.ExceedsStock);
                result.Code = StatusCode.BadRequest;
                return result;
            }

            var priceAtOrder = request.Quantity * variant.Price;
            var orderDetail = new OrderDetail
            {
                OrderId = request.OrderId,
                VariantId = request.VariantId,
                Quantity = request.Quantity,
                PriceAtOrder = priceAtOrder
            };

            await _orderDetailRepository.Add(orderDetail);

            order.TotalPrice = order.TotalPrice + priceAtOrder;
            await _orderRepository.Update(order);

            variant.StockQuantity -= request.Quantity;
            await _phoneVariantRepository.Update(variant);

            return result;
        }
    }
}
