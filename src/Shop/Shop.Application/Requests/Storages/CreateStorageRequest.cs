using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Storages
{
    public class CreateStorageRequest : IRequest<CommandResult>
    {
        public string Size { get; set; }
    }
}
