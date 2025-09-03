using MediatR;
using Shop.Application.DTOs.Phones;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Phones
{
    public class GetAllPhoneRequest : IRequest<QueryResult<List<PhoneDTO>>>
    {
    }
}
