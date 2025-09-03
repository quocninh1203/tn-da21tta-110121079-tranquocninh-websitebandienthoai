using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.PhoneImages
{
    public class CreatePhoneImageRequest : IRequest<CommandResult>
    {
        public int PhoneId { get; set; }
        public string ImageBase64 { get; set; }
    }
}
