using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.PaymentMethods
{
    public class DeletePaymentMethodRequest : IRequest<CommandResult>
    {
        public int PayMethodId { get; set; }
    }
}
