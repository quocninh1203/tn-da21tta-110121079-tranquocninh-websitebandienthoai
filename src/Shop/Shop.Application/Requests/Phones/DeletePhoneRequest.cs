using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Phones
{
    public class DeletePhoneRequest : IRequest<CommandResult>
    {
        public int PhoneId { get; set; }
    }
}
