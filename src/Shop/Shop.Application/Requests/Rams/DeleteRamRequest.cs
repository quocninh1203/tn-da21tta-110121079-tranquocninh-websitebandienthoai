using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Rams
{
    public class DeleteRamRequest : IRequest<CommandResult>
    {
        public int RamId { get; set; }
    }
}
