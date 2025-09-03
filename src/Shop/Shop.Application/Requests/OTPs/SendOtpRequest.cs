using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.OTPs
{
    public class SendOtpRequest : IRequest<CommandResult>
    {
        public string Email { get; set; }
    }
}
