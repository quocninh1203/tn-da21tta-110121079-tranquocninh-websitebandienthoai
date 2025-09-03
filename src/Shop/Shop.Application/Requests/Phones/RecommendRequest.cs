using MediatR;
using Shop.Application.DTOs.Phones;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Phones
{
    public class RecommendRequest : IRequest<QueryResult<List<PhoneDTO>>>
    {
        public int UserId { get; set; }
    }
}
