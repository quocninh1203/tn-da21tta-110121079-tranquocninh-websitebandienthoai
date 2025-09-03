using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Reviews
{
    public class DeleteReviewRequest : IRequest<CommandResult>
    {
        public int ReviewId { get; set; }
    }
}
