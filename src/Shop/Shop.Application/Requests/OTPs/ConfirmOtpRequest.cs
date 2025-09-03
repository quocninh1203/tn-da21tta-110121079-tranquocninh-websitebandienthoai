using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.OTPs
{
    public class ConfirmOtpRequest : IRequest<CommandResult>
    {
        public string Email { get; set; }
        public string OtpCode { get; set; }
    }
}
