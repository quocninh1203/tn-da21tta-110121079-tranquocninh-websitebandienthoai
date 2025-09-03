using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.PaymentMethods;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.PaymentMethods
{
    public class CreatePaymentMethodHandler : IRequestHandler<CreatePaymentMethodRequest, CommandResult>
    {
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        public CreatePaymentMethodHandler(IPaymentMethodRepository paymentMethodRepository)
        {
            _paymentMethodRepository = paymentMethodRepository;
        }
        public async Task<CommandResult> Handle(CreatePaymentMethodRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var check = await _paymentMethodRepository.GetSingleAsync(r => r.Name == request.Name);
            if (check != null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.AlreadyExists, nameof(PaymentMethod));
                result.Code = StatusCode.Conflict;
                return result;
            }

            var payMethod = new PaymentMethod
            {
                Name = request.Name,
            };
            await _paymentMethodRepository.Add(payMethod);

            return result;
        }
    }
}
