using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.PhoneVariants
{
    public class DeletePhoneVariantRequest : IRequest<CommandResult>
    {
        public int VariantId { get; set; }
    }
}
