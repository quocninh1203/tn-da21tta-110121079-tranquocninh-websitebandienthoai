using MediatR;
using Shop.Domain.Entities;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Reviews
{
    public class GetReviewByPhoneIdRequest : IRequest<QueryResult<List<Review>>>
    {
        public int PhoneId { get; set; }
    }
}
