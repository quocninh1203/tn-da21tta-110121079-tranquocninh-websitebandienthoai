using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Colors
{
    public class DeleteColorRequest : IRequest<CommandResult>
    {
        public int ColorId { get; set; }
    }
}
