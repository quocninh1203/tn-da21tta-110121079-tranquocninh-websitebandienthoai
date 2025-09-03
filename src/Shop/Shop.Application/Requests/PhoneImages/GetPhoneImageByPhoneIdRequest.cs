using MediatR;
using Shop.Domain.Entities;
using Shop.Domain.Results;

namespace Shop.Application.Requests.PhoneImages
{
    public class GetPhoneImageByPhoneIdRequest : IRequest<QueryResult<List<PhoneImage>>>
    {
        public int PhoneId { get; set; }
    }
}
