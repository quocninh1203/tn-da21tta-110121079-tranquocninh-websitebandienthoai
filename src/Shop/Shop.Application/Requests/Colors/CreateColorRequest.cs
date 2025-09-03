using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Colors
{
    public class CreateColorRequest : IRequest<CommandResult>
    {
        public string Name { get; set; }
    }
}
