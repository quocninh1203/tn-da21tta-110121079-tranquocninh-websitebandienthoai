using MediatR;
using Shop.Application.DTOs.Orders;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Orders;
using Shop.Application.Services.Email;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Orders
{
    public class ChangeStatusHandler : IRequestHandler<ChangeStatusRequest, CommandResult>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IPhoneVariantRepository _phoneVariantRepository;
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly IShipperRepository _shipperRepository;
        private readonly IUserProductInteractionRepository _userProductInteractionRepository;
        private readonly IEmailService _emailService;
        public ChangeStatusHandler(IOrderRepository orderRepository,
                                   IUserRepository userRepository,
                                   IOrderDetailRepository orderDetailRepository,
                                   IPhoneVariantRepository phoneVariantRepository,
                                   IPaymentMethodRepository paymentMethodRepository,
                                   IShipperRepository shipperRepository,
                                   IUserProductInteractionRepository userProductInteractionRepository,
                                   IEmailService emailService)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _orderDetailRepository = orderDetailRepository;
            _phoneVariantRepository = phoneVariantRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _shipperRepository = shipperRepository;
            _userProductInteractionRepository = userProductInteractionRepository;
            _emailService = emailService;
        }
        public async Task<CommandResult> Handle(ChangeStatusRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order is null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(Order));
                result.Code = StatusCode.NotFound;
                return result;
            }

            order.Status = request.Status;
            await _orderRepository.Update(order);

            if (request.Status == 2)
            {
                var user = await _userRepository.GetByIdAsync(order.UserId);

                var details = await _orderDetailRepository.GetAsync(d => d.OrderId == order.Id);

                var items = new List<OrderItemDTO>();
                foreach (var d in details)
                {
                    // Phải tự gọi repo để lấy Variant
                    var variant = await _phoneVariantRepository.GetSingleAsync(
                        v => v.Id == d.VariantId,
                        v => v.Phone,
                        v => v.Color,
                        v => v.Ram,
                        v => v.Storage
                    );

                    items.Add(new OrderItemDTO
                    {
                        Quantity = d.Quantity,
                        PriceAtOrder = d.PriceAtOrder, 
                        Price = variant?.Price ?? 0,
                        Color = variant?.Color?.Name ?? "N/A",
                        Ram = variant?.Ram?.Size ?? "N/A",
                        Storage = variant?.Storage?.Size ?? "N/A",
                        PhoneName = variant?.Phone?.Name ?? "N/A",
                        PhoneImageUrl = variant?.Phone?.ImageUrl ?? ""
                    });
                }

                var paymentMethod = await _paymentMethodRepository.GetByIdAsync(order.MethodId); 
                var shippingCost = await _shipperRepository.GetByIdAsync(order.ShipperId);

                await _emailService.SendOrderConfirmationAsync(user.Email, user.FullName, user.PhoneNumber,
                                                               order.ShippingAddress, order.OrderCode, order.OrderDate,
                                                               order.TotalPrice, items, paymentMethod.Name, shippingCost.Cost, order.IsPrepaid);
            }

            if (request.Status == 4)
            {
                var details = await _orderDetailRepository.GetAsync(d => d.OrderId == order.Id);
                foreach (var d in details)
                {
                    var variant = await _phoneVariantRepository.GetSingleAsync(v => v.Id == d.VariantId);
                    var userProductInteraction = await _userProductInteractionRepository.GetSingleAsync(u => u.UserId == order.UserId && u.ProductId == variant.PhoneId);

                    userProductInteraction.Label = true;
                    await _userProductInteractionRepository.Update(userProductInteraction);
                }
            }

            return result;
        }
    }
}
