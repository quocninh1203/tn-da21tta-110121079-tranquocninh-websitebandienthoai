using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Rams
{
    public class CreateRamRequest : IRequest<CommandResult>
    {
        public string Size { get; set; }
    }
}
