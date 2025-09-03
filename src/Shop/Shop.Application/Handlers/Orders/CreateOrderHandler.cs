using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Orders;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Methods;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Orders
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, QueryResult<Order>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly IShipperRepository _shipperRepository;
        public CreateOrderHandler(
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            IPaymentMethodRepository paymentMethodRepository,
            IShipperRepository shipperRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _shipperRepository = shipperRepository;
        }
        public async Task<QueryResult<Order>> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var result = new QueryResult<Order>();

            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(User));
                result.Code = StatusCode.NotFound;
                result.Model = null;
                return result;
            }
            var payMethod = await _paymentMethodRepository.GetByIdAsync(request.MethodId);
            if (payMethod == null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(PaymentMethod));
                result.Code = StatusCode.NotFound;
                result.Model = null;
                return result;
            }
            var ship = await _shipperRepository.GetByIdAsync(request.ShipperId);
            if (ship == null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(Shipper));
                result.Code = StatusCode.NotFound;
                result.Model = null;
                return result;
            }

            var orderCode = GenerateOrderCode.GenerateCode();
            var order = new Order
            {
                UserId = request.UserId,
                OrderDate = request.OrderDate,
                Status = request.Status,
                MethodId = request.MethodId,
                ShipperId = request.ShipperId,
                ShippingAddress = request.ShippingAddress,
                OrderCode = orderCode,
                IsPrepaid = request.IsPrepaid,
                TotalPrice = ship.Cost,
            };

            await _orderRepository.Add(order);
            result.Model = order;
            return result;
        }
    }
}
