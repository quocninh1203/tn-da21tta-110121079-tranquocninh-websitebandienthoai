using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.PaymentMethods;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.PaymentMethods
{
    public class DeletePaymentMethodHandler : IRequestHandler<DeletePaymentMethodRequest, CommandResult>
    {
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly IOrderRepository _orderRepository;
        public DeletePaymentMethodHandler(IPaymentMethodRepository paymentMethodRepository, IOrderRepository orderRepository)
        {
            _paymentMethodRepository = paymentMethodRepository;
            _orderRepository = orderRepository;
        }
        public async Task<CommandResult> Handle(DeletePaymentMethodRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();
            var order = await _orderRepository.GetAsync(p => p.MethodId == request.PayMethodId);
            if (order.Count > 0)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.CannotDelete, nameof(PaymentMethod));
                result.Code = StatusCode.Conflict;
                return result;
            }

            var payMethod = await _paymentMethodRepository.GetByIdAsync(request.PayMethodId);
            if (payMethod is null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(PaymentMethod));
                result.Code = StatusCode.NotFound;
                return result;
            }
            await _paymentMethodRepository.Delete(payMethod);

            return result;
        }
    }
}
