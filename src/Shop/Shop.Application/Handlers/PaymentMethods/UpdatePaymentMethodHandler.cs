using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.PaymentMethods;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.PaymentMethods
{
    public class UpdatePaymentMethodHandler : IRequestHandler<UpdatePaymentMethodRequest, CommandResult>
    {
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        public UpdatePaymentMethodHandler(IPaymentMethodRepository paymentMethodRepository)
        {
            _paymentMethodRepository = paymentMethodRepository;
        }
        public async Task<CommandResult> Handle(UpdatePaymentMethodRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var payMethod = await _paymentMethodRepository.GetByIdAsync(request.PayMethodId);
            if (payMethod is null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(PaymentMethod));
                result.Code = StatusCode.NotFound;
                return result;
            }

            var updateEntity = new PaymentMethod
            {
                Name = request.Name,
            };
            payMethod.UpdateWith(updateEntity);

            var check = await _paymentMethodRepository.GetSingleAsync(r => r.Name == payMethod.Name);
            if (check != null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.AlreadyExists, nameof(PaymentMethod));
                result.Code = StatusCode.Conflict;
                return result;
            }

            await _paymentMethodRepository.Update(payMethod);
            return result;
        }
    }
}
