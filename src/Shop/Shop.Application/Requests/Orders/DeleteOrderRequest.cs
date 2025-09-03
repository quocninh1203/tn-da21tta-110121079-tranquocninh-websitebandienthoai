using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Orders
{
    public class DeleteOrderRequest : IRequest<CommandResult>
    {
        public int OrderId { get; set; }
    }
}
