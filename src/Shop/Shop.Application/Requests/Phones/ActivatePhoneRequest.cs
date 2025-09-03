using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Phones
{
    public class ActivatePhoneRequest : IRequest<CommandResult>
    {
        public int PhoneId { get; set; }
    }
}
