using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Carts
{
    public class DeleteCartRequest : IRequest<CommandResult>
    {
        public int CartId { get; set; }
    }
}
