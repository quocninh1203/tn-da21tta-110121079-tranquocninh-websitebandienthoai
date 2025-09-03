using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.PhoneImages
{
    public class DeletePhoneImageRequest : IRequest<CommandResult>
    {
        public int ImgId { get; set; }
    }
}
