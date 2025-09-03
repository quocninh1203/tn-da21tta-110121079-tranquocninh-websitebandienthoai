using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.PaymentMethods
{
    public class CreatePaymentMethodRequest : IRequest<CommandResult>
    {
        public string Name { get; set; }
    }
}
