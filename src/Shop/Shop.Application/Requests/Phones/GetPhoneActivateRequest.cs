using MediatR;
using Shop.Application.DTOs.Phones;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Phones
{
    public class GetPhoneActivateRequest : IRequest<QueryResult<List<PhoneDTO>>>
    {
    }
}
