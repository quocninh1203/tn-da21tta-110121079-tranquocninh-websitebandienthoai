using MediatR;
using Shop.Domain.Entities;
using Shop.Domain.Results;

namespace Shop.Application.Requests.PhoneVariants
{
    public class GetDetailPhoneVariantRequest : IRequest<QueryResult<PhoneVariant>>
    {
        public int VariantId { get; set; }
    }
}
