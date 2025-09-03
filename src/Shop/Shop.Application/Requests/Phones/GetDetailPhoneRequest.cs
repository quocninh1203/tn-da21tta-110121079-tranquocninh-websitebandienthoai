using MediatR;
using Shop.Application.DTOs.Phones;
using Shop.Domain.Entities;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Phones
{
    public class GetDetailPhoneRequest : IRequest<QueryResult<PhoneDetailDTO>>
    {
        public int PhoneId { get; set; }
    }
}
