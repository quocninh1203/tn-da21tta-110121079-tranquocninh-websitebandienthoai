using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.PhoneImages
{
    public class UpdatePhoneImageRequest : IRequest<CommandResult>
    {
        public int ImgId { get; set; }
        public int? PhoneId { get; set; }
        public string? ImageBase64 { get; set; }
    }
}
