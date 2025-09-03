using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.PaymentMethods
{
    public class UpdatePaymentMethodRequest : IRequest<CommandResult>
    {
        public int PayMethodId { get; set; }
        public string? Name { get; set; }
    }
}
